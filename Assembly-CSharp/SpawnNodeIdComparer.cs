using System;
using System.Collections.Generic;

public class SpawnNodeIdComparer : global::System.Collections.Generic.IEqualityComparer<global::SpawnNodeId>
{
	public bool Equals(global::SpawnNodeId x, global::SpawnNodeId y)
	{
		return x == y;
	}

	public int GetHashCode(global::SpawnNodeId obj)
	{
		return (int)obj;
	}

	public static readonly global::SpawnNodeIdComparer Instance = new global::SpawnNodeIdComparer();
}
