using System;
using System.Collections.Generic;

public class UnitSlotTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitSlotTypeId>
{
	public bool Equals(global::UnitSlotTypeId x, global::UnitSlotTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitSlotTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitSlotTypeIdComparer Instance = new global::UnitSlotTypeIdComparer();
}
