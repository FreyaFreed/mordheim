using System;
using System.Globalization;
using System.Text;

public class FNV1a
{
	public static uint ComputeHash(global::System.Text.StringBuilder strb)
	{
		uint num = 2166136261U;
		for (int i = 0; i < strb.Length; i++)
		{
			num ^= (uint)global::System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToLower(strb[i]);
			num *= 16777619U;
		}
		return num;
	}

	public static uint ComputeHash(string str)
	{
		uint num = 2166136261U;
		for (int i = 0; i < str.Length; i++)
		{
			num ^= (uint)global::System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToLower(str[i]);
			num *= 16777619U;
		}
		return num;
	}

	private const uint FnvBasis = 2166136261U;

	private const uint FnvPrime = 16777619U;
}
