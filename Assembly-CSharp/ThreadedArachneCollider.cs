using System;
using UnityEngine;

public struct ThreadedArachneCollider
{
	public ThreadedArachneCollider(global::ArachneCollider collider)
	{
		this.position = collider.position;
		this.radius = collider.radius;
		this.sqRadius = this.radius * this.radius;
	}

	public global::UnityEngine.Vector3 position;

	public float radius;

	public float sqRadius;
}
