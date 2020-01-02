using System;
using System.Collections.Generic;

public class SkillTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SkillTypeId>
{
	public bool Equals(global::SkillTypeId x, global::SkillTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SkillTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::SkillTypeIdComparer Instance = new global::SkillTypeIdComparer();
}
