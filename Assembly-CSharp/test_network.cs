using System;
using UnityEngine;

public class test_network : global::UnityEngine.MonoBehaviour
{
	private void FixedUpdate()
	{
		if (global::UnityEngine.Network.isServer)
		{
			global::UnityEngine.Animator component = base.GetComponent<global::UnityEngine.Animator>();
			float axis = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("V", 0);
			float axis2 = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("H", 0);
			if (axis2 != 0f || axis != 0f)
			{
				global::UnityEngine.Vector3 a = global::UnityEngine.Camera.main.transform.forward;
				a.y = 0f;
				a.Normalize();
				a *= axis;
				global::UnityEngine.Vector3 vector = global::UnityEngine.Camera.main.transform.right;
				vector.y = 0f;
				vector.Normalize();
				vector *= axis2;
				global::UnityEngine.Vector3 forward = a + vector;
				global::UnityEngine.Quaternion b = global::UnityEngine.Quaternion.LookRotation(forward, global::UnityEngine.Vector3.up);
				global::UnityEngine.Quaternion rot = global::UnityEngine.Quaternion.Lerp(base.GetComponent<global::UnityEngine.Rigidbody>().rotation, b, 7f * global::UnityEngine.Time.fixedDeltaTime);
				base.GetComponent<global::UnityEngine.Rigidbody>().MoveRotation(rot);
				component.SetFloat(global::AnimatorIds.speed, forward.magnitude, 0.1f, global::UnityEngine.Time.fixedDeltaTime);
			}
			else
			{
				component.SetFloat(global::AnimatorIds.speed, 0f);
			}
		}
	}

	private void OnSerializeNetworkView(global::UnityEngine.BitStream stream, global::UnityEngine.NetworkMessageInfo info)
	{
		global::UnityEngine.Animator component = base.GetComponent<global::UnityEngine.Animator>();
		float value = 0f;
		global::UnityEngine.Quaternion rotation = base.transform.rotation;
		if (stream.isWriting)
		{
			value = component.GetFloat(global::AnimatorIds.speed);
			stream.Serialize(ref value);
			stream.Serialize(ref rotation);
			global::UnityEngine.Vector3 position = base.transform.position;
			stream.Serialize(ref position);
		}
		else
		{
			stream.Serialize(ref value);
			component.SetFloat(global::AnimatorIds.speed, value);
			stream.Serialize(ref rotation);
			base.transform.rotation = rotation;
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			stream.Serialize(ref zero);
			base.transform.position = zero;
		}
	}
}
