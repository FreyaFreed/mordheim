using System;
using System.Collections.Generic;

public class MovementIdComparer : global::System.Collections.Generic.IEqualityComparer<global::MovementId>
{
	public bool Equals(global::MovementId x, global::MovementId y)
	{
		return x == y;
	}

	public int GetHashCode(global::MovementId obj)
	{
		return (int)obj;
	}

	public static readonly global::MovementIdComparer Instance = new global::MovementIdComparer();
}
