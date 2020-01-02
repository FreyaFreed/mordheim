using System;
using System.Collections.Generic;

public class EnchantmentConsumeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::EnchantmentConsumeId>
{
	public bool Equals(global::EnchantmentConsumeId x, global::EnchantmentConsumeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::EnchantmentConsumeId obj)
	{
		return (int)obj;
	}

	public static readonly global::EnchantmentConsumeIdComparer Instance = new global::EnchantmentConsumeIdComparer();
}
