using System;
using System.Collections.Generic;

public class WyrdstonePlacementIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WyrdstonePlacementId>
{
	public bool Equals(global::WyrdstonePlacementId x, global::WyrdstonePlacementId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WyrdstonePlacementId obj)
	{
		return (int)obj;
	}

	public static readonly global::WyrdstonePlacementIdComparer Instance = new global::WyrdstonePlacementIdComparer();
}
