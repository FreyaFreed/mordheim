using System;
using System.Collections.Generic;

public class EnchantmentTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::EnchantmentTypeId>
{
	public bool Equals(global::EnchantmentTypeId x, global::EnchantmentTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::EnchantmentTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::EnchantmentTypeIdComparer Instance = new global::EnchantmentTypeIdComparer();
}
