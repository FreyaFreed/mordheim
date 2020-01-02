using System;
using System.Collections.Generic;

namespace Pathfinding
{
	public static class PathPool
	{
		public static void Pool(global::Pathfinding.Path path)
		{
			global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Collections.Generic.Stack<global::Pathfinding.Path>> obj = global::Pathfinding.PathPool.pool;
			lock (obj)
			{
				if (path.pooled)
				{
					throw new global::System.ArgumentException("The path is already pooled.");
				}
				global::System.Collections.Generic.Stack<global::Pathfinding.Path> stack;
				if (!global::Pathfinding.PathPool.pool.TryGetValue(path.GetType(), out stack))
				{
					stack = new global::System.Collections.Generic.Stack<global::Pathfinding.Path>();
					global::Pathfinding.PathPool.pool[path.GetType()] = stack;
				}
				path.pooled = true;
				path.OnEnterPool();
				stack.Push(path);
			}
		}

		public static int GetTotalCreated(global::System.Type type)
		{
			int result;
			if (global::Pathfinding.PathPool.totalCreated.TryGetValue(type, out result))
			{
				return result;
			}
			return 0;
		}

		public static int GetSize(global::System.Type type)
		{
			global::System.Collections.Generic.Stack<global::Pathfinding.Path> stack;
			if (global::Pathfinding.PathPool.pool.TryGetValue(type, out stack))
			{
				return stack.Count;
			}
			return 0;
		}

		public static T GetPath<T>() where T : global::Pathfinding.Path, new()
		{
			global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Collections.Generic.Stack<global::Pathfinding.Path>> obj = global::Pathfinding.PathPool.pool;
			T result;
			lock (obj)
			{
				global::System.Collections.Generic.Stack<global::Pathfinding.Path> stack;
				T t;
				if (global::Pathfinding.PathPool.pool.TryGetValue(typeof(T), out stack) && stack.Count > 0)
				{
					t = (stack.Pop() as T);
				}
				else
				{
					t = global::System.Activator.CreateInstance<T>();
					if (!global::Pathfinding.PathPool.totalCreated.ContainsKey(typeof(T)))
					{
						global::Pathfinding.PathPool.totalCreated[typeof(T)] = 0;
					}
					global::System.Collections.Generic.Dictionary<global::System.Type, int> dictionary2;
					global::System.Collections.Generic.Dictionary<global::System.Type, int> dictionary = dictionary2 = global::Pathfinding.PathPool.totalCreated;
					global::System.Type typeFromHandle;
					global::System.Type key = typeFromHandle = typeof(T);
					int num = dictionary2[typeFromHandle];
					dictionary[key] = num + 1;
				}
				t.pooled = false;
				t.Reset();
				result = t;
			}
			return result;
		}

		private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Collections.Generic.Stack<global::Pathfinding.Path>> pool = new global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Collections.Generic.Stack<global::Pathfinding.Path>>();

		private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, int> totalCreated = new global::System.Collections.Generic.Dictionary<global::System.Type, int>();
	}
}
