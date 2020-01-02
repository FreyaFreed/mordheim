using System;
using System.Collections.Generic;

public class ShrineIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ShrineId>
{
	public bool Equals(global::ShrineId x, global::ShrineId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ShrineId obj)
	{
		return (int)obj;
	}

	public static readonly global::ShrineIdComparer Instance = new global::ShrineIdComparer();
}
