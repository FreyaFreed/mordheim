using System;

namespace Pathfinding
{
	public abstract class PathEndingCondition
	{
		protected PathEndingCondition()
		{
		}

		public PathEndingCondition(global::Pathfinding.Path p)
		{
			if (p == null)
			{
				throw new global::System.ArgumentNullException("p");
			}
			this.path = p;
		}

		public abstract bool TargetFound(global::Pathfinding.PathNode node);

		protected global::Pathfinding.Path path;
	}
}
