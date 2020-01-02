using System;
using System.Collections.Generic;

public class ProcMissionRatingIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ProcMissionRatingId>
{
	public bool Equals(global::ProcMissionRatingId x, global::ProcMissionRatingId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ProcMissionRatingId obj)
	{
		return (int)obj;
	}

	public static readonly global::ProcMissionRatingIdComparer Instance = new global::ProcMissionRatingIdComparer();
}
