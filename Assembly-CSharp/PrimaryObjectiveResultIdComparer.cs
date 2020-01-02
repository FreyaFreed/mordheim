using System;
using System.Collections.Generic;

public class PrimaryObjectiveResultIdComparer : global::System.Collections.Generic.IEqualityComparer<global::PrimaryObjectiveResultId>
{
	public bool Equals(global::PrimaryObjectiveResultId x, global::PrimaryObjectiveResultId y)
	{
		return x == y;
	}

	public int GetHashCode(global::PrimaryObjectiveResultId obj)
	{
		return (int)obj;
	}

	public static readonly global::PrimaryObjectiveResultIdComparer Instance = new global::PrimaryObjectiveResultIdComparer();
}
