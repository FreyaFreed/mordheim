using System;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/SetAtkData")]
[global::WellFired.USequencerFriendlyName("SetAtkData")]
public class SeqSetAtkData : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		if (global::UnityEngine.Application.loadedLevelName != "sequence")
		{
			return;
		}
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		focusedUnit.SetCurrentAction(global::SkillId.BASE_ATTACK);
		global::UnitController defenderCtrlr = focusedUnit.defenderCtrlr;
		if (this.defenderStartingStatus == global::UnitStateId.NONE)
		{
			defenderCtrlr.animator.Play(global::AnimatorIds.idle);
			defenderCtrlr.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			defenderCtrlr.unit.SetStatus(global::UnitStateId.NONE);
		}
		else
		{
			defenderCtrlr.animator.Play(global::AnimatorIds.kneeling_stunned);
			defenderCtrlr.animator.SetInteger(global::AnimatorIds.unit_state, 2);
			defenderCtrlr.unit.SetStatus(global::UnitStateId.STUNNED);
		}
		focusedUnit.attackResultId = this.attackResult;
		defenderCtrlr.unit.PreviousStatus = defenderCtrlr.unit.Status;
		defenderCtrlr.unit.SetStatus(this.defenderStatus);
		focusedUnit.criticalHit = this.nextHitCritical;
		focusedUnit.currentActionData.SetAction(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.atkTypeLabel), null);
		focusedUnit.currentActionData.SetActionOutcome(this.atkOutcomeLabel);
		defenderCtrlr.flyingLabel = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.atkFlyingLabel);
		defenderCtrlr.Hide(false, true, null);
		focusedUnit.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public global::AttackResultId attackResult;

	public global::UnitStateId defenderStatus;

	public global::UnitStateId defenderStartingStatus;

	public bool nextHitCritical;

	public string atkTypeLabel;

	public string atkOutcomeLabel;

	public string atkFlyingLabel;
}
