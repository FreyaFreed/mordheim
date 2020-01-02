using System;
using System.Collections.Generic;

public class UnitSlotIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitSlotId>
{
	public bool Equals(global::UnitSlotId x, global::UnitSlotId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitSlotId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitSlotIdComparer Instance = new global::UnitSlotIdComparer();
}
