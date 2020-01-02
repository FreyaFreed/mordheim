using System;
using System.Collections.Generic;

public class ItemRangeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ItemRangeId>
{
	public bool Equals(global::ItemRangeId x, global::ItemRangeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ItemRangeId obj)
	{
		return (int)obj;
	}

	public static readonly global::ItemRangeIdComparer Instance = new global::ItemRangeIdComparer();
}
