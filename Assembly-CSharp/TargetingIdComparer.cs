using System;
using System.Collections.Generic;

public class TargetingIdComparer : global::System.Collections.Generic.IEqualityComparer<global::TargetingId>
{
	public bool Equals(global::TargetingId x, global::TargetingId y)
	{
		return x == y;
	}

	public int GetHashCode(global::TargetingId obj)
	{
		return (int)obj;
	}

	public static readonly global::TargetingIdComparer Instance = new global::TargetingIdComparer();
}
