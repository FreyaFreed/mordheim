using System;
using UnityEngine;

namespace mset
{
	public class RGB
	{
		public static void toRGBM(ref global::UnityEngine.Color32 rgbm, global::UnityEngine.Color color, bool useGamma)
		{
			if (useGamma)
			{
				color.r = global::UnityEngine.Mathf.Pow(color.r, global::mset.Gamma.toSRGB);
				color.g = global::UnityEngine.Mathf.Pow(color.g, global::mset.Gamma.toSRGB);
				color.b = global::UnityEngine.Mathf.Pow(color.b, global::mset.Gamma.toSRGB);
			}
			color *= 0.166666672f;
			float num = global::UnityEngine.Mathf.Max(global::UnityEngine.Mathf.Max(color.r, color.g), color.b);
			num = global::UnityEngine.Mathf.Clamp01(num);
			num = global::UnityEngine.Mathf.Ceil(num * 255f) / 255f;
			if (num > 0f)
			{
				float num2 = 1f / num;
				color.r = global::UnityEngine.Mathf.Clamp01(color.r * num2);
				color.g = global::UnityEngine.Mathf.Clamp01(color.g * num2);
				color.b = global::UnityEngine.Mathf.Clamp01(color.b * num2);
				rgbm.r = (byte)(color.r * 255f);
				rgbm.g = (byte)(color.g * 255f);
				rgbm.b = (byte)(color.b * 255f);
				rgbm.a = (byte)(num * 255f);
			}
			else
			{
				rgbm.r = (rgbm.g = (rgbm.b = (rgbm.a = 0)));
			}
		}

		public static void toRGBM(ref global::UnityEngine.Color rgbm, global::UnityEngine.Color color, bool useGamma)
		{
			if (useGamma)
			{
				color.r = global::UnityEngine.Mathf.Pow(color.r, global::mset.Gamma.toSRGB);
				color.g = global::UnityEngine.Mathf.Pow(color.g, global::mset.Gamma.toSRGB);
				color.b = global::UnityEngine.Mathf.Pow(color.b, global::mset.Gamma.toSRGB);
			}
			color *= 0.166666672f;
			float num = global::UnityEngine.Mathf.Max(global::UnityEngine.Mathf.Max(color.r, color.g), color.b);
			num = global::UnityEngine.Mathf.Clamp01(num);
			num = global::UnityEngine.Mathf.Ceil(num * 255f) / 255f;
			if (num > 0f)
			{
				float num2 = 1f / num;
				rgbm.r = global::UnityEngine.Mathf.Clamp01(color.r * num2);
				rgbm.g = global::UnityEngine.Mathf.Clamp01(color.g * num2);
				rgbm.b = global::UnityEngine.Mathf.Clamp01(color.b * num2);
				rgbm.a = num;
			}
			else
			{
				rgbm.r = (rgbm.g = (rgbm.b = (rgbm.a = 0f)));
			}
		}

		public static void fromRGBM(ref global::UnityEngine.Color color, global::UnityEngine.Color32 rgbm, bool useGamma)
		{
			float num = 0.003921569f;
			float b = (float)rgbm.a * num;
			color.r = (float)rgbm.r * num;
			color.g = (float)rgbm.g * num;
			color.b = (float)rgbm.b * num;
			color *= b;
			color *= 6f;
			if (useGamma)
			{
				color.r = global::UnityEngine.Mathf.Pow(color.r, global::mset.Gamma.toLinear);
				color.g = global::UnityEngine.Mathf.Pow(color.g, global::mset.Gamma.toLinear);
				color.b = global::UnityEngine.Mathf.Pow(color.b, global::mset.Gamma.toLinear);
			}
			color.a = 1f;
		}

		public static void fromRGBM(ref global::UnityEngine.Color color, global::UnityEngine.Color rgbm, bool useGamma)
		{
			float a = rgbm.a;
			color = rgbm;
			color *= a;
			color *= 6f;
			if (useGamma)
			{
				color.r = global::UnityEngine.Mathf.Pow(color.r, global::mset.Gamma.toLinear);
				color.g = global::UnityEngine.Mathf.Pow(color.g, global::mset.Gamma.toLinear);
				color.b = global::UnityEngine.Mathf.Pow(color.b, global::mset.Gamma.toLinear);
			}
			color.a = 1f;
		}

		public static void fromXYZ(ref global::UnityEngine.Color rgb, global::UnityEngine.Color xyz)
		{
			rgb.r = 3.2404542f * xyz.r - 1.53713846f * xyz.g - 0.4985314f * xyz.b;
			rgb.g = -0.969266f * xyz.r + 1.87601078f * xyz.g + 0.041556f * xyz.b;
			rgb.b = 0.0556434f * xyz.r - 0.2040259f * xyz.g + 1.05722523f * xyz.b;
		}

		public static void toXYZ(ref global::UnityEngine.Color xyz, global::UnityEngine.Color rgb)
		{
			xyz.r = 0.4124564f * rgb.r + 0.3575761f * rgb.g + 0.1804375f * rgb.b;
			xyz.g = 0.2126729f * rgb.r + 0.7151522f * rgb.g + 0.072175f * rgb.b;
			xyz.b = 0.0193339f * rgb.r + 0.119192f * rgb.g + 0.9503041f * rgb.b;
		}

		public static void toRGBE(ref global::UnityEngine.Color32 rgbe, global::UnityEngine.Color color)
		{
			float num = global::UnityEngine.Mathf.Max(global::UnityEngine.Mathf.Max(color.r, color.g), color.b);
			int num2 = global::UnityEngine.Mathf.CeilToInt(global::UnityEngine.Mathf.Log(num, 2f));
			num2 = global::UnityEngine.Mathf.Clamp(num2, -128, 127);
			num = global::UnityEngine.Mathf.Pow(2f, (float)num2);
			float num3 = 255f / num;
			rgbe.r = (byte)global::UnityEngine.Mathf.Clamp(color.r * num3, 0f, 255f);
			rgbe.g = (byte)global::UnityEngine.Mathf.Clamp(color.g * num3, 0f, 255f);
			rgbe.b = (byte)global::UnityEngine.Mathf.Clamp(color.b * num3, 0f, 255f);
			rgbe.a = (byte)(num2 + 128);
		}
	}
}
