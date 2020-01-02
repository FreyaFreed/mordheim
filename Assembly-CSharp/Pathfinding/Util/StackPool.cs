using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
	public static class StackPool<T>
	{
		public static global::System.Collections.Generic.Stack<T> Claim()
		{
			if (global::Pathfinding.Util.StackPool<T>.pool.Count > 0)
			{
				global::System.Collections.Generic.Stack<T> result = global::Pathfinding.Util.StackPool<T>.pool[global::Pathfinding.Util.StackPool<T>.pool.Count - 1];
				global::Pathfinding.Util.StackPool<T>.pool.RemoveAt(global::Pathfinding.Util.StackPool<T>.pool.Count - 1);
				return result;
			}
			return new global::System.Collections.Generic.Stack<T>();
		}

		public static void Warmup(int count)
		{
			global::System.Collections.Generic.Stack<T>[] array = new global::System.Collections.Generic.Stack<T>[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = global::Pathfinding.Util.StackPool<T>.Claim();
			}
			for (int j = 0; j < count; j++)
			{
				global::Pathfinding.Util.StackPool<T>.Release(array[j]);
			}
		}

		public static void Release(global::System.Collections.Generic.Stack<T> stack)
		{
			for (int i = 0; i < global::Pathfinding.Util.StackPool<T>.pool.Count; i++)
			{
				if (global::Pathfinding.Util.StackPool<T>.pool[i] == stack)
				{
					global::UnityEngine.Debug.LogError("The Stack is released even though it is inside the pool");
				}
			}
			stack.Clear();
			global::Pathfinding.Util.StackPool<T>.pool.Add(stack);
		}

		public static void Clear()
		{
			global::Pathfinding.Util.StackPool<T>.pool.Clear();
		}

		public static int GetSize()
		{
			return global::Pathfinding.Util.StackPool<T>.pool.Count;
		}

		private static readonly global::System.Collections.Generic.List<global::System.Collections.Generic.Stack<T>> pool = new global::System.Collections.Generic.List<global::System.Collections.Generic.Stack<T>>();
	}
}
