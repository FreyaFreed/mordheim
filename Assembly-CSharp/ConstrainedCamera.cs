using System;
using UnityEngine;

public class ConstrainedCamera : global::ICheapState
{
	public ConstrainedCamera(global::CameraManager camMngr)
	{
		this.mngr = camMngr;
		this.dummyCam = camMngr.dummyCam.transform;
		this.rotX = 0f;
		this.rotY = 0f;
		this.oRotX = 0f;
		this.oRotY = 0f;
	}

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		this.SetCamBehindUnit();
		this.rotX = 0f;
		this.rotY = 0f;
	}

	public void Exit(int iTo)
	{
	}

	public void SetOrigins(global::UnityEngine.Transform trans)
	{
		this.dummyCam.position = trans.position;
		this.dummyCam.rotation = trans.rotation;
		this.oRotX = this.dummyCam.rotation.eulerAngles.x;
		this.oRotY = this.dummyCam.rotation.eulerAngles.y;
	}

	private void SetCamBehindUnit()
	{
		this.mngr.SetSideCam(false, false);
		this.oRotX = this.dummyCam.rotation.eulerAngles.x;
		this.oRotY = this.dummyCam.rotation.eulerAngles.y;
	}

	public void Update()
	{
		float num = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_x", 0) / 2f;
		float num2 = -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_y", 0) / 2f;
		num += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_x", 0) * 4f;
		num2 += -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0) * 4f;
		this.rotX += num2;
		this.rotY += num;
		this.rotX = global::UnityEngine.Mathf.Clamp(this.rotX, -20f, 20f);
		this.rotY = global::UnityEngine.Mathf.Clamp(this.rotY, -20f, 20f);
		global::UnityEngine.Quaternion lhs = global::UnityEngine.Quaternion.AngleAxis(this.oRotY + this.rotY, global::UnityEngine.Vector3.up);
		global::UnityEngine.Quaternion rhs = global::UnityEngine.Quaternion.AngleAxis(this.oRotX + this.rotX, global::UnityEngine.Vector3.right);
		this.dummyCam.rotation = lhs * rhs;
	}

	public void FixedUpdate()
	{
	}

	private const float MAX_ROT = 20f;

	private global::CameraManager mngr;

	private global::UnityEngine.Transform dummyCam;

	private float rotX;

	private float rotY;

	private float oRotX;

	private float oRotY;
}
