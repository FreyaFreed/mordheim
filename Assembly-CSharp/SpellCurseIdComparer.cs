using System;
using System.Collections.Generic;

public class SpellCurseIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SpellCurseId>
{
	public bool Equals(global::SpellCurseId x, global::SpellCurseId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SpellCurseId obj)
	{
		return (int)obj;
	}

	public static readonly global::SpellCurseIdComparer Instance = new global::SpellCurseIdComparer();
}
