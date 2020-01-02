using System;
using UnityEngine;

namespace Pathfinding
{
	public class EndingConditionProximity : global::Pathfinding.ABPathEndingCondition
	{
		public EndingConditionProximity(global::Pathfinding.ABPath p, float maxDistance) : base(p)
		{
			this.maxDistance = maxDistance;
		}

		public override bool TargetFound(global::Pathfinding.PathNode node)
		{
			return ((global::UnityEngine.Vector3)node.node.position - this.abPath.originalEndPoint).sqrMagnitude <= this.maxDistance * this.maxDistance;
		}

		public float maxDistance = 10f;
	}
}
