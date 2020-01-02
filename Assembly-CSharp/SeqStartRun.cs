using System;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerFriendlyName("StartRun")]
[global::WellFired.USequencerEvent("Mordheim/StartRun")]
public class SeqStartRun : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController unitController = null;
		global::SequenceTargetId sequenceTargetId = this.targetId;
		if (sequenceTargetId != global::SequenceTargetId.FOCUSED_UNIT)
		{
			if (sequenceTargetId == global::SequenceTargetId.DEFENDER)
			{
				unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.defenderCtrlr;
			}
		}
		else
		{
			unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		}
		unitController.SetAnimSpeed(1f);
		unitController.SetFixed(false);
		unitController.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = false;
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public global::SequenceTargetId targetId;
}
