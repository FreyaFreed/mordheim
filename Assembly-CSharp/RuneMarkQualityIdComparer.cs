using System;
using System.Collections.Generic;

public class RuneMarkQualityIdComparer : global::System.Collections.Generic.IEqualityComparer<global::RuneMarkQualityId>
{
	public bool Equals(global::RuneMarkQualityId x, global::RuneMarkQualityId y)
	{
		return x == y;
	}

	public int GetHashCode(global::RuneMarkQualityId obj)
	{
		return (int)obj;
	}

	public static readonly global::RuneMarkQualityIdComparer Instance = new global::RuneMarkQualityIdComparer();
}
