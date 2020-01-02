using System;
using System.Collections.Generic;

public class PlayerRankIdComparer : global::System.Collections.Generic.IEqualityComparer<global::PlayerRankId>
{
	public bool Equals(global::PlayerRankId x, global::PlayerRankId y)
	{
		return x == y;
	}

	public int GetHashCode(global::PlayerRankId obj)
	{
		return (int)obj;
	}

	public static readonly global::PlayerRankIdComparer Instance = new global::PlayerRankIdComparer();
}
