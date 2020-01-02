using System;
using System.Collections.Generic;

public class UnitBaseIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitBaseId>
{
	public bool Equals(global::UnitBaseId x, global::UnitBaseId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitBaseId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitBaseIdComparer Instance = new global::UnitBaseIdComparer();
}
