using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapped : global::ICheapState
{
	public Trapped(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetAnimSpeed(0f);
		this.unitCtrlr.SetFixed(true);
		if (this.unitCtrlr == global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit())
		{
			this.unitCtrlr.ValidMove();
		}
		global::TrapEffectData effectData = ((global::Trap)this.unitCtrlr.activeTrigger).EffectData;
		int trapResistRoll = this.unitCtrlr.unit.TrapResistRoll;
		if (!this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, trapResistRoll, global::AttributeId.TRAP_RESIST_ROLL, false, true, 0))
		{
			global::System.Collections.Generic.List<global::TrapEffectJoinEnchantmentData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapEffectJoinEnchantmentData>("fk_trap_effect_id", effectData.Id.ToIntString<global::TrapEffectId>());
			for (int i = 0; i < list.Count; i++)
			{
				global::Enchantment enchantment = this.unitCtrlr.unit.AddEnchantment(list[i].EnchantmentId, this.unitCtrlr.unit, false, false, global::AllegianceId.NONE);
				if (enchantment != null && !enchantment.Data.NoDisplay)
				{
					this.unitCtrlr.buffResultId = enchantment.Data.EffectTypeId;
					global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, global::EffectTypeId, string>(global::Notices.RETROACTION_TARGET_ENCHANTMENT, this.unitCtrlr, enchantment.LocalizedName, enchantment.Data.EffectTypeId, string.Empty);
				}
			}
			int enchantmentDamage = this.unitCtrlr.unit.GetEnchantmentDamage(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, global::EnchantmentDmgTriggerId.ON_APPLY);
			if (enchantmentDamage > 0)
			{
				this.unitCtrlr.buffResultId = global::EffectTypeId.NONE;
				this.unitCtrlr.ComputeDirectWound(enchantmentDamage, true, null, false);
			}
			this.unitCtrlr.unit.UpdateAttributes();
		}
		if (this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.TRAPS, 1);
		}
		this.unitCtrlr.currentActionData.SetAction(global::PandoraSingleton<global::LocalizationManager>.Instance.BuildStringAndLocalize("skill_name_", effectData.Name, null, null), global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/trap", true));
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("trap", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.RETROACTION_SHOW_OUTCOME, this.unitCtrlr);
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

	public void OnSeqDone()
	{
		this.unitCtrlr.EndDefense();
		global::UnityEngine.Object.Destroy(this.unitCtrlr.activeTrigger);
		global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(this.Wait());
		this.unitCtrlr.activeTrigger = null;
	}

	private global::System.Collections.IEnumerator Wait()
	{
		yield return new global::UnityEngine.WaitForSeconds(0.75f);
		if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
		{
			this.unitCtrlr.SendStartMove(this.unitCtrlr.transform.position, this.unitCtrlr.transform.rotation);
		}
		else
		{
			this.unitCtrlr.StateMachine.ChangeState(9);
		}
		yield break;
	}

	private global::UnitController unitCtrlr;
}
