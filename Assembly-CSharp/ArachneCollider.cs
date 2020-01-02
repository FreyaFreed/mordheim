using System;
using UnityEngine;

public class ArachneCollider : global::UnityEngine.MonoBehaviour
{
	private void OnDrawGizmos()
	{
		this.position = base.transform.position + base.transform.rotation * this.center;
		global::UnityEngine.Gizmos.color = global::UnityEngine.Color.blue;
		global::UnityEngine.Gizmos.DrawWireSphere(this.position, this.radius);
	}

	private void Update()
	{
		this.position = base.transform.position + base.transform.rotation * this.center;
	}

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Vector3 center;

	public float radius;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Vector3 position;
}
