using System;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.SphereCollider))]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Rigidbody))]
[global::UnityEngine.AddComponentMenu("")]
public class AmplifyColorTriggerProxy : global::AmplifyColorTriggerProxyBase
{
	private void Start()
	{
		this.sphereCollider = base.GetComponent<global::UnityEngine.SphereCollider>();
		this.sphereCollider.radius = 0.01f;
		this.sphereCollider.isTrigger = true;
		this.rigidBody = base.GetComponent<global::UnityEngine.Rigidbody>();
		this.rigidBody.useGravity = false;
		this.rigidBody.isKinematic = true;
	}

	private void LateUpdate()
	{
		base.transform.position = this.Reference.position;
		base.transform.rotation = this.Reference.rotation;
	}

	private global::UnityEngine.SphereCollider sphereCollider;

	private global::UnityEngine.Rigidbody rigidBody;
}
