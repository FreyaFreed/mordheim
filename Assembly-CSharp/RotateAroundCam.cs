using System;
using UnityEngine;

public class RotateAroundCam : global::ICheapState
{
	public RotateAroundCam(global::CameraManager camMngr)
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
		this.previousAngle = this.dummyCam.rotation.eulerAngles.x;
		if (this.previousAngle > 180f)
		{
			this.previousAngle -= 360f;
		}
		this.previousAngle = global::UnityEngine.Mathf.Clamp(this.previousAngle, -30f, 50f);
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
		float num = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_x", 0) / 2f;
		float num2 = -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_y", 0) / 2f;
		num += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_x", 0) * 4f;
		num2 += -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0) * 4f;
		global::UnityEngine.Vector3 vector2 = -this.dummyCam.forward;
		this.previousAngle = global::UnityEngine.Mathf.Clamp(this.previousAngle, -30f, 50f);
		float num3 = this.previousAngle;
		if (num != 0f || num2 != 0f)
		{
			if (num2 > 0f && num2 + num3 > 50f)
			{
				num2 = 50f - num3;
			}
			else if (num2 < 0f && num2 + num3 < -30f)
			{
				num2 = -30f - num3;
			}
			global::UnityEngine.Quaternion lhs = global::UnityEngine.Quaternion.AngleAxis(num, global::UnityEngine.Vector3.up);
			global::UnityEngine.Quaternion rhs = global::UnityEngine.Quaternion.AngleAxis(num2, this.dummyCam.right);
			vector2 = lhs * rhs * -this.dummyCam.forward;
		}
		this.dummyCam.position = vector + vector2 * this.distance;
		float num4 = 0.2f;
		if (global::UnityEngine.Physics.SphereCast(vector, num4, vector2, out this.hit, this.distance, global::LayerMaskManager.groundMask) && this.hit.transform != this.mngr.Target)
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
		if (global::UnityEngine.Vector3.SqrMagnitude(this.mngr.transform.position - this.dummyCam.position) > 4f)
		{
			this.mngr.Transition(2f, false);
		}
	}

	public void FixedUpdate()
	{
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

	private const float MIN_ANGLE = -30f;

	private const float MAX_ANGLE = 50f;

	public global::UnityEngine.Vector3 centerOffset = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.Vector3 lastTargetPosition = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.RaycastHit hit;

	private global::CameraManager mngr;

	private global::UnityEngine.Transform dummyCam;

	private global::UnityEngine.Transform prevTarget;

	private float previousAngle;

	public float distance;
}
