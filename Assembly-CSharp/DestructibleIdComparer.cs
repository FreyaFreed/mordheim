using System;
using System.Collections.Generic;

public class DestructibleIdComparer : global::System.Collections.Generic.IEqualityComparer<global::DestructibleId>
{
	public bool Equals(global::DestructibleId x, global::DestructibleId y)
	{
		return x == y;
	}

	public int GetHashCode(global::DestructibleId obj)
	{
		return (int)obj;
	}

	public static readonly global::DestructibleIdComparer Instance = new global::DestructibleIdComparer();
}
