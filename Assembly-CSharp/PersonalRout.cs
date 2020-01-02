using System;
using UnityEngine;

public class PersonalRout : global::ICheapState
{
	public PersonalRout(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.success = true;
		this.unitCtrlr.currentActionLabel = global::AttributeId.ALL_ALONE_ROLL.ToString();
		int allAloneRoll = this.unitCtrlr.unit.AllAloneRoll;
		this.success = this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, allAloneRoll, global::AttributeId.ALL_ALONE_ROLL, false, true, 0);
		this.unitCtrlr.currentActionData.SetAction(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_name_all_alone_roll"), global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/all_alone", true));
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, bool, string>(global::Notices.RETROACTION_TARGET_OUTCOME, this.unitCtrlr, string.Empty, this.success, (!this.success) ? global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_fail") : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_success"));
		this.unitCtrlr.recoveryTarget = allAloneRoll;
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("moral_check", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
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

	private void OnSeqDone()
	{
		if (this.success || !this.unitCtrlr.CanDisengage())
		{
			this.unitCtrlr.CheckTerror();
		}
		else
		{
			this.unitCtrlr.fleeDistanceMultiplier = global::Constant.GetFloat(global::ConstantId.ALL_ALONE_MOVEMENT_MULTIPLIER);
			this.unitCtrlr.StateMachine.ChangeState(40);
		}
	}

	private global::UnitController unitCtrlr;

	private bool success;
}
