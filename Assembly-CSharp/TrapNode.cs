using System;
using UnityEngine;

public class TrapNode : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		base.gameObject.AddComponent<global::MeshBatcherBlocker>();
	}

	private void OnDrawGizmos()
	{
		global::UnityEngine.Gizmos.DrawIcon(base.transform.position + new global::UnityEngine.Vector3(0f, 0.5f, 0f), "traps.tga", true);
	}

	public global::TrapTypeId typeId;

	public bool forceInactive;
}
