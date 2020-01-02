using System;
using UnityEngine;

public class Gizmo : global::UnityEngine.MonoBehaviour
{
	private void OnDrawGizmos()
	{
		global::UnityEngine.Gizmos.DrawIcon(base.transform.position + this.offset, this.iconName);
	}

	public string iconName;

	public global::UnityEngine.Vector3 offset = new global::UnityEngine.Vector3(0f, 1f, 0f);
}
