using System;
using System.Collections.Generic;

public class UnitRoamingIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitRoamingId>
{
	public bool Equals(global::UnitRoamingId x, global::UnitRoamingId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitRoamingId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitRoamingIdComparer Instance = new global::UnitRoamingIdComparer();
}
