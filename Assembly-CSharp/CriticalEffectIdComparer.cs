using System;
using System.Collections.Generic;

public class CriticalEffectIdComparer : global::System.Collections.Generic.IEqualityComparer<global::CriticalEffectId>
{
	public bool Equals(global::CriticalEffectId x, global::CriticalEffectId y)
	{
		return x == y;
	}

	public int GetHashCode(global::CriticalEffectId obj)
	{
		return (int)obj;
	}

	public static readonly global::CriticalEffectIdComparer Instance = new global::CriticalEffectIdComparer();
}
