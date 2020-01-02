using System;
using System.Collections.Generic;

public class UnitRigIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitRigId>
{
	public bool Equals(global::UnitRigId x, global::UnitRigId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitRigId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitRigIdComparer Instance = new global::UnitRigIdComparer();
}
