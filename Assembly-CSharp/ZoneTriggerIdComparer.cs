using System;
using System.Collections.Generic;

public class ZoneTriggerIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ZoneTriggerId>
{
	public bool Equals(global::ZoneTriggerId x, global::ZoneTriggerId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ZoneTriggerId obj)
	{
		return (int)obj;
	}

	public static readonly global::ZoneTriggerIdComparer Instance = new global::ZoneTriggerIdComparer();
}
