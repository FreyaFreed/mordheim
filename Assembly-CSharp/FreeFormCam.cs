using System;
using UnityEngine;

public class FreeFormCam : global::ICheapState
{
	public FreeFormCam(global::CameraManager camMngr)
	{
		this.mngr = camMngr;
		this.dummyCam = this.mngr.dummyCam.transform;
	}

	public void Destroy()
	{
	}

	public void Enter(int from)
	{
	}

	public void Exit(int to)
	{
	}

	public void Update()
	{
		global::UnityEngine.Vector3 a = default(global::UnityEngine.Vector3) + this.dummyCam.forward * global::PandoraSingleton<global::PandoraInput>.Instance.GetAxisRaw("v", 0);
		a += this.dummyCam.right * global::PandoraSingleton<global::PandoraInput>.Instance.GetAxisRaw("h", 0);
		a.Normalize();
		if (global::UnityEngine.Input.GetKey(global::UnityEngine.KeyCode.Mouse1))
		{
			global::UnityEngine.Vector3 eulerAngles = this.dummyCam.rotation.eulerAngles;
			eulerAngles.y += this.rotationSpeed * global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_x", 0) * 4f * global::UnityEngine.Time.deltaTime;
			eulerAngles.x -= this.rotationSpeed * global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0) * 4f * global::UnityEngine.Time.deltaTime;
			this.dummyCam.rotation = global::UnityEngine.Quaternion.Euler(eulerAngles);
		}
		this.dummyCam.position += a * this.moveSpeed * global::UnityEngine.Time.deltaTime;
	}

	public void FixedUpdate()
	{
	}

	public float moveSpeed = 10f;

	public float rotationSpeed = 10f;

	private global::CameraManager mngr;

	private global::UnityEngine.Transform dummyCam;
}
