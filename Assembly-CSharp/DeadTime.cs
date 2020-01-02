using System;
using UnityEngine;

public class DeadTime : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		global::UnityEngine.Object.Destroy(base.gameObject, this.deadTime);
	}

	public float deadTime;
}
