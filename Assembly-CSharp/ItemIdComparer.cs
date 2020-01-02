using System;
using System.Collections.Generic;

public class ItemIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ItemId>
{
	public bool Equals(global::ItemId x, global::ItemId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ItemId obj)
	{
		return (int)obj;
	}

	public static readonly global::ItemIdComparer Instance = new global::ItemIdComparer();
}
