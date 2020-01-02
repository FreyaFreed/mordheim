using System;
using System.Collections.Generic;

public class AiProfileIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AiProfileId>
{
	public bool Equals(global::AiProfileId x, global::AiProfileId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AiProfileId obj)
	{
		return (int)obj;
	}

	public static readonly global::AiProfileIdComparer Instance = new global::AiProfileIdComparer();
}
