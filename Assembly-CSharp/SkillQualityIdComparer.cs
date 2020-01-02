using System;
using System.Collections.Generic;

public class SkillQualityIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SkillQualityId>
{
	public bool Equals(global::SkillQualityId x, global::SkillQualityId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SkillQualityId obj)
	{
		return (int)obj;
	}

	public static readonly global::SkillQualityIdComparer Instance = new global::SkillQualityIdComparer();
}
