using System;
using System.Collections.Generic;

public class WarbandRankIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WarbandRankId>
{
	public bool Equals(global::WarbandRankId x, global::WarbandRankId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WarbandRankId obj)
	{
		return (int)obj;
	}

	public static readonly global::WarbandRankIdComparer Instance = new global::WarbandRankIdComparer();
}
