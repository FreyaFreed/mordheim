using System;
using System.Collections.Generic;

public class ItemQualityIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ItemQualityId>
{
	public bool Equals(global::ItemQualityId x, global::ItemQualityId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ItemQualityId obj)
	{
		return (int)obj;
	}

	public static readonly global::ItemQualityIdComparer Instance = new global::ItemQualityIdComparer();
}
