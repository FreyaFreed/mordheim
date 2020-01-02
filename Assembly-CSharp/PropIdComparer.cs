using System;
using System.Collections.Generic;

public class PropIdComparer : global::System.Collections.Generic.IEqualityComparer<global::PropId>
{
	public bool Equals(global::PropId x, global::PropId y)
	{
		return x == y;
	}

	public int GetHashCode(global::PropId obj)
	{
		return (int)obj;
	}

	public static readonly global::PropIdComparer Instance = new global::PropIdComparer();
}
