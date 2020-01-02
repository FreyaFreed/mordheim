using System;
using UnityEngine;

public class FearCheck : global::ICheapState
{
	public FearCheck(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.currentActionData.SetAction(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_name_fear_roll"), global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/fear", true));
		int fearRoll = this.unitCtrlr.unit.FearRoll;
		this.success = this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, fearRoll, global::AttributeId.FEAR_ROLL, false, true, 0);
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
		this.unitCtrlr.ActionDone();
	}

	private global::UnitController unitCtrlr;

	private bool success;
}
