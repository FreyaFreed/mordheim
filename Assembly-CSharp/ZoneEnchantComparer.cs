using System;
using System.Collections.Generic;

public class ZoneEnchantComparer : global::System.Collections.Generic.IComparer<global::ZoneAoeEnchantmentData>
{
	int global::System.Collections.Generic.IComparer<global::ZoneAoeEnchantmentData>.Compare(global::ZoneAoeEnchantmentData x, global::ZoneAoeEnchantmentData y)
	{
		if (x.ZoneTriggerId > y.ZoneTriggerId)
		{
			return 1;
		}
		if (x.ZoneTriggerId < y.ZoneTriggerId)
		{
			return -1;
		}
		return 0;
	}
}
