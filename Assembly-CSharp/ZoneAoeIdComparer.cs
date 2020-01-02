using System;
using System.Collections.Generic;

public class ZoneAoeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ZoneAoeId>
{
	public bool Equals(global::ZoneAoeId x, global::ZoneAoeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ZoneAoeId obj)
	{
		return (int)obj;
	}

	public static readonly global::ZoneAoeIdComparer Instance = new global::ZoneAoeIdComparer();
}
