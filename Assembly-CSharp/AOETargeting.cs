using System;
using System.Collections.Generic;
using UnityEngine;

public class AOETargeting : global::ICheapState
{
	public AOETargeting(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.isPositionValid = true;
		this.rangeMin = (float)this.unitCtrlr.CurrentAction.RangeMin;
		this.needWalkable = this.unitCtrlr.CurrentAction.skillData.NeedValidGround;
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.defenders.Clear();
		this.unitCtrlr.destructTargets.Clear();
		this.availableTargets = this.unitCtrlr.GetCurrentActionTargets();
		this.maxRange = this.unitCtrlr.CurrentAction.RangeMax;
		this.sphereDist = (float)this.unitCtrlr.CurrentAction.RangeMax;
		this.camMngr = global::PandoraSingleton<global::MissionManager>.Instance.CamManager;
		if (this.maxRange > 0)
		{
			this.camMngr.SwitchToCam(global::CameraManager.CameraType.FIXED, this.unitCtrlr.transform, true, false, true, false);
		}
		else
		{
			this.camMngr.SwitchToCam(global::CameraManager.CameraType.ROTATE_AROUND, this.unitCtrlr.transform, true, false, true, false);
			global::RotateAroundCam currentCam = this.camMngr.GetCurrentCam<global::RotateAroundCam>();
			currentCam.distance = (float)this.unitCtrlr.CurrentAction.Radius + 6f;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.InitSphereTarget(this.unitCtrlr.transform, (float)this.unitCtrlr.CurrentAction.Radius, this.unitCtrlr.CurrentAction.TargetingId, out this.sphereRaySrc, out this.sphereDir);
		this.stickToGround = (this.unitCtrlr.CurrentAction.TargetingId == global::TargetingId.AREA_GROUND);
		if (this.stickToGround)
		{
			this.sphereDir = global::UnityEngine.Vector3.Normalize(this.unitCtrlr.transform.forward);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.sphereTarget.SetActive(true);
		this.SetSpherePos();
		if (this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, this.unitCtrlr.CurrentAction, null);
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::UIMissionManager>.Instance.interactiveMessage.Hide();
		this.unitCtrlr.ClearFlyingTexts();
		global::PandoraSingleton<global::MissionManager>.Instance.sphereTarget.SetActive(false);
		this.camMngr.ClearLookAtFocus();
	}

	void global::ICheapState.Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("esc_cancel", 0))
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_ACTION_CANCEL);
			this.unitCtrlr.StateMachine.ChangeState((!this.unitCtrlr.Engaged) ? 11 : 12);
		}
		else if (this.isPositionValid && global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_ACTION_CONFIRM);
			this.unitCtrlr.SendSkillTargets(this.unitCtrlr.CurrentAction.SkillId, global::PandoraSingleton<global::MissionManager>.Instance.sphereTarget.transform.position, global::PandoraSingleton<global::MissionManager>.Instance.sphereTarget.transform.position - this.unitCtrlr.transform.position);
		}
		else
		{
			float num = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_x", 0) / 2f;
			float num2 = -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_y", 0) / 2f;
			num += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_x", 0) * 4f;
			num2 += -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0) * 4f;
			float axis = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("v", 0);
			if (this.stickToGround)
			{
				if (num != 0f)
				{
					global::UnityEngine.Quaternion rotation = global::UnityEngine.Quaternion.AngleAxis(num, global::UnityEngine.Vector3.up);
					global::UnityEngine.Vector3 to = rotation * this.sphereDir;
					if (global::UnityEngine.Vector3.Angle(this.unitCtrlr.transform.forward, to) < 90f)
					{
						this.sphereDir = to;
					}
				}
				this.sphereDist += -num2 / 4f + axis;
			}
			else
			{
				if (num != 0f || num2 != 0f)
				{
					global::UnityEngine.Quaternion lhs = global::UnityEngine.Quaternion.AngleAxis(num, global::UnityEngine.Vector3.up);
					global::UnityEngine.Quaternion rhs = global::UnityEngine.Quaternion.AngleAxis(num2, this.unitCtrlr.transform.right);
					global::UnityEngine.Vector3 to2 = lhs * rhs * this.sphereDir;
					if (global::UnityEngine.Vector3.Angle(this.unitCtrlr.transform.forward, to2) < 90f)
					{
						this.sphereDir = to2;
					}
				}
				this.sphereDist += axis;
			}
			this.SetSpherePos();
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void SetSpherePos()
	{
		bool flag = true;
		global::UnityEngine.Transform transform = global::PandoraSingleton<global::MissionManager>.Instance.sphereTarget.transform;
		this.sphereDist = global::UnityEngine.Mathf.Clamp(this.sphereDist, 0f, (float)this.maxRange);
		float distance = this.sphereDist;
		global::UnityEngine.Ray ray = new global::UnityEngine.Ray(this.sphereRaySrc, this.sphereDir);
		global::UnityEngine.RaycastHit raycastHit;
		if (global::UnityEngine.Physics.Raycast(ray, out raycastHit, this.sphereDist, global::LayerMaskManager.groundMask))
		{
			distance = raycastHit.distance;
		}
		transform.position = this.sphereRaySrc + this.sphereDir * distance;
		if (this.stickToGround)
		{
			global::UnityEngine.Vector3 origin = transform.position + global::UnityEngine.Vector3.up * 0.05f;
			ray = new global::UnityEngine.Ray(origin, global::UnityEngine.Vector3.down);
			if (global::UnityEngine.Physics.Raycast(ray, out raycastHit, 100f, global::LayerMaskManager.groundMask))
			{
				transform.position = raycastHit.point + global::UnityEngine.Vector3.up * 0.05f;
			}
			else
			{
				global::PandoraDebug.LogWarning("No ground found when trying to stick aoe targeting sphere.", "TARGETING", this.unitCtrlr);
			}
		}
		this.unitCtrlr.FaceTarget(transform, true);
		if (this.maxRange > 0)
		{
			this.camMngr.SetShoulderCam(this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE, false);
			global::UnityEngine.Vector3 position = this.camMngr.dummyCam.transform.position;
			position.x = this.unitCtrlr.transform.position.x;
			position.z = this.unitCtrlr.transform.position.z;
			this.camMngr.dummyCam.transform.LookAt(transform);
			global::UnityEngine.Vector3 a = this.camMngr.dummyCam.transform.position - transform.position;
			float num = global::UnityEngine.Vector3.SqrMagnitude(a);
			if (num < ((float)this.unitCtrlr.CurrentAction.Radius + 3.5f) * ((float)this.unitCtrlr.CurrentAction.Radius + 3.5f))
			{
				this.camMngr.dummyCam.transform.position = transform.position + a.normalized * ((float)this.unitCtrlr.CurrentAction.Radius + 5f);
				if (global::UnityEngine.Physics.SphereCast(position, 0.2f, -this.camMngr.dummyCam.transform.forward, out raycastHit, (float)this.unitCtrlr.CurrentAction.Radius + 4f, global::LayerMaskManager.groundMask))
				{
					this.camMngr.dummyCam.transform.position = raycastHit.point + this.camMngr.dummyCam.transform.forward * 0.2f;
				}
			}
		}
		if (global::UnityEngine.Vector3.SqrMagnitude(transform.position - this.prevSpherePos) > 4f)
		{
			this.camMngr.Transition(2f, true);
		}
		if (!this.stickToGround)
		{
			this.unitCtrlr.SetAoeTargets(this.availableTargets, transform.transform, true);
		}
		else
		{
			this.unitCtrlr.SetCylinderTargets(this.availableTargets, transform.transform, true);
		}
		this.prevSpherePos = transform.position;
		if (this.rangeMin > 0f)
		{
			flag &= (global::UnityEngine.Vector3.SqrMagnitude(transform.position - this.unitCtrlr.transform.position) > this.rangeMin * this.rangeMin);
		}
		if (flag && this.needWalkable)
		{
			flag &= global::PandoraSingleton<global::MissionManager>.Instance.IsOnNavmesh(transform.position);
			if (flag)
			{
				global::System.Collections.Generic.List<global::UnitController> aliveAllies = global::PandoraSingleton<global::MissionManager>.Instance.GetAliveAllies(this.unitCtrlr.GetWarband().idx);
				global::UnityEngine.Vector2 point = new global::UnityEngine.Vector2(this.unitCtrlr.transform.position.x, this.unitCtrlr.transform.position.z);
				global::UnityEngine.Vector2 checkDestPoint = new global::UnityEngine.Vector2(transform.position.x, transform.position.z);
				for (int i = 0; i < aliveAllies.Count; i++)
				{
					global::UnityEngine.Vector3 position2 = aliveAllies[i].transform.position;
					if (aliveAllies[i] != this.unitCtrlr && global::UnityEngine.Mathf.Abs(position2.y - transform.position.y) < 1.9f && global::PandoraUtils.IsPointInsideEdges(aliveAllies[i].combatCircle.Edges, point, checkDestPoint, -1f))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				global::System.Collections.Generic.List<global::ActionZone> accessibleActionZones = global::PandoraSingleton<global::MissionManager>.Instance.accessibleActionZones;
				for (int j = 0; j < accessibleActionZones.Count; j++)
				{
					global::System.Collections.Generic.List<global::UnitActionId> unitActionIds = accessibleActionZones[j].GetUnitActionIds(this.unitCtrlr);
					if (unitActionIds.Count > 0 && (unitActionIds[0] == global::UnitActionId.CLIMB || unitActionIds[0] == global::UnitActionId.CLIMB_3M || unitActionIds[0] == global::UnitActionId.CLIMB_6M || unitActionIds[0] == global::UnitActionId.CLIMB_9M || unitActionIds[0] == global::UnitActionId.JUMP || unitActionIds[0] == global::UnitActionId.JUMP_3M || unitActionIds[0] == global::UnitActionId.JUMP_6M || unitActionIds[0] == global::UnitActionId.JUMP_9M || unitActionIds[0] == global::UnitActionId.LEAP))
					{
						float num2 = global::UnityEngine.Vector3.Dot(global::UnityEngine.Vector3.Normalize(transform.position - (accessibleActionZones[j].transform.position + accessibleActionZones[j].transform.forward)), accessibleActionZones[j].transform.forward);
						if (num2 < 0f)
						{
							float num3 = global::UnityEngine.Vector3.SqrMagnitude(transform.position - accessibleActionZones[j].transform.position);
							if (num3 < 4f)
							{
								flag = false;
								break;
							}
						}
					}
				}
			}
		}
		if (flag != this.isPositionValid)
		{
			this.isPositionValid = flag;
			transform.GetChild(1).gameObject.SetActive(this.isPositionValid);
			transform.GetChild(2).gameObject.SetActive(this.isPositionValid);
			transform.GetChild(3).gameObject.SetActive(this.isPositionValid);
			if (this.isPositionValid)
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.interactiveMessage.Hide();
			}
			else
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.interactiveMessage.Show(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("na_action_area_blocked"));
			}
		}
	}

	private global::UnitController unitCtrlr;

	private global::CameraManager camMngr;

	private int maxRange;

	private float sphereHeight;

	private float sphereMaxXZMagnitude;

	private global::UnityEngine.Vector3 sphereRaySrc;

	private global::UnityEngine.Vector3 sphereDir;

	private global::UnityEngine.Vector3 prevSpherePos;

	private float sphereDist;

	private bool stickToGround;

	private float rangeMin;

	private bool isPositionValid;

	private bool needWalkable;

	private global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> availableTargets = new global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour>();
}
