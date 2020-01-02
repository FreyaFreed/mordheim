using System;
using UnityEngine;

namespace mset
{
	public class Util
	{
		public static void cubeLookup(ref float s, ref float t, ref ulong face, global::UnityEngine.Vector3 dir)
		{
			float num = global::UnityEngine.Mathf.Abs(dir.x);
			float num2 = global::UnityEngine.Mathf.Abs(dir.y);
			float num3 = global::UnityEngine.Mathf.Abs(dir.z);
			if (num >= num2 && num >= num3)
			{
				if (dir.x >= 0f)
				{
					face = 0UL;
				}
				else
				{
					face = 1UL;
				}
			}
			else if (num2 >= num && num2 >= num3)
			{
				if (dir.y >= 0f)
				{
					face = 2UL;
				}
				else
				{
					face = 3UL;
				}
			}
			else if (dir.z >= 0f)
			{
				face = 4UL;
			}
			else
			{
				face = 5UL;
			}
			ulong num4 = face;
			if (num4 >= (ulong)0 && num4 <= (ulong)5)
			{
				switch ((int)num4)
				{
				case 0:
					s = 0.5f * (-dir.z / num + 1f);
					t = 0.5f * (-dir.y / num + 1f);
					break;
				case 1:
					s = 0.5f * (dir.z / num + 1f);
					t = 0.5f * (-dir.y / num + 1f);
					break;
				case 2:
					s = 0.5f * (dir.x / num2 + 1f);
					t = 0.5f * (dir.z / num2 + 1f);
					break;
				case 3:
					s = 0.5f * (dir.x / num2 + 1f);
					t = 0.5f * (-dir.z / num2 + 1f);
					break;
				case 4:
					s = 0.5f * (dir.x / num3 + 1f);
					t = 0.5f * (-dir.y / num3 + 1f);
					break;
				case 5:
					s = 0.5f * (-dir.x / num3 + 1f);
					t = 0.5f * (-dir.y / num3 + 1f);
					break;
				}
			}
		}

		public static void invCubeLookup(ref global::UnityEngine.Vector3 dst, ref float weight, ulong face, ulong col, ulong row, ulong faceSize)
		{
			float num = 2f / faceSize;
			float num2 = (col + 0.5f) * num - 1f;
			float num3 = (row + 0.5f) * num - 1f;
			if (face >= (ulong)0 && face <= (ulong)5)
			{
				switch ((int)face)
				{
				case 0:
					dst[0] = 1f;
					dst[1] = -num3;
					dst[2] = -num2;
					break;
				case 1:
					dst[0] = -1f;
					dst[1] = -num3;
					dst[2] = num2;
					break;
				case 2:
					dst[0] = num2;
					dst[1] = 1f;
					dst[2] = num3;
					break;
				case 3:
					dst[0] = num2;
					dst[1] = -1f;
					dst[2] = -num3;
					break;
				case 4:
					dst[0] = num2;
					dst[1] = -num3;
					dst[2] = 1f;
					break;
				case 5:
					dst[0] = -num2;
					dst[1] = -num3;
					dst[2] = -1f;
					break;
				}
			}
			float magnitude = dst.magnitude;
			weight = 4f / (magnitude * magnitude * magnitude);
			dst /= magnitude;
		}

		public static void invLatLongLookup(ref global::UnityEngine.Vector3 dst, ref float cosPhi, ulong col, ulong row, ulong width, ulong height)
		{
			float num = 0.5f;
			float num2 = (col + num) / width;
			float num3 = (row + num) / height;
			float f = -6.28318548f * num2 - 1.57079637f;
			float f2 = 1.57079637f * (2f * num3 - 1f);
			cosPhi = global::UnityEngine.Mathf.Cos(f2);
			dst.x = global::UnityEngine.Mathf.Cos(f) * cosPhi;
			dst.y = global::UnityEngine.Mathf.Sin(f2);
			dst.z = global::UnityEngine.Mathf.Sin(f) * cosPhi;
		}

		public static void cubeToLatLongLookup(ref float pano_u, ref float pano_v, ulong face, ulong col, ulong row, ulong faceSize)
		{
			global::UnityEngine.Vector3 vector = default(global::UnityEngine.Vector3);
			float num = -1f;
			global::mset.Util.invCubeLookup(ref vector, ref num, face, col, row, faceSize);
			pano_v = global::UnityEngine.Mathf.Asin(vector.y) / 3.14159274f + 0.5f;
			pano_u = 0.5f * global::UnityEngine.Mathf.Atan2(-vector.x, -vector.z) / 3.14159274f;
			pano_u = global::UnityEngine.Mathf.Repeat(pano_u, 1f);
		}

		public static void latLongToCubeLookup(ref float cube_u, ref float cube_v, ref ulong face, ulong col, ulong row, ulong width, ulong height)
		{
			global::UnityEngine.Vector3 dir = default(global::UnityEngine.Vector3);
			float num = -1f;
			global::mset.Util.invLatLongLookup(ref dir, ref num, col, row, width, height);
			global::mset.Util.cubeLookup(ref cube_u, ref cube_v, ref face, dir);
		}

		public static void rotationToInvLatLong(out float u, out float v, global::UnityEngine.Quaternion rot)
		{
			u = rot.eulerAngles.y;
			v = rot.eulerAngles.x;
			u = global::UnityEngine.Mathf.Repeat(u, 360f) / 360f;
			v = 1f - global::UnityEngine.Mathf.Repeat(v + 90f, 360f) / 180f;
		}

