using System;
using System.Collections.Generic;

public class WarbandIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WarbandId>
{
	public bool Equals(global::WarbandId x, global::WarbandId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WarbandId obj)
	{
		return (int)obj;
	}

	public static readonly global::WarbandIdComparer Instance = new global::WarbandIdComparer();
}
