using System;
using System.Collections.Generic;

public class EnchantmentComparer : global::System.Collections.Generic.IComparer<global::Enchantment>
{
	public int Compare(global::Enchantment x, global::Enchantment y)
	{
		if (x.Duration < y.Duration)
		{
			return -1;
		}
		if (x.Duration > y.Duration)
		{
			return 1;
		}
		return string.CompareOrdinal(x.Data.Name, y.Data.Name);
	}
}
