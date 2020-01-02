using System;
using System.Collections.Generic;

public class AssetExtensionIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AssetExtensionId>
{
	public bool Equals(global::AssetExtensionId x, global::AssetExtensionId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AssetExtensionId obj)
	{
		return (int)obj;
	}

	public static readonly global::AssetExtensionIdComparer Instance = new global::AssetExtensionIdComparer();
}
