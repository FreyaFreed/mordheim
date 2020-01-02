using System;
using System.Collections.Generic;

public class SkinColorIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SkinColorId>
{
	public bool Equals(global::SkinColorId x, global::SkinColorId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SkinColorId obj)
	{
		return (int)obj;
	}

	public static readonly global::SkinColorIdComparer Instance = new global::SkinColorIdComparer();
}
