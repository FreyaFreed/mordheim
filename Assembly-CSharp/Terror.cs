using System;
using UnityEngine;

public class Terror : global::ICheapState
{
	public Terror(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.currentActionLabel = global::AttributeId.TERROR_ROLL.ToString();
		int terrorRoll = this.unitCtrlr.unit.TerrorRoll;
		this.success = this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, terrorRoll, global::AttributeId.TERROR_ROLL, false, true, 0);
		this.unitCtrlr.recoveryTarget = ((terrorRoll <= this.unitCtrlr.recoveryTarget) ? this.unitCtrlr.recoveryTarget : terrorRoll);
		this.unitCtrlr.currentActionData.SetAction(global::PandoraSingleton<global::LocalizationManager>.Instance.BuildStringAndLocalize("skill_name_", global::AttributeId.TERROR_ROLL.ToString(), null, null), global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/terror", true));
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, bool, string>(global::Notices.RETROACTION_TARGET_OUTCOME, this.unitCtrlr, string.Empty, this.success, (!this.success) ? global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_fail") : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_success"));
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
		this.unitCtrlr.StateMachine.ChangeState(9);
		global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit().CheckFear();
	}

	private global::UnitController unitCtrlr;

	private bool success;
}
