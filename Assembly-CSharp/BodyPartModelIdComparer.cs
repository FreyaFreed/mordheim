using System;
using System.Collections.Generic;

public class BodyPartModelIdComparer : global::System.Collections.Generic.IEqualityComparer<global::BodyPartModelId>
{
	public bool Equals(global::BodyPartModelId x, global::BodyPartModelId y)
	{
		return x == y;
	}

	public int GetHashCode(global::BodyPartModelId obj)
	{
		return (int)obj;
	}

	public static readonly global::BodyPartModelIdComparer Instance = new global::BodyPartModelIdComparer();
}
