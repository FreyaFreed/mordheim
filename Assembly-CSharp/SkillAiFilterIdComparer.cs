using System;
using System.Collections.Generic;

public class SkillAiFilterIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SkillAiFilterId>
{
	public bool Equals(global::SkillAiFilterId x, global::SkillAiFilterId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SkillAiFilterId obj)
	{
		return (int)obj;
	}

	public static readonly global::SkillAiFilterIdComparer Instance = new global::SkillAiFilterIdComparer();
}
