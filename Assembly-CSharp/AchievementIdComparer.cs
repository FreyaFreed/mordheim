using System;
using System.Collections.Generic;

public class AchievementIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AchievementId>
{
	public bool Equals(global::AchievementId x, global::AchievementId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AchievementId obj)
	{
		return (int)obj;
	}

	public static readonly global::AchievementIdComparer Instance = new global::AchievementIdComparer();
}
