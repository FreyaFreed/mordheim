using System;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/SetVignetteDOF")]
[global::WellFired.USequencerFriendlyName("SetVignetteDOF")]
public class SeqSetVignetteDOF : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController unitController = null;
		global::UnityEngine.Transform target = null;
		switch (this.DOFTargetId)
		{
		case global::SequenceTargetId.FOCUSED_UNIT:
			unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
			target = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.transform;
			break;
		case global::SequenceTargetId.DEFENDER:
			unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.defenderCtrlr;
			target = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.defenderCtrlr.transform;
			break;
		case global::SequenceTargetId.ACTION_ZONE:
			unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
			target = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.interactivePoint.transform;
			break;
		case global::SequenceTargetId.ACTION_DEST:
			unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
			target = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.activeActionDest.destination.transform;
			break;
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.OwnUnitInvolved(unitController, unitController.defenderCtrlr) == null)
		{
			return;
		}
		if (this.setActive)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.TurnOnDOF(target);
		}
		else
		{
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.TurnOffDOF();
		}
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.ActivateOverlay(this.setActive, this.vignetteFadeTime);
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SetFOV(60f, 0.2f);
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public float vignetteFadeTime = 1f;

	public float fovTime = 0.2f;

	public global::SequenceTargetId DOFTargetId;

	public bool setActive;
}
