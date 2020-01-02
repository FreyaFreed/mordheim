using System;
using WellFired;

[global::WellFired.USequencerFriendlyName("TriggerTrapUnitAction")]
[global::WellFired.USequencerEvent("Mordheim/TriggerTrapUnitAction")]
public class SeqTriggerTrapUnitAction : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		focusedUnit.activeTrigger.ActionOnUnit(focusedUnit);
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}
}
