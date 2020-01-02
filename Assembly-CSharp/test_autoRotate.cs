using System;
using UnityEngine;

public class test_autoRotate : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (!this.around)
		{
			global::UnityEngine.Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			eulerAngles.y += global::UnityEngine.Time.deltaTime * 180f;
			base.transform.rotation = global::UnityEngine.Quaternion.Euler(eulerAngles);
		}
		else
		{
			base.transform.RotateAround(global::UnityEngine.Vector3.zero, global::UnityEngine.Vector3.up, 40f * global::UnityEngine.Time.deltaTime);
			base.transform.LookAt(new global::UnityEngine.Vector3(0f, 1.5f, 0f));
		}
	}

	public bool around;
}
