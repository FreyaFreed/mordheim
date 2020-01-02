using System;
using System.Collections.Generic;

public class SpellTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SpellTypeId>
{
	public bool Equals(global::SpellTypeId x, global::SpellTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SpellTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::SpellTypeIdComparer Instance = new global::SpellTypeIdComparer();
}
