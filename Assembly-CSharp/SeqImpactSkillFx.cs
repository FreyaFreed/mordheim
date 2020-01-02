using System;
using Prometheus;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerFriendlyName("ImpactSkillFx")]
[global::WellFired.USequencerEvent("Mordheim/ImpactSkillFx")]
public class SeqImpactSkillFx : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		if (focusedUnit.CurrentAction.fxData != null && focusedUnit.defenderCtrlr != null)
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(focusedUnit.CurrentAction.fxData.ImpactFx, focusedUnit.defenderCtrlr, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
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
