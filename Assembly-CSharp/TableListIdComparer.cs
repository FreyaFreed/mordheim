using System;
using System.Collections.Generic;

public class TableListIdComparer : global::System.Collections.Generic.IEqualityComparer<global::TableListId>
{
	public bool Equals(global::TableListId x, global::TableListId y)
	{
		return x == y;
	}

	public int GetHashCode(global::TableListId obj)
	{
		return (int)obj;
	}

	public static readonly global::TableListIdComparer Instance = new global::TableListIdComparer();
}
