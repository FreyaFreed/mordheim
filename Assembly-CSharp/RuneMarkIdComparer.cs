using System;
using System.Collections.Generic;

public class RuneMarkIdComparer : global::System.Collections.Generic.IEqualityComparer<global::RuneMarkId>
{
	public bool Equals(global::RuneMarkId x, global::RuneMarkId y)
	{
		return x == y;
	}

	public int GetHashCode(global::RuneMarkId obj)
	{
		return (int)obj;
	}

	public static readonly global::RuneMarkIdComparer Instance = new global::RuneMarkIdComparer();
}
