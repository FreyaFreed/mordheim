using System;
using System.Collections.Generic;

public class MissionMapIdComparer : global::System.Collections.Generic.IEqualityComparer<global::MissionMapId>
{
	public bool Equals(global::MissionMapId x, global::MissionMapId y)
	{
		return x == y;
	}

	public int GetHashCode(global::MissionMapId obj)
	{
		return (int)obj;
	}

	public static readonly global::MissionMapIdComparer Instance = new global::MissionMapIdComparer();
}
