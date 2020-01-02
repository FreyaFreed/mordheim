using System;
using System.Collections.Generic;

public class FlyingTextIdComparer : global::System.Collections.Generic.IEqualityComparer<global::FlyingTextId>
{
	public bool Equals(global::FlyingTextId x, global::FlyingTextId y)
	{
		return x == y;
	}

	public int GetHashCode(global::FlyingTextId obj)
	{
		return (int)obj;
	}

	public static readonly global::FlyingTextIdComparer Instance = new global::FlyingTextIdComparer();
}
