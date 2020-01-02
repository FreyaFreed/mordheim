using System;
using UnityEngine;

public class Rout : global::ICheapState
{
	public Rout(global::MissionManager mngr)
	{
		this.missionMngr = mngr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.warCtrlr = this.missionMngr.GetCurrentUnit().GetWarband();
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"WarbandIdx = ",
			this.warCtrlr.idx,
			" Ratio = ",
			this.warCtrlr.MoralRatio,
			"Rout at = ",
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.routThreshold
		}), "ROUT", null);
		if (this.missionMngr.GetCurrentUnit().unit.Status != global::UnitStateId.OUT_OF_ACTION)
		{
			this.missionMngr.CombatLogger.AddLog(global::CombatLogger.LogMessage.TURN_START, new string[]
			{
				this.missionMngr.GetCurrentUnit().GetLogName()
			});
			if (this.warCtrlr.defeated || !this.warCtrlr.canRout || this.warCtrlr.MoralValue == this.warCtrlr.OldMoralValue)
			{
				this.Done();
			}
			else if (this.warCtrlr.MoralRatio >= global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.routThreshold)
			{
				this.Done();
			}
			else if (this.warCtrlr.MoralValue <= 0)
			{
				this.warCtrlr.defeated = true;
				this.Done();
			}
			else if (this.warCtrlr.MoralRatio < global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.routThreshold)
			{
				this.missionMngr.CombatLogger.AddLog(global::CombatLogger.LogMessage.MORAL_BELOW, new string[]
				{
					this.warCtrlr.MoralValue.ToString(),
					this.warCtrlr.MaxMoralValue.ToString(),
					global::UnityEngine.Mathf.FloorToInt(global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.routThreshold * 100f).ToString()
				});
				this.RoutRoll();
			}
			else
			{
				this.Done();
			}
			return;
		}
		this.Done();
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void CheckWarband()
	{
	}

	private void RoutRoll()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.PHASE_ROUT);
		this.warCtrlr.OldMoralValue = this.warCtrlr.MoralValue;
		this.rollUnit = this.warCtrlr.GetAliveLeader();
		if (this.rollUnit == null)
		{
			this.warCtrlr.defeated = true;
			this.rollUnit = this.warCtrlr.GetLeader();
		}
		if (this.rollUnit == null)
		{
			this.rollUnit = this.warCtrlr.unitCtrlrs.Find((global::UnitController x) => x.unit.Status != global::UnitStateId.OUT_OF_ACTION);
		}
		if (this.rollUnit == null)
		{
			this.rollUnit = this.warCtrlr.unitCtrlrs[0];
		}
		this.rollUnit.currentActionData.SetAction(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_name_rout_roll"), global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/rout", true));
		int num = this.rollUnit.unit.WarbandRoutRoll;
		if (!this.rollUnit.unit.IsAvailable() || this.warCtrlr.MoralValue == 0 || this.warCtrlr.AllUnitsDead())
		{
			num = 0;
		}
		this.rollUnit.recoveryTarget = num;
		this.routSuccess = this.rollUnit.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, num, global::AttributeId.LEADERSHIP_ROLL, false, true, 0);
		this.warCtrlr.defeated = !this.routSuccess;
		global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog((!this.routSuccess) ? global::CombatLogger.LogMessage.ROUT_FAIL : global::CombatLogger.LogMessage.ROUT_SUCCESS, new string[0]);
		this.rollUnit.currentActionData.SetActionOutcome(this.routSuccess);
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("moral_check", this.rollUnit, new global::DelSequenceDone(this.Done));
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::WarbandController, bool>(global::Notices.MISSION_ROUT_TEST, this.warCtrlr, this.routSuccess);
	}

	private void Done()
	{
		if (!this.missionMngr.CheckEndGame())
		{
			this.missionMngr.GetCurrentUnit().nextState = global::UnitController.State.TURN_START;
			this.missionMngr.WatchOrMove();
		}
	}

	private global::MissionManager missionMngr;

	private bool routSuccess;

	private global::UnitController rollUnit;

	private global::WarbandController warCtrlr;
}
