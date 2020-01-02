using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEffects : global::ICheapState
{
	public UpdateEffects(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.currentActionData.SetAction(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_name_effect_roll"), global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/turn_start_effect", true));
		this.unitCtrlr.unit.ApplyTurnStartEnchantments();
		this.enchantments = this.unitCtrlr.unit.GetEnchantmentDamages(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, global::EnchantmentDmgTriggerId.ON_TURN_START);
		this.CheckNextEnchantment();
	}

	void global::ICheapState.Exit(int iTo)
	{
		this.unitCtrlr.unit.DestroyEnchantments(global::EnchantmentTriggerId.ON_POST_TURN_START, false);
		this.unitCtrlr.attackerCtrlr = null;
	}

	void global::ICheapState.Update()
	{
		if (this.showNext)
		{
			this.showNext = false;
			this.CheckNextEnchantment();
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void OnSeqDone()
	{
		this.showNext = true;
	}

	public void TriggerEffect()
	{
		if (this.unitCtrlr.lastActionWounds != 0)
		{
			this.unitCtrlr.PlayDefState(this.unitCtrlr.attackResultId, 0, this.unitCtrlr.unit.Status);
		}
	}

	private void CheckNextEnchantment()
	{
		if (this.unitCtrlr.unit.Status == global::UnitStateId.OUT_OF_ACTION)
		{
			this.unitCtrlr.KillUnit();
			this.unitCtrlr.SkillRPC(339);
			return;
		}
		if (this.enchantments.Count == 0)
		{
			this.unitCtrlr.nextState = global::UnitController.State.RECOVERY;
			return;
		}
		global::Tuple<global::Enchantment, int> tuple = this.enchantments[0];
		this.enchantments.RemoveAt(0);
		this.unitCtrlr.attackerCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(tuple.Item1.Provider, false);
		this.unitCtrlr.lastActionWounds = 0;
		this.unitCtrlr.flyingLabel = string.Empty;
		this.unitCtrlr.currentActionData.SetAction(tuple.Item1.LocalizedName, global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/turn_start_effect", true));
		if (tuple.Item2 > 0)
		{
			this.unitCtrlr.ComputeDirectWound(tuple.Item2, true, null, false);
		}
		else
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, bool, string>(global::Notices.RETROACTION_TARGET_OUTCOME, this.unitCtrlr, string.Empty, true, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_resist"));
		}
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("dot_effect", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
	}

	private global::UnitController unitCtrlr;

	private global::System.Collections.Generic.List<global::Tuple<global::Enchantment, int>> enchantments;

	private bool showNext;
}
