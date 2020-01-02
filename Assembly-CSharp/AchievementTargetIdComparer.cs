using System;
using System.Collections.Generic;

public class AchievementTargetIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AchievementTargetId>
{
	public bool Equals(global::AchievementTargetId x, global::AchievementTargetId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AchievementTargetId obj)
	{
		return (int)obj;
	}

	public static readonly global::AchievementTargetIdComparer Instance = new global::AchievementTargetIdComparer();
}
