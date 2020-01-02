using System;
using System.Collections.Generic;

public class WarbandAttributeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WarbandAttributeId>
{
	public bool Equals(global::WarbandAttributeId x, global::WarbandAttributeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WarbandAttributeId obj)
	{
		return (int)obj;
	}

	public static readonly global::WarbandAttributeIdComparer Instance = new global::WarbandAttributeIdComparer();
}
