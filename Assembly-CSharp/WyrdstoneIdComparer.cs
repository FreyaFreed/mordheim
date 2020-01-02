using System;
using System.Collections.Generic;

public class WyrdstoneIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WyrdstoneId>
{
	public bool Equals(global::WyrdstoneId x, global::WyrdstoneId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WyrdstoneId obj)
	{
		return (int)obj;
	}

	public static readonly global::WyrdstoneIdComparer Instance = new global::WyrdstoneIdComparer();
}
