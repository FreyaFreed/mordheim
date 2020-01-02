using System;
using System.Collections.Generic;

public class UnitActionRefreshIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitActionRefreshId>
{
	public bool Equals(global::UnitActionRefreshId x, global::UnitActionRefreshId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitActionRefreshId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitActionRefreshIdComparer Instance = new global::UnitActionRefreshIdComparer();
}
