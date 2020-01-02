using System;
using System.Collections.Generic;

public class AiFilterResultIdComparer : global::System.Collections.Generic.IEqualityComparer<global::AiFilterResultId>
{
	public bool Equals(global::AiFilterResultId x, global::AiFilterResultId y)
	{
		return x == y;
	}

	public int GetHashCode(global::AiFilterResultId obj)
	{
		return (int)obj;
	}

	public static readonly global::AiFilterResultIdComparer Instance = new global::AiFilterResultIdComparer();
}
