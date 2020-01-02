using System;
using UnityEngine;

public class MeleeAttackCamera : global::ICheapState
{
	public MeleeAttackCamera(global::CameraManager camMngr)
	{
		this.mngr = camMngr;
		this.dummyCam = camMngr.dummyCam.transform;
		this.centerOffset = new global::UnityEngine.Vector3(0f, 1.5f, 0f);
		this.hit = default(global::UnityEngine.RaycastHit);
	}

	public void Destroy()
	{
	}

	public void Enter(int from)
	{
		this.snappingTime = 0f;
		this.isSnapping = true;
		global::UnityEngine.Quaternion quaternion = default(global::UnityEngine.Quaternion);
		quaternion.SetLookRotation(this.mngr.LookAtTarget.position - this.mngr.Target.position, global::UnityEngine.Vector3.up);
		global::UnityEngine.Vector3 eulerAngles = quaternion.eulerAngles;
		global::UnityEngine.Vector3 vector = this.mngr.Target.position + (this.mngr.LookAtTarget.position - this.mngr.Target.position) / 2f + this.centerOffset;
		this.oRotH = eulerAngles.y;
		this.oRotV = eulerAngles.x;
		this.dummyCam.rotation = global::UnityEngine.Quaternion.Euler(this.oRotV, this.oRotH - 110f, 0f);
		this.dummyCam.position = vector;
		this.dummyCam.transform.Translate(-this.dummyCam.transform.forward * 3f, global::UnityEngine.Space.World);
		this.dummyCam.LookAt(vector - global::UnityEngine.Vector3.up * 0.5f);
		this.mngr.Transition(10f, true);
	}

	public void Exit(int to)
	{
	}

	public void Update()
	{
		global::UnityEngine.Vector3 vector = this.mngr.Target.position + this.centerOffset;
		if (this.isSnapping)
		{
		}
	}

	public void FixedUpdate()
	{
	}

	private void ApplySnapping(global::UnityEngine.Vector3 targetCenter)
	{
		global::UnityEngine.Vector3 position = this.dummyCam.position;
		(position - targetCenter).y = 0f;
		float num = this.oRotH - 90f;
		float y = this.dummyCam.eulerAngles.y;
		this.snappingTime += global::UnityEngine.Time.deltaTime;
		this.dummyCam.rotation = global::UnityEngine.Quaternion.Slerp(this.dummyCam.rotation, global::UnityEngine.Quaternion.Euler(15f, num, 0f), this.snappingTime / 7.5f);
		global::UnityEngine.Vector3 position2 = targetCenter + this.dummyCam.rotation * global::UnityEngine.Vector3.back * this.mngr.Zoom;
		this.dummyCam.position = position2;
		if (global::UnityEngine.Mathf.Abs(global::UnityEngine.Mathf.DeltaAngle(this.dummyCam.eulerAngles.y, num)) < 2f)
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

	private const float MIN_ANGLE = 25f;

	private const float MAX_ANGLE = 25f;

	private const float MAX_ROT_H = 25f;

	public global::UnityEngine.Vector3 centerOffset = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.RaycastHit hit;

	private global::CameraManager mngr;

	private global::UnityEngine.Transform dummyCam;

	private global::UnityEngine.Vector3 lastTargetPosition = global::UnityEngine.Vector3.zero;

	private bool isSnapping;

	private float snappingTime;

	private float oRotH;

	private float oRotV;
}
