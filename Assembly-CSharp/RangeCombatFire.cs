using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCombatFire : global::ICheapState
{
	public RangeCombatFire(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		if (this.unitCtrlr.CurrentAction.fxData != null && this.unitCtrlr.CurrentAction.fxData.HitFx != string.Empty)
		{
			this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].projectile.fxHitObstacle = this.unitCtrlr.CurrentAction.fxData.HitFx;
		}
		this.unitCtrlr.UpdateTargetsData();
		this.unitCtrlr.SetFixed(true);
		global::PandoraSingleton<global::MissionManager>.Instance.HideCombatCircles();
		this.targets.Clear();
		this.targetsPos.Clear();
		this.bodyParts.Clear();
		this.noHitCollisions.Clear();
		if (this.unitCtrlr.defenderCtrlr != null)
		{
			this.unitCtrlr.FaceTarget(this.unitCtrlr.defenderCtrlr.transform, false);
			for (int i = 0; i < this.unitCtrlr.defenders.Count; i++)
			{
				this.targets.Add(this.unitCtrlr.defenders[i]);
			}
		}
		for (int j = 0; j < this.targets.Count; j++)
		{
			this.unitCtrlr.defenderCtrlr = (global::UnitController)this.targets[j];
			this.SetTargetAttackResult();
		}
		for (int k = 0; k < this.unitCtrlr.destructTargets.Count; k++)
		{
			this.targets.Add(this.unitCtrlr.destructTargets[k]);
			this.SetDestructibleResult(this.unitCtrlr.destructTargets[k]);
		}
		this.autoDamages = this.unitCtrlr.unit.GetEnchantmentDamage(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, global::EnchantmentDmgTriggerId.ON_RANGE);
		this.autoDamages += this.unitCtrlr.unit.GetEnchantmentDamage(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, global::EnchantmentDmgTriggerId.ON_ATTACK);
		if (this.autoDamages > 0)
		{
			this.unitCtrlr.ComputeDirectWound(this.autoDamages, true, this.unitCtrlr, false);
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_ACTION_CONFIRM);
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		string sequence = string.Format("{0}_close_attack", (this.unitCtrlr.CurrentAction.SkillId != global::SkillId.BASE_SHOOT) ? "aiming" : "range");
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence(sequence, this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
	}

	void global::ICheapState.Exit(int iTo)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] is global::UnitController)
			{
				((global::UnitController)this.targets[i]).EndDefense();
			}
		}
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

	private void SetTargetAttackResult()
	{
		this.unitCtrlr.defenderCtrlr.currentActionData.Reset();
		this.unitCtrlr.defenderCtrlr.InitDefense(this.unitCtrlr, false);
		this.unitCtrlr.defenderCtrlr.beenShot = true;
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.AddLOSTarget(this.unitCtrlr.defenderCtrlr.transform);
		int roll = this.unitCtrlr.CurrentAction.GetRoll(false);
		if (!this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, roll, global::AttributeId.COMBAT_RANGE_HIT_ROLL, false, true, this.unitCtrlr.hitMod))
		{
			if (roll > 75)
			{
				this.unitCtrlr.hitMod += global::Constant.GetInt(global::ConstantId.HIT_ROLL_MOD);
			}
			else if (roll > 50)
			{
				this.unitCtrlr.hitMod += global::Constant.GetInt(global::ConstantId.HIT_ROLL_MOD) / 2;
			}
			this.unitCtrlr.attackResultId = global::AttackResultId.MISS;
			this.unitCtrlr.defenderCtrlr.attackResultId = global::AttackResultId.MISS;
			this.unitCtrlr.defenderCtrlr.flyingLabel = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_miss");
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, bool, string>(global::Notices.RETROACTION_TARGET_OUTCOME, this.unitCtrlr.defenderCtrlr, string.Empty, false, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_miss"));
		}
		else
		{
			this.unitCtrlr.hitMod = 0;
			this.unitCtrlr.ComputeWound();
		}
		global::UnityEngine.Transform transform = null;
		global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
		bool item = false;
		global::TargetData targetData = this.unitCtrlr.GetTargetData(this.unitCtrlr.defenderCtrlr);
		if (this.unitCtrlr.attackResultId < global::AttackResultId.HIT_NO_WOUND)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
			for (int i = 0; i < targetData.boneTargetRangeBlockingUnit.Count; i++)
			{
				if (!targetData.boneTargetRangeBlockingUnit[i].hitBone)
				{
					list.Add(targetData.boneTargetRangeBlockingUnit[i].hitPoint);
				}
			}
			if (list.Count == 0)
			{
				vector = global::PandoraSingleton<global::MissionManager>.Instance.CamManager.OrientOffset(this.unitCtrlr.defenderCtrlr.transform, this.missOffset);
				vector += this.unitCtrlr.defenderCtrlr.BonesTr[global::BoneId.RIG_HEAD].position;
				item = true;
			}
			else
			{
				vector = list[global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, list.Count)];
			}
		}
		else
		{
			global::BoneId boneIdTarget = this.unitCtrlr.CurrentAction.skillData.BoneIdTarget;
			if (boneIdTarget != global::BoneId.NONE)
			{
				for (int j = 0; j < this.unitCtrlr.defenderCtrlr.boneTargets.Count; j++)
				{
					if (this.unitCtrlr.defenderCtrlr.boneTargets[j].bone == boneIdTarget)
					{
						transform = this.unitCtrlr.defenderCtrlr.boneTargets[j].transform;
					}
				}
			}
			else
			{
				global::System.Collections.Generic.List<global::BoneTarget> list2 = new global::System.Collections.Generic.List<global::BoneTarget>();
				for (int k = 0; k < targetData.boneTargetRangeBlockingUnit.Count; k++)
				{
					if (targetData.boneTargetRangeBlockingUnit[k].hitBone)
					{
						list2.Add(this.unitCtrlr.defenderCtrlr.boneTargets[k]);
					}
				}
				transform = list2[global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, list2.Count)].transform;
			}
			vector = transform.position;
		}
		this.targetsPos.Add(vector);
		this.bodyParts.Add(transform);
		this.noHitCollisions.Add(item);
		if (this.unitCtrlr.IsPlayed() && !this.unitCtrlr.defenderCtrlr.IsPlayed() && this.unitCtrlr.transform.position.y - this.unitCtrlr.defenderCtrlr.transform.position.y >= 8.5f)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.RANGE_9M);
		}
	}

	private void SetDestructibleResult(global::Destructible dest)
	{
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.AddLOSTarget(dest.transform);
		this.unitCtrlr.ComputeDestructibleWound(dest);
		this.targetsPos.Add(dest.transform.position + global::UnityEngine.Vector3.up * 1f);
		this.bodyParts.Add(null);
		this.noHitCollisions.Add(false);
	}

	private void OnSeqDone()
	{
		if (this.autoDamages > 0)
		{
			this.unitCtrlr.attackResultId = global::AttackResultId.HIT;
			this.unitCtrlr.Hit();
			if (this.unitCtrlr.unit.Status == global::UnitStateId.OUT_OF_ACTION)
			{
				this.unitCtrlr.KillUnit();
			}
			this.unitCtrlr.StartCoroutine(this.Wait());
		}
		else
		{
			this.EndAction();
		}
	}

	private global::System.Collections.IEnumerator Wait()
	{
		yield return new global::UnityEngine.WaitForSeconds(1.5f);
		this.EndAction();
		yield break;
	}

	private void EndAction()
	{
		this.unitCtrlr.StateMachine.ChangeState(10);
	}

	public void WeaponAim()
	{
		if (this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot] != null && this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Item.TypeData.IsRange)
		{
			this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Aim();
		}
		if (this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + 1)] != null && this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + 1)].Item.TypeData.IsRange)
		{
			this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + 1)].Aim();
		}
	}

	public void ShootProjectile(int idx)
	{
		if (this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + idx)] != null && this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + idx)].Item.TypeData.IsRange)
		{
			this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + idx)].Shoot(this.unitCtrlr, this.targetsPos, this.targets, this.noHitCollisions, this.bodyParts, idx != 0);
		}
	}

	private const string SEQ_NAME = "{0}_close_attack";

	private const float CLOSE_DISTANCE = 20f;

	private global::UnitController unitCtrlr;

	public global::UnityEngine.Vector3 missOffset = new global::UnityEngine.Vector3(0.4f, 0.4f, 0f);

	public global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> targets = new global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour>();

	public global::System.Collections.Generic.List<global::UnityEngine.Vector3> targetsPos = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();

	public global::System.Collections.Generic.List<global::UnityEngine.Transform> bodyParts = new global::System.Collections.Generic.List<global::UnityEngine.Transform>();

	public global::System.Collections.Generic.List<bool> noHitCollisions = new global::System.Collections.Generic.List<bool>();

	private int autoDamages;
}
