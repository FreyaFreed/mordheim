using System;
using UnityEngine;

public class CharacterFollowCamHack : global::CameraBase
{
	private void Awake()
	{
		this.centerOffset = new global::UnityEngine.Vector3(0f, 1f, 0f);
		this.headOffset = new global::UnityEngine.Vector3(0f, 2f, 0f);
	}

	private void Start()
	{
		this.isSnapping = true;
	}

	private void LateUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		this.distance -= global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_wheel", 0);
		if (this.distance < this.minDistance)
		{
			this.distance = this.minDistance;
		}
		else if (this.distance > this.maxDistance)
		{
			this.distance = this.maxDistance;
		}
		global::UnityEngine.Vector3 vector = this.target.position + this.centerOffset;
		if (this.isSnapping)
		{
			this.ApplySnapping(vector);
		}
		else
		{
			this.ApplyPositionDamping(new global::UnityEngine.Vector3(vector.x, base.transform.position.y, vector.z));
		}
		global::UnityEngine.Vector3 vector2 = base.transform.rotation * global::UnityEngine.Vector3.back;
		vector2.Normalize();
		float num = 0f;
		float num2 = 0f;
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKey("action", 0))
		{
			num = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_x", 0) / 2f;
			num2 = -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_y", 0) / 2f;
			num += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_x", 0) * 10f;
			num2 += -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0) * 10f;
		}
		float num3 = base.transform.rotation.eulerAngles.x;
		if (num3 > 180f)
		{
			num3 -= 360f;
		}
		if (num2 > 0f && num2 + num3 > this.maxAngle)
		{
			num2 = this.maxAngle - num3;
		}
		else if (num2 < 0f && num2 + num3 < this.minAngle)
		{
			num2 = this.minAngle - num3;
		}
		if (num != 0f || num2 != 0f)
		{
			this.isSnapping = false;
			global::UnityEngine.Quaternion lhs = global::UnityEngine.Quaternion.AngleAxis(num, global::UnityEngine.Vector3.up);
			global::UnityEngine.Quaternion rhs = global::UnityEngine.Quaternion.AngleAxis(num2, base.transform.right);
			vector2 = lhs * rhs * vector2;
			base.transform.position = vector + vector2 * this.distance;
		}
		base.transform.LookAt(vector);
	}

	private void ApplySnapping(global::UnityEngine.Vector3 targetCenter)
	{
		global::UnityEngine.Vector3 position = base.transform.position;
		global::UnityEngine.Vector3 vector = position - targetCenter;
		vector.y = 0f;
		float num = vector.magnitude;
		float y = this.target.eulerAngles.y;
		float num2 = base.transform.eulerAngles.y;
		num2 = global::UnityEngine.Mathf.SmoothDampAngle(num2, y, ref this.velocity.x, this.snapLag);
		num = global::UnityEngine.Mathf.SmoothDamp(num, this.distance, ref this.velocity.z, this.snapLag);
		global::UnityEngine.Vector3 vector2 = targetCenter;
		vector2 += global::UnityEngine.Quaternion.Euler(0f, num2, 0f) * global::UnityEngine.Vector3.back * num;
		vector2.y = global::UnityEngine.Mathf.SmoothDamp(position.y, targetCenter.y + this.height, ref this.velocity.y, this.smoothLag, this.maxSpeed);
		vector2 = this.AdjustLineOfSight(vector2, targetCenter);
		base.transform.position = vector2;
		if (this.AngleDistance(num2, y) < 3f)
		{
			this.isSnapping = false;
			this.velocity = global::UnityEngine.Vector3.zero;
		}
	}

	private global::UnityEngine.Vector3 AdjustLineOfSight(global::UnityEngine.Vector3 newPosition, global::UnityEngine.Vector3 target)
	{
		global::UnityEngine.RaycastHit raycastHit;
		if (global::UnityEngine.Physics.Linecast(target, newPosition, out raycastHit, 0))
		{
			this.velocity = global::UnityEngine.Vector3.zero;
			return raycastHit.point;
		}
		return newPosition;
	}

	private void ApplyPositionDamping(global::UnityEngine.Vector3 targetCenter)
	{
		global::UnityEngine.Vector3 vector = base.transform.rotation * global::UnityEngine.Vector3.back * this.distance;
		vector.y = 0f;
		global::UnityEngine.Vector3 position = base.transform.position;
		global::UnityEngine.Vector3 vector2 = position - targetCenter;
		vector2.y = 0f;
		global::UnityEngine.Vector3 vector3 = vector2.normalized * vector.magnitude + targetCenter;
		global::UnityEngine.Vector3 vector4;
		vector4.x = global::UnityEngine.Mathf.SmoothDamp(position.x, vector3.x, ref this.velocity.x, this.smoothLag, this.maxSpeed);
		vector4.z = global::UnityEngine.Mathf.SmoothDamp(position.z, vector3.z, ref this.velocity.z, this.smoothLag, this.maxSpeed);
		vector4.y = global::UnityEngine.Mathf.SmoothDamp(position.y, targetCenter.y, ref this.velocity.y, this.smoothLag, this.maxSpeed);
		vector4 = this.AdjustLineOfSight(vector4, targetCenter);
		base.transform.position = vector4;
	}

	private void SetUpRotation(global::UnityEngine.Vector3 centerPos, global::UnityEngine.Vector3 headPos)
	{
		global::UnityEngine.Vector3 position = base.transform.position;
		global::UnityEngine.Vector3 vector = centerPos - position;
		global::UnityEngine.Quaternion lhs = global::UnityEngine.Quaternion.LookRotation(new global::UnityEngine.Vector3(vector.x, 0f, vector.z));
		global::UnityEngine.Vector3 forward = global::UnityEngine.Vector3.forward * this.distance + global::UnityEngine.Vector3.down * this.height;
		base.transform.rotation = lhs * global::UnityEngine.Quaternion.LookRotation(forward);
	}

	private float AngleDistance(float a, float b)
	{
		a = global::UnityEngine.Mathf.Repeat(a, 360f);
		b = global::UnityEngine.Mathf.Repeat(b, 360f);
		return global::UnityEngine.Mathf.Abs(b - a);
	}

	public override void GetNextPositionAngle(ref global::UnityEngine.Vector3 position, ref global::UnityEngine.Quaternion angle)
	{
		position = this.target.position + this.headOffset + this.target.transform.forward * -this.distance;
		angle = global::UnityEngine.Quaternion.LookRotation(this.target.position + this.centerOffset - position);
		this.isSnapping = true;
	}

	public float minDistance = 3f;

	public float maxDistance = 7f;

	public float minAngle = -10f;

	public float maxAngle = 45f;

	public float distance = 4f;

	public float height = 1f;

	public float smoothLag = 0.2f;

	public float maxSpeed = 10f;

	public float snapLag = 0.3f;

	public float clampHeadPositionScreenSpace = 0.75f;

	private bool isSnapping;

	private global::UnityEngine.Vector3 headOffset = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.Vector3 centerOffset = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.Vector3 velocity = global::UnityEngine.Vector3.zero;
}
