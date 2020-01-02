using System;
using System.Collections.Generic;

public class AiUnitIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AiUnitId>
{
	public bool Equals(global::AiUnitId x, global::AiUnitId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AiUnitId obj)
	{
		return (int)obj;
	}

	public static readonly global::AiUnitIdComparer Instance = new global::AiUnitIdComparer();
}
