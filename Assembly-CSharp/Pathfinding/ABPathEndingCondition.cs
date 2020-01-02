using System;

namespace Pathfinding
{
	public class ABPathEndingCondition : global::Pathfinding.PathEndingCondition
	{
		public ABPathEndingCondition(global::Pathfinding.ABPath p)
		{
			if (p == null)
			{
				throw new global::System.ArgumentNullException("p");
			}
			this.abPath = p;
			this.path = p;
		}

		public override bool TargetFound(global::Pathfinding.PathNode node)
		{
			return node.node == this.abPath.endNode;
		}

		protected global::Pathfinding.ABPath abPath;
	}
}
