using System;
using WellFired;

[global::WellFired.USequencerFriendlyName("DisplayDamage")]
[global::WellFired.USequencerEvent("Mordheim/DisplayDamage")]
public class SeqDisplayDamage : global::WellFired.USEventBase
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
			((global::FlyingLabel)fl).Play(unitCtrlr.BonesTr[global::BoneId.RIG_HEAD].position, true, "com_dash_value", new string[]
			{
				unitCtrlr.lastActionWounds.ToString()
			});
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
