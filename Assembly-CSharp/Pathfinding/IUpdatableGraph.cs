using System;

namespace Pathfinding
{
	public interface IUpdatableGraph
	{
		void UpdateArea(global::Pathfinding.GraphUpdateObject o);

		void UpdateAreaInit(global::Pathfinding.GraphUpdateObject o);

		void UpdateAreaPost(global::Pathfinding.GraphUpdateObject o);

		global::Pathfinding.GraphUpdateThreading CanUpdateAsync(global::Pathfinding.GraphUpdateObject o);
	}
}
