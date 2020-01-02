using System;
using System.Collections.Generic;

public class CompareUnitMenuNode : global::System.Collections.Generic.IComparer<global::MenuNode>
{
	int global::System.Collections.Generic.IComparer<global::MenuNode>.Compare(global::MenuNode x, global::MenuNode y)
	{
		return x.name.CompareTo(y.name);
	}
}
