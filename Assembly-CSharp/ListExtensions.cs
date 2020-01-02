using System;
using System.Collections.Generic;

public static class ListExtensions
{
	public static void SafeAddRange<T>(this global::System.Collections.Generic.List<T> self, global::System.Collections.Generic.IEnumerable<T> toAdd)
	{
		if (toAdd != null)
		{
			self.AddRange(toAdd);
		}
	}

	public static bool Contains<T>(this global::System.Collections.Generic.List<T> self, T elmt, global::System.Collections.Generic.IEqualityComparer<T> comparer)
	{
		for (int i = 0; i < self.Count; i++)
		{
			if (comparer.Equals(self[i], elmt))
			{
				return true;
			}
		}
		return false;
	}

	public static int IndexOf<T>(this global::System.Collections.Generic.List<T> self, T elmt, global::System.Collections.Generic.IEqualityComparer<T> comparer)
	{
		for (int i = 0; i < self.Count; i++)
		{
			if (comparer.Equals(self[i], elmt))
			{
				return i;
			}
		}
		return -1;
	}
}
