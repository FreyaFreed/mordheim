using System;
using UnityEngine;

public class SmoothLookAt : global::CameraBase
{
	private void Start()
	{
		this.ray = new global::UnityEngine.Ray(global::UnityEngine.Vector3.zero, global::UnityEngine.Vector3.zero);
		this.planY = new global::UnityEngine.Plane(global::UnityEngine.Vector3.up, global::UnityEngine.Vector3.zero);
	}

	private void Update()
	{
		global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
		if (this.target)
		{
			vector = this.target.position;
		}
		else
		{
			this.ray.origin = base.transform.position;
			this.ray.direction = base.transform.rotation * global::UnityEngine.Vector3.forward;
			float d = 0f;
			if (this.planY.Raycast(this.ray, out d))
			{
				vector = base.transform.position + this.ray.direction.normalized * d;
			}
		}
		float num = global::UnityEngine.Vector3.Distance(vector, base.transform.position);
		num -= global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_wheel", 0) * this.scrollSpeed;
		if (num < this.minDistance)
		{
			num = this.minDistance;
		}
		else if (num > this.maxDistance)
		{
			num = this.maxDistance;
		}
		global::UnityEngine.Vector3 vector2 = base.transform.position - vector;
		vector2.Normalize();
		global::UnityEngine.Quaternion rotation = global::UnityEngine.Quaternion.AngleAxis(global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("h", 0), global::UnityEngine.Vector3.up);
		vector2 = rotation * vector2;
		base.transform.position = vector + vector2 * num;
		if (this.smooth)
		{
			global::UnityEngine.Quaternion b = global::UnityEngine.Quaternion.LookRotation(vector - base.transform.position);
			base.transform.rotation = global::UnityEngine.Quaternion.Slerp(base.transform.rotation, b, global::UnityEngine.Time.deltaTime * this.damping);
		}
		else
		{
			base.transform.LookAt(vector);
		}
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

	public float damping = 6f;

	public bool smooth = true;

	public float minDistance = 10f;

	public float maxDistance = 50f;

	public float scrollSpeed = 10f;

	private global::UnityEngine.Ray ray;

	private global::UnityEngine.Plane planY;
}
