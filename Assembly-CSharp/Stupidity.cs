using System;
using UnityEngine;

public class Stupidity : global::ICheapState
{
	public Stupidity(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Stupidity Enter: unit is engaged : ",
			this.unitCtrlr.Engaged,
			" has condition : ",
			this.unitCtrlr.unit.HasEnchantment(global::EnchantmentTypeId.MENTAL_CONDITION_STUPIDITY)
		}), "uncategorised", null);
		if (this.unitCtrlr.unit.HasEnchantment(global::EnchantmentTypeId.MENTAL_CONDITION_STUPIDITY) && !this.unitCtrlr.Engaged)
		{
			int stupidityRoll = this.unitCtrlr.unit.StupidityRoll;
			this.stupidSuccess = this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, stupidityRoll, global::AttributeId.STUPIDITY_ROLL, false, true, 0);
			this.unitCtrlr.recoveryTarget = stupidityRoll;
			this.unitCtrlr.currentActionData.SetAction(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_name_stupidity_roll"), global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/stupidity", true));
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, bool, string>(global::Notices.RETROACTION_TARGET_OUTCOME, this.unitCtrlr, string.Empty, this.stupidSuccess, (!this.stupidSuccess) ? global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_fail") : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_success"));
			if (!this.stupidSuccess)
			{
				this.unitCtrlr.unit.SetAttribute(global::AttributeId.CURRENT_STRATEGY_POINTS, 0);
			}
			global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence((!this.stupidSuccess) ? "stupidity" : "moral_check", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
		}
		else
		{
			this.NextState();
		}
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
		if (!this.stupidSuccess)
		{
			this.unitCtrlr.SkillRPC(339);
		}
		else
		{
			this.NextState();
		}
	}

	private void NextState()
	{
		this.unitCtrlr.nextState = global::UnitController.State.START_MOVE;
	}

	private global::UnitController unitCtrlr;

	private bool stupidSuccess;
}
