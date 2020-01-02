using System;
using System.Collections.Generic;

public class SearchNodeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SearchNodeId>
{
	public bool Equals(global::SearchNodeId x, global::SearchNodeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SearchNodeId obj)
	{
		return (int)obj;
	}

	public static readonly global::SearchNodeIdComparer Instance = new global::SearchNodeIdComparer();
}
