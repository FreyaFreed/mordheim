using System;
using System.Collections.Generic;

public class AiFilterCheckIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AiFilterCheckId>
{
	public bool Equals(global::AiFilterCheckId x, global::AiFilterCheckId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AiFilterCheckId obj)
	{
		return (int)obj;
	}

	public static readonly global::AiFilterCheckIdComparer Instance = new global::AiFilterCheckIdComparer();
}
