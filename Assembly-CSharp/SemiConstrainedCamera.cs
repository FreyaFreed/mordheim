using System;
using UnityEngine;

public class SemiConstrainedCamera : global::ICheapState
{
	public SemiConstrainedCamera(global::CameraManager camMngr)
	{
		this.mngr = camMngr;
		this.dummyCam = camMngr.dummyCam.transform;
		this.centerOffset = new global::UnityEngine.Vector3(0f, 1.5f, 0f);
		this.hit = default(global::UnityEngine.RaycastHit);
		this.previousTarget = null;
	}

	public void Destroy()
	{
	}

	public void Enter(int from)
	{
		if (this.mngr.Target != this.previousTarget || this.mngr.LookAtTarget != this.previousLook || from != 6)
		{
			this.previousTarget = this.mngr.Target;
			this.previousLook = this.mngr.LookAtTarget;
			global::UnityEngine.Quaternion quaternion = default(global::UnityEngine.Quaternion);
			if (this.mngr.LookAtTarget == this.mngr.Target)
			{
				quaternion = this.mngr.Target.rotation;
			}
			else
			{
				quaternion.SetLookRotation(this.mngr.LookAtTarget.position - this.mngr.Target.position, global::UnityEngine.Vector3.up);
			}
			global::UnityEngine.Vector3 eulerAngles = quaternion.eulerAngles;
			global::UnityEngine.Vector3 position = this.mngr.Target.position + this.centerOffset;
			this.oRotH = eulerAngles.y;
			this.dummyCam.position = position;
			this.rotH = -25f;
			this.dummyCam.rotation = global::UnityEngine.Quaternion.Euler(0f, this.oRotH + this.rotH, 0f);
			this.dummyCam.Translate(-this.dummyCam.forward * this.mngr.Zoom, global::UnityEngine.Space.World);
			this.previousAngle = this.dummyCam.rotation.eulerAngles.x;
			if (this.previousAngle > 180f)
			{
				this.previousAngle -= 360f;
			}
			this.previousAngle = global::UnityEngine.Mathf.Clamp(this.previousAngle, -30f, 50f);
		}
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
		float num3 = this.previousAngle;
		num3 += num2;
		num3 = global::UnityEngine.Mathf.Clamp(num3, -30f, 50f);
		this.rotH = global::UnityEngine.Mathf.Clamp(this.rotH + num, -90f, 90f);
		global::UnityEngine.Quaternion lhs = global::UnityEngine.Quaternion.AngleAxis(this.oRotH + this.rotH, global::UnityEngine.Vector3.up);
		global::UnityEngine.Quaternion rhs = global::UnityEngine.Quaternion.AngleAxis(num3, global::UnityEngine.Vector3.right);
		this.dummyCam.rotation = lhs * rhs;
		this.dummyCam.position = vector + -this.dummyCam.forward * this.mngr.Zoom;
		if (global::UnityEngine.Physics.Raycast(vector, -this.dummyCam.forward, out this.hit, this.mngr.Zoom, global::LayerMaskManager.groundMask) && this.hit.transform != this.mngr.Target)
		{
			this.dummyCam.position = this.hit.point + this.dummyCam.forward * 0.1f;
		}
		this.previousAngle = this.dummyCam.rotation.eulerAngles.x;
		if (this.previousAngle > 180f)
		{
			this.previousAngle -= 360f;
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

	private const float MAX_ROT_H = 90f;

	public global::UnityEngine.Vector3 centerOffset = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.RaycastHit hit;

	private global::CameraManager mngr;

	private global::UnityEngine.Transform dummyCam;

	private global::UnityEngine.Transform previousTarget;

	private global::UnityEngine.Transform previousLook;

	private float previousAngle;

	private float oRotH;

	private float rotH;
}
