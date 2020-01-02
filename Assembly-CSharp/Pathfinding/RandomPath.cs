using System;
using UnityEngine;

namespace Pathfinding
{
	public class RandomPath : global::Pathfinding.ABPath
	{
		public RandomPath()
		{
		}

		[global::System.Obsolete("This constructor is obsolete. Please use the pooling API and the Construct methods")]
		public RandomPath(global::UnityEngine.Vector3 start, int length, global::Pathfinding.OnPathDelegate callback = null)
		{
			throw new global::System.Exception("This constructor is obsolete. Please use the pooling API and the Setup methods");
		}

		public override bool FloodingPath
		{
			get
			{
				return true;
			}
		}

		protected override bool hasEndPoint
		{
			get
			{
				return false;
			}
		}

		public override void Reset()
		{
			base.Reset();
			this.searchLength = 5000;
			this.spread = 5000;
			this.aimStrength = 0f;
			this.chosenNodeR = null;
			this.maxGScoreNodeR = null;
			this.maxGScore = 0;
			this.aim = global::UnityEngine.Vector3.zero;
			this.nodesEvaluatedRep = 0;
		}

		public static global::Pathfinding.RandomPath Construct(global::UnityEngine.Vector3 start, int length, global::Pathfinding.OnPathDelegate callback = null)
		{
			global::Pathfinding.RandomPath path = global::Pathfinding.PathPool.GetPath<global::Pathfinding.RandomPath>();
			path.Setup(start, length, callback);
			return path;
		}

		protected global::Pathfinding.RandomPath Setup(global::UnityEngine.Vector3 start, int length, global::Pathfinding.OnPathDelegate callback)
		{
			this.callback = callback;
			this.searchLength = length;
			this.originalStartPoint = start;
			this.originalEndPoint = global::UnityEngine.Vector3.zero;
			this.startPoint = start;
			this.endPoint = global::UnityEngine.Vector3.zero;
			this.startIntPoint = (global::Pathfinding.Int3)start;
			return this;
		}

		public override void ReturnPath()
		{
			if (this.path != null && this.path.Count > 0)
			{
				this.endNode = this.path[this.path.Count - 1];
				this.endPoint = (global::UnityEngine.Vector3)this.endNode.position;
				this.originalEndPoint = this.endPoint;
				this.hTarget = this.endNode.position;
			}
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		public override void Prepare()
		{
			this.nnConstraint.tags = this.enabledTags;
			global::Pathfinding.NNInfo nearest = global::AstarPath.active.GetNearest(this.startPoint, this.nnConstraint, this.startHint);
			this.startPoint = nearest.position;
			this.endPoint = this.startPoint;
			this.startIntPoint = (global::Pathfinding.Int3)this.startPoint;
			this.hTarget = (global::Pathfinding.Int3)this.aim;
			this.startNode = nearest.node;
			this.endNode = this.startNode;
			if (this.startNode == null || this.endNode == null)
			{
				base.Error();
				return;
			}
			if (!this.startNode.Walkable)
			{
				base.Error();
				return;
			}
			this.heuristicScale = this.aimStrength;
		}

		public override void Initialize()
		{
			global::Pathfinding.PathNode pathNode = base.pathHandler.GetPathNode(this.startNode);
			pathNode.node = this.startNode;
			if (this.searchLength + this.spread <= 0)
			{
				base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
				this.Trace(pathNode);
				return;
			}
			pathNode.pathID = base.pathID;
			pathNode.parent = null;
			pathNode.cost = 0U;
			pathNode.G = base.GetTraversalCost(this.startNode);
			pathNode.H = base.CalculateHScore(this.startNode);
			this.startNode.Open(this, pathNode, base.pathHandler);
			this.searchedNodes++;
			if (base.pathHandler.heap.isEmpty)
			{
				base.Error();
				return;
			}
			this.currentR = base.pathHandler.heap.Remove();
		}

		public override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == global::Pathfinding.PathCompleteState.NotCalculated)
			{
				this.searchedNodes++;
				if ((ulong)this.currentR.G >= (ulong)((long)this.searchLength))
				{
					if ((ulong)this.currentR.G > (ulong)((long)(this.searchLength + this.spread)))
					{
						if (this.chosenNodeR == null)
						{
							this.chosenNodeR = this.currentR;
						}
						base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
						break;
					}
					this.nodesEvaluatedRep++;
					if (this.rnd.NextDouble() <= (double)(1f / (float)this.nodesEvaluatedRep))
					{
						this.chosenNodeR = this.currentR;
					}
				}
				else if ((ulong)this.currentR.G > (ulong)((long)this.maxGScore))
				{
					this.maxGScore = (int)this.currentR.G;
					this.maxGScoreNodeR = this.currentR;
				}
				this.currentR.node.Open(this, this.currentR, base.pathHandler);
				if (base.pathHandler.heap.isEmpty)
				{
					if (this.chosenNodeR != null)
					{
						base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
					}
					else if (this.maxGScoreNodeR != null)
					{
						this.chosenNodeR = this.maxGScoreNodeR;
						base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
					}
					else
					{
						base.Error();
					}
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
			if (base.CompleteState == global::Pathfinding.PathCompleteState.Complete)
			{
				this.Trace(this.chosenNodeR);
			}
		}

		public int searchLength;

		public int spread = 5000;

		public float aimStrength;

		private global::Pathfinding.PathNode chosenNodeR;

		private global::Pathfinding.PathNode maxGScoreNodeR;

		private int maxGScore;

		public global::UnityEngine.Vector3 aim;

		private int nodesEvaluatedRep;

		private readonly global::System.Random rnd = new global::System.Random();
	}
}
