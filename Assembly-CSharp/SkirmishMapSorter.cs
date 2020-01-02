using System;
using System.Collections.Generic;

public class SkirmishMapSorter : global::System.Collections.Generic.IComparer<global::SkirmishMap>
{
	int global::System.Collections.Generic.IComparer<global::SkirmishMap>.Compare(global::SkirmishMap x, global::SkirmishMap y)
	{
		if (x.mapData.Idx > y.mapData.Idx)
		{
			return 1;
		}
		if (x.mapData.Idx < y.mapData.Idx)
		{
			return -1;
		}
		return 0;
	}
}
