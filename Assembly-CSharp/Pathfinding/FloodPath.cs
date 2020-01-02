using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public class FloodPath : global::Pathfinding.Path
	{
		public override bool FloodingPath
		{
			get
			{
				return true;
			}
		}

		public bool HasPathTo(global::Pathfinding.GraphNode node)
		{
			return this.parents != null && this.parents.ContainsKey(node);
		}

		public global::Pathfinding.GraphNode GetParent(global::Pathfinding.GraphNode node)
		{
			return this.parents[node];
		}

		public static global::Pathfinding.FloodPath Construct(global::UnityEngine.Vector3 start, global::Pathfinding.OnPathDelegate callback = null)
		{
			global::Pathfinding.FloodPath path = global::Pathfinding.PathPool.GetPath<global::Pathfinding.FloodPath>();
			path.Setup(start, callback);
			return path;
		}

		public static global::Pathfinding.FloodPath Construct(global::Pathfinding.GraphNode start, global::Pathfinding.OnPathDelegate callback = null)
		{
			if (start == null)
			{
				throw new global::System.ArgumentNullException("start");
			}
			global::Pathfinding.FloodPath path = global::Pathfinding.PathPool.GetPath<global::Pathfinding.FloodPath>();
			path.Setup(start, callback);
			return path;
		}

		protected void Setup(global::UnityEngine.Vector3 start, global::Pathfinding.OnPathDelegate callback)
		{
			this.callback = callback;
			this.originalStartPoint = start;
			this.startPoint = start;
			this.heuristic = global::Pathfinding.Heuristic.None;
		}

		protected void Setup(global::Pathfinding.GraphNode start, global::Pathfinding.OnPathDelegate callback)
		{
			this.callback = callback;
			this.originalStartPoint = (global::UnityEngine.Vector3)start.position;
			this.startNode = start;
			this.startPoint = (global::UnityEngine.Vector3)start.position;
			this.heuristic = global::Pathfinding.Heuristic.None;
		}

		public override void Reset()
		{
			base.Reset();
			this.originalStartPoint = global::UnityEngine.Vector3.zero;
			this.startPoint = global::UnityEngine.Vector3.zero;
			this.startNode = null;
			this.parents = new global::System.Collections.Generic.Dictionary<global::Pathfinding.GraphNode, global::Pathfinding.GraphNode>();
			this.saveParents = true;
		}

		public override void Prepare()
		{
			if (this.startNode == null)
			{
				this.nnConstraint.tags = this.enabledTags;
				global::Pathfinding.NNInfo nearest = global::AstarPath.active.GetNearest(this.originalStartPoint, this.nnConstraint);
				this.startPoint = nearest.position;
				this.startNode = nearest.node;
			}
			else
			{
				this.startPoint = (global::UnityEngine.Vector3)this.startNode.position;
			}
			if (this.startNode == null)
			{
				base.Error();
				return;
			}
			if (!this.startNode.Walkable)
			{
				base.Error();
				return;
			}
		}

		public override void Initialize()
		{
			global::Pathfinding.PathNode pathNode = base.pathHandler.GetPathNode(this.startNode);
			pathNode.node = this.startNode;
			pathNode.pathID = base.pathHandler.PathID;
			pathNode.parent = null;
			pathNode.cost = 0U;
			pathNode.G = base.GetTraversalCost(this.startNode);
			pathNode.H = base.CalculateHScore(this.startNode);
			this.parents[this.startNode] = null;
			this.startNode.Open(this, pathNode, base.pathHandler);
			this.searchedNodes++;
			if (base.pathHandler.heap.isEmpty)
			{
				base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
			}
			this.currentR = base.pathHandler.heap.Remove();
		}

		public override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == global::Pathfinding.PathCompleteState.NotCalculated)
			{
				this.searchedNodes++;
				this.currentR.node.Open(this, this.currentR, base.pathHandler);
				if (this.saveParents)
				{
					this.parents[this.currentR.node] = this.currentR.parent.node;
				}
				if (base.pathHandler.heap.isEmpty)
				{
					base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
					break;
				}
				this.currentR = base.pathHandler.heap.Remove();
				if (num > 500)
				{
					if (global::System.DateTime.UtcNow.Ticks >= targetTick)
					{
						return;
					}
					num = 0;
					if (this.searchedNodes > 1000000)
					{
						throw new global::System.Exception("Probable infinite loop. Over 1,000,000 nodes searched");
					}
				}
				num++;
			}
		}

		public global::UnityEngine.Vector3 originalStartPoint;

		public global::UnityEngine.Vector3 startPoint;

		public global::Pathfinding.GraphNode startNode;

		public bool saveParents = true;

		protected global::System.Collections.Generic.Dictionary<global::Pathfinding.GraphNode, global::Pathfinding.GraphNode> parents;
	}
}
