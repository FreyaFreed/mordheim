using System;
using System.Collections.Generic;

public class ArmorStyleIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ArmorStyleId>
{
	public bool Equals(global::ArmorStyleId x, global::ArmorStyleId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ArmorStyleId obj)
	{
		return (int)obj;
	}

	public static readonly global::ArmorStyleIdComparer Instance = new global::ArmorStyleIdComparer();
}
