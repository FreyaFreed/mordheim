using System;
using System.Collections.Generic;

public class HolidayIdComparer : global::System.Collections.Generic.IEqualityComparer<global::HolidayId>
{
	public bool Equals(global::HolidayId x, global::HolidayId y)
	{
		return x == y;
	}

	public int GetHashCode(global::HolidayId obj)
	{
		return (int)obj;
	}

	public static readonly global::HolidayIdComparer Instance = new global::HolidayIdComparer();
}
