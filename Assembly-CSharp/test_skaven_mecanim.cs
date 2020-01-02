using System;
using UnityEngine;

public class test_skaven_mecanim : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.animator = base.GetComponent<global::UnityEngine.Animator>();
	}

	private void FixedUpdate()
	{
		float axis = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("h", 0);
		float axis2 = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("v", 0);
		if (axis != 0f || axis2 != 0f)
		{
			global::UnityEngine.Vector3 a = global::UnityEngine.Camera.main.transform.forward;
			a.y = 0f;
			a.Normalize();
			a *= axis2;
			global::UnityEngine.Vector3 vector = global::UnityEngine.Camera.main.transform.right;
			vector.y = 0f;
			vector.Normalize();
			vector *= axis;
			global::UnityEngine.Vector3 forward = a + vector;
			global::UnityEngine.Quaternion b = global::UnityEngine.Quaternion.LookRotation(forward, global::UnityEngine.Vector3.up);
			global::UnityEngine.Quaternion rot = global::UnityEngine.Quaternion.Lerp(base.GetComponent<global::UnityEngine.Rigidbody>().rotation, b, this.turnSmoothing * global::UnityEngine.Time.deltaTime);
			base.GetComponent<global::UnityEngine.Rigidbody>().MoveRotation(rot);
			this.animator.SetFloat("speed", axis2, this.speedDampTime, global::UnityEngine.Time.deltaTime);
		}
		else
		{
			this.animator.SetFloat("speed", 0f);
		}
	}

	protected global::UnityEngine.Animator animator;

	public float turnSmoothing = 55f;

	public float speedDampTime = 0.1f;
}
