using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/InitRecoveryDebug")]
[global::WellFired.USequencerFriendlyName("InitRecoveryDebug")]
public class SeqInitRecoveryDebug : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		if (global::UnityEngine.Application.loadedLevelName != "sequence")
		{
			return;
		}
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::System.Collections.Generic.List<global::WarbandController>, float>(global::Notices.GAME_WARBAND_MORAL, global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs, global::Constant.GetFloat(global::ConstantId.ROUT_RATIO_ALIVE));
		if (this.startingState == global::UnitStateId.STUNNED)
		{
			focusedUnit.animator.Play(global::AnimatorIds.kneeling_stunned);
			focusedUnit.animator.SetInteger(global::AnimatorIds.unit_state, 2);
			focusedUnit.unit.SetStatus(global::UnitStateId.STUNNED);
		}
		else
		{
			focusedUnit.animator.Play(global::AnimatorIds.idle);
			focusedUnit.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			focusedUnit.unit.SetStatus(global::UnitStateId.NONE);
		}
		focusedUnit.recoveryTarget = this.recoveryTarget;
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public global::AttributeId recoveryAttributedId;

	public int recoveryTarget;

	public int recoveryDamage;

	public bool recoverySuccess;

	public global::UnitStateId startingState;
}
