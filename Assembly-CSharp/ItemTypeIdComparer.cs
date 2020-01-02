using System;
using System.Collections.Generic;

public class ItemTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ItemTypeId>
{
	public bool Equals(global::ItemTypeId x, global::ItemTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ItemTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::ItemTypeIdComparer Instance = new global::ItemTypeIdComparer();
}
