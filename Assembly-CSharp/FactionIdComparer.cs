using System;
using System.Collections.Generic;

public class FactionIdComparer : global::System.Collections.Generic.IEqualityComparer<global::FactionId>
{
	public bool Equals(global::FactionId x, global::FactionId y)
	{
		return x == y;
	}

	public int GetHashCode(global::FactionId obj)
	{
		return (int)obj;
	}

	public static readonly global::FactionIdComparer Instance = new global::FactionIdComparer();
}
