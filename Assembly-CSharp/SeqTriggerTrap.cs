using System;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/TriggerTrap")]
[global::WellFired.USequencerFriendlyName("TriggerTrap")]
public class SeqTriggerTrap : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		if (focusedUnit.activeTrigger != null)
		{
			focusedUnit.activeTrigger.Trigger(focusedUnit);
		}
		else if (focusedUnit.currentZoneAoe != null)
		{
			focusedUnit.EnterZoneAoeAnim();
		}
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}
}
