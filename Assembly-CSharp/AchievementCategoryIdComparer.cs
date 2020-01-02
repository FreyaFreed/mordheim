using System;
using System.Collections.Generic;

public class AchievementCategoryIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AchievementCategoryId>
{
	public bool Equals(global::AchievementCategoryId x, global::AchievementCategoryId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AchievementCategoryId obj)
	{
		return (int)obj;
	}

	public static readonly global::AchievementCategoryIdComparer Instance = new global::AchievementCategoryIdComparer();
}
