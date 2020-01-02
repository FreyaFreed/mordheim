using System;
using System.Collections.Generic;

public class BodyPartMaterialIdComparer : global::System.Collections.Generic.IEqualityComparer<global::BodyPartMaterialId>
{
	public bool Equals(global::BodyPartMaterialId x, global::BodyPartMaterialId y)
	{
		return x == y;
	}

	public int GetHashCode(global::BodyPartMaterialId obj)
	{
		return (int)obj;
	}

	public static readonly global::BodyPartMaterialIdComparer Instance = new global::BodyPartMaterialIdComparer();
}
