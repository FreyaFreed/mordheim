using System;
using System.Collections.Generic;

public class SearchDensityIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SearchDensityId>
{
	public bool Equals(global::SearchDensityId x, global::SearchDensityId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SearchDensityId obj)
	{
		return (int)obj;
	}

	public static readonly global::SearchDensityIdComparer Instance = new global::SearchDensityIdComparer();
}
