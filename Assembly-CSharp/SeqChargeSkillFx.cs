using System;
using Prometheus;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/ChargeSkillFx")]
[global::WellFired.USequencerFriendlyName("ChargeSkillFx")]
public class SeqChargeSkillFx : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController ctrlr = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		if (ctrlr.CurrentAction.fxData != null)
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(ctrlr.CurrentAction.fxData.ChargeFx, (!ctrlr.CurrentAction.fxData.ChargeOnTarget || !(ctrlr.defenderCtrlr != null)) ? ctrlr : ctrlr.defenderCtrlr, null, delegate(global::UnityEngine.GameObject fx)
			{
				if (fx != null)
				{
					ctrlr.chargeFx = fx.GetComponent<global::Prometheus.OlympusFire>();
				}
			}, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
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
