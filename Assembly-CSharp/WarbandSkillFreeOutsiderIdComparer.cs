using System;
using System.Collections.Generic;

public class WarbandSkillFreeOutsiderIdComparer : global::System.Collections.Generic.IEqualityComparer<global::WarbandSkillFreeOutsiderId>
{
	public bool Equals(global::WarbandSkillFreeOutsiderId x, global::WarbandSkillFreeOutsiderId y)
	{
		return x == y;
	}

	public int GetHashCode(global::WarbandSkillFreeOutsiderId obj)
	{
		return (int)obj;
	}

	public static readonly global::WarbandSkillFreeOutsiderIdComparer Instance = new global::WarbandSkillFreeOutsiderIdComparer();
}
