using System;
using UnityEngine;

public class ActionNode : global::UnityEngine.MonoBehaviour
{
	public void Init()
	{
		this.zone = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.zone);
		this.zone.transform.parent = base.gameObject.transform.parent;
		this.zone.transform.position = base.gameObject.transform.position;
		this.zone.transform.rotation = base.gameObject.transform.rotation;
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDrawGizmos()
	{
		if (this.zone == null)
		{
			return;
		}
		global::UnityEngine.Gizmos.color = global::UnityEngine.Color.cyan;
		if (this.zone.name == "action_zone3")
		{
			global::UnityEngine.Gizmos.DrawWireCube(base.transform.position + new global::UnityEngine.Vector3(0f, 3.5f, 0f), new global::UnityEngine.Vector3(2f, 1f, 2f));
			global::UnityEngine.Gizmos.DrawWireCube(base.transform.position + new global::UnityEngine.Vector3(0f, 0.5f, 0f) + base.transform.forward * 1.35f, new global::UnityEngine.Vector3(2f, 1f, 2f));
		}
		else if (this.zone.name == "action_zone6")
		{
			global::UnityEngine.Gizmos.DrawWireCube(base.transform.position + new global::UnityEngine.Vector3(0f, 6.5f, 0f), new global::UnityEngine.Vector3(2f, 1f, 2f));
			global::UnityEngine.Gizmos.DrawWireCube(base.transform.position + new global::UnityEngine.Vector3(0f, 3.5f, 0f) + base.transform.forward * 1.35f, new global::UnityEngine.Vector3(2f, 1f, 2f));
			global::UnityEngine.Gizmos.DrawWireCube(base.transform.position + new global::UnityEngine.Vector3(0f, 0.5f, 0f) + base.transform.forward * 1.35f, new global::UnityEngine.Vector3(2f, 1f, 2f));
		}
		else
		{
			global::UnityEngine.Gizmos.DrawWireCube(base.transform.position + new global::UnityEngine.Vector3(0f, 9.5f, 0f), new global::UnityEngine.Vector3(2f, 1f, 2f));
			global::UnityEngine.Gizmos.DrawWireCube(base.transform.position + new global::UnityEngine.Vector3(0f, 6.5f, 0f) + base.transform.forward * 1.35f, new global::UnityEngine.Vector3(2f, 1f, 2f));
			global::UnityEngine.Gizmos.DrawWireCube(base.transform.position + new global::UnityEngine.Vector3(0f, 3.5f, 0f) + base.transform.forward * 1.35f, new global::UnityEngine.Vector3(2f, 1f, 2f));
			global::UnityEngine.Gizmos.DrawWireCube(base.transform.position + new global::UnityEngine.Vector3(0f, 0.5f, 0f) + base.transform.forward * 1.35f, new global::UnityEngine.Vector3(2f, 1f, 2f));
		}
	}

	public global::UnityEngine.GameObject zone;
}
