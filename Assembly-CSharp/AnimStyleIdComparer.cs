using System;
using System.Collections.Generic;

public class AnimStyleIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AnimStyleId>
{
	public bool Equals(global::AnimStyleId x, global::AnimStyleId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AnimStyleId obj)
	{
		return (int)obj;
	}

	public static readonly global::AnimStyleIdComparer Instance = new global::AnimStyleIdComparer();
}
