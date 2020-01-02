using System;
using UnityEngine;

public class SmoothFollow : global::CameraBase
{
	private void Start()
	{
		if (this.target)
		{
			this.startingDirection = this.target.transform.position - base.transform.position;
			this.currentDistance = global::UnityEngine.Vector3.Distance(global::UnityEngine.Vector3.zero, this.startingDirection);
			this.startingDirection.Normalize();
			this.startingRotation = base.transform.rotation;
			this.targetStartRotation = global::UnityEngine.Quaternion.Inverse(this.target.transform.rotation);
			base.gameObject.layer = 2;
		}
	}

	private void Update()
	{
		if (!this.target)
		{
			return;
		}
		this.currentDistance -= global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_wheel", 0);
		if (this.currentDistance < this.minDistance)
		{
			this.currentDistance = this.minDistance;
		}
		else if (this.currentDistance > this.maxDistance)
		{
			this.currentDistance = this.maxDistance;
		}
		global::UnityEngine.Vector3 vector = this.startingDirection * this.currentDistance;
		if (!this.fixedCam)
		{
			global::UnityEngine.Quaternion quaternion = this.target.transform.rotation * this.targetStartRotation;
			base.transform.rotation = quaternion * this.startingRotation;
			vector = quaternion * vector;
		}
		else
		{
			base.transform.rotation = this.startingRotation;
		}
		global::UnityEngine.Vector3 vector2 = this.target.transform.position - vector;
		global::UnityEngine.Vector3 position = this.target.transform.position;
		float num = global::UnityEngine.Vector3.Distance(vector2, position);
		global::UnityEngine.RaycastHit raycastHit;
		if (global::UnityEngine.Physics.Raycast(position, vector2 - position, out raycastHit, num + 1f))
		{
			vector2 = raycastHit.point;
		}
		base.transform.position = global::UnityEngine.Vector3.SmoothDamp(base.transform.position, vector2, ref this.cameraSpeed, this.damping * global::UnityEngine.Time.deltaTime);
	}

	public override void GetNextPositionAngle(ref global::UnityEngine.Vector3 position, ref global::UnityEngine.Quaternion angle)
	{
		position = base.transform.position;
		if (this.target)
		{
			angle = global::UnityEngine.Quaternion.LookRotation(this.target.position - base.transform.position);
		}
		else
		{
			angle = base.transform.rotation;
		}
	}

	public float damping = 1f;

	public bool fixedCam = true;

	public float minDistance = 10f;

	public float maxDistance = 50f;

	private global::UnityEngine.Vector3 startingDirection;

	private float currentDistance;

	private global::UnityEngine.Quaternion startingRotation;

	private global::UnityEngine.Quaternion targetStartRotation;

	private global::UnityEngine.Vector3 cameraSpeed = global::UnityEngine.Vector3.zero;
}
