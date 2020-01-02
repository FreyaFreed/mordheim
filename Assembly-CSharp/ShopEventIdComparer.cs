using System;
using System.Collections.Generic;

public class ShopEventIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ShopEventId>
{
	public bool Equals(global::ShopEventId x, global::ShopEventId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ShopEventId obj)
	{
		return (int)obj;
	}

	public static readonly global::ShopEventIdComparer Instance = new global::ShopEventIdComparer();
}
