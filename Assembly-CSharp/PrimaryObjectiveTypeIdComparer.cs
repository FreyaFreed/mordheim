using System;
using System.Collections.Generic;

public class PrimaryObjectiveTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::PrimaryObjectiveTypeId>
{
	public bool Equals(global::PrimaryObjectiveTypeId x, global::PrimaryObjectiveTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::PrimaryObjectiveTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::PrimaryObjectiveTypeIdComparer Instance = new global::PrimaryObjectiveTypeIdComparer();
}
