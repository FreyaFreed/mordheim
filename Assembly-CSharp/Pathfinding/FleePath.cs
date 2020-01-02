using System;
using UnityEngine;

namespace Pathfinding
{
	public class FleePath : global::Pathfinding.RandomPath
	{
		public static global::Pathfinding.FleePath Construct(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 avoid, int searchLength, global::Pathfinding.OnPathDelegate callback = null)
		{
			global::Pathfinding.FleePath path = global::Pathfinding.PathPool.GetPath<global::Pathfinding.FleePath>();
			path.Setup(start, avoid, searchLength, callback);
			return path;
		}

		protected void Setup(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 avoid, int searchLength, global::Pathfinding.OnPathDelegate callback)
		{
			base.Setup(start, searchLength, callback);
			this.aim = avoid - start;
			this.aim *= 10f;
			this.aim = start - this.aim;
		}
	}
}
