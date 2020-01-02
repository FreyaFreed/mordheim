using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	public class GridNode : global::Pathfinding.GridNodeBase
	{
		public GridNode(global::AstarPath astar) : base(astar)
		{
		}

		public static global::Pathfinding.GridGraph GetGridGraph(uint graphIndex)
		{
			return global::Pathfinding.GridNode._gridGraphs[(int)graphIndex];
		}

		public static void SetGridGraph(int graphIndex, global::Pathfinding.GridGraph graph)
		{
			if (global::Pathfinding.GridNode._gridGraphs.Length <= graphIndex)
			{
				global::Pathfinding.GridGraph[] array = new global::Pathfinding.GridGraph[graphIndex + 1];
				for (int i = 0; i < global::Pathfinding.GridNode._gridGraphs.Length; i++)
				{
					array[i] = global::Pathfinding.GridNode._gridGraphs[i];
				}
				global::Pathfinding.GridNode._gridGraphs = array;
			}
			global::Pathfinding.GridNode._gridGraphs[graphIndex] = graph;
		}

		internal ushort InternalGridFlags
		{
			get
			{
				return this.gridFlags;
			}
			set
			{
				this.gridFlags = value;
			}
		}

		public bool GetConnectionInternal(int dir)
		{
			return (this.gridFlags >> dir & 1) != 0;
		}

		public void SetConnectionInternal(int dir, bool value)
		{
			this.gridFlags = (ushort)(((int)this.gridFlags & ~(1 << dir)) | ((!value) ? 0 : 1) << (dir & 31));
		}

		public void SetAllConnectionInternal(int connections)
		{
			this.gridFlags = (ushort)(((int)this.gridFlags & -256) | connections);
		}

		public void ResetConnectionsInternal()
		{
			this.gridFlags = (ushort)((int)this.gridFlags & -256);
		}

		public bool EdgeNode
		{
			get
			{
				return (this.gridFlags & 1024) != 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -1025) | ((!value) ? 0 : 1024));
			}
		}

		public int XCoordinateInGrid
		{
			get
			{
				return this.nodeInGridIndex % global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex).width;
			}
		}

		public int ZCoordinateInGrid
		{
			get
			{
				return this.nodeInGridIndex / global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex).width;
			}
		}

		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse)
			{
				global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex);
				for (int i = 0; i < 8; i++)
				{
					global::Pathfinding.GridNode nodeConnection = gridGraph.GetNodeConnection(this, i);
					if (nodeConnection != null)
					{
						nodeConnection.SetConnectionInternal((i >= 4) ? 7 : ((i + 2) % 4), false);
					}
				}
			}
			this.ResetConnectionsInternal();
			base.ClearConnections(alsoReverse);
		}

		public override void GetConnections(global::Pathfinding.GraphNodeDelegate del)
		{
			global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			global::Pathfinding.GridNode[] nodes = gridGraph.nodes;
			for (int i = 0; i < 8; i++)
			{
				if (this.GetConnectionInternal(i))
				{
					global::Pathfinding.GridNode gridNode = nodes[this.nodeInGridIndex + neighbourOffsets[i]];
					if (gridNode != null)
					{
						del(gridNode);
					}
				}
			}
			base.GetConnections(del);
		}

		public global::UnityEngine.Vector3 ClosestPointOnNode(global::UnityEngine.Vector3 p)
		{
			global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex);
			p = gridGraph.inverseMatrix.MultiplyPoint3x4(p);
			float value = (float)this.position.x - 0.5f;
			float value2 = (float)this.position.z - 0.5f;
			int num = this.nodeInGridIndex % gridGraph.width;
			int num2 = this.nodeInGridIndex / gridGraph.width;
			float y = gridGraph.inverseMatrix.MultiplyPoint3x4(p).y;
			global::UnityEngine.Vector3 v = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Clamp(value, (float)num - 0.5f, (float)num + 0.5f) + 0.5f, y, global::UnityEngine.Mathf.Clamp(value2, (float)num2 - 0.5f, (float)num2 + 0.5f) + 0.5f);
			return gridGraph.matrix.MultiplyPoint3x4(v);
		}

		public override bool GetPortal(global::Pathfinding.GraphNode other, global::System.Collections.Generic.List<global::UnityEngine.Vector3> left, global::System.Collections.Generic.List<global::UnityEngine.Vector3> right, bool backwards)
		{
			if (backwards)
			{
				return true;
			}
			global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			global::Pathfinding.GridNode[] nodes = gridGraph.nodes;
			for (int i = 0; i < 4; i++)
			{
				if (this.GetConnectionInternal(i) && other == nodes[this.nodeInGridIndex + neighbourOffsets[i]])
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
			for (int j = 4; j < 8; j++)
			{
				if (this.GetConnectionInternal(j) && other == nodes[this.nodeInGridIndex + neighbourOffsets[j]])
				{
					bool flag = false;
					bool flag2 = false;
					if (this.GetConnectionInternal(j - 4))
					{
						global::Pathfinding.GridNode gridNode = nodes[this.nodeInGridIndex + neighbourOffsets[j - 4]];
						if (gridNode.Walkable && gridNode.GetConnectionInternal((j - 4 + 1) % 4))
						{
							flag = true;
						}
					}
					if (this.GetConnectionInternal((j - 4 + 1) % 4))
					{
						global::Pathfinding.GridNode gridNode2 = nodes[this.nodeInGridIndex + neighbourOffsets[(j - 4 + 1) % 4]];
						if (gridNode2.Walkable && gridNode2.GetConnectionInternal(j - 4))
						{
							flag2 = true;
						}
					}
					global::UnityEngine.Vector3 a2 = (global::UnityEngine.Vector3)(this.position + other.position) * 0.5f;
					global::UnityEngine.Vector3 vector2 = global::UnityEngine.Vector3.Cross(gridGraph.collision.up, (global::UnityEngine.Vector3)(other.position - this.position));
					vector2.Normalize();
					vector2 *= gridGraph.nodeSize * 1.4142f;
					left.Add(a2 - ((!flag2) ? global::UnityEngine.Vector3.zero : vector2));
					right.Add(a2 + ((!flag) ? global::UnityEngine.Vector3.zero : vector2));
					return true;
				}
			}
			return false;
		}

		public override void FloodFill(global::System.Collections.Generic.Stack<global::Pathfinding.GraphNode> stack, uint region)
		{
			global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			global::Pathfinding.GridNode[] nodes = gridGraph.nodes;
			for (int i = 0; i < 8; i++)
			{
				if (this.GetConnectionInternal(i))
				{
					global::Pathfinding.GridNode gridNode = nodes[this.nodeInGridIndex + neighbourOffsets[i]];
					if (gridNode != null && gridNode.Area != region)
					{
						gridNode.Area = region;
						stack.Push(gridNode);
					}
				}
			}
			base.FloodFill(stack, region);
		}

		public override bool ContainsConnection(global::Pathfinding.GraphNode node)
		{
			global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			global::Pathfinding.GridNode[] nodes = gridGraph.nodes;
			for (int i = 0; i < 8; i++)
			{
				if (this.GetConnectionInternal(i))
				{
					global::Pathfinding.GridNode gridNode = nodes[this.nodeInGridIndex + neighbourOffsets[i]];
					if (gridNode == node)
					{
						return true;
					}
				}
			}
			return base.ContainsConnection(node);
		}

		public override void UpdateRecursiveG(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler)
		{
			global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			global::Pathfinding.GridNode[] nodes = gridGraph.nodes;
			base.UpdateG(path, pathNode);
			handler.heap.Add(pathNode);
			ushort pathID = handler.PathID;
			for (int i = 0; i < 8; i++)
			{
				if (this.GetConnectionInternal(i))
				{
					global::Pathfinding.GridNode gridNode = nodes[this.nodeInGridIndex + neighbourOffsets[i]];
					global::Pathfinding.PathNode pathNode2 = handler.GetPathNode(gridNode);
					if (pathNode2.parent == pathNode && pathNode2.pathID == pathID)
					{
						gridNode.UpdateRecursiveG(path, pathNode2, handler);
					}
				}
			}
			base.UpdateRecursiveG(path, pathNode, handler);
		}

		public override void Open(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler)
		{
			global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex);
			ushort pathID = handler.PathID;
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			uint[] neighbourCosts = gridGraph.neighbourCosts;
			global::Pathfinding.GridNode[] nodes = gridGraph.nodes;
			for (int i = 0; i < 8; i++)
			{
				if (this.GetConnectionInternal(i))
				{
					global::Pathfinding.GridNode gridNode = nodes[this.nodeInGridIndex + neighbourOffsets[i]];
					if (path.CanTraverse(gridNode))
					{
						global::Pathfinding.PathNode pathNode2 = handler.GetPathNode(gridNode);
						uint num = neighbourCosts[i];
						if (pathNode2.pathID != pathID)
						{
							pathNode2.parent = pathNode;
							pathNode2.pathID = pathID;
							pathNode2.cost = num;
							pathNode2.H = path.CalculateHScore(gridNode);
							gridNode.UpdateG(path, pathNode2);
							handler.heap.Add(pathNode2);
						}
						else if (pathNode.G + num + path.GetTraversalCost(gridNode) < pathNode2.G)
						{
							pathNode2.cost = num;
							pathNode2.parent = pathNode;
							gridNode.UpdateRecursiveG(path, pathNode2, handler);
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
			base.Open(path, pathNode, handler);
		}

		public override void SerializeNode(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
			ctx.writer.Write(this.gridFlags);
		}

		public override void DeserializeNode(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
			this.gridFlags = ctx.reader.ReadUInt16();
		}

		private const int GridFlagsConnectionOffset = 0;

		private const int GridFlagsConnectionBit0 = 1;

		private const int GridFlagsConnectionMask = 255;

		private const int GridFlagsEdgeNodeOffset = 10;

		private const int GridFlagsEdgeNodeMask = 1024;

		private static global::Pathfinding.GridGraph[] _gridGraphs = new global::Pathfinding.GridGraph[0];
	}
}
