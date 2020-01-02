using System;
using System.Collections.Generic;

public class AssetBundleIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AssetBundleId>
{
	public bool Equals(global::AssetBundleId x, global::AssetBundleId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AssetBundleId obj)
	{
		return (int)obj;
	}

	public static readonly global::AssetBundleIdComparer Instance = new global::AssetBundleIdComparer();
}
