using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class ConstantPath : global::Pathfinding.Path
	{
		public override bool FloodingPath
		{
			get
			{
				return true;
			}
		}

		public static global::Pathfinding.ConstantPath Construct(global::UnityEngine.Vector3 start, int maxGScore, global::Pathfinding.OnPathDelegate callback = null)
		{
			global::Pathfinding.ConstantPath path = global::Pathfinding.PathPool.GetPath<global::Pathfinding.ConstantPath>();
			path.Setup(start, maxGScore, callback);
			return path;
		}

		protected void Setup(global::UnityEngine.Vector3 start, int maxGScore, global::Pathfinding.OnPathDelegate callback)
		{
			this.callback = callback;
			this.startPoint = start;
			this.originalStartPoint = this.startPoint;
			this.endingCondition = new global::Pathfinding.EndingConditionDistance(this, maxGScore);
		}

		public override void OnEnterPool()
		{
			base.OnEnterPool();
			if (this.allNodes != null)
			{
				global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Release(this.allNodes);
			}
		}

		public override void Reset()
		{
			base.Reset();
			this.allNodes = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Claim();
			this.endingCondition = null;
			this.originalStartPoint = global::UnityEngine.Vector3.zero;
			this.startPoint = global::UnityEngine.Vector3.zero;
			this.startNode = null;
			this.heuristic = global::Pathfinding.Heuristic.None;
		}

		public override void Prepare()
		{
			this.nnConstraint.tags = this.enabledTags;
			this.startNode = global::AstarPath.active.GetNearest(this.startPoint, this.nnConstraint).node;
			if (this.startNode == null)
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
			this.startNode.Open(this, pathNode, base.pathHandler);
			this.searchedNodes++;
			pathNode.flag1 = true;
			this.allNodes.Add(this.startNode);
			if (base.pathHandler.heap.isEmpty)
			{
				base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
				return;
			}
			this.currentR = base.pathHandler.heap.Remove();
		}

		public override void Cleanup()
		{
			int count = this.allNodes.Count;
			for (int i = 0; i < count; i++)
			{
				base.pathHandler.GetPathNode(this.allNodes[i]).flag1 = false;
			}
		}

		public override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == global::Pathfinding.PathCompleteState.NotCalculated)
			{
				this.searchedNodes++;
				if (this.endingCondition.TargetFound(this.currentR))
				{
					base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
					break;
				}
				if (!this.currentR.flag1)
				{
					this.allNodes.Add(this.currentR.node);
					this.currentR.flag1 = true;
				}
				this.currentR.node.Open(this, this.currentR, base.pathHandler);
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

		public global::Pathfinding.GraphNode startNode;

		public global::UnityEngine.Vector3 startPoint;

		public global::UnityEngine.Vector3 originalStartPoint;

		public global::System.Collections.Generic.List<global::Pathfinding.GraphNode> allNodes;

		public global::Pathfinding.PathEndingCondition endingCondition;
	}
}
