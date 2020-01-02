using System;
using System.Collections.Generic;

public class WarbandSkillTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WarbandSkillTypeId>
{
	public bool Equals(global::WarbandSkillTypeId x, global::WarbandSkillTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WarbandSkillTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::WarbandSkillTypeIdComparer Instance = new global::WarbandSkillTypeIdComparer();
}
