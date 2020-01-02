using System;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/LaunchCurseFx")]
[global::WellFired.USequencerFriendlyName("LaunchCurseFx")]
public class SeqLaunchCurseFx : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		global::SpellCurse spellCurse = (global::SpellCurse)focusedUnit.StateMachine.GetState(29);
		spellCurse.LaunchFx();
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}
}
