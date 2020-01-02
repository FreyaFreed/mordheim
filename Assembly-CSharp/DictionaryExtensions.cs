using System;
using System.Collections.Generic;

public static class DictionaryExtensions
{
	public static global::System.Collections.Generic.List<TListValue> GetOrNull<TKey, TListValue>(this global::System.Collections.Generic.Dictionary<TKey, global::System.Collections.Generic.List<TListValue>> dict, TKey key)
	{
		global::System.Collections.Generic.List<TListValue> result;
		if (!dict.TryGetValue(key, out result))
		{
			result = new global::System.Collections.Generic.List<TListValue>(0);
		}
		return result;
	}

	public static global::System.Collections.Generic.List<global::AttributeMod> GetOrNull(this global::System.Collections.Generic.Dictionary<global::AttributeId, global::System.Collections.Generic.List<global::AttributeMod>> dict, global::AttributeId key)
	{
		global::System.Collections.Generic.List<global::AttributeMod> result;
		if (!dict.TryGetValue(key, out result))
		{
			result = global::DictionaryExtensions.tempEmptyList;
		}
		return result;
	}

	private static global::System.Collections.Generic.List<global::AttributeMod> tempEmptyList = new global::System.Collections.Generic.List<global::AttributeMod>(0);
}
