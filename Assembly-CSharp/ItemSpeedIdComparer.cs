using System;
using System.Collections.Generic;

public class ItemSpeedIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ItemSpeedId>
{
	public bool Equals(global::ItemSpeedId x, global::ItemSpeedId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ItemSpeedId obj)
	{
		return (int)obj;
	}

	public static readonly global::ItemSpeedIdComparer Instance = new global::ItemSpeedIdComparer();
}
