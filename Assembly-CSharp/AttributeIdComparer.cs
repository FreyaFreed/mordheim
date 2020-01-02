using System;
using System.Collections.Generic;

public class AttributeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AttributeId>
{
	public bool Equals(global::AttributeId x, global::AttributeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AttributeId obj)
	{
		return (int)obj;
	}

	public static readonly global::AttributeIdComparer Instance = new global::AttributeIdComparer();
}
