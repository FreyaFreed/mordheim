using System;
using System.Collections.Generic;

public class FactionConsequenceTargetIdComparer : global::System.Collections.Generic.IEqualityComparer<global::FactionConsequenceTargetId>
{
	public bool Equals(global::FactionConsequenceTargetId x, global::FactionConsequenceTargetId y)
	{
		return x == y;
	}

	public int GetHashCode(global::FactionConsequenceTargetId obj)
	{
		return (int)obj;
	}

	public static readonly global::FactionConsequenceTargetIdComparer Instance = new global::FactionConsequenceTargetIdComparer();
}
