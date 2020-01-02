using System;
using System.Collections.Generic;

public class UnitWoundIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitWoundId>
{
	public bool Equals(global::UnitWoundId x, global::UnitWoundId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitWoundId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitWoundIdComparer Instance = new global::UnitWoundIdComparer();
}
