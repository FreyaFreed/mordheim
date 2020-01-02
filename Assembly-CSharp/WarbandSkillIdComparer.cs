using System;
using System.Collections.Generic;

public class WarbandSkillIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WarbandSkillId>
{
	public bool Equals(global::WarbandSkillId x, global::WarbandSkillId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WarbandSkillId obj)
	{
		return (int)obj;
	}

	public static readonly global::WarbandSkillIdComparer Instance = new global::WarbandSkillIdComparer();
}
