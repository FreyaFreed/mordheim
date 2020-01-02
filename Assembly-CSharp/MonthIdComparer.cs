using System;
using System.Collections.Generic;

public class MonthIdComparer : global::System.Collections.Generic.IEqualityComparer<global::MonthId>
{
	public bool Equals(global::MonthId x, global::MonthId y)
	{
		return x == y;
	}

	public int GetHashCode(global::MonthId obj)
	{
		return (int)obj;
	}

	public static readonly global::MonthIdComparer Instance = new global::MonthIdComparer();
}
