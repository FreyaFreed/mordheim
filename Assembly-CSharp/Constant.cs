using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class Constant
{
	static Constant()
	{
		for (int i = 0; i < global::Constant.intConstants.Length; i++)
		{
			global::Constant.intConstants[i] = i.ToString();
			global::Constant.negIntConstants[i] = (-i).ToString();
		}
		for (int j = 0; j <= 100; j++)
		{
			global::Constant.percConstants[j] = j + "%";
			global::Constant.negPercConstants[j] = -j + "%";
		}
	}

	public static void Init()
	{
		global::System.Collections.Generic.List<global::ConstantData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ConstantData>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			global::Constant.constants[(int)list[i].Id] = list[i].Value;
		}
	}

	public static int GetInt(global::ConstantId constant)
	{
		int num;
		if (!global::Constant.cachedInt.TryGetValue((int)constant, out num))
		{
			num = int.Parse(global::Constant.constants[(int)constant]);
			global::Constant.cachedInt[(int)constant] = num;
		}
		return num;
	}

	public static float GetFloat(global::ConstantId constant)
	{
		float num;
		if (!global::Constant.cachedFloat.TryGetValue((int)constant, out num))
		{
			num = float.Parse(global::Constant.constants[(int)constant], global::System.Globalization.NumberFormatInfo.InvariantInfo);
			global::Constant.cachedFloat[(int)constant] = num;
		}
		return num;
	}

	public static float GetFloatSqr(global::ConstantId constant)
	{
		float @float = global::Constant.GetFloat(constant);
		return @float * @float;
	}

	public static string GetString(global::ConstantId constant)
	{
		return global::Constant.constants[(int)constant];
	}

	public static global::UnityEngine.Color GetColor(global::ConstantId constant)
	{
		global::UnityEngine.Color color;
		if (!global::Constant.cachedColor.TryGetValue((int)constant, out color))
		{
			string[] array = global::Constant.constants[(int)constant].Split(new char[]
			{
				','
			});
			color = new global::UnityEngine.Color((float)global::System.Convert.ToInt32(array[0]) / 255f, (float)global::System.Convert.ToInt32(array[1]) / 255f, (float)global::System.Convert.ToInt32(array[2]) / 255f, (array.Length <= 3) ? 1f : ((float)global::System.Convert.ToInt32(array[3]) / 255f));
			global::Constant.cachedColor[(int)constant] = color;
		}
		return color;
	}

	public static string ToString(int value)
	{
		int num = global::System.Math.Abs(value);
		if (num < 0 || num >= 2000)
		{
			return value.ToString();
		}
		if (value >= 0)
		{
			return global::Constant.intConstants[value];
		}
		return global::Constant.negIntConstants[num];
	}

	public static string ToPercString(int value)
	{
		if (value < -100 || value > 100)
		{
			return global::PandoraUtils.StringBuilder.Append(value.ToConstantString()).Append('%').ToString();
		}
		if (value >= 0)
		{
			return global::Constant.percConstants[value];
		}
		return global::Constant.negPercConstants[-value];
	}

	public static string ToConstantString(this int value)
	{
		return global::Constant.ToString(value);
	}

	public static string ToConstantPercString(this int value)
	{
		return global::Constant.ToPercString(value);
	}

	private static readonly global::System.Collections.Generic.Dictionary<int, float> cachedFloat = new global::System.Collections.Generic.Dictionary<int, float>();

	private static readonly global::System.Collections.Generic.Dictionary<int, int> cachedInt = new global::System.Collections.Generic.Dictionary<int, int>();

	private static readonly global::System.Collections.Generic.Dictionary<int, global::UnityEngine.Color> cachedColor = new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.Color>();

	private static readonly string[] constants = new string[159];

	private static readonly string[] intConstants = new string[2000];

	private static readonly string[] negIntConstants = new string[2000];

	private static readonly string[] negPercConstants = new string[101];

	private static readonly string[] percConstants = new string[101];
}
