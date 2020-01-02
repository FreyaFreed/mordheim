using System;
using System.Collections.Generic;
using Prometheus;
using UnityEngine;
using UnityEngine.Events;

public class SpellCasting : global::ICheapState
{
	public SpellCasting(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.damageApplied = false;
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.FaceTarget(this.unitCtrlr.currentSpellTargetPosition, false);
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		int roll = this.unitCtrlr.CurrentAction.GetRoll(false);
		if (this.unitCtrlr.CurrentAction.skillData.AutoSuccess)
		{
			this.spellSuccess = true;
		}
		else
		{
			this.spellSuccess = this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, roll, global::AttributeId.SPELLCASTING_ROLL, false, true, 0);
		}
		this.unitCtrlr.currentActionData.SetActionOutcome(this.spellSuccess);
		if (this.spellSuccess)
		{
			this.defaultTarget = this.unitCtrlr.defenderCtrlr;
			for (int i = this.unitCtrlr.defenders.Count - 1; i >= 0; i--)
			{
				global::UnitController unitController = this.unitCtrlr.defenders[i];
				unitController.beenShot = true;
				global::PandoraSingleton<global::MissionManager>.Instance.CamManager.AddLOSTarget(unitController.transform);
				unitController.attackerCtrlr = this.unitCtrlr;
				unitController.flyingLabel = string.Empty;
				bool flag = false;
				if (!this.unitCtrlr.CurrentAction.skillData.BypassMagicResist)
				{
					int target = unitController.unit.MagicResistance + this.unitCtrlr.unit.MagicResistDefenderModifier;
					flag = unitController.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, target, global::AttributeId.MAGIC_RESISTANCE, false, true, 0);
					global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, bool, string>(global::Notices.RETROACTION_TARGET_OUTCOME, unitController, (!flag) ? global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_fail_param", new string[]
					{
						"#retro_outcome_resist"
					}) : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_resist"), flag, (!flag) ? string.Empty : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_resist"));
				}
				if (!flag)
				{
					unitController.flyingLabel = this.unitCtrlr.CurrentAction.skillData.EffectTypeId.ToString();
					unitController.attackResultId = global::AttackResultId.NONE;
					if (this.unitCtrlr.CurrentAction.skillData.WoundMax > 0)
					{
						int damage = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(this.unitCtrlr.CurrentAction.GetMinDamage(false), this.unitCtrlr.CurrentAction.GetMaxDamage(false) + 1);
						unitController.ComputeDirectWound(damage, this.unitCtrlr.CurrentAction.skillData.BypassArmor, this.unitCtrlr, false);
						this.damageApplied = true;
					}
				}
				else
				{
					this.unitCtrlr.defenders.RemoveAt(i);
				}
			}
			for (int j = 0; j < this.unitCtrlr.destructTargets.Count; j++)
			{
				this.unitCtrlr.ComputeDestructibleWound(this.unitCtrlr.destructTargets[j]);
			}
		}
		string text = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SequenceData>((int)this.unitCtrlr.CurrentAction.fxData.SequenceId).Name;
		if (!this.spellSuccess)
		{
			text += "_fizzle";
		}
		else if (this.unitCtrlr.unit.Status == global::UnitStateId.OUT_OF_ACTION)
		{
			text += "_ooa";
		}
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence(text, this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
		if (this.nextFrameHit)
		{
			this.nextFrameHit = false;
			this.ProjectileHit();
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void OnSeqDone()
	{
		if (this.spellSuccess && this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.SPELLS_CAST, 1);
		}
		for (int i = 0; i < this.unitCtrlr.defenders.Count; i++)
		{
			this.unitCtrlr.defenders[i].EndDefense();
		}
		this.copyDefenders.Clear();
		this.copyDefenders.AddRange(this.unitCtrlr.defenders);
		for (int j = 0; j < this.copyDefenders.Count; j++)
		{
			if (this.copyDefenders[j].Engaged)
			{
				this.copyDefenders[j].defenders.Clear();
				this.copyDefenders[j].defenders.AddRange(this.copyDefenders[j].EngagedUnits);
			}
			else
			{
				this.copyDefenders[j].defenders.Clear();
			}
			this.copyDefenders[j].TriggerEnchantments(global::EnchantmentTriggerId.ON_SPELL_RECEIVED, global::SkillId.NONE, global::UnitActionId.NONE);
		}
		this.unitCtrlr.currentSpellTypeId = this.unitCtrlr.CurrentAction.skillData.SpellTypeId;
		this.unitCtrlr.currentSpellId = this.unitCtrlr.CurrentAction.SkillId;
		this.unitCtrlr.currentSpellSuccess = this.spellSuccess;
		if (this.unitCtrlr.CurrentAction.skillData.AutoSuccess)
		{
			this.unitCtrlr.StateMachine.ChangeState(10);
		}
		else if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
		{
			this.unitCtrlr.SendCurse();
		}
		else
		{
			this.unitCtrlr.StateMachine.ChangeState(9);
		}
	}

	public void ShootProjectile()
	{
		if (this.spellSuccess)
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.unitCtrlr.CurrentAction.fxData.LaunchFx, this.unitCtrlr, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
			global::UnitController unitController = this.unitCtrlr;
			global::UnitController endUnitCtrlr = null;
			if (this.unitCtrlr.CurrentAction.TargetingId == global::TargetingId.SINGLE_TARGET)
			{
				global::UnitController unitController2 = (!(this.unitCtrlr.defenderCtrlr != null)) ? this.defaultTarget : this.unitCtrlr.defenderCtrlr;
				if (unitController2 != null)
				{
					if (this.unitCtrlr.CurrentAction.fxData.ProjFromTarget)
					{
						unitController = unitController2;
					}
					endUnitCtrlr = unitController2;
				}
			}
			global::UnityEngine.Vector3 startPos = (this.unitCtrlr.CurrentAction.fxData.SequenceId != global::SequenceId.SPELL_POINT) ? (-this.unitCtrlr.transform.position) : (-(this.unitCtrlr.BonesTr[global::BoneId.RIG_LARMPALM].position + this.unitCtrlr.BonesTr[global::BoneId.RIG_RARMPALM].position) / 2f);
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.unitCtrlr.CurrentAction.fxData.ProjectileFx, unitController, endUnitCtrlr, delegate(global::UnityEngine.GameObject fx)
			{
				this.projectileFx = fx;
				if (this.projectileFx == null)
				{
					this.nextFrameHit = true;
				}
			}, startPos, this.unitCtrlr.currentSpellTargetPosition, new global::UnityEngine.Events.UnityAction(this.ProjectileHit));
		}
		else
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.unitCtrlr.CurrentAction.fxData.FizzleFx, this.unitCtrlr, null, delegate(global::UnityEngine.GameObject fx)
			{
				if (fx != null && this.unitCtrlr.CurrentAction.fxData.SequenceId == global::SequenceId.SPELL_POINT)
				{
					fx.transform.LookAt(this.unitCtrlr.currentSpellTargetPosition);
				}
			}, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
		}
	}

	private void ProjectileHit()
	{
		if (this.spellSuccess)
		{
			this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_SPELL_IMPACT, global::SkillId.NONE, global::UnitActionId.NONE);
			this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_SPELL_IMPACT_RANDOM, global::SkillId.NONE, global::UnitActionId.NONE);
			if (this.damageApplied)
			{
				this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_SPELL_IMPACT_DMG, global::SkillId.NONE, global::UnitActionId.NONE);
			}
			for (int i = 0; i < this.unitCtrlr.defenders.Count; i++)
			{
				this.unitCtrlr.defenders[i].TriggerAlliesEnchantments();
			}
		}
		global::UnityEngine.Vector3 projDir = (!(this.projectileFx != null)) ? this.unitCtrlr.transform.forward : this.projectileFx.transform.forward;
		projDir.y = 0f;
		if (this.unitCtrlr.destructibleTarget != null)
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.unitCtrlr.CurrentAction.fxData.ImpactFx, this.unitCtrlr.destructibleTarget.transform, false, new global::UnityEngine.Events.UnityAction<global::UnityEngine.GameObject>(this.SetImpactFx));
		}
		else if (this.unitCtrlr.defenderCtrlr != null)
		{
			global::UnitController defenderCtrlr = this.unitCtrlr;
			if (this.unitCtrlr.CurrentAction.TargetingId == global::TargetingId.SINGLE_TARGET)
			{
				if (this.unitCtrlr.defenders.Count > 0)
				{
					defenderCtrlr = this.unitCtrlr.defenderCtrlr;
				}
				else if (this.defaultTarget != null)
				{
					defenderCtrlr = this.defaultTarget;
				}
			}
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.unitCtrlr.CurrentAction.fxData.ImpactFx, defenderCtrlr, null, new global::UnityEngine.Events.UnityAction<global::UnityEngine.GameObject>(this.SetImpactFx), default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
		}
		else
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.unitCtrlr.CurrentAction.fxData.ImpactFx, null, null, new global::UnityEngine.Events.UnityAction<global::UnityEngine.GameObject>(this.SetImpactFx), default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
		}
		if (this.projectileFx != null)
		{
			global::UnityEngine.Object.Destroy(this.projectileFx);
		}
		this.unitCtrlr.HitDefenders(projDir, true);
		for (int j = 0; j < this.unitCtrlr.destructTargets.Count; j++)
		{
			this.unitCtrlr.destructTargets[j].Hit(this.unitCtrlr);
		}
		if (this.unitCtrlr.CurrentAction.ZoneAoeId != global::ZoneAoeId.NONE && this.unitCtrlr.CurrentAction.skillData.AutoSuccess)
		{
			global::ZoneAoe.Spawn(this.unitCtrlr, this.unitCtrlr.CurrentAction, null);
		}
		if (this.spellSuccess && this.unitCtrlr.CurrentAction.skillData.DestructibleId != global::DestructibleId.NONE)
		{
			global::Destructible.Spawn(this.unitCtrlr.CurrentAction.skillData.DestructibleId, this.unitCtrlr, this.unitCtrlr.currentSpellTargetPosition, -1);
		}
	}

	private void SetImpactFx(global::UnityEngine.GameObject impactFx)
	{
		if (impactFx != null && this.unitCtrlr.CurrentAction.TargetingId != global::TargetingId.SINGLE_TARGET && this.unitCtrlr.CurrentAction.RangeMax > 0)
		{
			impactFx.transform.position = this.unitCtrlr.currentSpellTargetPosition;
			impactFx.transform.rotation = global::UnityEngine.Quaternion.identity;
		}
	}

	private const float CLOSE_DISTANCE = 20f;

	private global::UnitController unitCtrlr;

	private global::UnityEngine.GameObject projectileFx;

	private bool nextFrameHit;

	private global::UnitController defaultTarget;

	private global::System.Collections.Generic.List<global::UnitController> copyDefenders = new global::System.Collections.Generic.List<global::UnitController>();

	private bool damageApplied;

	public bool spellSuccess;
}
