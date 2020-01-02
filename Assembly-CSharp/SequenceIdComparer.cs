using System;
using System.Collections.Generic;

public class SequenceIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SequenceId>
{
	public bool Equals(global::SequenceId x, global::SequenceId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SequenceId obj)
	{
		return (int)obj;
	}

	public static readonly global::SequenceIdComparer Instance = new global::SequenceIdComparer();
}
