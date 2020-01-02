using System;
using System.Collections.Generic;

public class NoticesComparer : global::System.Collections.Generic.IEqualityComparer<global::Notices>
{
	public bool Equals(global::Notices x, global::Notices y)
	{
		return x == y;
	}

	public int GetHashCode(global::Notices obj)
	{
		return (int)obj;
	}

	public static readonly global::NoticesComparer Instance = new global::NoticesComparer();
}
