using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public interface IRaycastableGraph
	{
		bool Linecast(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end);

		bool Linecast(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, global::Pathfinding.GraphNode hint);

		bool Linecast(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, global::Pathfinding.GraphNode hint, out global::Pathfinding.GraphHitInfo hit);

		bool Linecast(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, global::Pathfinding.GraphNode hint, out global::Pathfinding.GraphHitInfo hit, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> trace);
	}
}
