using System;
using System.Collections.Generic;

public class UnitTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitTypeId>
{
	public bool Equals(global::UnitTypeId x, global::UnitTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitTypeIdComparer Instance = new global::UnitTypeIdComparer();
}
