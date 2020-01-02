using System;
using UnityEngine;

public class test_camera : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.Distance = global::UnityEngine.Mathf.Clamp(this.Distance, this.DistanceMin, this.DistanceMax);
		this.startingDistance = this.Distance;
		this.Reset();
	}

	private void LateUpdate()
	{
		if (this.TargetLookAt == null)
		{
			this.TargetLookAt = global::UnityEngine.GameObject.Find("rig_pelvis").transform;
		}
		this.HandlePlayerInput();
		this.CalculateDesiredPosition();
		this.UpdatePosition();
	}

	private void HandlePlayerInput()
	{
		float num = 0.01f;
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("Button1", 0) > 0f)
		{
			this.mouseX += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_x", 0) * this.X_MouseSensitivity;
			this.mouseY -= global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_y", 0) * this.Y_MouseSensitivity;
		}
		this.mouseY = this.ClampAngle(this.mouseY, this.Y_MinLimit, this.Y_MaxLimit);
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("MouseWheel", 0) < -num || global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("MouseWheel", 0) > num)
		{
			this.desiredDistance = global::UnityEngine.Mathf.Clamp(this.Distance - global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("MouseWheel", 0) * this.MouseWheelSensitivity, this.DistanceMin, this.DistanceMax);
		}
	}

	private float ClampAngle(float angle, float min, float max)
	{
		while (angle < -360f || angle > 360f)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
		}
		return global::UnityEngine.Mathf.Clamp(angle, min, max);
	}

	private void CalculateDesiredPosition()
	{
		this.Distance = global::UnityEngine.Mathf.SmoothDamp(this.Distance, this.desiredDistance, ref this.velocityDistance, this.DistanceSmooth);
		this.desiredPosition = this.CalculatePosition(this.mouseY, this.mouseX, this.Distance);
	}

	public global::UnityEngine.Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
	{
		this.direction = new global::UnityEngine.Vector3(0f, 0f, -distance);
		global::UnityEngine.Quaternion rotation = global::UnityEngine.Quaternion.Euler(rotationX, rotationY, 0f);
		return this.TargetLookAt.position + rotation * this.direction;
	}

	private void UpdatePosition()
	{
		float x = global::UnityEngine.Mathf.SmoothDamp(this.position.x, this.desiredPosition.x, ref this.velX, this.X_Smooth);
		float y = global::UnityEngine.Mathf.SmoothDamp(this.position.y, this.desiredPosition.y, ref this.velY, this.Y_Smooth);
		float z = global::UnityEngine.Mathf.SmoothDamp(this.position.z, this.desiredPosition.z, ref this.velZ, this.X_Smooth);
		this.position = new global::UnityEngine.Vector3(x, y, z);
		base.transform.position = this.position;
		base.transform.LookAt(this.TargetLookAt);
	}

	private void Reset()
	{
		this.mouseX = 0f;
		this.mouseY = 10f;
		this.Distance = this.startingDistance;
		this.desiredDistance = this.Distance;
	}

	public global::UnityEngine.Transform TargetLookAt;

	public float Distance = 5f;

	public float DistanceMin = 3f;

	public float DistanceMax = 10f;

	private float mouseX;

	private float mouseY;

	private float startingDistance;

	private float desiredDistance;

	public float X_MouseSensitivity = 5f;

	public float Y_MouseSensitivity = 5f;

	public float MouseWheelSensitivity = 5f;

	public float Y_MinLimit = -40f;

	public float Y_MaxLimit = 80f;

	public float DistanceSmooth = 0.05f;

	public float velocityDistance;

	private global::UnityEngine.Vector3 desiredPosition = global::UnityEngine.Vector3.zero;

	public float X_Smooth = 0.05f;

	public float Y_Smooth = 0.1f;

	private float velX;

	private float velY;

	private float velZ;

	private global::UnityEngine.Vector3 position = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.Vector3 direction = global::UnityEngine.Vector3.zero;

	private float rotationX;

	private float rotationY;

	private float distance;
}
