using System;

namespace Pathfinding
{
	public class NNConstraint
	{
		public virtual bool SuitableGraph(int graphIndex, global::Pathfinding.NavGraph graph)
		{
			return (this.graphMask >> graphIndex & 1) != 0;
		}

		public virtual bool Suitable(global::Pathfinding.GraphNode node)
		{
			return (!this.constrainWalkability || node.Walkable == this.walkable) && (!this.constrainArea || this.area < 0 || (ulong)node.Area == (ulong)((long)this.area)) && (!this.constrainTags || (this.tags >> (int)node.Tag & 1) != 0);
		}

		public static global::Pathfinding.NNConstraint Default
		{
			get
			{
				return new global::Pathfinding.NNConstraint();
			}
		}

		public static global::Pathfinding.NNConstraint None
		{
			get
			{
				return new global::Pathfinding.NNConstraint
				{
					constrainWalkability = false,
					constrainArea = false,
					constrainTags = false,
					constrainDistance = false,
					graphMask = -1
				};
			}
		}

		public int graphMask = -1;

		public bool constrainArea;

		public int area = -1;

		public bool constrainWalkability = true;

		public bool walkable = true;

		public bool distanceXZ;

		public bool constrainTags = true;

		public int tags = -1;

		public bool constrainDistance = true;
	}
}
