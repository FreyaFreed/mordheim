using System;
using System.Collections.Generic;

public class WeekDayIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WeekDayId>
{
	public bool Equals(global::WeekDayId x, global::WeekDayId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WeekDayId obj)
	{
		return (int)obj;
	}

	public static readonly global::WeekDayIdComparer Instance = new global::WeekDayIdComparer();
}
