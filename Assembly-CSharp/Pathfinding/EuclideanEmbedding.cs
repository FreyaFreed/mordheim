using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::System.Serializable]
	public class EuclideanEmbedding
	{
		private uint GetRandom()
		{
			this.rval = 12820163U * this.rval + 1140671485U;
			return this.rval;
		}

		private void EnsureCapacity(int index)
		{
			if (index > this.maxNodeIndex)
			{
				object obj = this.lockObj;
				lock (obj)
				{
					if (index > this.maxNodeIndex)
					{
						if (index >= this.costs.Length)
						{
							uint[] array = new uint[global::System.Math.Max(index * 2, this.pivots.Length * 2)];
							for (int i = 0; i < this.costs.Length; i++)
							{
								array[i] = this.costs[i];
							}
							this.costs = array;
						}
						this.maxNodeIndex = index;
					}
				}
			}
		}

		public uint GetHeuristic(int nodeIndex1, int nodeIndex2)
		{
			nodeIndex1 *= this.pivotCount;
			nodeIndex2 *= this.pivotCount;
			if (nodeIndex1 >= this.costs.Length || nodeIndex2 >= this.costs.Length)
			{
				this.EnsureCapacity((nodeIndex1 <= nodeIndex2) ? nodeIndex2 : nodeIndex1);
			}
			uint num = 0U;
			for (int i = 0; i < this.pivotCount; i++)
			{
				uint num2 = (uint)global::System.Math.Abs((int)(this.costs[nodeIndex1 + i] - this.costs[nodeIndex2 + i]));
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		private void GetClosestWalkableNodesToChildrenRecursively(global::UnityEngine.Transform tr, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> nodes)
		{
			foreach (object obj in tr)
			{
				global::UnityEngine.Transform transform = (global::UnityEngine.Transform)obj;
				global::Pathfinding.NNInfo nearest = global::AstarPath.active.GetNearest(transform.position, global::Pathfinding.NNConstraint.Default);
				if (nearest.node != null && nearest.node.Walkable)
				{
					nodes.Add(nearest.node);
				}
				this.GetClosestWalkableNodesToChildrenRecursively(transform, nodes);
			}
		}

		private void PickNRandomNodes(int count, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> buffer)
		{
			int n = 0;
			global::Pathfinding.NavGraph[] graphs = global::AstarPath.active.graphs;
			for (int i = 0; i < graphs.Length; i++)
			{
				graphs[i].GetNodes(delegate(global::Pathfinding.GraphNode node)
				{
					if (!node.Destroyed && node.Walkable)
					{
						n++;
						if ((ulong)this.GetRandom() % (ulong)((long)n) < (ulong)((long)count))
						{
							if (buffer.Count < count)
							{
								buffer.Add(node);
							}
							else
							{
								buffer[(int)((ulong)this.GetRandom() % (ulong)((long)buffer.Count))] = node;
							}
						}
					}
					return true;
				});
			}
		}

		private global::Pathfinding.GraphNode PickAnyWalkableNode()
		{
			global::Pathfinding.NavGraph[] graphs = global::AstarPath.active.graphs;
			global::Pathfinding.GraphNode first = null;
			for (int i = 0; i < graphs.Length; i++)
			{
				graphs[i].GetNodes(delegate(global::Pathfinding.GraphNode node)
				{
					if (node != null && node.Walkable)
					{
						first = node;
						return false;
					}
					return true;
				});
			}
			return first;
		}

		public void RecalculatePivots()
		{
			if (this.mode == global::Pathfinding.HeuristicOptimizationMode.None)
			{
				this.pivotCount = 0;
				this.pivots = null;
				return;
			}
			this.rval = (uint)this.seed;
			global::System.Collections.Generic.List<global::Pathfinding.GraphNode> list = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Claim();
			switch (this.mode)
			{
			case global::Pathfinding.HeuristicOptimizationMode.Random:
				this.PickNRandomNodes(this.spreadOutCount, list);
				break;
			case global::Pathfinding.HeuristicOptimizationMode.RandomSpreadOut:
			{
				if (this.pivotPointRoot != null)
				{
					this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, list);
				}
				if (list.Count == 0)
				{
					global::Pathfinding.GraphNode graphNode = this.PickAnyWalkableNode();
					if (graphNode == null)
					{
						global::UnityEngine.Debug.LogError("Could not find any walkable node in any of the graphs.");
						global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Release(list);
						return;
					}
					list.Add(graphNode);
				}
				int num = this.spreadOutCount - list.Count;
				for (int i = 0; i < num; i++)
				{
					list.Add(null);
				}
				break;
			}
			case global::Pathfinding.HeuristicOptimizationMode.Custom:
				if (this.pivotPointRoot == null)
				{
					throw new global::System.Exception("heuristicOptimizationMode is HeuristicOptimizationMode.Custom, but no 'customHeuristicOptimizationPivotsRoot' is set");
				}
				this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, list);
				break;
			default:
				throw new global::System.Exception("Invalid HeuristicOptimizationMode: " + this.mode);
			}
			this.pivots = list.ToArray();
			global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Release(list);
		}

		public void RecalculateCosts()
		{
			if (this.pivots == null)
			{
				this.RecalculatePivots();
			}
			if (this.mode == global::Pathfinding.HeuristicOptimizationMode.None)
			{
				return;
			}
			this.pivotCount = 0;
			for (int i = 0; i < this.pivots.Length; i++)
			{
				if (this.pivots[i] != null && (this.pivots[i].Destroyed || !this.pivots[i].Walkable))
				{
					throw new global::System.Exception("Invalid pivot nodes (destroyed or unwalkable)");
				}
			}
			if (this.mode != global::Pathfinding.HeuristicOptimizationMode.RandomSpreadOut)
			{
				for (int j = 0; j < this.pivots.Length; j++)
				{
					if (this.pivots[j] == null)
					{
						throw new global::System.Exception("Invalid pivot nodes (null)");
					}
				}
			}
			global::UnityEngine.Debug.Log("Recalculating costs...");
			this.pivotCount = this.pivots.Length;
			global::System.Action<int> startCostCalculation = null;
			int numComplete = 0;
			global::Pathfinding.OnPathDelegate onComplete = delegate(global::Pathfinding.Path path)
			{
				numComplete++;
				if (numComplete == this.pivotCount)
				{
					global::UnityEngine.Debug.Log("Grid graph special case!");
					this.ApplyGridGraphEndpointSpecialCase();
				}
			};
			startCostCalculation = delegate(int k)
			{
				global::Pathfinding.GraphNode pivot = this.pivots[k];
				global::Pathfinding.FloodPath fp = null;
				fp = global::Pathfinding.FloodPath.Construct(pivot, onComplete);
				fp.immediateCallback = delegate(global::Pathfinding.Path _p)
				{
					_p.Claim(this);
					global::Pathfinding.MeshNode meshNode = pivot as global::Pathfinding.MeshNode;
					uint costOffset = 0U;
					int k;
					if (meshNode != null && meshNode.connectionCosts != null)
					{
						for (k = 0; k < meshNode.connectionCosts.Length; k++)
						{
							costOffset = global::System.Math.Max(costOffset, meshNode.connectionCosts[k]);
						}
					}
					global::Pathfinding.NavGraph[] graphs = global::AstarPath.active.graphs;
					for (int m = graphs.Length - 1; m >= 0; m--)
					{
						graphs[m].GetNodes(delegate(global::Pathfinding.GraphNode node)
						{
							int num6 = node.NodeIndex * this.pivotCount + k;
							this.EnsureCapacity(num6);
							global::Pathfinding.PathNode pathNode = fp.pathHandler.GetPathNode(node);
							if (costOffset > 0U)
							{
								this.costs[num6] = ((pathNode.pathID != fp.pathID || pathNode.parent == null) ? 0U : global::System.Math.Max(pathNode.parent.G - costOffset, 0U));
							}
							else
							{
								this.costs[num6] = ((pathNode.pathID != fp.pathID) ? 0U : pathNode.G);
							}
							return true;
						});
					}
					if (this.mode == global::Pathfinding.HeuristicOptimizationMode.RandomSpreadOut && k < this.pivots.Length - 1)
					{
						if (this.pivots[k + 1] == null)
						{
							int num = -1;
							uint num2 = 0U;
							int num3 = this.maxNodeIndex / this.pivotCount;
							for (int n = 1; n < num3; n++)
							{
								uint num4 = 1073741824U;
								for (int num5 = 0; num5 <= k; num5++)
								{
									num4 = global::System.Math.Min(num4, this.costs[n * this.pivotCount + num5]);
								}
								global::Pathfinding.GraphNode node2 = fp.pathHandler.GetPathNode(n).node;
								if ((num4 > num2 || num == -1) && node2 != null && !node2.Destroyed && node2.Walkable)
								{
									num = n;
									num2 = num4;
								}
							}
							if (num == -1)
							{
								global::UnityEngine.Debug.LogError("Failed generating random pivot points for heuristic optimizations");
								return;
							}
							this.pivots[k + 1] = fp.pathHandler.GetPathNode(num).node;
						}
						startCostCalculation(k + 1);
					}
					_p.Release(this, false);
				};
				global::AstarPath.StartPath(fp, true);
			};
			if (this.mode != global::Pathfinding.HeuristicOptimizationMode.RandomSpreadOut)
			{
				for (int l = 0; l < this.pivots.Length; l++)
				{
					startCostCalculation(l);
				}
			}
			else
			{
				startCostCalculation(0);
			}
			this.dirty = false;
		}

		private void ApplyGridGraphEndpointSpecialCase()
		{
			global::Pathfinding.NavGraph[] graphs = global::AstarPath.active.graphs;
			for (int i = 0; i < graphs.Length; i++)
			{
				global::Pathfinding.GridGraph gridGraph = graphs[i] as global::Pathfinding.GridGraph;
				if (gridGraph != null)
				{
					global::Pathfinding.GridNode[] nodes = gridGraph.nodes;
					int num = (gridGraph.neighbours != global::Pathfinding.NumNeighbours.Four) ? ((gridGraph.neighbours != global::Pathfinding.NumNeighbours.Eight) ? 6 : 8) : 4;
					for (int j = 0; j < gridGraph.depth; j++)
					{
						for (int k = 0; k < gridGraph.width; k++)
						{
							global::Pathfinding.GridNode gridNode = nodes[j * gridGraph.width + k];
							if (!gridNode.Walkable)
							{
								int num2 = gridNode.NodeIndex * this.pivotCount;
								for (int l = 0; l < this.pivotCount; l++)
								{
									this.costs[num2 + l] = uint.MaxValue;
								}
								for (int m = 0; m < num; m++)
								{
									int num3;
									int num4;
									if (gridGraph.neighbours == global::Pathfinding.NumNeighbours.Six)
									{
										num3 = k + gridGraph.neighbourXOffsets[global::Pathfinding.GridGraph.hexagonNeighbourIndices[m]];
										num4 = j + gridGraph.neighbourZOffsets[global::Pathfinding.GridGraph.hexagonNeighbourIndices[m]];
									}
									else
									{
										num3 = k + gridGraph.neighbourXOffsets[m];
										num4 = j + gridGraph.neighbourZOffsets[m];
									}
									if (num3 >= 0 && num4 >= 0 && num3 < gridGraph.width && num4 < gridGraph.depth)
									{
										global::Pathfinding.GridNode gridNode2 = gridGraph.nodes[num4 * gridGraph.width + num3];
										if (gridNode2.Walkable)
										{
											for (int n = 0; n < this.pivotCount; n++)
											{
												uint val = this.costs[gridNode2.NodeIndex * this.pivotCount + n] + gridGraph.neighbourCosts[m];
												this.costs[num2 + n] = global::System.Math.Min(this.costs[num2 + n], val);
												global::UnityEngine.Debug.DrawLine((global::UnityEngine.Vector3)gridNode.position, (global::UnityEngine.Vector3)gridNode2.position, global::UnityEngine.Color.blue, 1f);
											}
										}
									}
								}
								for (int num5 = 0; num5 < this.pivotCount; num5++)
								{
									if (this.costs[num2 + num5] == 4294967295U)
									{
										this.costs[num2 + num5] = 0U;
									}
								}
							}
						}
					}
				}
			}
		}

		public void OnDrawGizmos()
		{
			if (this.pivots != null)
			{
				for (int i = 0; i < this.pivots.Length; i++)
				{
					global::UnityEngine.Gizmos.color = new global::UnityEngine.Color(0.623529434f, 0.368627459f, 0.7607843f, 0.8f);
					if (this.pivots[i] != null && !this.pivots[i].Destroyed)
					{
						global::UnityEngine.Gizmos.DrawCube((global::UnityEngine.Vector3)this.pivots[i].position, global::UnityEngine.Vector3.one);
					}
				}
			}
		}

		private const uint ra = 12820163U;

		private const uint rc = 1140671485U;

		public global::Pathfinding.HeuristicOptimizationMode mode;

		public int seed;

		public global::UnityEngine.Transform pivotPointRoot;

		public int spreadOutCount = 1;

		[global::System.NonSerialized]
		public bool dirty;

		private uint[] costs = new uint[8];

		private int maxNodeIndex;

		private int pivotCount;

		private global::Pathfinding.GraphNode[] pivots;

		private uint rval;

		private object lockObj = new object();
	}
}
