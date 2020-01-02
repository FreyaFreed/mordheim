using System;
using System.Collections.Generic;

public class TableFilterIdComparer : global::System.Collections.Generic.IEqualityComparer<global::TableFilterId>
{
	public bool Equals(global::TableFilterId x, global::TableFilterId y)
	{
		return x == y;
	}

	public int GetHashCode(global::TableFilterId obj)
	{
		return (int)obj;
	}

	public static readonly global::TableFilterIdComparer Instance = new global::TableFilterIdComparer();
}
