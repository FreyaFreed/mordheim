using System;
using WellFired;

[global::WellFired.USequencerFriendlyName("LaunchSearch")]
[global::WellFired.USequencerEvent("Mordheim/LaunchSearch")]
public class SeqLaunchSearch : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		if (focusedUnit != null)
		{
			focusedUnit.LaunchAction(global::UnitActionId.SEARCH, true, global::UnitStateId.NONE, focusedUnit.searchVariation);
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
