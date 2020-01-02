using System;
using System.Collections.Generic;

public class WarbandSlotTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WarbandSlotTypeId>
{
	public bool Equals(global::WarbandSlotTypeId x, global::WarbandSlotTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WarbandSlotTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::WarbandSlotTypeIdComparer Instance = new global::WarbandSlotTypeIdComparer();
}
