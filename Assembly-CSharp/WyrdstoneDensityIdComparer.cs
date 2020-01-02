using System;
using System.Collections.Generic;

public class WyrdstoneDensityIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WyrdstoneDensityId>
{
	public bool Equals(global::WyrdstoneDensityId x, global::WyrdstoneDensityId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WyrdstoneDensityId obj)
	{
		return (int)obj;
	}

	public static readonly global::WyrdstoneDensityIdComparer Instance = new global::WyrdstoneDensityIdComparer();
}
