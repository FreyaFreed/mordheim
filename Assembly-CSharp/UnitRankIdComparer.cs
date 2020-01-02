using System;
using System.Collections.Generic;

public class UnitRankIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitRankId>
{
	public bool Equals(global::UnitRankId x, global::UnitRankId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitRankId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitRankIdComparer Instance = new global::UnitRankIdComparer();
}
