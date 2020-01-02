using System;
using UnityEngine;

namespace Pathfinding
{
	public class FloodPathTracer : global::Pathfinding.ABPath
	{
		protected override bool hasEndPoint
		{
			get
			{
				return false;
			}
		}

		public static global::Pathfinding.FloodPathTracer Construct(global::UnityEngine.Vector3 start, global::Pathfinding.FloodPath flood, global::Pathfinding.OnPathDelegate callback = null)
		{
			global::Pathfinding.FloodPathTracer path = global::Pathfinding.PathPool.GetPath<global::Pathfinding.FloodPathTracer>();
			path.Setup(start, flood, callback);
			return path;
		}

		protected void Setup(global::UnityEngine.Vector3 start, global::Pathfinding.FloodPath flood, global::Pathfinding.OnPathDelegate callback)
		{
			this.flood = flood;
			if (flood == null || flood.GetState() < global::Pathfinding.PathState.Returned)
			{
				throw new global::System.ArgumentException("You must supply a calculated FloodPath to the 'flood' argument");
			}
			base.Setup(start, flood.originalStartPoint, callback);
			this.nnConstraint = new global::Pathfinding.FloodPathConstraint(flood);
		}

		public override void Reset()
		{
			base.Reset();
			this.flood = null;
		}

		public override void Initialize()
		{
			if (this.startNode != null && this.flood.HasPathTo(this.startNode))
			{
				this.Trace(this.startNode);
				base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
			}
			else
			{
				base.Error();
			}
		}

		public override void CalculateStep(long targetTick)
		{
			if (!base.IsDone())
			{
				base.Error();
			}
		}

		public void Trace(global::Pathfinding.GraphNode from)
		{
			global::Pathfinding.GraphNode graphNode = from;
			int num = 0;
			while (graphNode != null)
			{
				this.path.Add(graphNode);
				this.vectorPath.Add((global::UnityEngine.Vector3)graphNode.position);
				graphNode = this.flood.GetParent(graphNode);
				num++;
				if (num > 1024)
				{
					global::UnityEngine.Debug.LogWarning("Inifinity loop? >1024 node path. Remove this message if you really have that long paths (FloodPathTracer.cs, Trace function)");
					break;
				}
			}
		}

		protected global::Pathfinding.FloodPath flood;
	}
}
