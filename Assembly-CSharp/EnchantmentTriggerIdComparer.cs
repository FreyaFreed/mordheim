using System;
using System.Collections.Generic;

public class EnchantmentTriggerIdComparer : global::System.Collections.Generic.IEqualityComparer<global::EnchantmentTriggerId>
{
	public bool Equals(global::EnchantmentTriggerId x, global::EnchantmentTriggerId y)
	{
		return x == y;
	}

	public int GetHashCode(global::EnchantmentTriggerId obj)
	{
		return (int)obj;
	}

	public static readonly global::EnchantmentTriggerIdComparer Instance = new global::EnchantmentTriggerIdComparer();
}
