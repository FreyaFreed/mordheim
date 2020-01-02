using System;
using UnityEngine;

public class SM_randomRotation : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		float xAngle = global::UnityEngine.Random.Range(-this.rotationMaxX, this.rotationMaxX);
		float yAngle = global::UnityEngine.Random.Range(-this.rotationMaxY, this.rotationMaxY);
		float zAngle = global::UnityEngine.Random.Range(-this.rotationMaxZ, this.rotationMaxZ);
		base.transform.Rotate(xAngle, yAngle, zAngle);
	}

	public float rotationMaxX;

	public float rotationMaxY = 360f;

	public float rotationMaxZ;
}
