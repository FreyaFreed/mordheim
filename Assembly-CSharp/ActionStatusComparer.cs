using System;
using System.Collections.Generic;

public class ActionStatusComparer : global::System.Collections.Generic.IComparer<global::ActionStatus>
{
	int global::System.Collections.Generic.IComparer<global::ActionStatus>.Compare(global::ActionStatus x, global::ActionStatus y)
	{
		if (x.actionData.SortWeight < y.actionData.SortWeight)
		{
			return 1;
		}
		if (x.actionData.SortWeight > y.actionData.SortWeight)
		{
			return -1;
		}
		return string.Compare(x.actionData.Name, y.actionData.Name, global::System.StringComparison.OrdinalIgnoreCase);
	}
}
