using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public class NodeLink3Node : global::Pathfinding.PointNode
	{
		public NodeLink3Node(global::AstarPath active) : base(active)
		{
		}

		public override bool GetPortal(global::Pathfinding.GraphNode other, global::System.Collections.Generic.List<global::UnityEngine.Vector3> left, global::System.Collections.Generic.List<global::UnityEngine.Vector3> right, bool backwards)
		{
			if (this.connections.Length < 2)
			{
				return false;
			}
			if (this.connections.Length != 2)
			{
				throw new global::System.Exception("Invalid NodeLink3Node. Expected 2 connections, found " + this.connections.Length);
			}
			if (left != null)
			{
				left.Add(this.portalA);
				right.Add(this.portalB);
			}
			return true;
		}

		public global::Pathfinding.GraphNode GetOther(global::Pathfinding.GraphNode a)
		{
			if (this.connections.Length < 2)
			{
				return null;
			}
			if (this.connections.Length != 2)
			{
				throw new global::System.Exception("Invalid NodeLink3Node. Expected 2 connections, found " + this.connections.Length);
			}
			return (a != this.connections[0]) ? (this.connections[0] as global::Pathfinding.NodeLink3Node).GetOtherInternal(this) : (this.connections[1] as global::Pathfinding.NodeLink3Node).GetOtherInternal(this);
		}

		private global::Pathfinding.GraphNode GetOtherInternal(global::Pathfinding.GraphNode a)
		{
			if (this.connections.Length < 2)
			{
				return null;
			}
			return (a != this.connections[0]) ? this.connections[0] : this.connections[1];
		}

		public global::Pathfinding.NodeLink3 link;

		public global::UnityEngine.Vector3 portalA;

		public global::UnityEngine.Vector3 portalB;
	}
}
