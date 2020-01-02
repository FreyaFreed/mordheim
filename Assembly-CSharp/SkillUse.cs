using System;
using System.Collections;
using System.Collections.Generic;
using Prometheus;
using UnityEngine;
using UnityEngine.Events;

public class SkillUse : global::ICheapState
{
	public SkillUse(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.currentActionData.Reset();
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		this.unitCtrlr.flyingLabel = string.Empty;
		this.unitCtrlr.searchVariation = 0;
		this.projectileShot = false;
		if (this.unitCtrlr.CurrentAction.LinkedItem != null && this.unitCtrlr.CurrentAction.LinkedItem.ConsumableData.ConsumeItem)
		{
			this.unitCtrlr.unit.RemoveItem(this.unitCtrlr.CurrentAction.LinkedItem);
		}
		string sequence = string.Empty;
		if (this.unitCtrlr.CurrentAction.fxData != null && this.unitCtrlr.CurrentAction.fxData.SequenceId != global::SequenceId.NONE)
		{
			sequence = this.unitCtrlr.CurrentAction.fxData.SequenceId.ToLowerString();
		}
		else if (this.unitCtrlr.CurrentAction.ActionId == global::UnitActionId.CONSUMABLE)
		{
			sequence = "consumable";
		}
		else
		{
			sequence = "skill";
			this.unitCtrlr.StopCoroutine(this.ApplyVisualEffect());
			this.unitCtrlr.StartCoroutine(this.ApplyVisualEffect());
		}
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence(sequence, this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
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

	private global::System.Collections.IEnumerator ApplyVisualEffect()
	{
		yield return new global::UnityEngine.WaitForSeconds(0.5f);
		this.unitCtrlr.HitDefenders(this.unitCtrlr.transform.forward, false);
		yield break;
	}

	public void OnInteract()
	{
		if (this.unitCtrlr.CurrentAction.skillData.TrapTypeId != global::TrapTypeId.NONE)
		{
			global::Trap.SpawnTrap(this.unitCtrlr.CurrentAction.skillData.TrapTypeId, this.unitCtrlr.GetWarband().teamIdx, this.unitCtrlr.transform.position, this.unitCtrlr.transform.rotation, null, true);
			this.unitCtrlr.LaunchAction(global::UnitActionId.SEARCH, true, global::UnitStateId.NONE, 1);
		}
	}

	public void ShootProjectile()
	{
		this.projectileShot = true;
		global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.unitCtrlr.CurrentAction.fxData.LaunchFx, this.unitCtrlr, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
		global::UnityEngine.Vector3 position = this.unitCtrlr.BonesTr[global::BoneId.RIG_RARMPALM].position;
		global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.unitCtrlr.CurrentAction.fxData.ProjectileFx, null, null, delegate(global::UnityEngine.GameObject fx)
		{
			this.projectileFx = fx;
		}, position, this.unitCtrlr.currentSpellTargetPosition, new global::UnityEngine.Events.UnityAction(this.OnProjectileHit));
	}

	public void OnProjectileHit()
	{
		this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_SKILL_IMPACT, global::SkillId.NONE, global::UnitActionId.NONE);
		this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_SKILL_IMPACT_RANDOM, global::SkillId.NONE, global::UnitActionId.NONE);
		global::UnityEngine.Object.Destroy(this.projectileFx);
		global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.unitCtrlr.CurrentAction.fxData.ImpactFx, null, null, delegate(global::UnityEngine.GameObject fx)
		{
			if (fx != null)
			{
				fx.transform.position = this.unitCtrlr.currentSpellTargetPosition;
				fx.transform.rotation = global::UnityEngine.Quaternion.identity;
			}
		}, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
		global::PandoraSingleton<global::SequenceManager>.Instance.EndSequence();
	}

	public void OnSeqDone()
	{
		if (this.unitCtrlr.CurrentAction.ActionId == global::UnitActionId.CONSUMABLE || this.projectileShot)
		{
			this.unitCtrlr.HitDefenders(global::UnityEngine.Vector3.zero, true);
		}
		global::ZoneAoe.Spawn(this.unitCtrlr, this.unitCtrlr.CurrentAction, null);
		global::System.Collections.Generic.List<global::SkillPerformSkillData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillPerformSkillData>("fk_skill_id", ((int)this.unitCtrlr.CurrentAction.SkillId).ToConstantString());
		global::SkillId skillId = global::SkillId.NONE;
		if (this.unitCtrlr.defenderCtrlr != null && list != null && list.Count > 0 && list[0].SkillIdPerformed != global::SkillId.NONE)
		{
			skillId = list[0].SkillIdPerformed;
			if (list[0].AttributeIdRoll != global::AttributeId.NONE)
			{
				bool flag = this.unitCtrlr.defenderCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, list[0].AttributeIdRoll, false, true);
				if (list[0].Success != flag)
				{
					skillId = global::SkillId.NONE;
				}
			}
		}
		if (this.unitCtrlr.CurrentAction.skillData.DestructibleId != global::DestructibleId.NONE)
		{
			global::Destructible.Spawn(this.unitCtrlr.CurrentAction.skillData.DestructibleId, this.unitCtrlr, this.unitCtrlr.currentSpellTargetPosition, -1);
		}
		if (skillId != global::SkillId.NONE)
		{
			global::ActionStatus action = this.unitCtrlr.defenderCtrlr.GetAction(skillId);
			global::TargetingId targetingId = action.TargetingId;
			if (targetingId != global::TargetingId.NONE)
			{
				if (targetingId == global::TargetingId.SINGLE_TARGET)
				{
					if (action.Targets.Count > 0)
					{
						int num = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, action.Targets.Count);
						if (this.unitCtrlr.CurrentAction.SkillId == global::SkillId.THREATEN || this.unitCtrlr.CurrentAction.SkillId == global::SkillId.THREATEN_MSTR)
						{
							int num2 = action.Targets.IndexOf(this.unitCtrlr);
							num = ((num2 == -1) ? num : num2);
						}
						this.unitCtrlr.defenderCtrlr.SkillSingleTargetRPC((int)skillId, action.Targets[num].uid);
					}
					else if (action.Destructibles.Count > 0)
					{
						int index = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, action.Destructibles.Count);
						this.unitCtrlr.defenderCtrlr.SendSkillSingleDestructible(skillId, action.Destructibles[index]);
					}
				}
			}
			else
			{
				this.unitCtrlr.defenderCtrlr.SkillRPC((int)list[0].SkillIdPerformed);
			}
			if (this.unitCtrlr != this.unitCtrlr.defenderCtrlr)
			{
				this.unitCtrlr.WaitForAction(global::UnitController.State.START_MOVE);
			}
		}
		else
		{
			this.unitCtrlr.StateMachine.ChangeState(10);
		}
	}

	private global::UnitController unitCtrlr;

	private global::UnityEngine.GameObject projectileFx;

	private bool projectileShot;
}
