using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
	public static class ObjectPoolSimple<T> where T : class, new()
	{
		public static T Claim()
		{
			global::System.Collections.Generic.List<T> obj = global::Pathfinding.Util.ObjectPoolSimple<T>.pool;
			T result;
			lock (obj)
			{
				if (global::Pathfinding.Util.ObjectPoolSimple<T>.pool.Count > 0)
				{
					T t = global::Pathfinding.Util.ObjectPoolSimple<T>.pool[global::Pathfinding.Util.ObjectPoolSimple<T>.pool.Count - 1];
					global::Pathfinding.Util.ObjectPoolSimple<T>.pool.RemoveAt(global::Pathfinding.Util.ObjectPoolSimple<T>.pool.Count - 1);
					global::Pathfinding.Util.ObjectPoolSimple<T>.inPool.Remove(t);
					result = t;
				}
				else
				{
					result = global::System.Activator.CreateInstance<T>();
				}
			}
			return result;
		}

		public static void Release(ref T obj)
		{
			global::System.Collections.Generic.List<T> obj2 = global::Pathfinding.Util.ObjectPoolSimple<T>.pool;
			lock (obj2)
			{
				global::Pathfinding.Util.ObjectPoolSimple<T>.pool.Add(obj);
			}
			obj = (T)((object)null);
		}

		public static void Clear()
		{
			global::System.Collections.Generic.List<T> obj = global::Pathfinding.Util.ObjectPoolSimple<T>.pool;
			lock (obj)
			{
				global::Pathfinding.Util.ObjectPoolSimple<T>.pool.Clear();
			}
		}

		public static int GetSize()
		{
			return global::Pathfinding.Util.ObjectPoolSimple<T>.pool.Count;
		}

		private static global::System.Collections.Generic.List<T> pool = new global::System.Collections.Generic.List<T>();

		private static readonly global::System.Collections.Generic.HashSet<T> inPool = new global::System.Collections.Generic.HashSet<T>();
	}
}
