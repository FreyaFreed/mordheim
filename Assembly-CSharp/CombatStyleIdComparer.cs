using System;
using System.Collections.Generic;

public class CombatStyleIdComparer : global::System.Collections.Generic.IEqualityComparer<global::CombatStyleId>
{
	public bool Equals(global::CombatStyleId x, global::CombatStyleId y)
	{
		return x == y;
	}

	public int GetHashCode(global::CombatStyleId obj)
	{
		return (int)obj;
	}

	public static readonly global::CombatStyleIdComparer Instance = new global::CombatStyleIdComparer();
}
