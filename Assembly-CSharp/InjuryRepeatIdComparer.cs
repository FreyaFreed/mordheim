using System;
using System.Collections.Generic;

public class InjuryRepeatIdComparer : global::System.Collections.Generic.IEqualityComparer<global::InjuryRepeatId>
{
	public bool Equals(global::InjuryRepeatId x, global::InjuryRepeatId y)
	{
		return x == y;
	}

	public int GetHashCode(global::InjuryRepeatId obj)
	{
		return (int)obj;
	}

	public static readonly global::InjuryRepeatIdComparer Instance = new global::InjuryRepeatIdComparer();
}
