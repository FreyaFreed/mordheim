using System;
using System.Collections.Generic;

public class EnchantmentIdComparer : global::System.Collections.Generic.IEqualityComparer<global::EnchantmentId>
{
	public bool Equals(global::EnchantmentId x, global::EnchantmentId y)
	{
		return x == y;
	}

	public int GetHashCode(global::EnchantmentId obj)
	{
		return (int)obj;
	}

	public static readonly global::EnchantmentIdComparer Instance = new global::EnchantmentIdComparer();
}
