using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
	public static class ListPool<T>
	{
		public static global::System.Collections.Generic.List<T> Claim()
		{
			global::System.Collections.Generic.List<global::System.Collections.Generic.List<T>> obj = global::Pathfinding.Util.ListPool<T>.pool;
			global::System.Collections.Generic.List<T> result;
			lock (obj)
			{
				if (global::Pathfinding.Util.ListPool<T>.pool.Count > 0)
				{
					global::System.Collections.Generic.List<T> list = global::Pathfinding.Util.ListPool<T>.pool[global::Pathfinding.Util.ListPool<T>.pool.Count - 1];
					global::Pathfinding.Util.ListPool<T>.pool.RemoveAt(global::Pathfinding.Util.ListPool<T>.pool.Count - 1);
					global::Pathfinding.Util.ListPool<T>.inPool.Remove(list);
					result = list;
				}
				else
				{
					result = new global::System.Collections.Generic.List<T>();
				}
			}
			return result;
		}

		public static global::System.Collections.Generic.List<T> Claim(int capacity)
		{
			global::System.Collections.Generic.List<global::System.Collections.Generic.List<T>> obj = global::Pathfinding.Util.ListPool<T>.pool;
			global::System.Collections.Generic.List<T> result;
			lock (obj)
			{
				global::System.Collections.Generic.List<T> list = null;
				int index = -1;
				int num = 0;
				while (num < global::Pathfinding.Util.ListPool<T>.pool.Count && num < 8)
				{
					global::System.Collections.Generic.List<T> list2 = global::Pathfinding.Util.ListPool<T>.pool[global::Pathfinding.Util.ListPool<T>.pool.Count - 1 - num];
					if (list2.Capacity >= capacity)
					{
						global::Pathfinding.Util.ListPool<T>.pool.RemoveAt(global::Pathfinding.Util.ListPool<T>.pool.Count - 1 - num);
						global::Pathfinding.Util.ListPool<T>.inPool.Remove(list2);
						return list2;
					}
					if (list == null || list2.Capacity > list.Capacity)
					{
						list = list2;
						index = global::Pathfinding.Util.ListPool<T>.pool.Count - 1 - num;
					}
					num++;
				}
				if (list == null)
				{
					list = new global::System.Collections.Generic.List<T>(capacity);
				}
				else
				{
					list.Capacity = capacity;
					global::Pathfinding.Util.ListPool<T>.pool[index] = global::Pathfinding.Util.ListPool<T>.pool[global::Pathfinding.Util.ListPool<T>.pool.Count - 1];
					global::Pathfinding.Util.ListPool<T>.pool.RemoveAt(global::Pathfinding.Util.ListPool<T>.pool.Count - 1);
					global::Pathfinding.Util.ListPool<T>.inPool.Remove(list);
				}
				result = list;
			}
			return result;
		}

		public static void Warmup(int count, int size)
		{
			global::System.Collections.Generic.List<global::System.Collections.Generic.List<T>> obj = global::Pathfinding.Util.ListPool<T>.pool;
			lock (obj)
			{
				global::System.Collections.Generic.List<T>[] array = new global::System.Collections.Generic.List<T>[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = global::Pathfinding.Util.ListPool<T>.Claim(size);
				}
				for (int j = 0; j < count; j++)
				{
					global::Pathfinding.Util.ListPool<T>.Release(array[j]);
				}
			}
		}

		public static void Release(global::System.Collections.Generic.List<T> list)
		{
			list.Clear();
			global::System.Collections.Generic.List<global::System.Collections.Generic.List<T>> obj = global::Pathfinding.Util.ListPool<T>.pool;
			lock (obj)
			{
				global::Pathfinding.Util.ListPool<T>.pool.Add(list);
			}
		}

		public static void Clear()
		{
			global::System.Collections.Generic.List<global::System.Collections.Generic.List<T>> obj = global::Pathfinding.Util.ListPool<T>.pool;
			lock (obj)
			{
				global::Pathfinding.Util.ListPool<T>.pool.Clear();
			}
		}

		public static int GetSize()
		{
			return global::Pathfinding.Util.ListPool<T>.pool.Count;
		}

		private const int MaxCapacitySearchLength = 8;

		private static readonly global::System.Collections.Generic.List<global::System.Collections.Generic.List<T>> pool = new global::System.Collections.Generic.List<global::System.Collections.Generic.List<T>>();

		private static readonly global::System.Collections.Generic.HashSet<global::System.Collections.Generic.List<T>> inPool = new global::System.Collections.Generic.HashSet<global::System.Collections.Generic.List<T>>();
	}
}
