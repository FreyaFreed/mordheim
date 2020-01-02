using System;
using System.Collections.Generic;

public class PropRestrictionIdComparer : global::System.Collections.Generic.IEqualityComparer<global::PropRestrictionId>
{
	public bool Equals(global::PropRestrictionId x, global::PropRestrictionId y)
	{
		return x == y;
	}

	public int GetHashCode(global::PropRestrictionId obj)
	{
		return (int)obj;
	}

	public static readonly global::PropRestrictionIdComparer Instance = new global::PropRestrictionIdComparer();
}
