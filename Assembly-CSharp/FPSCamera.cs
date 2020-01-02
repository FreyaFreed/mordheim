using System;
using UnityEngine;

public class FPSCamera : global::CameraBase
{
	private void LateUpdate()
	{
		if (this.shooter)
		{
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			global::UnityEngine.Quaternion identity = global::UnityEngine.Quaternion.identity;
			this.GetNextPositionAngle(ref zero, ref identity);
			base.transform.position = zero;
			float num = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_x", 0) * global::UnityEngine.Time.deltaTime * this.mouseSpeed;
			float num2 = -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_y", 0) * global::UnityEngine.Time.deltaTime * this.mouseSpeed * (float)((!this.invertedAxis) ? 1 : -1);
			num += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_x", 0) * global::UnityEngine.Time.deltaTime;
			num2 += -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0) * global::UnityEngine.Time.deltaTime * (float)((!this.invertedAxis) ? 1 : -1);
			float num3 = base.transform.rotation.eulerAngles.x;
			if (num3 > 180f)
			{
				num3 -= 360f;
			}
			if (num2 > 0f && num2 + num3 > 30f)
			{
				num2 = 30f - num3;
			}
			else if (num2 < 0f && num2 + num3 < -30f)
			{
				num2 = -30f - num3;
			}
			num3 = (base.transform.rotation.eulerAngles.y - this.shooter.rotation.eulerAngles.y) % 360f;
			if (num3 > 180f)
			{
				num3 -= 360f;
			}
			else if (num3 < -180f)
			{
				num3 += 360f;
			}
			num3 += num;
			if (num3 > 30f)
			{
				num3 = 30f;
			}
			else if (num3 < -30f)
			{
				num3 = -30f;
			}
			base.transform.rotation = global::UnityEngine.Quaternion.Euler(base.transform.rotation.eulerAngles.x + num2, this.shooter.rotation.eulerAngles.y + num3, base.transform.rotation.eulerAngles.z);
		}
	}

	public override void GetNextPositionAngle(ref global::UnityEngine.Vector3 position, ref global::UnityEngine.Quaternion angle)
	{
		if (this.shooter)
		{
			position = this.shooter.position + global::UnityEngine.Vector3.up * this.offsetY;
			angle = this.shooter.rotation;
		}
		else
		{
			position = base.transform.position;
			angle = base.transform.rotation;
		}
	}

	public override void SetTarget(global::UnityEngine.Transform target)
	{
		this.shooter = target;
	}

	public override global::UnityEngine.Transform GetTarget()
	{
		return this.shooter;
	}

	private const float maxAngle = 30f;

	public global::UnityEngine.Transform shooter;

	public float offsetY;

	public bool invertedAxis;

	public float mouseSpeed = 10f;
}
