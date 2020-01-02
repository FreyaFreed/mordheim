using System;
using System.Collections.Generic;

public class MutationGroupIdComparer : global::System.Collections.Generic.IEqualityComparer<global::MutationGroupId>
{
	public bool Equals(global::MutationGroupId x, global::MutationGroupId y)
	{
		return x == y;
	}

	public int GetHashCode(global::MutationGroupId obj)
	{
		return (int)obj;
	}

	public static readonly global::MutationGroupIdComparer Instance = new global::MutationGroupIdComparer();
}
