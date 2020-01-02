using System;
using System.Collections.Generic;

public class ObjectiveComparer : global::System.Collections.Generic.IComparer<global::Objective>
{
	int global::System.Collections.Generic.IComparer<global::Objective>.Compare(global::Objective x, global::Objective y)
	{
		if (x.SortWeight < y.SortWeight)
		{
			return 1;
		}
		if (x.SortWeight > y.SortWeight)
		{
			return -1;
		}
		return 0;
	}
}
