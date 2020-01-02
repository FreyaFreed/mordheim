using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Pathfinding.WindowsStore;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	[global::System.Serializable]
	public class AstarData
	{
		public static global::AstarPath active
		{
			get
			{
				return global::AstarPath.active;
			}
		}

		public global::Pathfinding.NavMeshGraph navmesh { get; private set; }

		public global::Pathfinding.GridGraph gridGraph { get; private set; }

		public global::Pathfinding.LayerGridGraph layerGridGraph { get; private set; }

		public global::Pathfinding.PointGraph pointGraph { get; private set; }

		public global::Pathfinding.RecastGraph recastGraph { get; private set; }

		public global::System.Type[] graphTypes { get; private set; }

		private byte[] data
		{
			get
			{
				if (this.upgradeData != null && this.upgradeData.Length > 0)
				{
					this.data = this.upgradeData;
					this.upgradeData = null;
				}
				return (this.dataString == null) ? null : global::System.Convert.FromBase64String(this.dataString);
			}
			set
			{
				this.dataString = ((value == null) ? null : global::System.Convert.ToBase64String(value));
			}
		}

		public byte[] GetData()
		{
			return this.data;
		}

		public void SetData(byte[] data)
		{
			this.data = data;
		}

		public void Awake()
		{
			this.graphs = new global::Pathfinding.NavGraph[0];
			if (this.cacheStartup && this.file_cachedStartup != null)
			{
				this.LoadFromCache();
			}
			else
			{
				this.DeserializeGraphs();
			}
		}

		public void UpdateShortcuts()
		{
			this.navmesh = (global::Pathfinding.NavMeshGraph)this.FindGraphOfType(typeof(global::Pathfinding.NavMeshGraph));
			this.gridGraph = (global::Pathfinding.GridGraph)this.FindGraphOfType(typeof(global::Pathfinding.GridGraph));
			this.layerGridGraph = (global::Pathfinding.LayerGridGraph)this.FindGraphOfType(typeof(global::Pathfinding.LayerGridGraph));
			this.pointGraph = (global::Pathfinding.PointGraph)this.FindGraphOfType(typeof(global::Pathfinding.PointGraph));
			this.recastGraph = (global::Pathfinding.RecastGraph)this.FindGraphOfType(typeof(global::Pathfinding.RecastGraph));
		}

		public void LoadFromCache()
		{
			global::AstarPath.active.BlockUntilPathQueueBlocked();
			if (this.file_cachedStartup != null)
			{
				byte[] bytes = this.file_cachedStartup.bytes;
				this.DeserializeGraphs(bytes);
				global::Pathfinding.GraphModifier.TriggerEvent(global::Pathfinding.GraphModifier.EventType.PostCacheLoad);
			}
			else
			{
				global::UnityEngine.Debug.LogError("Can't load from cache since the cache is empty");
			}
		}

		public byte[] SerializeGraphs()
		{
			return this.SerializeGraphs(global::Pathfinding.Serialization.SerializeSettings.Settings);
		}

		public byte[] SerializeGraphs(global::Pathfinding.Serialization.SerializeSettings settings)
		{
			uint num;
			return this.SerializeGraphs(settings, out num);
		}

		public byte[] SerializeGraphs(global::Pathfinding.Serialization.SerializeSettings settings, out uint checksum)
		{
			global::AstarPath.active.BlockUntilPathQueueBlocked();
			global::Pathfinding.Serialization.AstarSerializer astarSerializer = new global::Pathfinding.Serialization.AstarSerializer(this, settings);
			astarSerializer.OpenSerialize();
			this.SerializeGraphsPart(astarSerializer);
			byte[] result = astarSerializer.CloseSerialize();
			checksum = astarSerializer.GetChecksum();
			return result;
		}

		public void SerializeGraphsPart(global::Pathfinding.Serialization.AstarSerializer sr)
		{
			sr.SerializeGraphs(this.graphs);
			sr.SerializeExtraInfo();
		}

		public void DeserializeGraphs()
		{
			if (this.data != null)
			{
				this.DeserializeGraphs(this.data);
			}
		}

		private void ClearGraphs()
		{
			if (this.graphs == null)
			{
				return;
			}
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					this.graphs[i].OnDestroy();
				}
			}
			this.graphs = null;
			this.UpdateShortcuts();
		}

		public void OnDestroy()
		{
			this.ClearGraphs();
		}

		public void DeserializeGraphs(byte[] bytes)
		{
			global::AstarPath.active.BlockUntilPathQueueBlocked();
			this.ClearGraphs();
			this.DeserializeGraphsAdditive(bytes);
		}

		public void DeserializeGraphsAdditive(byte[] bytes)
		{
			global::AstarPath.active.BlockUntilPathQueueBlocked();
			try
			{
				if (bytes == null)
				{
					throw new global::System.ArgumentNullException("bytes");
				}
				global::Pathfinding.Serialization.AstarSerializer astarSerializer = new global::Pathfinding.Serialization.AstarSerializer(this);
				if (astarSerializer.OpenDeserialize(bytes))
				{
					this.DeserializeGraphsPartAdditive(astarSerializer);
					astarSerializer.CloseDeserialize();
					this.UpdateShortcuts();
				}
				else
				{
					global::UnityEngine.Debug.Log("Invalid data file (cannot read zip).\nThe data is either corrupt or it was saved using a 3.0.x or earlier version of the system");
				}
				global::Pathfinding.AstarData.active.VerifyIntegrity();
			}
			catch (global::System.Exception arg)
			{
				global::UnityEngine.Debug.LogError("Caught exception while deserializing data.\n" + arg);
				this.graphs = new global::Pathfinding.NavGraph[0];
				this.data_backup = bytes;
			}
		}

		public void DeserializeGraphsPart(global::Pathfinding.Serialization.AstarSerializer sr)
		{
			this.ClearGraphs();
			this.DeserializeGraphsPartAdditive(sr);
		}

		public void DeserializeGraphsPartAdditive(global::Pathfinding.Serialization.AstarSerializer sr)
		{
			if (this.graphs == null)
			{
				this.graphs = new global::Pathfinding.NavGraph[0];
			}
			global::System.Collections.Generic.List<global::Pathfinding.NavGraph> list = new global::System.Collections.Generic.List<global::Pathfinding.NavGraph>(this.graphs);
			sr.SetGraphIndexOffset(list.Count);
			list.AddRange(sr.DeserializeGraphs());
			this.graphs = list.ToArray();
			sr.DeserializeExtraInfo();
			int i;
			for (i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					this.graphs[i].GetNodes(delegate(global::Pathfinding.GraphNode node)
					{
						node.GraphIndex = (uint)i;
						return true;
					});
				}
			}
			for (int k = 0; k < this.graphs.Length; k++)
			{
				for (int j = k + 1; j < this.graphs.Length; j++)
				{
					if (this.graphs[k] != null && this.graphs[j] != null && this.graphs[k].guid == this.graphs[j].guid)
					{
						global::UnityEngine.Debug.LogWarning("Guid Conflict when importing graphs additively. Imported graph will get a new Guid.\nThis message is (relatively) harmless.");
						this.graphs[k].guid = global::Pathfinding.Util.Guid.NewGuid();
						break;
					}
				}
			}
			sr.PostDeserialization();
		}

		public void FindGraphTypes()
		{
			global::System.Reflection.Assembly assembly = global::Pathfinding.WindowsStore.WindowsStoreCompatibility.GetTypeInfo(typeof(global::AstarPath)).Assembly;
			global::System.Type[] types = assembly.GetTypes();
			global::System.Collections.Generic.List<global::System.Type> list = new global::System.Collections.Generic.List<global::System.Type>();
			foreach (global::System.Type type in types)
			{
				for (global::System.Type baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
				{
					if (object.Equals(baseType, typeof(global::Pathfinding.NavGraph)))
					{
						list.Add(type);
						break;
					}
				}
			}
			this.graphTypes = list.ToArray();
		}

		[global::System.Obsolete("If really necessary. Use System.Type.GetType instead.")]
		public global::System.Type GetGraphType(string type)
		{
			for (int i = 0; i < this.graphTypes.Length; i++)
			{
				if (this.graphTypes[i].Name == type)
				{
					return this.graphTypes[i];
				}
			}
			return null;
		}

		[global::System.Obsolete("Use CreateGraph(System.Type) instead")]
		public global::Pathfinding.NavGraph CreateGraph(string type)
		{
			global::UnityEngine.Debug.Log("Creating Graph of type '" + type + "'");
			for (int i = 0; i < this.graphTypes.Length; i++)
			{
				if (this.graphTypes[i].Name == type)
				{
					return this.CreateGraph(this.graphTypes[i]);
				}
			}
			global::UnityEngine.Debug.LogError("Graph type (" + type + ") wasn't found");
			return null;
		}

		public global::Pathfinding.NavGraph CreateGraph(global::System.Type type)
		{
			global::Pathfinding.NavGraph navGraph = global::System.Activator.CreateInstance(type) as global::Pathfinding.NavGraph;
			navGraph.active = global::Pathfinding.AstarData.active;
			return navGraph;
		}

		[global::System.Obsolete("Use AddGraph(System.Type) instead")]
		public global::Pathfinding.NavGraph AddGraph(string type)
		{
			global::Pathfinding.NavGraph navGraph = null;
			for (int i = 0; i < this.graphTypes.Length; i++)
			{
				if (this.graphTypes[i].Name == type)
				{
					navGraph = this.CreateGraph(this.graphTypes[i]);
				}
			}
			if (navGraph == null)
			{
				global::UnityEngine.Debug.LogError("No NavGraph of type '" + type + "' could be found");
				return null;
			}
			this.AddGraph(navGraph);
			return navGraph;
		}

		public global::Pathfinding.NavGraph AddGraph(global::System.Type type)
		{
			global::Pathfinding.NavGraph navGraph = null;
			for (int i = 0; i < this.graphTypes.Length; i++)
			{
				if (object.Equals(this.graphTypes[i], type))
				{
					navGraph = this.CreateGraph(this.graphTypes[i]);
				}
			}
			if (navGraph == null)
			{
				global::UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"No NavGraph of type '",
					type,
					"' could be found, ",
					this.graphTypes.Length,
					" graph types are avaliable"
				}));
				return null;
			}
			this.AddGraph(navGraph);
			return navGraph;
		}

		public void AddGraph(global::Pathfinding.NavGraph graph)
		{
			global::AstarPath.active.BlockUntilPathQueueBlocked();
			bool flag = false;
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] == null)
				{
					this.graphs[i] = graph;
					graph.graphIndex = (uint)i;
					flag = true;
				}
			}
			if (!flag)
			{
				if (this.graphs != null && (long)this.graphs.Length >= 255L)
				{
					throw new global::System.Exception("Graph Count Limit Reached. You cannot have more than " + 255U + " graphs.");
				}
				this.graphs = new global::System.Collections.Generic.List<global::Pathfinding.NavGraph>(this.graphs ?? new global::Pathfinding.NavGraph[0])
				{
					graph
				}.ToArray();
				graph.graphIndex = (uint)(this.graphs.Length - 1);
			}
			this.UpdateShortcuts();
			graph.active = global::Pathfinding.AstarData.active;
			graph.Awake();
		}

		public bool RemoveGraph(global::Pathfinding.NavGraph graph)
		{
			global::Pathfinding.AstarData.active.FlushWorkItemsInternal(false);
			global::Pathfinding.AstarData.active.BlockUntilPathQueueBlocked();
			graph.OnDestroy();
			int num = global::System.Array.IndexOf<global::Pathfinding.NavGraph>(this.graphs, graph);
			if (num == -1)
			{
				return false;
			}
			this.graphs[num] = null;
			this.UpdateShortcuts();
			return true;
		}

		public static global::Pathfinding.NavGraph GetGraph(global::Pathfinding.GraphNode node)
		{
			if (node == null)
			{
				return null;
			}
			global::AstarPath active = global::AstarPath.active;
			if (active == null)
			{
				return null;
			}
			global::Pathfinding.AstarData astarData = active.astarData;
			if (astarData == null || astarData.graphs == null)
			{
				return null;
			}
			uint graphIndex = node.GraphIndex;
			if ((ulong)graphIndex >= (ulong)((long)astarData.graphs.Length))
			{
				return null;
			}
			return astarData.graphs[(int)graphIndex];
		}

		public global::Pathfinding.NavGraph FindGraphOfType(global::System.Type type)
		{
			if (this.graphs != null)
			{
				for (int i = 0; i < this.graphs.Length; i++)
				{
					if (this.graphs[i] != null && object.Equals(this.graphs[i].GetType(), type))
					{
						return this.graphs[i];
					}
				}
			}
			return null;
		}

		public global::System.Collections.IEnumerable FindGraphsOfType(global::System.Type type)
		{
			if (this.graphs == null)
			{
				yield break;
			}
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null && object.Equals(this.graphs[i].GetType(), type))
				{
					yield return this.graphs[i];
				}
			}
			yield break;
		}

		public global::System.Collections.IEnumerable GetUpdateableGraphs()
		{
			if (this.graphs == null)
			{
				yield break;
			}
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] is global::Pathfinding.IUpdatableGraph)
				{
					yield return this.graphs[i];
				}
			}
			yield break;
		}

		[global::System.Obsolete("Obsolete because it is not used by the package internally and the use cases are few. Iterate through the graphs array instead.")]
		public global::System.Collections.IEnumerable GetRaycastableGraphs()
		{
			if (this.graphs == null)
			{
				yield break;
			}
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] is global::Pathfinding.IRaycastableGraph)
				{
					yield return this.graphs[i];
				}
			}
			yield break;
		}

		public int GetGraphIndex(global::Pathfinding.NavGraph graph)
		{
			if (graph == null)
			{
				throw new global::System.ArgumentNullException("graph");
			}
			int num = -1;
			if (this.graphs != null)
			{
				num = global::System.Array.IndexOf<global::Pathfinding.NavGraph>(this.graphs, graph);
				if (num == -1)
				{
					global::UnityEngine.Debug.LogError("Graph doesn't exist");
				}
			}
			return num;
		}

		[global::System.NonSerialized]
		public global::Pathfinding.NavGraph[] graphs = new global::Pathfinding.NavGraph[0];

		[global::UnityEngine.SerializeField]
		private string dataString;

		[global::UnityEngine.SerializeField]
		[global::UnityEngine.Serialization.FormerlySerializedAs("data")]
		private byte[] upgradeData;

		public byte[] data_backup;

		public global::UnityEngine.TextAsset file_cachedStartup;

		public byte[] data_cachedStartup;

		[global::UnityEngine.SerializeField]
		public bool cacheStartup;
	}
}
