using System;
using System.Collections.Generic;
using UnityEngine;

public class LineTargeting : global::ICheapState
{
	public LineTargeting(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.cam = global::PandoraSingleton<global::MissionManager>.Instance.CamManager;
		this.cam.SwitchToCam(global::CameraManager.CameraType.FIXED, this.unitCtrlr.transform, true, false, true, false);
		this.unitCtrlr.defenders.Clear();
		this.unitCtrlr.destructTargets.Clear();
		this.availableTargets = this.unitCtrlr.GetCurrentActionTargets();
		global::PandoraSingleton<global::MissionManager>.Instance.InitLineTarget(this.unitCtrlr.transform, (float)this.unitCtrlr.CurrentAction.Radius, (float)this.unitCtrlr.CurrentAction.RangeMax, out this.lineSrc, out this.lineDir);
		this.SetLinePos();
		global::PandoraSingleton<global::MissionManager>.Instance.lineTarget.SetActive(true);
		if (this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, this.unitCtrlr.CurrentAction, null);
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
		this.unitCtrlr.ClearFlyingTexts();
		global::PandoraSingleton<global::MissionManager>.Instance.lineTarget.SetActive(false);
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
			this.unitCtrlr.SendSkillTargets(this.unitCtrlr.CurrentAction.SkillId, this.lineSrc, this.lineDir);
		}
		else
		{
			float num = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_x", 0) / 2f;
			float num2 = -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_y", 0) / 2f;
			num += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_x", 0) * 4f;
			num2 += -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0) * 4f;
			if (num != 0f || num2 != 0f)
			{
				global::UnityEngine.Quaternion lhs = global::UnityEngine.Quaternion.AngleAxis(num, global::UnityEngine.Vector3.up);
				global::UnityEngine.Quaternion rhs = global::UnityEngine.Quaternion.AngleAxis(num2, this.unitCtrlr.transform.right);
				global::UnityEngine.Vector3 to = lhs * rhs * this.lineDir;
				if (global::UnityEngine.Vector3.Angle(this.unitCtrlr.transform.forward, to) < 90f)
				{
					this.lineDir = to;
				}
			}
			this.SetLinePos();
			this.unitCtrlr.SetLineTargets(this.availableTargets, global::PandoraSingleton<global::MissionManager>.Instance.lineTarget.transform, true);
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void SetLinePos()
	{
		global::PandoraSingleton<global::MissionManager>.Instance.lineTarget.transform.position = this.lineSrc;
		global::UnityEngine.Quaternion rotation = global::UnityEngine.Quaternion.LookRotation(this.lineDir);
		global::PandoraSingleton<global::MissionManager>.Instance.lineTarget.transform.rotation = rotation;
		this.unitCtrlr.FaceTarget(global::PandoraSingleton<global::MissionManager>.Instance.lineTarget.transform.GetChild(0), false);
		this.cam.SetShoulderCam(this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE, false);
		this.cam.dummyCam.transform.rotation = rotation;
	}

	private const float MIN_DIST = 0.01f;

	private global::UnitController unitCtrlr;

	private global::CameraManager cam;

	private global::UnityEngine.Vector3 lineSrc;

	private global::UnityEngine.Vector3 lineDir;

	private global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> availableTargets = new global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour>();
}
