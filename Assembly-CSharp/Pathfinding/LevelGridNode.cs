using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	public class LevelGridNode : global::Pathfinding.GridNodeBase
	{
		public LevelGridNode(global::AstarPath astar) : base(astar)
		{
		}

		public static global::Pathfinding.LayerGridGraph GetGridGraph(uint graphIndex)
		{
			return global::Pathfinding.LevelGridNode._gridGraphs[(int)graphIndex];
		}

		public static void SetGridGraph(int graphIndex, global::Pathfinding.LayerGridGraph graph)
		{
			if (global::Pathfinding.LevelGridNode._gridGraphs.Length <= graphIndex)
			{
				global::Pathfinding.LayerGridGraph[] array = new global::Pathfinding.LayerGridGraph[graphIndex + 1];
				for (int i = 0; i < global::Pathfinding.LevelGridNode._gridGraphs.Length; i++)
				{
					array[i] = global::Pathfinding.LevelGridNode._gridGraphs[i];
				}
				global::Pathfinding.LevelGridNode._gridGraphs = array;
			}
			global::Pathfinding.LevelGridNode._gridGraphs[graphIndex] = graph;
		}

		public void ResetAllGridConnections()
		{
			this.gridConnections = uint.MaxValue;
		}

		public bool HasAnyGridConnections()
		{
			return this.gridConnections != uint.MaxValue;
		}

		public void SetPosition(global::Pathfinding.Int3 position)
		{
			this.position = position;
		}

		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse)
			{
				global::Pathfinding.LayerGridGraph gridGraph = global::Pathfinding.LevelGridNode.GetGridGraph(base.GraphIndex);
				int[] neighbourOffsets = gridGraph.neighbourOffsets;
				global::Pathfinding.LevelGridNode[] nodes = gridGraph.nodes;
				for (int i = 0; i < 4; i++)
				{
					int connectionValue = this.GetConnectionValue(i);
					if (connectionValue != 255)
					{
						global::Pathfinding.LevelGridNode levelGridNode = nodes[base.NodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
						if (levelGridNode != null)
						{
							levelGridNode.SetConnectionValue((i + 2) % 4, 255);
						}
					}
				}
			}
			this.ResetAllGridConnections();
			base.ClearConnections(alsoReverse);
		}

		public override void GetConnections(global::Pathfinding.GraphNodeDelegate del)
		{
			int nodeInGridIndex = base.NodeInGridIndex;
			global::Pathfinding.LayerGridGraph gridGraph = global::Pathfinding.LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			global::Pathfinding.LevelGridNode[] nodes = gridGraph.nodes;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 255)
				{
					global::Pathfinding.LevelGridNode levelGridNode = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					if (levelGridNode != null)
					{
						del(levelGridNode);
					}
				}
			}
			base.GetConnections(del);
		}

		public override void FloodFill(global::System.Collections.Generic.Stack<global::Pathfinding.GraphNode> stack, uint region)
		{
			int nodeInGridIndex = base.NodeInGridIndex;
			global::Pathfinding.LayerGridGraph gridGraph = global::Pathfinding.LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			global::Pathfinding.LevelGridNode[] nodes = gridGraph.nodes;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 255)
				{
					global::Pathfinding.LevelGridNode levelGridNode = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					if (levelGridNode != null && levelGridNode.Area != region)
					{
						levelGridNode.Area = region;
						stack.Push(levelGridNode);
					}
				}
			}
			base.FloodFill(stack, region);
		}

		public bool GetConnection(int i)
		{
			return (this.gridConnections >> i * 8 & 255U) != 255U;
		}

		public void SetConnectionValue(int dir, int value)
		{
			this.gridConnections = ((this.gridConnections & ~(255U << dir * 8)) | (uint)((uint)value << dir * 8));
		}

		public int GetConnectionValue(int dir)
		{
			return (int)(this.gridConnections >> dir * 8 & 255U);
		}

		public override bool GetPortal(global::Pathfinding.GraphNode other, global::System.Collections.Generic.List<global::UnityEngine.Vector3> left, global::System.Collections.Generic.List<global::UnityEngine.Vector3> right, bool backwards)
		{
			if (backwards)
			{
				return true;
			}
			global::Pathfinding.LayerGridGraph gridGraph = global::Pathfinding.LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			global::Pathfinding.LevelGridNode[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 255 && other == nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue])
				{
					global::UnityEngine.Vector3 a = (global::UnityEngine.Vector3)(this.position + other.position) * 0.5f;
					global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.Cross(gridGraph.collision.up, (global::UnityEngine.Vector3)(other.position - this.position));
					vector.Normalize();
					vector *= gridGraph.nodeSize * 0.5f;
					left.Add(a - vector);
					right.Add(a + vector);
					return true;
				}
			}
			return false;
		}

		public override void UpdateRecursiveG(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler)
		{
			handler.heap.Add(pathNode);
			base.UpdateG(path, pathNode);
			global::Pathfinding.LayerGridGraph gridGraph = global::Pathfinding.LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			global::Pathfinding.LevelGridNode[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 255)
				{
					global::Pathfinding.LevelGridNode levelGridNode = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					global::Pathfinding.PathNode pathNode2 = handler.GetPathNode(levelGridNode);
					if (pathNode2 != null && pathNode2.parent == pathNode && pathNode2.pathID == handler.PathID)
					{
						levelGridNode.UpdateRecursiveG(path, pathNode2, handler);
					}
				}
			}
			base.UpdateRecursiveG(path, pathNode, handler);
		}

		public override void Open(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler)
		{
			global::Pathfinding.LayerGridGraph gridGraph = global::Pathfinding.LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			uint[] neighbourCosts = gridGraph.neighbourCosts;
			global::Pathfinding.LevelGridNode[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 255)
				{
					global::Pathfinding.GraphNode graphNode = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					if (path.CanTraverse(graphNode))
					{
						global::Pathfinding.PathNode pathNode2 = handler.GetPathNode(graphNode);
						if (pathNode2.pathID != handler.PathID)
						{
							pathNode2.parent = pathNode;
							pathNode2.pathID = handler.PathID;
							pathNode2.cost = neighbourCosts[i];
							pathNode2.H = path.CalculateHScore(graphNode);
							graphNode.UpdateG(path, pathNode2);
							handler.heap.Add(pathNode2);
						}
						else
						{
							uint num = neighbourCosts[i];
							if (pathNode.G + num + path.GetTraversalCost(graphNode) < pathNode2.G)
							{
								pathNode2.cost = num;
								pathNode2.parent = pathNode;
								graphNode.UpdateRecursiveG(path, pathNode2, handler);
							}
							else if (pathNode2.G + num + path.GetTraversalCost(this) < pathNode.G)
							{
								pathNode.parent = pathNode2;
								pathNode.cost = num;
								this.UpdateRecursiveG(path, pathNode, handler);
							}
						}
					}
				}
			}
			base.Open(path, pathNode, handler);
		}

		public override void SerializeNode(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
			ctx.writer.Write(this.gridFlags);
			ctx.writer.Write(this.gridConnections);
		}

		public override void DeserializeNode(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
			this.gridFlags = ctx.reader.ReadUInt16();
			this.gridConnections = ctx.reader.ReadUInt32();
		}

		public const int NoConnection = 255;

		public const int ConnectionMask = 255;

		private const int ConnectionStride = 8;

		public const int MaxLayerCount = 255;

		private static global::Pathfinding.LayerGridGraph[] _gridGraphs = new global::Pathfinding.LayerGridGraph[0];

		protected uint gridConnections;

		protected static global::Pathfinding.LayerGridGraph[] gridGraphs;
	}
}
