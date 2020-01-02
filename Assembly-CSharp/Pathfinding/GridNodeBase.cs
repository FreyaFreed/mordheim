using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	public abstract class GridNodeBase : global::Pathfinding.GraphNode
	{
		protected GridNodeBase(global::AstarPath astar) : base(astar)
		{
		}

		public int NodeInGridIndex
		{
			get
			{
				return this.nodeInGridIndex;
			}
			set
			{
				this.nodeInGridIndex = value;
			}
		}

		public bool WalkableErosion
		{
			get
			{
				return (this.gridFlags & 256) != 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -257) | ((!value) ? 0 : 256));
			}
		}

		public bool TmpWalkable
		{
			get
			{
				return (this.gridFlags & 512) != 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -513) | ((!value) ? 0 : 512));
			}
		}

		public override float SurfaceArea()
		{
			global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex);
			return gridGraph.nodeSize * gridGraph.nodeSize;
		}

		public override global::UnityEngine.Vector3 RandomPointOnSurface()
		{
			global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(base.GraphIndex);
			return (global::UnityEngine.Vector3)this.position + new global::UnityEngine.Vector3(global::UnityEngine.Random.value - 0.5f, 0f, global::UnityEngine.Random.value - 0.5f) * gridGraph.nodeSize;
		}

		public override void FloodFill(global::System.Collections.Generic.Stack<global::Pathfinding.GraphNode> stack, uint region)
		{
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					global::Pathfinding.GraphNode graphNode = this.connections[i];
					if (graphNode.Area != region)
					{
						graphNode.Area = region;
						stack.Push(graphNode);
					}
				}
			}
		}

		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse && this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					this.connections[i].RemoveConnection(this);
				}
			}
			this.connections = null;
			this.connectionCosts = null;
		}

		public override bool ContainsConnection(global::Pathfinding.GraphNode node)
		{
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i] == node)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void GetConnections(global::Pathfinding.GraphNodeDelegate del)
		{
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					del(this.connections[i]);
				}
			}
		}

		public override void UpdateRecursiveG(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler)
		{
			ushort pathID = handler.PathID;
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					global::Pathfinding.GraphNode graphNode = this.connections[i];
					global::Pathfinding.PathNode pathNode2 = handler.GetPathNode(graphNode);
					if (pathNode2.parent == pathNode && pathNode2.pathID == pathID)
					{
						graphNode.UpdateRecursiveG(path, pathNode2, handler);
					}
				}
			}
		}

		public override void Open(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler)
		{
			ushort pathID = handler.PathID;
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					global::Pathfinding.GraphNode graphNode = this.connections[i];
					if (path.CanTraverse(graphNode))
					{
						global::Pathfinding.PathNode pathNode2 = handler.GetPathNode(graphNode);
						uint num = this.connectionCosts[i];
						if (pathNode2.pathID != pathID)
						{
							pathNode2.parent = pathNode;
							pathNode2.pathID = pathID;
							pathNode2.cost = num;
							pathNode2.H = path.CalculateHScore(graphNode);
							graphNode.UpdateG(path, pathNode2);
							handler.heap.Add(pathNode2);
						}
						else if (pathNode.G + num + path.GetTraversalCost(graphNode) < pathNode2.G)
						{
							pathNode2.cost = num;
							pathNode2.parent = pathNode;
							graphNode.UpdateRecursiveG(path, pathNode2, handler);
						}
						else if (pathNode2.G + num + path.GetTraversalCost(this) < pathNode.G && graphNode.ContainsConnection(this))
						{
							pathNode.parent = pathNode2;
							pathNode.cost = num;
							this.UpdateRecursiveG(path, pathNode, handler);
						}
					}
				}
			}
		}

		public override void AddConnection(global::Pathfinding.GraphNode node, uint cost)
		{
			if (node == null)
			{
				throw new global::System.ArgumentNullException();
			}
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i] == node)
					{
						this.connectionCosts[i] = cost;
						return;
					}
				}
			}
			int num = (this.connections == null) ? 0 : this.connections.Length;
			global::Pathfinding.GraphNode[] array = new global::Pathfinding.GraphNode[num + 1];
			uint[] array2 = new uint[num + 1];
			for (int j = 0; j < num; j++)
			{
				array[j] = this.connections[j];
				array2[j] = this.connectionCosts[j];
			}
			array[num] = node;
			array2[num] = cost;
			this.connections = array;
			this.connectionCosts = array2;
		}

		public override void RemoveConnection(global::Pathfinding.GraphNode node)
		{
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				if (this.connections[i] == node)
				{
					int num = this.connections.Length;
					global::Pathfinding.GraphNode[] array = new global::Pathfinding.GraphNode[num - 1];
					uint[] array2 = new uint[num - 1];
					for (int j = 0; j < i; j++)
					{
						array[j] = this.connections[j];
						array2[j] = this.connectionCosts[j];
					}
					for (int k = i + 1; k < num; k++)
					{
						array[k - 1] = this.connections[k];
						array2[k - 1] = this.connectionCosts[k];
					}
					this.connections = array;
					this.connectionCosts = array2;
					return;
				}
			}
		}

		public override void SerializeReferences(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			if (this.connections == null)
			{
				ctx.writer.Write(-1);
			}
			else
			{
				ctx.writer.Write(this.connections.Length);
				for (int i = 0; i < this.connections.Length; i++)
				{
					ctx.SerializeNodeReference(this.connections[i]);
					ctx.writer.Write(this.connectionCosts[i]);
				}
			}
		}

		public override void DeserializeReferences(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			if (ctx.meta.version < global::Pathfinding.GridNodeBase.VERSION_3_8_3)
			{
				return;
			}
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.connections = null;
				this.connectionCosts = null;
			}
			else
			{
				this.connections = new global::Pathfinding.GraphNode[num];
				this.connectionCosts = new uint[num];
				for (int i = 0; i < num; i++)
				{
					this.connections[i] = ctx.DeserializeNodeReference();
					this.connectionCosts[i] = ctx.reader.ReadUInt32();
				}
			}
		}

		private const int GridFlagsWalkableErosionOffset = 8;

		private const int GridFlagsWalkableErosionMask = 256;

		private const int GridFlagsWalkableTmpOffset = 9;

		private const int GridFlagsWalkableTmpMask = 512;

		protected int nodeInGridIndex;

		protected ushort gridFlags;

		public global::Pathfinding.GraphNode[] connections;

		public uint[] connectionCosts;

		private static readonly global::System.Version VERSION_3_8_3 = new global::System.Version(3, 8, 3);
	}
}
