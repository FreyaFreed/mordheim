using System;
using System.Collections.Generic;

public class SkillLineIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SkillLineId>
{
	public bool Equals(global::SkillLineId x, global::SkillLineId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SkillLineId obj)
	{
		return (int)obj;
	}

	public static readonly global::SkillLineIdComparer Instance = new global::SkillLineIdComparer();
}
