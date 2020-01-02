using System;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerFriendlyName("SetActionZone")]
[global::WellFired.USequencerEvent("Mordheim/SetActionZone")]
public class SeqSetActionZone : global::WellFired.USEventBase
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
		focusedUnit.animator.Play(global::AnimatorIds.idle);
		global::ActionZoneSeqHelper actionZoneHelper = global::PandoraSingleton<global::SequenceHelper>.Instance.GetActionZoneHelper(this.actionId);
		focusedUnit.interactivePoint = actionZoneHelper.actionZone;
		focusedUnit.transform.position = actionZoneHelper.actionZone.transform.position;
		focusedUnit.transform.rotation = actionZoneHelper.actionZone.transform.rotation;
		if (actionZoneHelper.actionDest != null)
		{
			focusedUnit.activeActionDest = new global::ActionDestination
			{
				actionId = this.actionId,
				destination = actionZoneHelper.actionDest
			};
		}
		if (actionZoneHelper.actionDef != null)
		{
			focusedUnit.defenderCtrlr.transform.position = actionZoneHelper.actionDef.transform.position;
			focusedUnit.defenderCtrlr.transform.rotation = actionZoneHelper.actionDef.transform.rotation;
		}
		focusedUnit.currentActionLabel = this.currentAction;
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public global::UnitActionId actionId;

	public string currentAction = "action";
}
