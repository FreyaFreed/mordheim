using System;
using Prometheus;
using UnityEngine;

public class ApolloObjectRotate : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.objectTrans = base.GetComponentsInChildren<global::UnityEngine.Transform>(true);
		global::CameraManager component = global::UnityEngine.Camera.main.gameObject.GetComponent<global::CameraManager>();
		component.SetDOFTarget(base.transform, 0.5f);
		for (int i = 0; i < base.transform.childCount; i++)
		{
			global::Prometheus.Rotate rotate = base.transform.GetChild(i).gameObject.AddComponent<global::Prometheus.Rotate>();
			rotate.useWorldSpace = false;
			rotate.rotPerSec = this.RotationPerSecond;
		}
	}

	public global::UnityEngine.Vector3 RotationPerSecond = new global::UnityEngine.Vector3(0f, 360f, 0f);

	private global::UnityEngine.Transform[] objectTrans;
}
