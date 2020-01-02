using System;
using System.Collections.Generic;

public class SceneLoadingTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SceneLoadingTypeId>
{
	public bool Equals(global::SceneLoadingTypeId x, global::SceneLoadingTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SceneLoadingTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::SceneLoadingTypeIdComparer Instance = new global::SceneLoadingTypeIdComparer();
}
