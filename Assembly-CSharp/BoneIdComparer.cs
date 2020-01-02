using System;
using System.Collections.Generic;

public class BoneIdComparer : global::System.Collections.Generic.IEqualityComparer<global::BoneId>
{
	public bool Equals(global::BoneId x, global::BoneId y)
	{
		return x == y;
	}

	public int GetHashCode(global::BoneId obj)
	{
		return (int)obj;
	}

	public static readonly global::BoneIdComparer Instance = new global::BoneIdComparer();
}
