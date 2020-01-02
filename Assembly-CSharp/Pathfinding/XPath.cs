using System;
using UnityEngine;

namespace Pathfinding
{
	public class XPath : global::Pathfinding.ABPath
	{
		public new static global::Pathfinding.XPath Construct(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, global::Pathfinding.OnPathDelegate callback = null)
		{
			global::Pathfinding.XPath path = global::Pathfinding.PathPool.GetPath<global::Pathfinding.XPath>();
			path.Setup(start, end, callback);
			path.endingCondition = new global::Pathfinding.ABPathEndingCondition(path);
			return path;
		}

		public override void Reset()
		{
			base.Reset();
			this.endingCondition = null;
		}

		protected override bool EndPointGridGraphSpecialCase(global::Pathfinding.GraphNode endNode)
		{
			return false;
		}

		protected override void CompletePathIfStartIsValidTarget()
		{
			global::Pathfinding.PathNode pathNode = base.pathHandler.GetPathNode(this.startNode);
			if (this.endingCondition.TargetFound(pathNode))
			{
				this.ChangeEndNode(this.startNode);
				this.Trace(pathNode);
				base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
			}
		}

		private void ChangeEndNode(global::Pathfinding.GraphNode target)
		{
			if (this.endNode != null && this.endNode != this.startNode)
			{
				global::Pathfinding.PathNode pathNode = base.pathHandler.GetPathNode(this.endNode);
				global::Pathfinding.PathNode pathNode2 = pathNode;
				bool flag = false;
				pathNode.flag2 = flag;
				pathNode2.flag1 = flag;
			}
			this.endNode = target;
			this.endPoint = (global::UnityEngine.Vector3)target.position;
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
				this.currentR.node.Open(this, this.currentR, base.pathHandler);
				if (base.pathHandler.heap.isEmpty)
				{
					base.Error();
					return;
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
				this.ChangeEndNode(this.currentR.node);
				this.Trace(this.currentR);
			}
		}

		public global::Pathfinding.PathEndingCondition endingCondition;
	}
}
