using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerFriendlyName("InitSpellDebug")]
[global::WellFired.USequencerEvent("Mordheim/InitSpellDebug")]
public class SeqInitSpellDebug : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		if (global::UnityEngine.Application.loadedLevelName != "sequence")
		{
			return;
		}
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		focusedUnit.SetCurrentAction(focusedUnit.actionStatus.First((global::ActionStatus x) => x.skillData.SkillTypeId == global::SkillTypeId.SPELL_ACTION && ((this.area && x.fxData.SequenceId == global::SequenceId.SPELL_AREA) || (!this.area && x.fxData.SequenceId == global::SequenceId.SPELL_POINT))).SkillId);
		global::UnitController defenderCtrlr = focusedUnit.defenderCtrlr;
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
		defenderCtrlr.unit.SetStatus(this.defenderStatus);
		focusedUnit.currentSpellTargetPosition = ((focusedUnit.CurrentAction.skillData.Range != 0) ? defenderCtrlr.BonesTr[global::BoneId.RIG_PELVIS].position : focusedUnit.BonesTr[global::BoneId.RIG_PELVIS].position);
		focusedUnit.defenders = new global::System.Collections.Generic.List<global::UnitController>();
		focusedUnit.defenders.Add(defenderCtrlr);
		focusedUnit.defenderCtrlr.attackResultId = this.result;
		global::SpellCasting spellCasting = (global::SpellCasting)focusedUnit.StateMachine.GetState(28);
		spellCasting.spellSuccess = this.success;
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public bool area;

	public bool success;

	public global::AttackResultId result;

	public global::UnitStateId defenderStatus;

	public global::UnitStateId defenderStartingStatus;

	public bool isCloseToTarget;
}
