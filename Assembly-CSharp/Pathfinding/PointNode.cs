using System;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	public class PointNode : global::Pathfinding.GraphNode
	{
		public PointNode(global::AstarPath astar) : base(astar)
		{
		}

		public void SetPosition(global::Pathfinding.Int3 value)
		{
			this.position = value;
		}

		public override void GetConnections(global::Pathfinding.GraphNodeDelegate del)
		{
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				del(this.connections[i]);
			}
		}

		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse && this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i] != null)
					{
						this.connections[i].RemoveConnection(this);
					}
				}
			}
			this.connections = null;
			this.connectionCosts = null;
		}

		public override void UpdateRecursiveG(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler)
		{
			base.UpdateG(path, pathNode);
			handler.heap.Add(pathNode);
			for (int i = 0; i < this.connections.Length; i++)
			{
				global::Pathfinding.GraphNode graphNode = this.connections[i];
				global::Pathfinding.PathNode pathNode2 = handler.GetPathNode(graphNode);
				if (pathNode2.parent == pathNode && pathNode2.pathID == handler.PathID)
				{
					graphNode.UpdateRecursiveG(path, pathNode2, handler);
				}
			}
		}

		public override bool ContainsConnection(global::Pathfinding.GraphNode node)
		{
			for (int i = 0; i < this.connections.Length; i++)
			{
				if (this.connections[i] == node)
				{
					return true;
				}
			}
			return false;
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

		public override void Open(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler)
		{
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				global::Pathfinding.GraphNode graphNode = this.connections[i];
				if (path.CanTraverse(graphNode))
				{
					global::Pathfinding.PathNode pathNode2 = handler.GetPathNode(graphNode);
					if (pathNode2.pathID != handler.PathID)
					{
						pathNode2.parent = pathNode;
						pathNode2.pathID = handler.PathID;
						pathNode2.cost = this.connectionCosts[i];
						pathNode2.H = path.CalculateHScore(graphNode);
						graphNode.UpdateG(path, pathNode2);
						handler.heap.Add(pathNode2);
					}
					else
					{
						uint num = this.connectionCosts[i];
						if (pathNode.G + num + path.GetTraversalCost(graphNode) < pathNode2.G)
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

		public override void SerializeNode(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
		}

		public override void DeserializeNode(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
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

		public global::Pathfinding.GraphNode[] connections;

		public uint[] connectionCosts;

		public global::UnityEngine.GameObject gameObject;

		public global::Pathfinding.PointNode next;
	}
}
