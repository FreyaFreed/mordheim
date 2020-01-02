using System;
using System.Collections.Generic;

public class SearchZoneIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SearchZoneId>
{
	public bool Equals(global::SearchZoneId x, global::SearchZoneId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SearchZoneId obj)
	{
		return (int)obj;
	}

	public static readonly global::SearchZoneIdComparer Instance = new global::SearchZoneIdComparer();
}
