using System;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerFriendlyName("SetUnitStartAnimDebug")]
[global::WellFired.USequencerEvent("Mordheim/SetUnitStartAnimDebug")]
public class SeqSetUnitStartAnimDebug : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		if (global::UnityEngine.Application.loadedLevelName != "sequence")
		{
			return;
		}
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		focusedUnit.SetFixed(false);
		focusedUnit.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = true;
		focusedUnit.animator.Play(global::UnityEngine.Animator.StringToHash(this.animState));
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public string animState;
}
