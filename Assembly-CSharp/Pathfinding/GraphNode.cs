using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	public abstract class GraphNode
	{
		protected GraphNode(global::AstarPath astar)
		{
			if (!object.ReferenceEquals(astar, null))
			{
				this.nodeIndex = astar.GetNewNodeIndex();
				astar.InitializeNode(this);
				return;
			}
			throw new global::System.Exception("No active AstarPath object to bind to");
		}

		internal void Destroy()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.ClearConnections(true);
			if (global::AstarPath.active != null)
			{
				global::AstarPath.active.DestroyNode(this);
			}
			this.nodeIndex = -1;
		}

		public bool Destroyed
		{
			get
			{
				return this.nodeIndex == -1;
			}
		}

		public int NodeIndex
		{
			get
			{
				return this.nodeIndex;
			}
		}

		public uint Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		public uint Penalty
		{
			get
			{
				return 0U;
			}
			set
			{
			}
		}

		public bool Walkable
		{
			get
			{
				return (this.flags & 1U) != 0U;
			}
			set
			{
				this.flags = ((this.flags & 4294967294U) | ((!value) ? 0U : 1U));
			}
		}

		public uint Area
		{
			get
			{
				return (this.flags & 262142U) >> 1;
			}
			set
			{
				this.flags = ((this.flags & 4294705153U) | value << 1);
			}
		}

		public uint GraphIndex
		{
			get
			{
				return (this.flags & 4278190080U) >> 24;
			}
			set
			{
				this.flags = ((this.flags & 16777215U) | value << 24);
			}
		}

		public uint Tag
		{
			get
			{
				return (this.flags & 16252928U) >> 19;
			}
			set
			{
				this.flags = ((this.flags & 4278714367U) | value << 19);
			}
		}

		public void UpdateG(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode)
		{
			pathNode.G = pathNode.parent.G + pathNode.cost + path.GetTraversalCost(this);
		}

		public virtual void UpdateRecursiveG(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler)
		{
			this.UpdateG(path, pathNode);
			handler.heap.Add(pathNode);
			this.GetConnections(delegate(global::Pathfinding.GraphNode other)
			{
				global::Pathfinding.PathNode pathNode2 = handler.GetPathNode(other);
				if (pathNode2.parent == pathNode && pathNode2.pathID == handler.PathID)
				{
					other.UpdateRecursiveG(path, pathNode2, handler);
				}
			});
		}

		public virtual void FloodFill(global::System.Collections.Generic.Stack<global::Pathfinding.GraphNode> stack, uint region)
		{
			this.GetConnections(delegate(global::Pathfinding.GraphNode other)
			{
				if (other.Area != region)
				{
					other.Area = region;
					stack.Push(other);
				}
			});
		}

		public abstract void GetConnections(global::Pathfinding.GraphNodeDelegate del);

		public abstract void AddConnection(global::Pathfinding.GraphNode node, uint cost);

		public abstract void RemoveConnection(global::Pathfinding.GraphNode node);

		public abstract void ClearConnections(bool alsoReverse);

		public virtual bool ContainsConnection(global::Pathfinding.GraphNode node)
		{
			bool contains = false;
			this.GetConnections(delegate(global::Pathfinding.GraphNode neighbour)
			{
				contains |= (neighbour == node);
			});
			return contains;
		}

		public virtual void RecalculateConnectionCosts()
		{
		}

		public virtual bool GetPortal(global::Pathfinding.GraphNode other, global::System.Collections.Generic.List<global::UnityEngine.Vector3> left, global::System.Collections.Generic.List<global::UnityEngine.Vector3> right, bool backwards)
		{
			return false;
		}

		public abstract void Open(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler);

		public virtual float SurfaceArea()
		{
			return 0f;
		}

		public virtual global::UnityEngine.Vector3 RandomPointOnSurface()
		{
			return (global::UnityEngine.Vector3)this.position;
		}

		public virtual void SerializeNode(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			ctx.writer.Write(this.Penalty);
			ctx.writer.Write(this.Flags);
		}

		public virtual void DeserializeNode(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			this.Penalty = ctx.reader.ReadUInt32();
			this.Flags = ctx.reader.ReadUInt32();
			this.GraphIndex = ctx.graphIndex;
		}

		public virtual void SerializeReferences(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
		}

		public virtual void DeserializeReferences(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
		}

		private const int FlagsWalkableOffset = 0;

		private const uint FlagsWalkableMask = 1U;

		private const int FlagsAreaOffset = 1;

		private const uint FlagsAreaMask = 262142U;

		private const int FlagsGraphOffset = 24;

		private const uint FlagsGraphMask = 4278190080U;

		public const uint MaxAreaIndex = 131071U;

		public const uint MaxGraphIndex = 255U;

		private const int FlagsTagOffset = 19;

		private const uint FlagsTagMask = 16252928U;

		private int nodeIndex;

		protected uint flags;

		public global::Pathfinding.Int3 position;
	}
}
