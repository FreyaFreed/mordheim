using System;
using System.Collections.Generic;

public class ProjectileIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ProjectileId>
{
	public bool Equals(global::ProjectileId x, global::ProjectileId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ProjectileId obj)
	{
		return (int)obj;
	}

	public static readonly global::ProjectileIdComparer Instance = new global::ProjectileIdComparer();
}
