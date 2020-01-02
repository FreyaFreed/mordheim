using System;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerFriendlyName("InitRangeDebug")]
[global::WellFired.USequencerEvent("Mordheim/InitRangeDebug")]
public class SeqInitRangeDebug : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		if (global::UnityEngine.Application.loadedLevelName != "sequence")
		{
			return;
		}
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		focusedUnit.SetCurrentAction(global::SkillId.BASE_SHOOT);
		focusedUnit.Equipments[4].Reload();
		if (focusedUnit.Equipments[5] != null)
		{
			focusedUnit.Equipments[5].Reload();
		}
		focusedUnit.SwitchWeapons(global::UnitSlotId.SET2_MAINHAND);
		if (this.defenderStartingStatus == global::UnitStateId.NONE)
		{
			focusedUnit.defenderCtrlr.animator.Play(global::AnimatorIds.idle);
			focusedUnit.defenderCtrlr.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			focusedUnit.defenderCtrlr.unit.SetStatus(global::UnitStateId.NONE);
		}
		else
		{
			focusedUnit.defenderCtrlr.animator.Play(global::AnimatorIds.kneeling_stunned);
			focusedUnit.defenderCtrlr.animator.SetInteger(global::AnimatorIds.unit_state, 2);
			focusedUnit.defenderCtrlr.unit.SetStatus(global::UnitStateId.STUNNED);
		}
		focusedUnit.attackResultId = this.atkResult;
		focusedUnit.currentActionData.SetAction(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.atkTypeLabel), null);
		focusedUnit.currentActionData.SetActionOutcome(this.atkOutcomeLabel);
		focusedUnit.defenderCtrlr.unit.SetStatus(this.defenderStatus);
		focusedUnit.defenderCtrlr.flyingLabel = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.atkFlyingLabel);
		global::RangeCombatFire rangeCombatFire = (global::RangeCombatFire)focusedUnit.StateMachine.GetState(31);
		rangeCombatFire.bodyParts.Clear();
		rangeCombatFire.noHitCollisions.Clear();
		rangeCombatFire.targetsPos.Clear();
		rangeCombatFire.bodyParts.Add(focusedUnit.defenderCtrlr.BonesTr[this.targetBone]);
		rangeCombatFire.noHitCollisions.Add(this.noCollision);
		if (!this.noCollision)
		{
			rangeCombatFire.targetsPos.Add(rangeCombatFire.bodyParts[0].transform.position);
		}
		else if (this.missTarget != null)
		{
			rangeCombatFire.targetsPos.Add(this.missTarget.transform.position);
		}
		else
		{
			rangeCombatFire.targetsPos.Add(focusedUnit.defenderCtrlr.BonesTr[global::BoneId.RIG_HEAD].position + rangeCombatFire.missOffset);
		}
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public global::AttackResultId atkResult;

	public global::UnitStateId defenderStatus;

	public global::UnitStateId defenderStartingStatus;

	public string atkOutcomeLabel;

	public string atkFlyingLabel;

	public string atkTypeLabel;

	public global::BoneId targetBone;

	public bool noCollision;

	public global::UnityEngine.Transform missTarget;

	public bool isCloseToTarget;
}
