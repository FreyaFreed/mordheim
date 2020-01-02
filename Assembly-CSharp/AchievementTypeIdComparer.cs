using System;
using System.Collections.Generic;

public class AchievementTypeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AchievementTypeId>
{
	public bool Equals(global::AchievementTypeId x, global::AchievementTypeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AchievementTypeId obj)
	{
		return (int)obj;
	}

	public static readonly global::AchievementTypeIdComparer Instance = new global::AchievementTypeIdComparer();
}
