using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.AddComponentMenu("Pathfinding/Navmesh/RecastTileUpdate")]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_recast_tile_update.php")]
	public class RecastTileUpdate : global::UnityEngine.MonoBehaviour
	{
		public static event global::System.Action<global::UnityEngine.Bounds> OnNeedUpdates;

		private void Start()
		{
			this.ScheduleUpdate();
		}

		private void OnDestroy()
		{
			this.ScheduleUpdate();
		}

		public void ScheduleUpdate()
		{
			global::UnityEngine.Collider component = base.GetComponent<global::UnityEngine.Collider>();
			if (component != null)
			{
				if (global::Pathfinding.RecastTileUpdate.OnNeedUpdates != null)
				{
					global::Pathfinding.RecastTileUpdate.OnNeedUpdates(component.bounds);
				}
			}
			else if (global::Pathfinding.RecastTileUpdate.OnNeedUpdates != null)
			{
				global::Pathfinding.RecastTileUpdate.OnNeedUpdates(new global::UnityEngine.Bounds(base.transform.position, global::UnityEngine.Vector3.zero));
			}
		}
	}
}
