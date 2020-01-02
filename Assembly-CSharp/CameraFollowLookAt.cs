using System;
using UnityEngine;

public class CameraFollowLookAt : global::CameraBase
{
	public void MoveLookAt(global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation)
	{
		this.targetPosition = position;
		this.targetRotation = rotation;
		this.done = false;
	}

	private void Update()
	{
		if (this.done)
		{
			return;
		}
		base.transform.position = global::UnityEngine.Vector3.Lerp(base.transform.position, this.targetPosition, 10f * global::UnityEngine.Time.deltaTime);
		base.transform.rotation = global::UnityEngine.Quaternion.Lerp(base.transform.rotation, this.targetRotation, 10f * global::UnityEngine.Time.deltaTime);
		this.done = (base.transform.position == this.targetPosition && base.transform.rotation == this.targetRotation);
	}

	private global::UnityEngine.Quaternion targetRotation;

	private global::UnityEngine.Vector3 targetPosition;

	private bool done;
}
