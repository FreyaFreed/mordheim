using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::System.Obsolete("Generic version is now obsolete to trade an extremely tiny performance decrease for a large decrease in boilerplate for Path classes")]
	public static class PathPool<T> where T : global::Pathfinding.Path, new()
	{
		public static void Recycle(T path)
		{
			global::Pathfinding.PathPool.Pool(path);
		}

		public static void Warmup(int count, int length)
		{
			global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Warmup(count, length);
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Warmup(count, length);
			global::Pathfinding.Path[] array = new global::Pathfinding.Path[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = global::Pathfinding.PathPool<T>.GetPath();
				array[i].Claim(array);
			}
			for (int j = 0; j < count; j++)
			{
				array[j].Release(array, false);
			}
		}

		public static int GetTotalCreated()
		{
			return global::Pathfinding.PathPool.GetTotalCreated(typeof(T));
		}

		public static int GetSize()
		{
			return global::Pathfinding.PathPool.GetSize(typeof(T));
		}

		[global::System.Obsolete("Use PathPool.GetPath<T> instead of PathPool<T>.GetPath")]
		public static T GetPath()
		{
			return global::Pathfinding.PathPool.GetPath<T>();
		}
	}
}
