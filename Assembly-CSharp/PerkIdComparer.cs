using System;
using System.Collections.Generic;

public class PerkIdComparer : global::System.Collections.Generic.IEqualityComparer<global::PerkId>
{
	public bool Equals(global::PerkId x, global::PerkId y)
	{
		return x == y;
	}

	public int GetHashCode(global::PerkId obj)
	{
		return (int)obj;
	}

	public static readonly global::PerkIdComparer Instance = new global::PerkIdComparer();
}
