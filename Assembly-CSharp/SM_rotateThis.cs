using System;
using UnityEngine;

public class SM_rotateThis : global::UnityEngine.MonoBehaviour
{
	private void Update()
	{
		if (this.local)
		{
			base.transform.Rotate(new global::UnityEngine.Vector3(this.rotationSpeedX, this.rotationSpeedY, this.rotationSpeedZ) * global::UnityEngine.Time.deltaTime);
		}
		else
		{
			base.transform.Rotate(new global::UnityEngine.Vector3(this.rotationSpeedX, this.rotationSpeedY, this.rotationSpeedZ) * global::UnityEngine.Time.deltaTime, global::UnityEngine.Space.World);
		}
	}

	public float rotationSpeedX = 90f;

	public float rotationSpeedY;

	public float rotationSpeedZ;

	public bool local = true;
}
