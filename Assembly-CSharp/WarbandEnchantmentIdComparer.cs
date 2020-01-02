using System;
using System.Collections.Generic;

public class WarbandEnchantmentIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WarbandEnchantmentId>
{
	public bool Equals(global::WarbandEnchantmentId x, global::WarbandEnchantmentId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WarbandEnchantmentId obj)
	{
		return (int)obj;
	}

	public static readonly global::WarbandEnchantmentIdComparer Instance = new global::WarbandEnchantmentIdComparer();
}
