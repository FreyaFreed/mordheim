using System;
using System.Collections.Generic;

public class ConstantIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ConstantId>
{
	public bool Equals(global::ConstantId x, global::ConstantId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ConstantId obj)
	{
		return (int)obj;
	}

	public static readonly global::ConstantIdComparer Instance = new global::ConstantIdComparer();
}
