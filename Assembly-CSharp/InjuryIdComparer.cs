using System;
using System.Collections.Generic;

public class InjuryIdComparer : global::System.Collections.Generic.IEqualityComparer<global::InjuryId>
{
	public bool Equals(global::InjuryId x, global::InjuryId y)
	{
		return x == y;
	}

	public int GetHashCode(global::InjuryId obj)
	{
		return (int)obj;
	}

	public static readonly global::InjuryIdComparer Instance = new global::InjuryIdComparer();
}
