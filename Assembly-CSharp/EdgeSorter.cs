using System;
using System.Collections.Generic;

public class EdgeSorter : global::System.Collections.Generic.IComparer<global::MapEdge>
{
	int global::System.Collections.Generic.IComparer<global::MapEdge>.Compare(global::MapEdge x, global::MapEdge y)
	{
		if (x.idx < y.idx)
		{
			return 1;
		}
		if (x.idx > y.idx)
		{
			return -1;
		}
		return 0;
	}
}
