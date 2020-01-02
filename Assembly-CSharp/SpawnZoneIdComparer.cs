using System;
using System.Collections.Generic;

public class SpawnZoneIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SpawnZoneId>
{
	public bool Equals(global::SpawnZoneId x, global::SpawnZoneId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SpawnZoneId obj)
	{
		return (int)obj;
	}

	public static readonly global::SpawnZoneIdComparer Instance = new global::SpawnZoneIdComparer();
}
