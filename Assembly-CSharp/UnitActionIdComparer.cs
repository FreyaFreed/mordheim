using System;
using System.Collections.Generic;

public class UnitActionIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitActionId>
{
	public bool Equals(global::UnitActionId x, global::UnitActionId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitActionId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitActionIdComparer Instance = new global::UnitActionIdComparer();
}
