using System;
using System.Collections.Generic;

public class SkillIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SkillId>
{
	public bool Equals(global::SkillId x, global::SkillId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SkillId obj)
	{
		return (int)obj;
	}

	public static readonly global::SkillIdComparer Instance = new global::SkillIdComparer();
}
