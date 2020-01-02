using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.AddComponentMenu("Pathfinding/Navmesh/RecastTileUpdateHandler")]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_recast_tile_update_handler.php")]
	public class RecastTileUpdateHandler : global::UnityEngine.MonoBehaviour
	{
		public void SetGraph(global::Pathfinding.RecastGraph graph)
		{
			this.graph = graph;
			if (graph == null)
			{
				return;
			}
			this.dirtyTiles = new bool[graph.tileXCount * graph.tileZCount];
			this.anyDirtyTiles = false;
		}

		public void ScheduleUpdate(global::UnityEngine.Bounds bounds)
		{
			if (this.graph == null)
			{
				if (global::AstarPath.active != null)
				{
					this.SetGraph(global::AstarPath.active.astarData.recastGraph);
				}
				if (this.graph == null)
				{
					global::UnityEngine.Debug.LogError("Received tile update request (from RecastTileUpdate), but no RecastGraph could be found to handle it");
					return;
				}
			}
			int num = global::UnityEngine.Mathf.CeilToInt(this.graph.characterRadius / this.graph.cellSize);
			int num2 = num + 3;
			bounds.Expand(new global::UnityEngine.Vector3((float)num2, 0f, (float)num2) * this.graph.cellSize * 2f);
			global::Pathfinding.IntRect touchingTiles = this.graph.GetTouchingTiles(bounds);
			if (touchingTiles.Width * touchingTiles.Height > 0)
			{
				if (!this.anyDirtyTiles)
				{
					this.earliestDirty = global::UnityEngine.Time.time;
					this.anyDirtyTiles = true;
				}
				for (int i = touchingTiles.ymin; i <= touchingTiles.ymax; i++)
				{
					for (int j = touchingTiles.xmin; j <= touchingTiles.xmax; j++)
					{
						this.dirtyTiles[i * this.graph.tileXCount + j] = true;
					}
				}
			}
		}

		private void OnEnable()
		{
			global::Pathfinding.RecastTileUpdate.OnNeedUpdates += this.ScheduleUpdate;
		}

		private void OnDisable()
		{
			global::Pathfinding.RecastTileUpdate.OnNeedUpdates -= this.ScheduleUpdate;
		}

		private void Update()
		{
			if (this.anyDirtyTiles && global::UnityEngine.Time.time - this.earliestDirty >= this.maxThrottlingDelay && this.graph != null)
			{
				this.UpdateDirtyTiles();
			}
		}

		public void UpdateDirtyTiles()
		{
			if (this.graph == null)
			{
				new global::System.InvalidOperationException("No graph is set on this object");
			}
			if (this.graph.tileXCount * this.graph.tileZCount != this.dirtyTiles.Length)
			{
				global::UnityEngine.Debug.LogError("Graph has changed dimensions. Clearing queued graph updates and resetting.");
				this.SetGraph(this.graph);
				return;
			}
			for (int i = 0; i < this.graph.tileZCount; i++)
			{
				for (int j = 0; j < this.graph.tileXCount; j++)
				{
					if (this.dirtyTiles[i * this.graph.tileXCount + j])
					{
						this.dirtyTiles[i * this.graph.tileXCount + j] = false;
						global::UnityEngine.Bounds tileBounds = this.graph.GetTileBounds(j, i, 1, 1);
						tileBounds.extents *= 0.5f;
						global::Pathfinding.GraphUpdateObject graphUpdateObject = new global::Pathfinding.GraphUpdateObject(tileBounds);
						graphUpdateObject.nnConstraint.graphMask = 1 << (int)this.graph.graphIndex;
						global::AstarPath.active.UpdateGraphs(graphUpdateObject);
					}
				}
			}
			this.anyDirtyTiles = false;
		}

		private global::Pathfinding.RecastGraph graph;

		private bool[] dirtyTiles;

		private bool anyDirtyTiles;

		private float earliestDirty = float.NegativeInfinity;

		public float maxThrottlingDelay = 0.5f;
	}
}
