using System;
using System.Collections.Generic;
using UnityEngine;

public class TransformComparer : global::System.Collections.Generic.IComparer<global::UnityEngine.Transform>
{
	int global::System.Collections.Generic.IComparer<global::UnityEngine.Transform>.Compare(global::UnityEngine.Transform x, global::UnityEngine.Transform y)
	{
		return string.Compare(x.name, y.name, global::System.StringComparison.OrdinalIgnoreCase);
	}

	public static global::TransformComparer Default = new global::TransformComparer();
}
