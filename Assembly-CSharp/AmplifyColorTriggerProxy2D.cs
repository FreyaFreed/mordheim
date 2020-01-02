using System;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("")]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.CircleCollider2D))]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Rigidbody2D))]
public class AmplifyColorTriggerProxy2D : global::AmplifyColorTriggerProxyBase
{
	private void Start()
	{
		this.circleCollider = base.GetComponent<global::UnityEngine.CircleCollider2D>();
		this.circleCollider.radius = 0.01f;
		this.circleCollider.isTrigger = true;
		this.rigidBody = base.GetComponent<global::UnityEngine.Rigidbody2D>();
		this.rigidBody.gravityScale = 0f;
		this.rigidBody.isKinematic = true;
	}

	private void LateUpdate()
	{
		base.transform.position = this.Reference.position;
		base.transform.rotation = this.Reference.rotation;
	}

	private global::UnityEngine.CircleCollider2D circleCollider;

	private global::UnityEngine.Rigidbody2D rigidBody;
}
