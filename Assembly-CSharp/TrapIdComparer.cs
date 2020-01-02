using System;
using System.Collections.Generic;

public class TrapIdComparer : global::System.Collections.Generic.IEqualityComparer<global::TrapId>
{
	public bool Equals(global::TrapId x, global::TrapId y)
	{
		return x == y;
	}

	public int GetHashCode(global::TrapId obj)
	{
		return (int)obj;
	}

	public static readonly global::TrapIdComparer Instance = new global::TrapIdComparer();
}
