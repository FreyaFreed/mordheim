using System;
using System.Collections.Generic;

public class EffectTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::EffectTypeId>
{
	public bool Equals(global::EffectTypeId x, global::EffectTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::EffectTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::EffectTypeIdComparer Instance = new global::EffectTypeIdComparer();
}
