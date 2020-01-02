using System;
using System.Collections.Generic;

public class ItemCategoryIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ItemCategoryId>
{
	public bool Equals(global::ItemCategoryId x, global::ItemCategoryId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ItemCategoryId obj)
	{
		return (int)obj;
	}

	public static readonly global::ItemCategoryIdComparer Instance = new global::ItemCategoryIdComparer();
}
