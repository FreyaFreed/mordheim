using System;
using UnityEngine;

public class BoundingBox : global::UnityEngine.MonoBehaviour
{
	private void OnDrawGizmosSelected()
	{
		global::UnityEngine.Gizmos.color = global::UnityEngine.Color.yellow;
		global::UnityEngine.Gizmos.DrawWireCube(this.center, this.size);
	}

	public global::UnityEngine.Vector3 size;

	public global::UnityEngine.Vector3 center;
}
