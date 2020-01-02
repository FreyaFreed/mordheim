using System;
using UnityEngine;

public class CameraFixed : global::ICheapState
{
	public CameraFixed(global::CameraManager camMngr)
	{
		this.mngr = camMngr;
		this.dummyCam = camMngr.dummyCam.transform;
	}

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		this.dummyCam.position = this.mngr.transform.position;
		this.dummyCam.rotation = this.mngr.transform.rotation;
	}

	public void Exit(int iTo)
	{
	}

	public void Update()
	{
	}

	public void FixedUpdate()
	{
	}

	private global::CameraManager mngr;

	private global::UnityEngine.Transform dummyCam;
}
