using System;
using System.Collections.Generic;

public class SearchIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SearchId>
{
	public bool Equals(global::SearchId x, global::SearchId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SearchId obj)
	{
		return (int)obj;
	}

	public static readonly global::SearchIdComparer Instance = new global::SearchIdComparer();
}
