using System;
using System.Collections.Generic;

public class SceneLoadingIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SceneLoadingId>
{
	public bool Equals(global::SceneLoadingId x, global::SceneLoadingId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SceneLoadingId obj)
	{
		return (int)obj;
	}

	public static readonly global::SceneLoadingIdComparer Instance = new global::SceneLoadingIdComparer();
}
