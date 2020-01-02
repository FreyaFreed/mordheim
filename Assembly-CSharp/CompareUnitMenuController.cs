using System;
using System.Collections.Generic;

public class CompareUnitMenuController : global::System.Collections.Generic.IComparer<global::UnitMenuController>
{
	int global::System.Collections.Generic.IComparer<global::UnitMenuController>.Compare(global::UnitMenuController x, global::UnitMenuController y)
	{
		return global::WarbandMenuController.Compare(x, y);
	}
}
