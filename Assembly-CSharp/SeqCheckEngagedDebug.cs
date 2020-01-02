using System;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/CheckEngagedDebug")]
[global::WellFired.USequencerFriendlyName("CheckEngagedDebug")]
public class SeqCheckEngagedDebug : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
	}

	public override void ProcessEvent(float runningTime)
	{
		if (global::UnityEngine.Application.loadedLevelName != "sequence")
		{
			return;
		}
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		if (global::UnityEngine.Vector3.SqrMagnitude(focusedUnit.transform.position - focusedUnit.defenderCtrlr.transform.position) <= 4f)
		{
			focusedUnit.SetAnimSpeed(0f);
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.GetComponent<global::CameraAnim>().Stop();
		}
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}
}
