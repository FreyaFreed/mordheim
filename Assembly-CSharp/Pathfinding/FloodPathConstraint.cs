using System;
using UnityEngine;

namespace Pathfinding
{
	public class FloodPathConstraint : global::Pathfinding.NNConstraint
	{
		public FloodPathConstraint(global::Pathfinding.FloodPath path)
		{
			if (path == null)
			{
				global::UnityEngine.Debug.LogWarning("FloodPathConstraint should not be used with a NULL path");
			}
			this.path = path;
		}

		public override bool Suitable(global::Pathfinding.GraphNode node)
		{
			return base.Suitable(node) && this.path.HasPathTo(node);
		}

		private readonly global::Pathfinding.FloodPath path;
	}
}
