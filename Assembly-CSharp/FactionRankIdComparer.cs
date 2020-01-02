using System;
using System.Collections.Generic;

public class FactionRankIdComparer : global::System.Collections.Generic.IEqualityComparer<global::FactionRankId>
{
	public bool Equals(global::FactionRankId x, global::FactionRankId y)
	{
		return x == y;
	}

	public int GetHashCode(global::FactionRankId obj)
	{
		return (int)obj;
	}

	public static readonly global::FactionRankIdComparer Instance = new global::FactionRankIdComparer();
}
