using System;
using System.Collections.Generic;

public class UnitIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitId>
{
	public bool Equals(global::UnitId x, global::UnitId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitIdComparer Instance = new global::UnitIdComparer();
}
