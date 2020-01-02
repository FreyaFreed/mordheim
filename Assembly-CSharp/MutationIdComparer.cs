using System;
using System.Collections.Generic;

public class MutationIdComparer : global::System.Collections.Generic.IEqualityComparer<global::MutationId>
{
	public bool Equals(global::MutationId x, global::MutationId y)
	{
		return x == y;
	}

	public int GetHashCode(global::MutationId obj)
	{
		return (int)obj;
	}

	public static readonly global::MutationIdComparer Instance = new global::MutationIdComparer();
}
