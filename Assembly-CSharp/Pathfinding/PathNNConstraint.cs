using System;

namespace Pathfinding
{
	public class PathNNConstraint : global::Pathfinding.NNConstraint
	{
		public new static global::Pathfinding.PathNNConstraint Default
		{
			get
			{
				return new global::Pathfinding.PathNNConstraint
				{
					constrainArea = true
				};
			}
		}

		public virtual void SetStart(global::Pathfinding.GraphNode node)
		{
			if (node != null)
			{
				this.area = (int)node.Area;
			}
			else
			{
				this.constrainArea = false;
			}
		}
	}
}
