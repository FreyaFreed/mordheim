using System;
using UnityEngine;

public class GameCamera : global::CameraBase
{
	private void Start()
	{
		this.currentDistance = (this.maxDistance - this.minDistance) / 2f;
	}

	private void Update()
	{
		this.currentDistance -= global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_wheel", 0) * this.scrollSpeed;
		if (this.currentDistance < this.minDistance)
		{
			this.currentDistance = this.minDistance;
		}
		else if (this.currentDistance > this.maxDistance)
		{
			this.currentDistance = this.maxDistance;
		}
		float axisRaw = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxisRaw("h", 0);
		float axisRaw2 = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxisRaw("v", 0);
		if (axisRaw != 0f || axisRaw2 != 0f)
		{
			global::UnityEngine.Vector3 forward = base.transform.forward;
			forward.y = 0f;
			global::UnityEngine.Vector3 a = axisRaw * base.transform.right + axisRaw2 * forward;
			a.Normalize();
			base.transform.position += a * global::UnityEngine.Time.deltaTime * this.moveSpeed;
			this.targetPosition += a * global::UnityEngine.Time.deltaTime * this.moveSpeed;
		}
		global::UnityEngine.Vector3 vector = base.transform.position - this.targetPosition;
		vector.Normalize();
		global::UnityEngine.Quaternion rotation = global::UnityEngine.Quaternion.AngleAxis(global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_x", 0), global::UnityEngine.Vector3.up);
		vector = rotation * vector;
		base.transform.position = this.targetPosition + vector * this.currentDistance;
		if (this.smooth)
		{
			global::UnityEngine.Quaternion b = global::UnityEngine.Quaternion.LookRotation(this.targetPosition - base.transform.position);
			base.transform.rotation = global::UnityEngine.Quaternion.Slerp(base.transform.rotation, b, global::UnityEngine.Time.deltaTime * this.damping);
		}
		else
		{
			base.transform.LookAt(this.targetPosition);
		}
	}

	public override void GetNextPositionAngle(ref global::UnityEngine.Vector3 position, ref global::UnityEngine.Quaternion angle)
	{
		position = base.transform.position;
		angle = global::UnityEngine.Quaternion.LookRotation(this.targetPosition - base.transform.position);
	}

	public override void SetTarget(global::UnityEngine.Transform target)
	{
		if (target != null)
		{
			this.targetPosition = target.position + this.offsetVector;
		}
	}

	public global::UnityEngine.Vector3 targetPosition = global::UnityEngine.Vector3.zero;

	public float damping = 6f;

	public bool smooth;

	public float minDistance = 5f;

	public float maxDistance = 30f;

	public float moveSpeed = 10f;

	public float scrollSpeed = 5f;

	private float currentDistance;

	private global::UnityEngine.Vector3 offsetVector = new global::UnityEngine.Vector3(0f, 1f, 0f);
}
