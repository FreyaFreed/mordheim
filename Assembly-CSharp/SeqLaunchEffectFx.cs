using System;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/LaunchEffectFx")]
[global::WellFired.USequencerFriendlyName("LaunchEffectFx")]
public class SeqLaunchEffectFx : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		global::UpdateEffects updateEffects = (global::UpdateEffects)focusedUnit.StateMachine.GetState(3);
		updateEffects.TriggerEffect();
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}
}
