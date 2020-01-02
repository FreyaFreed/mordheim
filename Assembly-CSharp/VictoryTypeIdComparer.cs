using System;
using System.Collections.Generic;

public class VictoryTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::VictoryTypeId>
{
	public bool Equals(global::VictoryTypeId x, global::VictoryTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::VictoryTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::VictoryTypeIdComparer Instance = new global::VictoryTypeIdComparer();
}
