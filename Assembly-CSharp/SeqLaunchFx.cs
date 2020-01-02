using System;
using Prometheus;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerFriendlyName("LaunchFx")]
[global::WellFired.USequencerEvent("Mordheim/LaunchFx")]
public class SeqLaunchFx : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.fxName, global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public string fxName;
}
