using System;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/DisplayFlyingText")]
[global::WellFired.USequencerFriendlyName("DisplayFlyingText")]
public class SeqDisplayFlyingText : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController unitCtrlr = null;
		global::SequenceTargetId sequenceTargetId = this.targetId;
		if (sequenceTargetId != global::SequenceTargetId.FOCUSED_UNIT)
		{
			if (sequenceTargetId == global::SequenceTargetId.DEFENDER)
			{
				unitCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.defenderCtrlr;
			}
		}
		else
		{
			unitCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		}
		global::PandoraSingleton<global::FlyingTextManager>.Instance.GetFlyingText(global::FlyingTextId.ACTION, delegate(global::FlyingText fl)
		{
			((global::FlyingLabel)fl).Play(unitCtrlr.BonesTr[global::BoneId.RIG_HEAD].position, false, unitCtrlr.flyingLabel, new string[0]);
		});
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
