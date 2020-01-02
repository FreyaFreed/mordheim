using System;

namespace FxProNS
{
	public abstract class Singleton<T> where T : class, new()
	{
		private static bool Compare<T>(T x, T y) where T : class
		{
			return x == y;
		}

		public static T Instance
		{
			get
			{
				if (global::FxProNS.Singleton<T>.Compare<T>((T)((object)null), global::FxProNS.Singleton<T>.instance))
				{
					global::FxProNS.Singleton<T>.instance = global::System.Activator.CreateInstance<T>();
				}
				return global::FxProNS.Singleton<T>.instance;
			}
		}

		private static T instance;
	}
}
