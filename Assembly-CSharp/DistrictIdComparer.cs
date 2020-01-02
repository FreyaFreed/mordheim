using System;
using System.Collections.Generic;

public class DistrictIdComparer : global::System.Collections.Generic.IEqualityComparer<global::DistrictId>
{
	public bool Equals(global::DistrictId x, global::DistrictId y)
	{
		return x == y;
	}

	public int GetHashCode(global::DistrictId obj)
	{
		return (int)obj;
	}

	public static readonly global::DistrictIdComparer Instance = new global::DistrictIdComparer();
}
