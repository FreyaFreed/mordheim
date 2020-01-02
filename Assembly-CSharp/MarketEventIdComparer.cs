using System;
using System.Collections.Generic;

public class MarketEventIdComparer : global::System.Collections.Generic.IEqualityComparer<global::MarketEventId>
{
	public bool Equals(global::MarketEventId x, global::MarketEventId y)
	{
		return x == y;
	}

	public int GetHashCode(global::MarketEventId obj)
	{
		return (int)obj;
	}

	public static readonly global::MarketEventIdComparer Instance = new global::MarketEventIdComparer();
}
