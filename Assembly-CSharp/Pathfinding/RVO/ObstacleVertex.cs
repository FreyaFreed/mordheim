using System;
using UnityEngine;

namespace Pathfinding.RVO
{
	public class ObstacleVertex
	{
		public bool ignore;

		public global::UnityEngine.Vector3 position;

		public global::UnityEngine.Vector2 dir;

		public float height;

		public global::Pathfinding.RVO.RVOLayer layer = global::Pathfinding.RVO.RVOLayer.DefaultObstacle;

		public global::Pathfinding.RVO.ObstacleVertex next;

		public global::Pathfinding.RVO.ObstacleVertex prev;
	}
}
