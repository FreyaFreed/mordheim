using System;
using System.Collections.Generic;
using Pathfinding.Util;

namespace Pathfinding
{
	public static class GraphUpdateUtilities
	{
		public static bool UpdateGraphsNoBlock(global::Pathfinding.GraphUpdateObject guo, global::Pathfinding.GraphNode node1, global::Pathfinding.GraphNode node2, bool alwaysRevert = false)
		{
			global::System.Collections.Generic.List<global::Pathfinding.GraphNode> list = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Claim();
			list.Add(node1);
			list.Add(node2);
			bool result = global::Pathfinding.GraphUpdateUtilities.UpdateGraphsNoBlock(guo, list, alwaysRevert);
			global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Release(list);
			return result;
		}

		public static bool UpdateGraphsNoBlock(global::Pathfinding.GraphUpdateObject guo, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> nodes, bool alwaysRevert = false)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				if (!nodes[i].Walkable)
				{
					return false;
				}
			}
			guo.trackChangedNodes = true;
			bool worked = true;
			global::AstarPath.RegisterSafeUpdate(delegate()
			{
				global::AstarPath.active.UpdateGraphs(guo);
				global::AstarPath.active.FlushGraphUpdates();
				worked = (worked && global::Pathfinding.PathUtilities.IsPathPossible(nodes));
				if (!worked || alwaysRevert)
				{
					guo.RevertFromBackup();
					global::AstarPath.active.FloodFill();
				}
			});
			global::AstarPath.active.FlushThreadSafeCallbacks();
			guo.trackChangedNodes = false;
			return worked;
		}
	}
}
