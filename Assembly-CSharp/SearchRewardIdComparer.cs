using System;
using System.Collections.Generic;

public class SearchRewardIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SearchRewardId>
{
	public bool Equals(global::SearchRewardId x, global::SearchRewardId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SearchRewardId obj)
	{
		return (int)obj;
	}

	public static readonly global::SearchRewardIdComparer Instance = new global::SearchRewardIdComparer();
}
