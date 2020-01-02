using System;
using UnityEngine;

public class SM_randomScale : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		float num = global::UnityEngine.Random.Range(this.minScale, this.maxScale);
		base.transform.localScale = new global::UnityEngine.Vector3(num, num, num);
	}

	public float minScale = 1f;

	public float maxScale = 2f;
}
