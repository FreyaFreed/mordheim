using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePointTarget : global::ICheapState
{
	public InteractivePointTarget(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
		this.targets = new global::System.Collections.Generic.List<global::InteractiveTarget>();
	}

	void global::ICheapState.Destroy()
	{
		this.targets.Clear();
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.camRange = global::PandoraSingleton<global::MissionManager>.Instance.CamManager;
		this.unitCtrlr.SetFixed(true);
		this.targets.Clear();
		for (int i = 0; i < this.unitCtrlr.interactivePoints.Count; i++)
		{
			global::System.Collections.Generic.List<global::UnitActionId> unitActionIds = this.unitCtrlr.interactivePoints[i].GetUnitActionIds(this.unitCtrlr);
			for (int j = 0; j < unitActionIds.Count; j++)
			{
				global::System.Collections.Generic.List<global::ActionStatus> list = this.unitCtrlr.GetActions(unitActionIds[j]);
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k].Available)
					{
						this.targets.Add(new global::InteractiveTarget(list[k], this.unitCtrlr.interactivePoints[i]));
					}
				}
			}
		}
		this.targets.Sort(new global::InteractiveTargetComparer());
		this.actions = new global::System.Collections.Generic.List<global::ActionStatus>();
		for (int l = 0; l < this.targets.Count; l++)
		{
			this.actions.Add(this.targets[l].action);
		}
		this.targetIdx = 0;
		this.UpdateDestination(0);
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_ACTION_ZONE_CONFIRMED);
			this.unitCtrlr.SendInteractiveAction(this.unitCtrlr.CurrentAction.SkillId, this.targets[this.targetIdx].point);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cycling", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 0))
		{
			this.UpdateDestination(1);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("cycling", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 0))
		{
			this.UpdateDestination(-1);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("esc_cancel", 0))
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_ACTION_ZONE_CANCEL);
			this.unitCtrlr.StateMachine.ChangeState(11);
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].point.Highlight.On((i != this.targetIdx) ? global::UnityEngine.Color.yellow : global::UnityEngine.Color.red);
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void UpdateDestination(int move = 0)
	{
		this.targetIdx += move;
		if (this.targetIdx >= this.targets.Count)
		{
			this.targetIdx = 0;
		}
		else if (this.targetIdx < 0)
		{
			this.targetIdx = this.targets.Count - 1;
		}
		global::UnityEngine.Transform transform = null;
		this.unitCtrlr.interactivePoint = this.targets[this.targetIdx].point;
		this.unitCtrlr.SetCurrentAction(this.targets[this.targetIdx].action.SkillId);
		this.unitCtrlr.prevInteractiveTarget = this.GetPrevInteractivePoint();
		this.unitCtrlr.nextInteractiveTarget = this.GetNextInteractivePoint();
		global::UnityEngine.Transform transform2 = null;
		global::UnitActionId actionId = this.targets[this.targetIdx].action.ActionId;
		global::UnitActionId unitActionId = actionId;
		switch (unitActionId)
		{
		case global::UnitActionId.ACTIVATE:
			break;
		default:
			if (unitActionId == global::UnitActionId.LEAP)
			{
				transform = ((global::ActionZone)this.unitCtrlr.interactivePoint).GetLeap().destination.transform;
				goto IL_1CB;
			}
			if (unitActionId != global::UnitActionId.SEARCH)
			{
				goto IL_1CB;
			}
			break;
		case global::UnitActionId.CLIMB:
			transform = ((global::ActionZone)this.unitCtrlr.interactivePoint).GetClimb().destination.transform;
			goto IL_1CB;
		case global::UnitActionId.JUMP:
			transform = ((global::ActionZone)this.unitCtrlr.interactivePoint).GetJump().destination.transform;
			goto IL_1CB;
		}
		transform = this.unitCtrlr.interactivePoint.transform;
		transform2 = ((!(this.unitCtrlr.interactivePoint.cameraAnchor != null)) ? null : this.unitCtrlr.interactivePoint.cameraAnchor.transform);
		IL_1CB:
		this.unitCtrlr.FaceTarget(transform.position, false);
		this.camRange.LookAtFocus(transform, false, true);
		this.camRange.SwitchToCam(global::CameraManager.CameraType.CONSTRAINED, this.unitCtrlr.transform, true, true, false, this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
		if (transform2 != null)
		{
			global::ConstrainedCamera currentCam = this.camRange.GetCurrentCam<global::ConstrainedCamera>();
			currentCam.SetOrigins(transform2);
		}
		this.camRange.AddLOSTarget(this.unitCtrlr.transform);
		unitActionId = actionId;
		if (unitActionId != global::UnitActionId.CLIMB)
		{
			if (unitActionId != global::UnitActionId.JUMP)
			{
				if (unitActionId == global::UnitActionId.LEAP)
				{
					this.unitCtrlr.activeActionDest = ((global::ActionZone)this.targets[this.targetIdx].point).GetLeap();
				}
			}
			else
			{
				this.unitCtrlr.activeActionDest = ((global::ActionZone)this.targets[this.targetIdx].point).GetJump();
			}
		}
		else
		{
			this.unitCtrlr.activeActionDest = ((global::ActionZone)this.targets[this.targetIdx].point).GetClimb();
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, this.targets[this.targetIdx].action, this.actions);
	}

	public global::InteractiveTarget GetPrevInteractivePoint()
	{
		if (this.targetIdx - 1 < 0)
		{
			return this.targets[this.targets.Count - 1];
		}
		return this.targets[this.targetIdx - 1];
	}

	public global::InteractiveTarget GetNextInteractivePoint()
	{
		if (this.targetIdx + 1 >= this.targets.Count)
		{
			return this.targets[0];
		}
		return this.targets[this.targetIdx + 1];
	}

	private global::UnitController unitCtrlr;

	private global::System.Collections.Generic.List<global::InteractiveTarget> targets;

	private global::System.Collections.Generic.List<global::ActionStatus> actions;

	private global::CameraManager camRange;

	private int targetIdx;

	private int numActionZoneDestinations;
}
