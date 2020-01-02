using System;
using System.Collections.Generic;

public class WarbandContactIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WarbandContactId>
{
	public bool Equals(global::WarbandContactId x, global::WarbandContactId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WarbandContactId obj)
	{
		return (int)obj;
	}

	public static readonly global::WarbandContactIdComparer Instance = new global::WarbandContactIdComparer();
}
