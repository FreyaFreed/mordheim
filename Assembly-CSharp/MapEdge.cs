using System;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class MapEdge : global::UnityEngine.MonoBehaviour
{
	private void OnDrawGizmos()
	{
		global::UnityEngine.Gizmos.DrawIcon(base.transform.position + new global::UnityEngine.Vector3(0f, 50f, 0f), "map_edge.tga", true);
		global::UnityEngine.Gizmos.color = global::UnityEngine.Color.white;
		global::PandoraUtils.DrawFacingGizmoCube(base.transform, 50f, 0.25f, 0.25f, 0f, 0f, false);
	}

	public int idx;
}
