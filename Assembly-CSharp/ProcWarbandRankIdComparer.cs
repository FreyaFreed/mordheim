using System;
using System.Collections.Generic;

public class ProcWarbandRankIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ProcWarbandRankId>
{
	public bool Equals(global::ProcWarbandRankId x, global::ProcWarbandRankId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ProcWarbandRankId obj)
	{
		return (int)obj;
	}

	public static readonly global::ProcWarbandRankIdComparer Instance = new global::ProcWarbandRankIdComparer();
}
