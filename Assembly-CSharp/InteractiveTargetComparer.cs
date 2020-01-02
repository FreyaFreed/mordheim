using System;
using System.Collections.Generic;

public class InteractiveTargetComparer : global::System.Collections.Generic.IComparer<global::InteractiveTarget>
{
	int global::System.Collections.Generic.IComparer<global::InteractiveTarget>.Compare(global::InteractiveTarget x, global::InteractiveTarget y)
	{
		if (x.action.actionData.SortWeight < y.action.actionData.SortWeight)
		{
			return 1;
		}
		if (x.action.actionData.SortWeight > y.action.actionData.SortWeight)
		{
			return -1;
		}
		return 0;
	}
}
