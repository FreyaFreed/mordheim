using System;
using System.Collections.Generic;

public class HitRollSorter : global::System.Collections.Generic.IComparer<global::Tuple<int, global::UnitController>>
{
	int global::System.Collections.Generic.IComparer<global::Tuple<int, global::UnitController>>.Compare(global::Tuple<int, global::UnitController> x, global::Tuple<int, global::UnitController> y)
	{
		if (x.Item1 < y.Item1)
		{
			return -1;
		}
		if (x.Item1 > y.Item1)
		{
			return 1;
		}
		return 0;
	}
}
