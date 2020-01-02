using System;
using System.Collections.Generic;

public class MoonIdComparer : global::System.Collections.Generic.IEqualityComparer<global::MoonId>
{
	public bool Equals(global::MoonId x, global::MoonId y)
	{
		return x == y;
	}

	public int GetHashCode(global::MoonId obj)
	{
		return (int)obj;
	}

	public static readonly global::MoonIdComparer Instance = new global::MoonIdComparer();
}
