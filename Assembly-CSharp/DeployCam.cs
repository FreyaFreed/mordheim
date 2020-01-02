using System;
using UnityEngine;

public class DeployCam : global::ICheapState
{
	public DeployCam(global::CameraManager camMngr)
	{
		this.mngr = camMngr;
		this.dummyCam = camMngr.dummyCam.transform;
		this.centerOffset = new global::UnityEngine.Vector3(0f, 1.5f, 0f);
		this.hit = default(global::UnityEngine.RaycastHit);
		this.prevTarget = null;
	}

	public void Destroy()
	{
	}

	public void Enter(int from)
	{
		this.mngr.SetZoomLevel(2U);
		if (this.prevTarget != this.mngr.Target)
		{
			this.prevTarget = this.mngr.Target;
			global::UnityEngine.Vector3 position = this.mngr.Target.position + this.centerOffset;
			this.dummyCam.position = position;
			this.dummyCam.rotation = global::UnityEngine.Quaternion.Euler(0f, this.mngr.Target.transform.rotation.eulerAngles.y - 90f, 0f);
			this.dummyCam.Translate(-this.dummyCam.forward * this.mngr.Zoom * 3f, global::UnityEngine.Space.World);
		}
		this.snappingTime = 0f;
		this.isSnapping = true;
		this.previousAngle = this.dummyCam.rotation.eulerAngles.x;
		if (this.previousAngle > 180f)
		{
			this.previousAngle -= 360f;
		}
		this.previousAngle = global::UnityEngine.Mathf.Clamp(this.previousAngle, 10f, 75f);
	}

	public void Exit(int to)
	{
	}

	public void Update()
	{
		if (this.mngr.Target == null)
		{
			return;
		}
		global::UnityEngine.Vector3 vector = this.mngr.Target.position + this.centerOffset;
		if (this.isSnapping)
		{
			this.ApplySnapping(vector);
		}
		float num = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_x", 0) / 2f;
		float num2 = -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_y", 0) / 2f;
		num += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_x", 0) * 4f;
		num2 += -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0) * 4f;
		global::UnityEngine.Vector3 vector2 = -this.dummyCam.forward;
		this.previousAngle = global::UnityEngine.Mathf.Clamp(this.previousAngle, 10f, 75f);
		float num3 = this.previousAngle;
		if (num != 0f || num2 != 0f)
		{
			if (num2 > 0f && num2 + num3 > 75f)
			{
				num2 = 75f - num3;
			}
			else if (num2 < 0f && num2 + num3 < 10f)
			{
				num2 = 10f - num3;
			}
			this.isSnapping = false;
			global::UnityEngine.Quaternion lhs = global::UnityEngine.Quaternion.AngleAxis(num, global::UnityEngine.Vector3.up);
			global::UnityEngine.Quaternion rhs = global::UnityEngine.Quaternion.AngleAxis(num2, this.dummyCam.right);
			vector2 = lhs * rhs * -this.dummyCam.forward;
		}
		if (!this.isSnapping)
		{
			this.dummyCam.position = vector + vector2 * this.mngr.Zoom * 3f;
		}
		float num4 = 0.2f;
		if (global::UnityEngine.Physics.SphereCast(vector, num4, vector2, out this.hit, this.mngr.Zoom * 3f, global::LayerMaskManager.groundMask) && this.hit.transform != this.mngr.Target)
		{
			this.dummyCam.position = this.hit.point + this.hit.normal * num4;
		}
		this.lastTargetPosition = vector;
		this.previousAngle = this.dummyCam.rotation.eulerAngles.x;
		if (this.previousAngle > 180f)
		{
			this.previousAngle -= 360f;
		}
		if (this.mngr.LookAtTarget != null)
		{
			this.dummyCam.LookAt(this.mngr.LookAtTarget);
		}
		else
		{
			this.dummyCam.LookAt(vector);
		}
	}

	public void FixedUpdate()
	{
	}

	private void ApplySnapping(global::UnityEngine.Vector3 targetCenter)
	{
		global::UnityEngine.Vector3 position = this.dummyCam.position;
		(position - targetCenter).y = 0f;
		float y = this.mngr.Target.eulerAngles.y;
		float y2 = this.dummyCam.eulerAngles.y;
		this.snappingTime += global::UnityEngine.Time.deltaTime;
		this.dummyCam.rotation = global::UnityEngine.Quaternion.Slerp(this.dummyCam.rotation, global::UnityEngine.Quaternion.Euler(15f, y, 0f), this.snappingTime / 7.5f);
		global::UnityEngine.Vector3 position2 = targetCenter + this.dummyCam.rotation * global::UnityEngine.Vector3.back * this.mngr.Zoom;
		this.dummyCam.position = position2;
		if (global::UnityEngine.Mathf.Abs(global::UnityEngine.Mathf.DeltaAngle(this.dummyCam.eulerAngles.y, y)) < 2f && global::UnityEngine.Mathf.Abs(global::UnityEngine.Mathf.DeltaAngle(this.dummyCam.eulerAngles.x, 15f)) < 2f)
		{
			this.isSnapping = false;
		}
	}

	private global::UnityEngine.Vector3 AdjustLineOfSight(global::UnityEngine.Vector3 newPosition, global::UnityEngine.Vector3 target)
	{
		global::UnityEngine.RaycastHit raycastHit;
		if (global::UnityEngine.Physics.Linecast(target, newPosition, out raycastHit, 0))
		{
			return raycastHit.point;
		}
		return newPosition;
	}

	private const float MIN_ANGLE = 10f;

	private const float MAX_ANGLE = 75f;

	private bool isSnapping;

	public global::UnityEngine.Vector3 centerOffset = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.Vector3 lastTargetPosition = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.RaycastHit hit;

	private global::CameraManager mngr;

	private global::UnityEngine.Transform dummyCam;

	private global::UnityEngine.Transform prevTarget;

	private float snappingTime;

	private float previousAngle;
}
