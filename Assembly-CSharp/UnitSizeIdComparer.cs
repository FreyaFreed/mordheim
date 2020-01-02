using System;
using System.Collections.Generic;

public class UnitSizeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitSizeId>
{
	public bool Equals(global::UnitSizeId x, global::UnitSizeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitSizeId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitSizeIdComparer Instance = new global::UnitSizeIdComparer();
}
