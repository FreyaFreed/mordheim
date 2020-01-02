using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO
{
	[global::UnityEngine.AddComponentMenu("Pathfinding/Local Avoidance/RVO Navmesh")]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_navmesh.php")]
	public class RVONavmesh : global::Pathfinding.GraphModifier
	{
		public override void OnPostCacheLoad()
		{
			this.OnLatePostScan();
		}

		public override void OnLatePostScan()
		{
			if (!global::UnityEngine.Application.isPlaying)
			{
				return;
			}
			this.RemoveObstacles();
			global::Pathfinding.NavGraph[] graphs = global::AstarPath.active.graphs;
			global::Pathfinding.RVO.RVOSimulator rvosimulator = global::UnityEngine.Object.FindObjectOfType<global::Pathfinding.RVO.RVOSimulator>();
			if (rvosimulator == null)
			{
				throw new global::System.NullReferenceException("No RVOSimulator could be found in the scene. Please add one to any GameObject");
			}
			global::Pathfinding.RVO.Simulator simulator = rvosimulator.GetSimulator();
			for (int i = 0; i < graphs.Length; i++)
			{
				global::Pathfinding.RecastGraph recastGraph = graphs[i] as global::Pathfinding.RecastGraph;
				if (recastGraph != null)
				{
					foreach (global::Pathfinding.RecastGraph.NavmeshTile ng in recastGraph.GetTiles())
					{
						this.AddGraphObstacles(simulator, ng);
					}
				}
				else
				{
					global::Pathfinding.INavmesh navmesh = graphs[i] as global::Pathfinding.INavmesh;
					if (navmesh != null)
					{
						this.AddGraphObstacles(simulator, navmesh);
					}
				}
			}
			simulator.UpdateObstacles();
		}

		public void RemoveObstacles()
		{
			if (this.lastSim == null)
			{
				return;
			}
			global::Pathfinding.RVO.Simulator simulator = this.lastSim;
			this.lastSim = null;
			for (int i = 0; i < this.obstacles.Count; i++)
			{
				simulator.RemoveObstacle(this.obstacles[i]);
			}
			this.obstacles.Clear();
		}

		public void AddGraphObstacles(global::Pathfinding.RVO.Simulator sim, global::Pathfinding.INavmesh ng)
		{
			if (this.obstacles.Count > 0 && this.lastSim != null && this.lastSim != sim)
			{
				global::UnityEngine.Debug.LogError("Simulator has changed but some old obstacles are still added for the previous simulator. Deleting previous obstacles.");
				this.RemoveObstacles();
			}
			this.lastSim = sim;
			int[] uses = new int[20];
			global::System.Collections.Generic.Dictionary<int, int> outline = new global::System.Collections.Generic.Dictionary<int, int>();
			global::System.Collections.Generic.Dictionary<int, global::Pathfinding.Int3> vertexPositions = new global::System.Collections.Generic.Dictionary<int, global::Pathfinding.Int3>();
			global::System.Collections.Generic.HashSet<int> hasInEdge = new global::System.Collections.Generic.HashSet<int>();
			ng.GetNodes(delegate(global::Pathfinding.GraphNode _node)
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode = _node as global::Pathfinding.TriangleMeshNode;
				uses[0] = (uses[1] = (uses[2] = 0));
				if (triangleMeshNode != null)
				{
					for (int j = 0; j < triangleMeshNode.connections.Length; j++)
					{
						global::Pathfinding.TriangleMeshNode triangleMeshNode2 = triangleMeshNode.connections[j] as global::Pathfinding.TriangleMeshNode;
						if (triangleMeshNode2 != null)
						{
							int num3 = triangleMeshNode.SharedEdge(triangleMeshNode2);
							if (num3 != -1)
							{
								uses[num3] = 1;
							}
						}
					}
					for (int k = 0; k < 3; k++)
					{
						if (uses[k] == 0)
						{
							int i2 = k;
							int i3 = (k + 1) % triangleMeshNode.GetVertexCount();
							outline[triangleMeshNode.GetVertexIndex(i2)] = triangleMeshNode.GetVertexIndex(i3);
							hasInEdge.Add(triangleMeshNode.GetVertexIndex(i3));
							vertexPositions[triangleMeshNode.GetVertexIndex(i2)] = triangleMeshNode.GetVertex(i2);
							vertexPositions[triangleMeshNode.GetVertexIndex(i3)] = triangleMeshNode.GetVertex(i3);
						}
					}
				}
				return true;
			});
			for (int i = 0; i < 2; i++)
			{
				bool flag = i == 1;
				foreach (int num in new global::System.Collections.Generic.List<int>(outline.Keys))
				{
					if (flag || !hasInEdge.Contains(num))
					{
						int key = num;
						global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
						list.Add((global::UnityEngine.Vector3)vertexPositions[key]);
						while (outline.ContainsKey(key))
						{
							int num2 = outline[key];
							outline.Remove(key);
							global::UnityEngine.Vector3 item = (global::UnityEngine.Vector3)vertexPositions[num2];
							list.Add(item);
							if (num2 == num)
							{
								break;
							}
							key = num2;
						}
						if (list.Count > 1)
						{
							sim.AddObstacle(list.ToArray(), this.wallHeight, flag);
						}
					}
				}
			}
		}

		public float wallHeight = 5f;

		private readonly global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex> obstacles = new global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex>();

		private global::Pathfinding.RVO.Simulator lastSim;
	}
}
