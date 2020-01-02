using System;
using System.Collections.Generic;

public class AllegianceIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AllegianceId>
{
	public bool Equals(global::AllegianceId x, global::AllegianceId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AllegianceId obj)
	{
		return (int)obj;
	}

	public static readonly global::AllegianceIdComparer Instance = new global::AllegianceIdComparer();
}
