using System;
using System.Collections.Generic;

public class TrapTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::TrapTypeId>
{
	public bool Equals(global::TrapTypeId x, global::TrapTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::TrapTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::TrapTypeIdComparer Instance = new global::TrapTypeIdComparer();
}
