using System;
using System.Collections.Generic;

public class AttackResultIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AttackResultId>
{
	public bool Equals(global::AttackResultId x, global::AttackResultId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AttackResultId obj)
	{
		return (int)obj;
	}

	public static readonly global::AttackResultIdComparer Instance = new global::AttackResultIdComparer();
}
