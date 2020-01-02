using System;
using UnityEngine;

public class SearchNode : global::UnityEngine.MonoBehaviour
{
	public bool IsOfType(global::SearchNodeId id)
	{
		return (this.types & 1 << (int)id) != 0;
	}

	private void OnDrawGizmos()
	{
		global::UnityEngine.Gizmos.DrawIcon(base.transform.position + new global::UnityEngine.Vector3(0f, 1f, 0f), "search.png", true);
		if (this.IsOfType(global::SearchNodeId.WYRDSTONE))
		{
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.green;
			global::PandoraUtils.DrawFacingGizmoCube(base.transform, 0.25f, 0.375f, 0.5f, 0f, 0f, true);
		}
		else if (this.IsOfType(global::SearchNodeId.SEARCH))
		{
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.yellow;
			if (this.IsOfType(global::SearchNodeId.INDOOR))
			{
				global::PandoraUtils.DrawFacingGizmoCube(base.transform, 0.47f, 0.242f, 0.54f, 0f, 0f, true);
				global::PandoraUtils.DrawFacingGizmoCube(base.transform, 1f, 0.5f, 0.5f, 0f, 0.783f, true);
			}
			else if (this.IsOfType(global::SearchNodeId.OUTDOOR))
			{
				global::PandoraUtils.DrawFacingGizmoCube(base.transform, 0.45f, 1.03f, 0.4f, 0.13f, -0.07f, true);
				global::PandoraUtils.DrawFacingGizmoCube(base.transform, 0.5f, 1.35f, 0.705f, 0.13f, 0f, false);
			}
		}
	}

	public int types;

	public bool forceInit;

	[global::UnityEngine.HideInInspector]
	public bool claimed;
}
