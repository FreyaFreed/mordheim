using System;
using System.Collections.Generic;

public class ColorPresetIdComparer : global::System.Collections.Generic.IEqualityComparer<global::ColorPresetId>
{
	public bool Equals(global::ColorPresetId x, global::ColorPresetId y)
	{
		return x == y;
	}

	public int GetHashCode(global::ColorPresetId obj)
	{
		return (int)obj;
	}

	public static readonly global::ColorPresetIdComparer Instance = new global::ColorPresetIdComparer();
}
