using System;
using System.Collections.Generic;

public class Engaged : global::ICheapState
{
	public Engaged(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	public void Destroy()
	{
		this.unitCtrlr = null;
	}

	public void Enter(int iFrom)
	{
		this.unitCtrlr.interactivePoint = null;
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.UpdateActionStatus(true, global::UnitActionRefreshId.NONE);
		this.unitCtrlr.wasEngaged = true;
		this.UpdateCurrentAction(0);
		if (!this.unitCtrlr.EngagedUnits.Contains(this.unitCtrlr.defenderCtrlr))
		{
			this.unitCtrlr.defenderCtrlr = this.unitCtrlr.EngagedUnits[0];
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, int>(global::Notices.UPDATE_TARGET, this.unitCtrlr.defenderCtrlr, this.unitCtrlr.defenderCtrlr.unit.warbandIdx);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.GAME_STATUS_ENGAGE, true);
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, this.unitCtrlr.transform, true, false, true, this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
		global::PandoraSingleton<global::MissionManager>.Instance.ShowCombatCircles(this.unitCtrlr);
		this.unitCtrlr.Send(true, global::Hermes.SendTarget.OTHERS, this.unitCtrlr.uid, 1U, new object[]
		{
			0f,
			this.unitCtrlr.transform.rotation,
			this.unitCtrlr.transform.position
		});
		if (this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.UNIT_START_MOVE);
		}
	}

	public void Exit(int iTo)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.GAME_STATUS_ENGAGE, false);
		this.unitCtrlr.lastActionWounds = 0;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.RETROACTION_ACTION_CLEAR);
	}

	public void FixedUpdate()
	{
	}

	public void Update()
	{
		if (this.unitCtrlr.CurrentAction.waitForConfirmation)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
			{
				this.unitCtrlr.CurrentAction.Select();
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0))
			{
				this.unitCtrlr.CurrentAction.Cancel();
			}
			return;
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("overview", 0))
		{
			this.unitCtrlr.StateMachine.ChangeState(44);
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0))
		{
			this.unitCtrlr.SetCurrentAction(global::SkillId.BASE_END_TURN);
			this.unitCtrlr.CurrentAction.Select();
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, this.unitCtrlr.CurrentAction);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
		{
			this.unitCtrlr.CurrentAction.Select();
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cycling", 0))
		{
			this.UpdateCurrentAction(1);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("cycling", 0))
		{
			this.UpdateCurrentAction(-1);
		}
	}

	private void UpdateCurrentAction(int dir = 0)
	{
		if (this.unitCtrlr.availableActionStatus.Count > 0)
		{
			this.actionIndex += dir;
			this.actionIndex = ((this.actionIndex < this.unitCtrlr.availableActionStatus.Count) ? ((this.actionIndex >= 0) ? this.actionIndex : (this.unitCtrlr.availableActionStatus.Count - 1)) : 0);
			this.unitCtrlr.SetCurrentAction(this.unitCtrlr.availableActionStatus[this.actionIndex].SkillId);
			if (this.unitCtrlr.IsPlayed())
			{
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, this.unitCtrlr.CurrentAction);
			}
		}
		else if (this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, null);
		}
	}

	private global::UnitController unitCtrlr;

	public int actionIndex;
}
