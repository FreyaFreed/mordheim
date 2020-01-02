using System;
using System.Collections.Generic;

public class LadderSorter : global::System.Collections.Generic.IComparer<global::UnitController>
{
	int global::System.Collections.Generic.IComparer<global::UnitController>.Compare(global::UnitController x, global::UnitController y)
	{
		if (x.unit.Initiative < y.unit.Initiative)
		{
			return 1;
		}
		if (x.unit.Initiative > y.unit.Initiative)
		{
			return -1;
		}
		return 0;
	}
}
