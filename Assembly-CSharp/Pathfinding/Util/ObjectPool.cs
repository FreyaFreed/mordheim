using System;

namespace Pathfinding.Util
{
	public static class ObjectPool<T> where T : class, global::Pathfinding.Util.IAstarPooledObject, new()
	{
		public static T Claim()
		{
			return global::Pathfinding.Util.ObjectPoolSimple<T>.Claim();
		}

		public static void Release(ref T obj)
		{
			T t = obj;
			global::Pathfinding.Util.ObjectPoolSimple<T>.Release(ref obj);
			t.OnEnterPool();
		}
	}
}
