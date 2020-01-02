using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcTargeting : global::ICheapState
{
	public ArcTargeting(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.cam = global::PandoraSingleton<global::MissionManager>.Instance.CamManager;
		this.unitCtrlr.SetFixed(true);
		this.cam.SwitchToCam(global::CameraManager.CameraType.FIXED, this.unitCtrlr.transform, true, false, true, false);
		this.unitCtrlr.defenders.Clear();
		this.unitCtrlr.destructTargets.Clear();
		this.availableTargets = this.unitCtrlr.GetCurrentActionTargets();
		global::PandoraSingleton<global::MissionManager>.Instance.InitArcTarget(this.unitCtrlr.transform, out this.arcSrc, out this.arcDir);
		this.SetArcPos();
		global::PandoraSingleton<global::MissionManager>.Instance.arcTarget.SetActive(true);
		if (this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, this.unitCtrlr.CurrentAction, null);
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
		this.unitCtrlr.ClearFlyingTexts();
		global::PandoraSingleton<global::MissionManager>.Instance.arcTarget.SetActive(false);
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
			this.unitCtrlr.SendSkillTargets(this.unitCtrlr.CurrentAction.SkillId, global::PandoraSingleton<global::MissionManager>.Instance.arcTarget.transform.position, global::PandoraSingleton<global::MissionManager>.Instance.arcTarget.transform.forward);
		}
		else
		{
			float num = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_x", 0) / 2f;
			num += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_x", 0) * 4f;
			if (num != 0f)
			{
				global::UnityEngine.Quaternion rotation = global::UnityEngine.Quaternion.AngleAxis(num, global::UnityEngine.Vector3.up);
				global::UnityEngine.Vector3 to = rotation * this.arcDir;
				if (global::UnityEngine.Vector3.Angle(this.unitCtrlr.transform.forward, to) < 90f)
				{
					this.arcDir = to;
				}
			}
			this.SetArcPos();
			this.unitCtrlr.SetArcTargets(this.availableTargets, this.unitCtrlr.transform.forward, true);
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void SetArcPos()
	{
		global::PandoraSingleton<global::MissionManager>.Instance.arcTarget.transform.position = this.arcSrc;
		global::UnityEngine.Quaternion rotation = global::UnityEngine.Quaternion.LookRotation(this.arcDir);
		global::PandoraSingleton<global::MissionManager>.Instance.arcTarget.transform.rotation = rotation;
		this.unitCtrlr.FaceTarget(global::PandoraSingleton<global::MissionManager>.Instance.arcTarget.transform.GetChild(0), false);
		this.cam.SetShoulderCam(this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE, false);
		this.cam.dummyCam.transform.rotation = rotation;
		this.cam.dummyCam.transform.position += this.cam.dummyCam.transform.forward * -1.5f + this.cam.dummyCam.transform.right * -0.25f;
	}

	private const float MIN_DIST = 0.01f;

	private global::UnitController unitCtrlr;

	private global::CameraManager cam;

	private global::UnityEngine.Vector3 arcSrc;

	private global::UnityEngine.Vector3 arcDir;

	private global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> availableTargets = new global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour>();
}
