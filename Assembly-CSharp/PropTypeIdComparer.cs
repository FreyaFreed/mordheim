using System;
using System.Collections.Generic;

public class PropTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::PropTypeId>
{
	public bool Equals(global::PropTypeId x, global::PropTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::PropTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::PropTypeIdComparer Instance = new global::PropTypeIdComparer();
}
