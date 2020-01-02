using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class MordheimTileHandlerHelper : global::UnityEngine.MonoBehaviour
	{
		private void OnEnable()
		{
			global::Pathfinding.NavmeshCut.OnDestroyCallback += this.HandleOnDestroyCallback;
		}

		private void OnDisable()
		{
			global::Pathfinding.NavmeshCut.OnDestroyCallback -= this.HandleOnDestroyCallback;
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
			this.handlers = new global::System.Collections.Generic.List<global::Pathfinding.Util.TileHandler>();
			if (global::AstarPath.active == null || global::AstarPath.active.astarData.recastGraph == null)
			{
				global::UnityEngine.Debug.LogWarning("No AstarPath object in the scene or no RecastGraph on that AstarPath object");
			}
			foreach (object obj in global::AstarPath.active.astarData.FindGraphsOfType(typeof(global::Pathfinding.RecastGraph)))
			{
				global::Pathfinding.RecastGraph graph = (global::Pathfinding.RecastGraph)obj;
				global::Pathfinding.Util.TileHandler tileHandler = new global::Pathfinding.Util.TileHandler(graph);
				tileHandler.CreateTileTypesFromGraph();
				this.handlers.Add(tileHandler);
			}
		}

		private void HandleOnDestroyCallback(global::Pathfinding.NavmeshCut obj)
		{
			this.forcedReloadBounds.Add(obj.LastBounds);
			this.lastUpdateTime = -999f;
		}

		private void Update()
		{
			if (this.updateInterval == -1f || global::UnityEngine.Time.realtimeSinceStartup - this.lastUpdateTime < this.updateInterval || this.handlers.Count == 0)
			{
				return;
			}
			this.ForceUpdate();
		}

		public void ForceUpdate()
		{
			global::AstarPath.active.maxGraphUpdateFreq = 1.5f;
			if (this.handlers.Count == 0)
			{
				throw new global::System.Exception("Cannot update graphs. No TileHandler. Do not call this method in Awake.");
			}
			this.lastUpdateTime = global::UnityEngine.Time.realtimeSinceStartup;
			global::System.Collections.Generic.List<global::Pathfinding.NavmeshCut> all = global::Pathfinding.NavmeshCut.GetAll();
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
			for (int j = 0; j < this.handlers.Count; j++)
			{
				global::Pathfinding.Util.TileHandler tileHandler = this.handlers[j];
				bool flag = tileHandler.StartBatchLoad();
				if (flag)
				{
					for (int k = 0; k < this.forcedReloadBounds.Count; k++)
					{
						tileHandler.ReloadInBounds(this.forcedReloadBounds[k]);
					}
					this.forcedReloadBounds.Clear();
					for (int l = 0; l < all.Count; l++)
					{
						if (all[l].enabled)
						{
							if (all[l].RequiresUpdate())
							{
								tileHandler.ReloadInBounds(all[l].LastBounds);
								tileHandler.ReloadInBounds(all[l].GetBounds());
							}
						}
						else if (all[l].RequiresUpdate())
						{
							tileHandler.ReloadInBounds(all[l].LastBounds);
						}
					}
					if (flag)
					{
						tileHandler.EndBatchLoad();
					}
				}
			}
			for (int m = 0; m < all.Count; m++)
			{
				if (all[m].RequiresUpdate())
				{
					all[m].NotifyUpdated();
				}
			}
		}

		private global::System.Collections.Generic.List<global::Pathfinding.Util.TileHandler> handlers;

		public float updateInterval;

		private float lastUpdateTime = -999f;

		private global::System.Collections.Generic.List<global::UnityEngine.Bounds> forcedReloadBounds = new global::System.Collections.Generic.List<global::UnityEngine.Bounds>();
	}
}
