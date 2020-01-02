using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombatAttack : global::ICheapState
{
	public CloseCombatAttack(global::UnitController ctrler)
	{
		this.unitCtrlr = ctrler;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.autoDamages = 0;
		this.unitCtrlr.SetFixed(true);
		global::PandoraSingleton<global::MissionManager>.Instance.HideCombatCircles();
		this.LaunchAttack();
	}

	void global::ICheapState.Exit(int iTo)
	{
		if (this.unitCtrlr.defenderCtrlr != null && this.unitCtrlr.defenderCtrlr != global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, int>(global::Notices.UPDATE_TARGET, this.unitCtrlr.defenderCtrlr, this.unitCtrlr.defenderCtrlr.unit.warbandIdx);
		}
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void LaunchAttack()
	{
		this.unitCtrlr.attackResultId = global::AttackResultId.MISS;
		this.unitCtrlr.criticalHit = false;
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		this.unitCtrlr.flyingLabel = string.Empty;
		this.unitCtrlr.lastActionWounds = 0;
		bool flag = true;
		bool flag2 = false;
		bool flag3 = false;
		this.unitCtrlr.seqData.actionSuccess = true;
		this.unitCtrlr.seqData.action = (int)this.unitCtrlr.CurrentAction.ActionId;
		if (this.unitCtrlr.seqData.action == 20 || this.unitCtrlr.seqData.action == 58)
		{
			this.unitCtrlr.seqData.action = 19;
		}
		this.unitCtrlr.seqData.attackVariation = 0;
		this.unitCtrlr.seqData.emoteVariation = 0;
		if (this.unitCtrlr.CurrentAction.skillData.SkillTypeId != global::SkillTypeId.BASE_ACTION && this.unitCtrlr.CurrentAction.skillData.Id != global::SkillId.BASE_COUNTER_ATTACK && this.unitCtrlr.CurrentAction.skillData.Id != global::SkillId.BASE_FLEE_ATTACK && this.unitCtrlr.CurrentAction.skillData.Id != global::SkillId.BASE_ATTACK_FREE && this.unitCtrlr.CurrentAction.skillData.Id != global::SkillId.BASE_AMBUSH_ATTACK && this.unitCtrlr.CurrentAction.skillData.Id != global::SkillId.BASE_CHARGE_FREE)
		{
			this.unitCtrlr.seqData.emoteVariation = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(1, 5);
		}
		this.targets.Clear();
		this.targets.AddRange(this.unitCtrlr.defenders);
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.unitCtrlr.defenderCtrlr = this.targets[i];
			this.unitCtrlr.defenderCtrlr.flyingLabel = string.Empty;
			this.unitCtrlr.defenderCtrlr.currentActionData.Reset();
			this.unitCtrlr.defenderCtrlr.attackResultId = global::AttackResultId.NONE;
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.AddLOSTarget(this.unitCtrlr.defenderCtrlr.transform);
			this.unitCtrlr.defenderCtrlr.InitDefense(this.unitCtrlr, true);
			if (this.unitCtrlr.defenderCtrlr.unit.IsAvailable())
			{
				int roll = this.unitCtrlr.CurrentAction.GetRoll(false);
				flag = this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, roll, global::AttributeId.COMBAT_MELEE_HIT_ROLL, false, true, this.unitCtrlr.hitMod);
				if (!flag)
				{
					if (roll > 75)
					{
						this.unitCtrlr.hitMod += global::Constant.GetInt(global::ConstantId.HIT_ROLL_MOD);
					}
					else if (roll > 50)
					{
						this.unitCtrlr.hitMod += global::Constant.GetInt(global::ConstantId.HIT_ROLL_MOD) / 2;
					}
					this.unitCtrlr.defenderCtrlr.attackResultId = global::AttackResultId.MISS;
					this.unitCtrlr.defenderCtrlr.flyingLabel = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_miss");
					global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, bool, string>(global::Notices.RETROACTION_TARGET_OUTCOME, this.unitCtrlr.defenderCtrlr, string.Empty, false, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_miss"));
				}
			}
			else
			{
				global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this.unitCtrlr, global::CombatLogger.LogMessage.STUNNED_HIT, new string[]
				{
					this.unitCtrlr.defenderCtrlr.GetLogName()
				});
			}
			if (flag)
			{
				this.unitCtrlr.hitMod = 0;
				this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_MELEE_HIT_SUCCESS, global::SkillId.NONE, global::UnitActionId.NONE);
				if (this.unitCtrlr.CurrentAction.ActionId == global::UnitActionId.AMBUSH || this.unitCtrlr.CurrentAction.ActionId == global::UnitActionId.CHARGE)
				{
					this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_CHARGE_SUCCESS, global::SkillId.NONE, global::UnitActionId.NONE);
				}
			}
			if (flag && this.unitCtrlr.unit.DodgeBypass <= 0 && this.unitCtrlr.defenderCtrlr.unit.IsAvailable() && this.unitCtrlr.defenderCtrlr.unit.DodgeLeft > 0)
			{
				int num = this.unitCtrlr.defenderCtrlr.unit.DodgeRoll + this.unitCtrlr.unit.DodgeDefenderModifier;
				num = global::UnityEngine.Mathf.Clamp(num, 1, global::Constant.GetInt(global::ConstantId.MAX_ROLL));
				flag2 = this.unitCtrlr.defenderCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, num, global::AttributeId.DODGE_ROLL, false, true, 0);
				this.unitCtrlr.defenderCtrlr.unit.ConsumeEnchantments(global::EnchantmentConsumeId.DODGE);
				if (flag2)
				{
					this.unitCtrlr.defenderCtrlr.unit.UpdateValidNextActionEnchantments();
					this.unitCtrlr.defenderCtrlr.attackResultId = global::AttackResultId.DODGE;
					this.unitCtrlr.defenderCtrlr.currentActionData.SetActionOutcome("act_effect_dodged");
				}
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, bool, string>(global::Notices.RETROACTION_TARGET_OUTCOME, this.unitCtrlr.defenderCtrlr, (!flag2) ? global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_fail_param", new string[]
				{
					"#retro_outcome_dodge"
				}) : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_dodge"), flag2, (!flag2) ? string.Empty : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_dodge"));
			}
			if (flag && this.unitCtrlr.unit.ParryBypass <= 0 && !flag2 && this.unitCtrlr.defenderCtrlr.unit.IsAvailable() && this.unitCtrlr.defenderCtrlr.unit.ParryLeft > 0)
			{
				int num2 = this.unitCtrlr.defenderCtrlr.unit.ParryingRoll + this.unitCtrlr.unit.ParryDefenderModifier;
				num2 = global::UnityEngine.Mathf.Clamp(num2, 1, global::Constant.GetInt(global::ConstantId.MAX_ROLL));
				flag3 = this.unitCtrlr.defenderCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, num2, global::AttributeId.PARRYING_ROLL, false, true, 0);
				this.unitCtrlr.defenderCtrlr.unit.ConsumeEnchantments(global::EnchantmentConsumeId.PARRY);
				if (flag3)
				{
					this.unitCtrlr.seqData.actionSuccess = false;
					this.unitCtrlr.defenderCtrlr.unit.UpdateValidNextActionEnchantments();
					this.unitCtrlr.defenderCtrlr.attackResultId = global::AttackResultId.PARRY;
					this.unitCtrlr.defenderCtrlr.currentActionData.SetActionOutcome("act_effect_dodged");
				}
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, bool, string>(global::Notices.RETROACTION_TARGET_OUTCOME, this.unitCtrlr.defenderCtrlr, (!flag3) ? global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_fail_param", new string[]
				{
					"#retro_outcome_parry"
				}) : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_parry"), flag3, (!flag3) ? string.Empty : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_parry"));
			}
			if (flag && !flag2 && !flag3)
			{
				this.unitCtrlr.ComputeWound();
			}
			this.unitCtrlr.seqData.attackVariation = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, 2);
		}
		for (int j = 0; j < this.unitCtrlr.destructTargets.Count; j++)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.AddLOSTarget(this.unitCtrlr.destructTargets[j].transform);
			this.unitCtrlr.ComputeDestructibleWound(this.unitCtrlr.destructTargets[j]);
		}
		if (this.unitCtrlr.CurrentAction.fxData != null && this.unitCtrlr.CurrentAction.fxData.OverrideVariation)
		{
			this.unitCtrlr.seqData.attackVariation = this.unitCtrlr.CurrentAction.fxData.Variation;
		}
		else if (this.unitCtrlr.unit.Id == global::UnitId.MANTICORE)
		{
			global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
			for (int k = 0; k < this.targets.Count; k++)
			{
				vector += this.targets[k].transform.position;
			}
			vector /= (float)this.targets.Count;
			vector -= this.unitCtrlr.transform.position;
			float num3 = global::UnityEngine.Vector3.Dot(this.unitCtrlr.transform.forward, vector);
			int num4 = (num3 >= 0f) ? 0 : 1;
			num3 = global::UnityEngine.Vector3.Dot(this.unitCtrlr.transform.right, vector);
			num4 += ((num3 >= 0f) ? 0 : 2);
			this.unitCtrlr.seqData.attackVariation = num4;
		}
		this.autoDamages = this.unitCtrlr.unit.GetEnchantmentDamage(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, global::EnchantmentDmgTriggerId.ON_MELEE);
		this.autoDamages += this.unitCtrlr.unit.GetEnchantmentDamage(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, global::EnchantmentDmgTriggerId.ON_ATTACK);
		if (this.autoDamages > 0)
		{
			this.unitCtrlr.ComputeDirectWound(this.autoDamages, true, this.unitCtrlr, false);
		}
		this.unitCtrlr.defenders.Clear();
		this.unitCtrlr.defenders.AddRange(this.targets);
		string sequence = "attack_right" + ((!flag3) ? string.Empty : "_fail");
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence(sequence, this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
		this.unitCtrlr.StartCoroutine(this.WaitToShowOutcome());
	}

	private global::System.Collections.IEnumerator WaitToShowOutcome()
	{
		yield return new global::UnityEngine.WaitForSeconds(0.5f);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.RETROACTION_SHOW_OUTCOME, this.unitCtrlr);
		yield break;
	}

	private void OnSeqDone()
	{
		if (this.autoDamages > 0)
		{
			this.unitCtrlr.attackResultId = global::AttackResultId.HIT;
			this.unitCtrlr.Hit();
			this.unitCtrlr.StartCoroutine(this.WaitForHitAnim());
		}
		else
		{
			this.EndAttack();
		}
	}

	private global::System.Collections.IEnumerator WaitForHitAnim()
	{
		yield return new global::UnityEngine.WaitForSeconds(1.5f);
		this.EndAttack();
		yield break;
	}

	private void EndAttack()
	{
		this.unitCtrlr.attackUsed++;
		for (int i = 0; i < this.unitCtrlr.defenders.Count; i++)
		{
			this.unitCtrlr.defenders[i].EndDefense();
		}
		if (this.unitCtrlr.unit.Status == global::UnitStateId.OUT_OF_ACTION)
		{
			this.unitCtrlr.KillUnit();
			if (this.unitCtrlr == global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit())
			{
				this.unitCtrlr.StateMachine.ChangeState(39);
				return;
			}
		}
		if (this.unitCtrlr == global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() && this.unitCtrlr.attackUsed < this.unitCtrlr.unit.AttackPerAction && this.unitCtrlr.defenderCtrlr.unit.Status != global::UnitStateId.OUT_OF_ACTION)
		{
			this.LaunchAttack();
		}
		else
		{
			if (this.unitCtrlr == global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() && this.unitCtrlr.defenderCtrlr != null && this.unitCtrlr.defenderCtrlr.unit.IsAvailable() && !this.unitCtrlr.defenderCtrlr.Resurected && this.unitCtrlr.defenders.Count == 1)
			{
				this.unitCtrlr.defenderCtrlr.UpdateActionStatus(false, global::UnitActionRefreshId.NONE);
				if (this.unitCtrlr.defenderCtrlr.CanCounterAttack())
				{
					this.unitCtrlr.defenderCtrlr.StateMachine.ChangeState(34);
					this.unitCtrlr.WaitForAction(global::UnitController.State.START_MOVE);
					if (this.unitCtrlr.IsPlayed())
					{
						global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.UNIT_START_SINGLE_TARGETING);
					}
					return;
				}
			}
			if (this.unitCtrlr.defenderCtrlr != null && this.unitCtrlr.defenderCtrlr.unit.Status != global::UnitStateId.OUT_OF_ACTION && this.unitCtrlr.defenderCtrlr.Fleeing)
			{
				this.unitCtrlr.ActionDone();
			}
			else
			{
				this.unitCtrlr.StateMachine.ChangeState(10);
			}
		}
	}

	private global::UnitController unitCtrlr;

	private global::System.Collections.Generic.List<global::UnitController> targets = new global::System.Collections.Generic.List<global::UnitController>();

	private int autoDamages;
}
