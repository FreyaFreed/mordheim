using System;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargeting : global::ICheapState
{
	public SingleTargeting(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.camRange = global::PandoraSingleton<global::MissionManager>.Instance.CamManager;
		this.availableTargets = this.unitCtrlr.GetCurrentActionTargets();
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.defenderCtrlr = null;
		if (this.lastTarget != null && this.availableTargets.IndexOf(this.lastTarget) != -1)
		{
			this.currentTargetIdx = this.availableTargets.IndexOf(this.lastTarget);
			this.UpdateTarget(0);
		}
		else
		{
			this.currentTargetIdx = -1;
			this.UpdateTarget(1);
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.UNIT_START_SINGLE_TARGETING);
		if (this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, this.unitCtrlr.CurrentAction, null);
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
		this.currentTargetIdx = -1;
		this.availableTargets.Clear();
		this.camRange.ClearLookAtFocus();
	}

	void global::ICheapState.Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("esc_cancel", 0))
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_ACTION_CANCEL);
			this.unitCtrlr.StateMachine.ChangeState((!this.unitCtrlr.Engaged) ? 11 : 12);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_ACTION_CONFIRM);
			if (this.unitCtrlr.defenderCtrlr != null)
			{
				this.unitCtrlr.SendSkillSingleTarget(this.unitCtrlr.CurrentAction.SkillId, this.unitCtrlr.defenderCtrlr);
			}
			else
			{
				this.unitCtrlr.SendSkillSingleDestructible(this.unitCtrlr.CurrentAction.SkillId, this.unitCtrlr.destructibleTarget);
			}
		}
		else
		{
			for (int i = 0; i < this.availableTargets.Count; i++)
			{
				if (this.availableTargets[i] is global::UnitController)
				{
					((global::UnitController)this.availableTargets[i]).Highlight.On((i != this.currentTargetIdx) ? global::UnityEngine.Color.yellow : global::UnityEngine.Color.red);
				}
				else if (this.availableTargets[i] is global::Destructible)
				{
					((global::Destructible)this.availableTargets[i]).Highlight.On((i != this.currentTargetIdx) ? global::UnityEngine.Color.yellow : global::UnityEngine.Color.red);
				}
			}
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cycling", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 0))
			{
				this.UpdateTarget(1);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("cycling", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 0))
			{
				this.UpdateTarget(-1);
			}
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void UpdateTarget(int move = 0)
	{
		int num = this.currentTargetIdx;
		this.currentTargetIdx += move;
		this.currentTargetIdx = ((this.currentTargetIdx < this.availableTargets.Count) ? ((this.currentTargetIdx >= 0) ? this.currentTargetIdx : (this.availableTargets.Count - 1)) : 0);
		if (this.currentTargetIdx == num && move != 0)
		{
			return;
		}
		global::UnityEngine.MonoBehaviour monoBehaviour = this.availableTargets[this.currentTargetIdx];
		this.lastTarget = monoBehaviour;
		if (this.unitCtrlr != this.unitCtrlr.defenderCtrlr)
		{
			this.unitCtrlr.FaceTarget(monoBehaviour.transform, false);
		}
		this.camRange.LookAtFocus(monoBehaviour.transform, false, true);
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.SEMI_CONSTRAINED, this.unitCtrlr.transform, true, true, false, this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
		for (int i = 0; i < this.availableTargets.Count; i++)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.AddLOSTarget(this.availableTargets[i].transform);
		}
		if (monoBehaviour is global::UnitController)
		{
			this.unitCtrlr.defenderCtrlr = (global::UnitController)monoBehaviour;
			this.unitCtrlr.destructibleTarget = null;
			if (this.unitCtrlr.IsPlayed())
			{
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.CURRENT_UNIT_TARGET_CHANGED, this.unitCtrlr.defenderCtrlr);
			}
		}
		else if (monoBehaviour is global::Destructible)
		{
			this.unitCtrlr.defenderCtrlr = null;
			this.unitCtrlr.destructibleTarget = (global::Destructible)monoBehaviour;
			this.unitCtrlr.currentSpellTargetPosition = monoBehaviour.transform.position;
			if (this.unitCtrlr.IsPlayed())
			{
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Destructible>(global::Notices.CURRENT_UNIT_TARGET_DESTUCTIBLE_CHANGED, this.unitCtrlr.destructibleTarget);
			}
		}
	}

	private global::UnitController unitCtrlr;

	private global::CameraManager camRange;

	private int currentTargetIdx;

	private global::UnityEngine.MonoBehaviour lastTarget;

	private global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> availableTargets = new global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour>();
}
