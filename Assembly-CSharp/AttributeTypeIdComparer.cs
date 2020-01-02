using System;
using System.Collections.Generic;

public class AttributeTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AttributeTypeId>
{
	public bool Equals(global::AttributeTypeId x, global::AttributeTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AttributeTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::AttributeTypeIdComparer Instance = new global::AttributeTypeIdComparer();
}
