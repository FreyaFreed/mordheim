using System;
using System.Collections.Generic;

public class EnchantmentQualityIdComparer : global::System.Collections.Generic.IEqualityComparer<global::EnchantmentQualityId>
{
	public bool Equals(global::EnchantmentQualityId x, global::EnchantmentQualityId y)
	{
		return x == y;
	}

	public int GetHashCode(global::EnchantmentQualityId obj)
	{
		return (int)obj;
	}

	public static readonly global::EnchantmentQualityIdComparer Instance = new global::EnchantmentQualityIdComparer();
}
