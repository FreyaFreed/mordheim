using System;
using System.Collections.Generic;

public class WyrdstoneShipmentIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WyrdstoneShipmentId>
{
	public bool Equals(global::WyrdstoneShipmentId x, global::WyrdstoneShipmentId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WyrdstoneShipmentId obj)
	{
		return (int)obj;
	}

	public static readonly global::WyrdstoneShipmentIdComparer Instance = new global::WyrdstoneShipmentIdComparer();
}
