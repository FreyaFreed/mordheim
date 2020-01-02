using System;
using System.Collections.Generic;

public class UnitStateIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitStateId>
{
	public bool Equals(global::UnitStateId x, global::UnitStateId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitStateId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitStateIdComparer Instance = new global::UnitStateIdComparer();
}
