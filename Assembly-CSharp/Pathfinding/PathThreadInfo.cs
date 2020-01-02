using System;

namespace Pathfinding
{
	public struct PathThreadInfo
	{
		public PathThreadInfo(int index, global::AstarPath astar, global::Pathfinding.PathHandler runData)
		{
			this.threadIndex = index;
			this.astar = astar;
			this.runData = runData;
		}

		public readonly int threadIndex;

		public readonly global::AstarPath astar;

		public readonly global::Pathfinding.PathHandler runData;
	}
}
