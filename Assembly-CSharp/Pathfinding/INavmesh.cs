using System;

namespace Pathfinding
{
	public interface INavmesh
	{
		void GetNodes(global::Pathfinding.GraphNodeDelegateCancelable del);
	}
}
