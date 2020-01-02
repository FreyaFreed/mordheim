using System;
using System.Collections.Generic;

public class TrapEffectIdComparer : global::System.Collections.Generic.IEqualityComparer<global::TrapEffectId>
{
	public bool Equals(global::TrapEffectId x, global::TrapEffectId y)
	{
		return x == y;
	}

	public int GetHashCode(global::TrapEffectId obj)
	{
		return (int)obj;
	}

	public static readonly global::TrapEffectIdComparer Instance = new global::TrapEffectIdComparer();
}
