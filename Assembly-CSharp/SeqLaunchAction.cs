using System;
using WellFired;

[global::WellFired.USequencerFriendlyName("LaunchAction")]
[global::WellFired.USequencerEvent("Mordheim/LaunchAction")]
public class SeqLaunchAction : global::WellFired.USEventBase
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
		unitController.LaunchAction(this.actionId, this.success, this.stateId, this.variation);
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public global::SequenceTargetId targetId;

	public global::UnitActionId actionId;

	public bool success;

	public int variation;

	public global::UnitStateId stateId;
}
