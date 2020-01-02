using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Prometheus;
using UnityEngine;

public class SpellCurse : global::ICheapState
{
	public SpellCurse(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
		this.logBldr = new global::System.Text.StringBuilder();
	}

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		this.logBldr.Length = 0;
		global::SkillId skillId = global::SkillId.NONE;
		string name = null;
		switch (this.unitCtrlr.currentSpellTypeId)
		{
		case global::SpellTypeId.ARCANE:
			this.hasCurse = this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, this.unitCtrlr.unit.TzeentchsCurseRoll, global::AttributeId.TZEENTCHS_CURSE_ROLL, true, true, 0);
			name = "action/curse_tzeentch";
			break;
		case global::SpellTypeId.DIVINE:
			this.hasCurse = this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, this.unitCtrlr.unit.DivineWrathRoll, global::AttributeId.DIVINE_WRATH_ROLL, true, true, 0);
			name = "action/curse_divine_wrath";
			break;
		case global::SpellTypeId.WYRDSTONE:
			this.hasCurse = !this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, this.unitCtrlr.unit.WyrdstoneResistRoll + this.unitCtrlr.wyrdstoneRollModifier, global::AttributeId.WYRDSTONE_RESIST_ROLL, false, true, 0);
			name = "action/curse_wyrdstone";
			break;
		case global::SpellTypeId.MISSION:
		{
			this.hasCurse = true;
			skillId = this.unitCtrlr.currentCurseSkillId;
			global::SkillData skillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)skillId);
			if (skillData.EffectTypeId == global::EffectTypeId.BUFF)
			{
				name = "action/curse_buff";
			}
			else
			{
				name = "action/curse_debuff";
			}
			break;
		}
		}
		this.unitCtrlr.defenderCtrlr = null;
		if (this.hasCurse)
		{
			if ((this.unitCtrlr.IsPlayed() && this.unitCtrlr.currentSpellTypeId == global::SpellTypeId.ARCANE) || this.unitCtrlr.currentSpellTypeId == global::SpellTypeId.DIVINE)
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.SPELLS_CURSES, 1);
			}
			if (skillId == global::SkillId.NONE)
			{
				global::System.Collections.Generic.List<global::SpellCurseData> datas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SpellCurseData>("fk_spell_type_id", ((int)this.unitCtrlr.currentSpellTypeId).ToConstantString());
				global::System.Collections.Generic.Dictionary<global::SpellCurseId, int> curseModifiers = this.unitCtrlr.unit.GetCurseModifiers();
				global::SpellCurseData randomRatio = global::SpellCurseData.GetRandomRatio(datas, global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, curseModifiers);
				skillId = ((randomRatio == null) ? global::SkillId.NONE : randomRatio.SkillId);
			}
			if (skillId != global::SkillId.NONE)
			{
				this.unitCtrlr.SetCurrentAction(skillId);
				this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction.LocalizedName, global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>(name, true));
				global::SkillData skillData2 = this.unitCtrlr.CurrentAction.skillData;
				if (skillData2.TargetingId == global::TargetingId.NONE)
				{
					if (skillData2.TargetSelf && !skillData2.TargetAlly && !skillData2.TargetEnemy)
					{
						this.unitCtrlr.defenderCtrlr = this.unitCtrlr;
					}
				}
				else
				{
					this.unitCtrlr.UpdateTargetsData();
					this.unitCtrlr.CurrentAction.SetTargets();
					switch (skillData2.TargetingId)
					{
					case global::TargetingId.SINGLE_TARGET:
						this.unitCtrlr.defenders = this.unitCtrlr.CurrentAction.Targets;
						break;
					case global::TargetingId.LINE:
					{
						global::UnityEngine.Vector3 vector;
						global::UnityEngine.Vector3 vector2;
						global::PandoraSingleton<global::MissionManager>.Instance.InitLineTarget(this.unitCtrlr.transform, (float)this.unitCtrlr.CurrentAction.Radius, (float)this.unitCtrlr.CurrentAction.RangeMax, out vector, out vector2);
						this.unitCtrlr.SetLineTargets(this.unitCtrlr.GetCurrentActionTargets(), global::PandoraSingleton<global::MissionManager>.Instance.lineTarget.transform, false);
						break;
					}
					case global::TargetingId.CONE:
					{
						global::UnityEngine.Vector3 vector;
						global::UnityEngine.Vector3 vector2;
						global::PandoraSingleton<global::MissionManager>.Instance.InitConeTarget(this.unitCtrlr.transform, (float)this.unitCtrlr.CurrentAction.Radius, (float)this.unitCtrlr.CurrentAction.RangeMax, out vector, out vector2);
						this.unitCtrlr.SetConeTargets(this.unitCtrlr.GetCurrentActionTargets(), global::PandoraSingleton<global::MissionManager>.Instance.coneTarget.transform, false);
						break;
					}
					case global::TargetingId.AREA:
					{
						global::UnityEngine.Vector3 vector;
						global::UnityEngine.Vector3 vector2;
						global::PandoraSingleton<global::MissionManager>.Instance.InitSphereTarget(this.unitCtrlr.transform, (float)this.unitCtrlr.CurrentAction.Radius, this.unitCtrlr.CurrentAction.TargetingId, out vector, out vector2);
						this.unitCtrlr.SetAoeTargets(this.unitCtrlr.GetCurrentActionTargets(), global::PandoraSingleton<global::MissionManager>.Instance.sphereTarget.transform, false);
						break;
					}
					}
				}
				this.damageApplied = false;
				for (int i = 0; i < this.unitCtrlr.defenders.Count; i++)
				{
					global::UnitController unitController = this.unitCtrlr.defenders[i];
					this.logBldr.Append(unitController.GetLogName());
					if (i < this.unitCtrlr.defenders.Count - 1)
					{
						this.logBldr.Append(",");
					}
					global::PandoraSingleton<global::MissionManager>.Instance.CamManager.AddLOSTarget(unitController.transform);
					unitController.attackerCtrlr = this.unitCtrlr;
					unitController.flyingLabel = skillData2.EffectTypeId.ToString();
					if (skillData2.WoundMax > 0)
					{
						int damage = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(this.unitCtrlr.CurrentAction.GetMinDamage(false), this.unitCtrlr.CurrentAction.GetMaxDamage(false) + 1);
						unitController.ComputeDirectWound(damage, skillData2.BypassArmor, null, false);
						this.damageApplied = true;
					}
				}
				global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this.unitCtrlr, global::CombatLogger.LogMessage.CURSE_TARGET, new string[]
				{
					this.unitCtrlr.CurrentAction.LocalizedName,
					this.logBldr.ToString()
				});
				string text = this.unitCtrlr.CurrentAction.fxData.SequenceId.ToLowerString();
				global::PandoraDebug.LogInfo(string.Concat(new object[]
				{
					"loading sequence ",
					text,
					" for curse ",
					skillId
				}), "uncategorised", null);
				global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence(text, this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
				global::PandoraSingleton<global::MissionManager>.Instance.StartCoroutine(this.PlayDefense());
			}
			else
			{
				this.hasCurse = false;
			}
		}
	}

	private global::System.Collections.IEnumerator PlayDefense()
	{
		yield return new global::UnityEngine.WaitForSeconds(0.9f);
		for (int i = 0; i < this.unitCtrlr.defenders.Count; i++)
		{
			this.unitCtrlr.defenders[i].PlayDefState(this.unitCtrlr.defenders[i].attackResultId, (this.unitCtrlr.defenders[i].unit.Status != global::UnitStateId.OUT_OF_ACTION) ? 0 : 1, this.unitCtrlr.defenders[i].unit.Status);
		}
		yield break;
	}

	public void Exit(int iTo)
	{
	}

	public void Update()
	{
		if (!this.hasCurse)
		{
			this.OnSeqDone();
		}
	}

	public void FixedUpdate()
	{
	}

	private void OnSeqDone()
	{
		for (int i = 0; i < this.unitCtrlr.defenders.Count; i++)
		{
			this.unitCtrlr.defenders[i].EndDefense();
		}
		this.unitCtrlr.StateMachine.ChangeState(10);
		if (this.unitCtrlr.currentSpellSuccess && this.unitCtrlr.currentSpellId != global::SkillId.NONE)
		{
			global::ZoneAoe.Spawn(this.unitCtrlr, this.unitCtrlr.GetAction(this.unitCtrlr.currentSpellId), null);
			this.unitCtrlr.currentSpellId = global::SkillId.NONE;
		}
	}

	public void LaunchFx()
	{
		global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.unitCtrlr.CurrentAction.fxData.LaunchFx, this.unitCtrlr, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
		this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_SPELL_CURSE, global::SkillId.NONE, global::UnitActionId.NONE);
		this.unitCtrlr.HitDefenders(this.unitCtrlr.transform.forward, this.damageApplied);
	}

	private global::UnitController unitCtrlr;

	private bool hasCurse;

	private global::System.Text.StringBuilder logBldr;

	private bool damageApplied;
}
