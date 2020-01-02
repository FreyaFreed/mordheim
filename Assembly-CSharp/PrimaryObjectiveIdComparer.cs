using System;
using System.Collections.Generic;

public class PrimaryObjectiveIdComparer : global::System.Collections.Generic.IEqualityComparer<global::PrimaryObjectiveId>
{
	public bool Equals(global::PrimaryObjectiveId x, global::PrimaryObjectiveId y)
	{
		return x == y;
	}

	public int GetHashCode(global::PrimaryObjectiveId obj)
	{
		return (int)obj;
	}

	public static readonly global::PrimaryObjectiveIdComparer Instance = new global::PrimaryObjectiveIdComparer();
}
