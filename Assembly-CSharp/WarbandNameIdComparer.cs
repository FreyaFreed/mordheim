using System;
using System.Collections.Generic;

public class WarbandNameIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WarbandNameId>
{
	public bool Equals(global::WarbandNameId x, global::WarbandNameId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WarbandNameId obj)
	{
		return (int)obj;
	}

	public static readonly global::WarbandNameIdComparer Instance = new global::WarbandNameIdComparer();
}
