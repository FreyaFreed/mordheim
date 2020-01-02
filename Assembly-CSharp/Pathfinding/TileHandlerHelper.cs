using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_tile_handler_helper.php")]
	public class TileHandlerHelper : global::UnityEngine.MonoBehaviour
	{
		public void UseSpecifiedHandler(global::Pathfinding.Util.TileHandler handler)
		{
			this.handler = handler;
		}

		private void OnEnable()
		{
			global::Pathfinding.NavmeshCut.OnDestroyCallback += this.HandleOnDestroyCallback;
			if (this.handler != null)
			{
				this.handler.graph.OnRecalculatedTiles += this.OnRecalculatedTiles;
			}
		}

		private void OnDisable()
		{
			global::Pathfinding.NavmeshCut.OnDestroyCallback -= this.HandleOnDestroyCallback;
			if (this.handler != null)
			{
				this.handler.graph.OnRecalculatedTiles -= this.OnRecalculatedTiles;
			}
		}

		public void DiscardPending()
		{
			global::System.Collections.Generic.List<global::Pathfinding.NavmeshCut> all = global::Pathfinding.NavmeshCut.GetAll();
			for (int i = 0; i < all.Count; i++)
			{
				if (all[i].RequiresUpdate())
				{
					all[i].NotifyUpdated();
				}
			}
		}

		private void Awake()
		{
			if (global::UnityEngine.Object.FindObjectsOfType(typeof(global::Pathfinding.TileHandlerHelper)).Length > 1)
			{
				global::UnityEngine.Debug.LogError("There should only be one TileHandlerHelper per scene. Destroying.");
				global::UnityEngine.Object.Destroy(this);
				return;
			}
			if (this.handler == null)
			{
				if (global::AstarPath.active == null || global::AstarPath.active.astarData.recastGraph == null)
				{
					global::UnityEngine.Debug.LogWarning("No AstarPath object in the scene or no RecastGraph on that AstarPath object");
				}
				global::Pathfinding.RecastGraph recastGraph = global::AstarPath.active.astarData.recastGraph;
				this.handler = new global::Pathfinding.Util.TileHandler(recastGraph);
				recastGraph.OnRecalculatedTiles += this.OnRecalculatedTiles;
				this.handler.CreateTileTypesFromGraph();
			}
		}

		private void OnRecalculatedTiles(global::Pathfinding.RecastGraph.NavmeshTile[] tiles)
		{
			if (this.handler != null)
			{
				if (!this.handler.isValid)
				{
					this.handler = new global::Pathfinding.Util.TileHandler(this.handler.graph);
				}
				this.handler.OnRecalculatedTiles(tiles);
			}
		}

		private void HandleOnDestroyCallback(global::Pathfinding.NavmeshCut obj)
		{
			this.forcedReloadBounds.Add(obj.LastBounds);
			this.lastUpdateTime = -999f;
		}

		private void Update()
		{
			if (this.handler == null || ((this.updateInterval == -1f || global::UnityEngine.Time.realtimeSinceStartup - this.lastUpdateTime < this.updateInterval) && this.handler.isValid) || global::AstarPath.active.isScanning)
			{
				return;
			}
			this.ForceUpdate();
		}

		public void ForceUpdate()
		{
			if (this.handler == null)
			{
				throw new global::System.Exception("Cannot update graphs. No TileHandler. Do not call this method in Awake.");
			}
			this.lastUpdateTime = global::UnityEngine.Time.realtimeSinceStartup;
			global::System.Collections.Generic.List<global::Pathfinding.NavmeshCut> all = global::Pathfinding.NavmeshCut.GetAll();
			if (!this.handler.isValid)
			{
				global::UnityEngine.Debug.Log("TileHandler no longer matched the underlaying RecastGraph (possibly because of a graph scan). Recreating TileHandler...");
				this.handler = new global::Pathfinding.Util.TileHandler(this.handler.graph);
				this.handler.CreateTileTypesFromGraph();
				this.forcedReloadBounds.Add(new global::UnityEngine.Bounds(global::UnityEngine.Vector3.zero, new global::UnityEngine.Vector3(1E+07f, 1E+07f, 1E+07f)));
			}
			if (this.forcedReloadBounds.Count == 0)
			{
				int num = 0;
				for (int i = 0; i < all.Count; i++)
				{
					if (all[i].RequiresUpdate())
					{
						num++;
						break;
					}
				}
				if (num == 0)
				{
					return;
				}
			}
			bool flag = this.handler.StartBatchLoad();
			for (int j = 0; j < this.forcedReloadBounds.Count; j++)
			{
				this.handler.ReloadInBounds(this.forcedReloadBounds[j]);
			}
			this.forcedReloadBounds.Clear();
			for (int k = 0; k < all.Count; k++)
			{
				if (all[k].enabled)
				{
					if (all[k].RequiresUpdate())
					{
						this.handler.ReloadInBounds(all[k].LastBounds);
						this.handler.ReloadInBounds(all[k].GetBounds());
					}
				}
				else if (all[k].RequiresUpdate())
				{
					this.handler.ReloadInBounds(all[k].LastBounds);
				}
			}
			for (int l = 0; l < all.Count; l++)
			{
				if (all[l].RequiresUpdate())
				{
					all[l].NotifyUpdated();
				}
			}
			if (flag)
			{
				this.handler.EndBatchLoad();
			}
		}

		private global::Pathfinding.Util.TileHandler handler;

		public float updateInterval;

		private float lastUpdateTime = -999f;

		private readonly global::System.Collections.Generic.List<global::UnityEngine.Bounds> forcedReloadBounds = new global::System.Collections.Generic.List<global::UnityEngine.Bounds>();
	}
}
