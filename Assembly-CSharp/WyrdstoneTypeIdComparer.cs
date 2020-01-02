using System;
using System.Collections.Generic;

public class WyrdstoneTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WyrdstoneTypeId>
{
	public bool Equals(global::WyrdstoneTypeId x, global::WyrdstoneTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WyrdstoneTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::WyrdstoneTypeIdComparer Instance = new global::WyrdstoneTypeIdComparer();
}
