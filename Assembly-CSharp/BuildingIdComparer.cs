using System;
using System.Collections.Generic;

public class BuildingIdComparer : global::System.Collections.Generic.IEqualityComparer<global::BuildingId>
{
	public bool Equals(global::BuildingId x, global::BuildingId y)
	{
		return x == y;
	}

	public int GetHashCode(global::BuildingId obj)
	{
		return (int)obj;
	}

	public static readonly global::BuildingIdComparer Instance = new global::BuildingIdComparer();
}
