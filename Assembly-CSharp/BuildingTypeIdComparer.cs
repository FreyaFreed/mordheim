using System;
using System.Collections.Generic;

public class BuildingTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::BuildingTypeId>
{
	public bool Equals(global::BuildingTypeId x, global::BuildingTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::BuildingTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::BuildingTypeIdComparer Instance = new global::BuildingTypeIdComparer();
}
