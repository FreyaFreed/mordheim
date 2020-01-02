using System;
using System.Collections.Generic;

public class RaceIdComparer : global::System.Collections.Generic.IEqualityComparer<global::RaceId>
{
	public bool Equals(global::RaceId x, global::RaceId y)
	{
		return x == y;
	}

	public int GetHashCode(global::RaceId obj)
	{
		return (int)obj;
	}

	public static readonly global::RaceIdComparer Instance = new global::RaceIdComparer();
}
