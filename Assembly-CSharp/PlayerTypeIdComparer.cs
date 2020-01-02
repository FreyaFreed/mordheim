using System;
using System.Collections.Generic;

public class PlayerTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::PlayerTypeId>
{
	public bool Equals(global::PlayerTypeId x, global::PlayerTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::PlayerTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::PlayerTypeIdComparer Instance = new global::PlayerTypeIdComparer();
}
