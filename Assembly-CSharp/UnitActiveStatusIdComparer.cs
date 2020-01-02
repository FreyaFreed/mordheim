using System;
using System.Collections.Generic;

public class UnitActiveStatusIdComparer : global::System.Collections.Generic.IEqualityComparer<global::UnitActiveStatusId>
{
	public bool Equals(global::UnitActiveStatusId x, global::UnitActiveStatusId y)
	{
		return x == y;
	}

	public int GetHashCode(global::UnitActiveStatusId obj)
	{
		return (int)obj;
	}

	public static readonly global::UnitActiveStatusIdComparer Instance = new global::UnitActiveStatusIdComparer();
}
