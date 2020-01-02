using System;
using System.Collections.Generic;

public class MissionMapLayoutIdComparer : global::System.Collections.Generic.IEqualityComparer<global::MissionMapLayoutId>
{
	public bool Equals(global::MissionMapLayoutId x, global::MissionMapLayoutId y)
	{
		return x == y;
	}

	public int GetHashCode(global::MissionMapLayoutId obj)
	{
		return (int)obj;
	}

	public static readonly global::MissionMapLayoutIdComparer Instance = new global::MissionMapLayoutIdComparer();
}
