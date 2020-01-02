using System;
using System.Collections.Generic;

public class MissionMapGameplayIdComparer : global::System.Collections.Generic.IEqualityComparer<global::MissionMapGameplayId>
{
	public bool Equals(global::MissionMapGameplayId x, global::MissionMapGameplayId y)
	{
		return x == y;
	}

	public int GetHashCode(global::MissionMapGameplayId obj)
	{
		return (int)obj;
	}

	public static readonly global::MissionMapGameplayIdComparer Instance = new global::MissionMapGameplayIdComparer();
}
