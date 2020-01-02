using System;
using System.Collections.Generic;

public class EnchantmentDmgTriggerIdComparer : global::System.Collections.Generic.IEqualityComparer<global::EnchantmentDmgTriggerId>
{
	public bool Equals(global::EnchantmentDmgTriggerId x, global::EnchantmentDmgTriggerId y)
	{
		return x == y;
	}

	public int GetHashCode(global::EnchantmentDmgTriggerId obj)
	{
		return (int)obj;
	}

	public static readonly global::EnchantmentDmgTriggerIdComparer Instance = new global::EnchantmentDmgTriggerIdComparer();
}
