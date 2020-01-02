using System;
using WellFired;

[global::WellFired.USequencerFriendlyName("PlayDefense")]
[global::WellFired.USequencerEvent("Mordheim/PlayDefense")]
public class SeqPlayDefense : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		focusedUnit.PlayDefState(focusedUnit.attackResultId, (focusedUnit.unit.Status != global::UnitStateId.OUT_OF_ACTION) ? 0 : 1, focusedUnit.unit.Status);
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}
}
