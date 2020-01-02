using System;

namespace Pathfinding
{
	public class EndingConditionDistance : global::Pathfinding.PathEndingCondition
	{
		public EndingConditionDistance(global::Pathfinding.Path p, int maxGScore) : base(p)
		{
			this.maxGScore = maxGScore;
		}

		public override bool TargetFound(global::Pathfinding.PathNode node)
		{
			return (ulong)node.G >= (ulong)((long)this.maxGScore);
		}

		public int maxGScore = 100;
	}
}
