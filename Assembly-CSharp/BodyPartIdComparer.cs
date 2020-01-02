using System;
using System.Collections.Generic;

public class BodyPartIdComparer : global::System.Collections.Generic.IEqualityComparer<global::BodyPartId>
{
	public bool Equals(global::BodyPartId x, global::BodyPartId y)
	{
		return x == y;
	}

	public int GetHashCode(global::BodyPartId obj)
	{
		return (int)obj;
	}

	public static readonly global::BodyPartIdComparer Instance = new global::BodyPartIdComparer();
}
