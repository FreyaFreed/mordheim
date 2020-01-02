using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class GraphUpdateObject
	{
		public GraphUpdateObject()
		{
		}

		public GraphUpdateObject(global::UnityEngine.Bounds b)
		{
			this.bounds = b;
		}

		public virtual void WillUpdateNode(global::Pathfinding.GraphNode node)
		{
			if (this.trackChangedNodes && node != null)
			{
				if (this.changedNodes == null)
				{
					this.changedNodes = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Claim();
					this.backupData = global::Pathfinding.Util.ListPool<uint>.Claim();
					this.backupPositionData = global::Pathfinding.Util.ListPool<global::Pathfinding.Int3>.Claim();
				}
				this.changedNodes.Add(node);
				this.backupPositionData.Add(node.position);
				this.backupData.Add(node.Penalty);
				this.backupData.Add(node.Flags);
				global::Pathfinding.GridNode gridNode = node as global::Pathfinding.GridNode;
				if (gridNode != null)
				{
					this.backupData.Add((uint)gridNode.InternalGridFlags);
				}
			}
		}

		public virtual void RevertFromBackup()
		{
			if (!this.trackChangedNodes)
			{
				throw new global::System.InvalidOperationException("Changed nodes have not been tracked, cannot revert from backup");
			}
			if (this.changedNodes == null)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < this.changedNodes.Count; i++)
			{
				this.changedNodes[i].Penalty = this.backupData[num];
				num++;
				this.changedNodes[i].Flags = this.backupData[num];
				num++;
				global::Pathfinding.GridNode gridNode = this.changedNodes[i] as global::Pathfinding.GridNode;
				if (gridNode != null)
				{
					gridNode.InternalGridFlags = (ushort)this.backupData[num];
					num++;
				}
				this.changedNodes[i].position = this.backupPositionData[i];
			}
			global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Release(this.changedNodes);
			global::Pathfinding.Util.ListPool<uint>.Release(this.backupData);
			global::Pathfinding.Util.ListPool<global::Pathfinding.Int3>.Release(this.backupPositionData);
		}

		public virtual void Apply(global::Pathfinding.GraphNode node)
		{
			if (this.shape == null || this.shape.Contains(node))
			{
				node.Penalty = (uint)((ulong)node.Penalty + (ulong)((long)this.addPenalty));
				if (this.modifyWalkability)
				{
					node.Walkable = this.setWalkability;
				}
				if (this.modifyTag)
				{
					node.Tag = (uint)this.setTag;
				}
			}
		}

		public global::UnityEngine.Bounds bounds;

		public bool requiresFloodFill = true;

		public bool updatePhysics = true;

		public bool resetPenaltyOnPhysics = true;

		public bool updateErosion = true;

		public global::Pathfinding.NNConstraint nnConstraint = global::Pathfinding.NNConstraint.None;

		public int addPenalty;

		public bool modifyWalkability;

		public bool setWalkability;

		public bool modifyTag;

		public int setTag;

		public bool trackChangedNodes;

		public global::System.Collections.Generic.List<global::Pathfinding.GraphNode> changedNodes;

		private global::System.Collections.Generic.List<uint> backupData;

		private global::System.Collections.Generic.List<global::Pathfinding.Int3> backupPositionData;

		public global::Pathfinding.GraphUpdateShape shape;
	}
}