		public static void dirToLatLong(out float u, out float v, global::UnityEngine.Vector3 dir)
		{
			dir = dir.normalized;
			u = 0.5f * global::UnityEngine.Mathf.Atan2(-dir.x, -dir.z) / 3.14159274f;
			u = global::UnityEngine.Mathf.Repeat(u, 1f);
			v = global::UnityEngine.Mathf.Asin(dir.y) / 3.14159274f + 0.5f;
			v = 1f - global::UnityEngine.Mathf.Repeat(v, 1f);
		}

		public static void applyGamma(ref global::UnityEngine.Color c, float gamma)
		{
			c.r = global::UnityEngine.Mathf.Pow(c.r, gamma);
			c.g = global::UnityEngine.Mathf.Pow(c.g, gamma);
			c.b = global::UnityEngine.Mathf.Pow(c.b, gamma);
		}

		public static void applyGamma(ref global::UnityEngine.Color[] c, float gamma)
		{
			for (int i = 0; i < c.Length; i++)
			{
				c[i].r = global::UnityEngine.Mathf.Pow(c[i].r, gamma);
				c[i].g = global::UnityEngine.Mathf.Pow(c[i].g, gamma);
				c[i].b = global::UnityEngine.Mathf.Pow(c[i].b, gamma);
			}
		}

		public static void applyGamma(ref global::UnityEngine.Color[] dst, global::UnityEngine.Color[] src, float gamma)
		{
			for (int i = 0; i < src.Length; i++)
			{
				dst[i].r = global::UnityEngine.Mathf.Pow(src[i].r, gamma);
				dst[i].g = global::UnityEngine.Mathf.Pow(src[i].g, gamma);
				dst[i].b = global::UnityEngine.Mathf.Pow(src[i].b, gamma);
				dst[i].a = src[i].a;
			}
		}

		public static void applyGamma(ref global::UnityEngine.Color[] dst, int dst_offset, global::UnityEngine.Color[] src, int src_offset, int count, float gamma)
		{
			int num = 0;
			while (num < count && num < src.Length)
			{
				dst[num + dst_offset].r = global::UnityEngine.Mathf.Pow(src[num + src_offset].r, gamma);
				dst[num + dst_offset].g = global::UnityEngine.Mathf.Pow(src[num + src_offset].g, gamma);
				dst[num + dst_offset].b = global::UnityEngine.Mathf.Pow(src[num + src_offset].b, gamma);
				dst[num + dst_offset].a = src[num + src_offset].a;
				num++;
			}
		}

		public static void applyGamma2D(ref global::UnityEngine.Texture2D tex, float gamma)
		{
			for (int i = 0; i < tex.mipmapCount; i++)
			{
				global::UnityEngine.Color[] pixels = tex.GetPixels(i);
				global::mset.Util.applyGamma(ref pixels, gamma);
				tex.SetPixels(pixels);
			}
			tex.Apply(false);
		}

		public static void clearTo(ref global::UnityEngine.Color[] c, global::UnityEngine.Color color)
		{
			for (int i = 0; i < c.Length; i++)
			{
				c[i] = color;
			}
		}

		public static void clearTo2D(ref global::UnityEngine.Texture2D tex, global::UnityEngine.Color color)
		{
			for (int i = 0; i < tex.mipmapCount; i++)
			{
				global::UnityEngine.Color[] pixels = tex.GetPixels(i);
				global::mset.Util.clearTo(ref pixels, color);
				tex.SetPixels(pixels, i);
			}
			tex.Apply(false);
		}

		public static void clearChecker2D(ref global::UnityEngine.Texture2D tex)
		{
			global::UnityEngine.Color color = new global::UnityEngine.Color(0.25f, 0.25f, 0.25f, 0.25f);
			global::UnityEngine.Color color2 = new global::UnityEngine.Color(0.5f, 0.5f, 0.5f, 0.25f);
			global::UnityEngine.Color[] pixels = tex.GetPixels();
			int width = tex.width;
			int height = tex.height;
			int num = height / 4;
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					if (i / num % 2 == j / num % 2)
					{
						pixels[j * width + i] = color;
					}
					else
					{
						pixels[j * width + i] = color2;
					}
				}
			}
			tex.SetPixels(pixels);
			tex.Apply(false);
		}

		public static void clearCheckerCube(ref global::UnityEngine.Cubemap cube)
		{
			global::UnityEngine.Color color = new global::UnityEngine.Color(0.25f, 0.25f, 0.25f, 0.25f);
			global::UnityEngine.Color color2 = new global::UnityEngine.Color(0.5f, 0.5f, 0.5f, 0.25f);
			global::UnityEngine.Color[] pixels = cube.GetPixels(global::UnityEngine.CubemapFace.NegativeX);
			int width = cube.width;
			int num = global::UnityEngine.Mathf.Max(1, width / 4);
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < width; j++)
				{
					for (int k = 0; k < width; k++)
					{
						if (j / num % 2 == k / num % 2)
						{
							pixels[k * width + j] = color;
						}
						else
						{
							pixels[k * width + j] = color2;
						}
					}
				}
				cube.SetPixels(pixels, (global::UnityEngine.CubemapFace)i);
			}
			cube.Apply(true);
		}
	}
}
