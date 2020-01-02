using System;
using Prometheus;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/LaunchSkillFx")]
[global::WellFired.USequencerFriendlyName("LaunchSkillFx")]
public class SeqLaunchSkillFx : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		if (focusedUnit.CurrentAction.fxData != null)
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(focusedUnit.CurrentAction.fxData.LaunchFx, focusedUnit, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
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
