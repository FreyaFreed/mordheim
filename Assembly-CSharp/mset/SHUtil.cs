using System;
using UnityEngine;

namespace mset
{
	public class SHUtil
	{
		private static float project_l0_m0(global::UnityEngine.Vector3 u)
		{
			return global::mset.SHEncoding.sEquationConstants[0];
		}

		private static float project_l1_mneg1(global::UnityEngine.Vector3 u)
		{
			return global::mset.SHEncoding.sEquationConstants[1] * u.y;
		}

		private static float project_l1_m0(global::UnityEngine.Vector3 u)
		{
			return global::mset.SHEncoding.sEquationConstants[2] * u.z;
		}

		private static float project_l1_m1(global::UnityEngine.Vector3 u)
		{
			return global::mset.SHEncoding.sEquationConstants[3] * u.x;
		}

		private static float project_l2_mneg2(global::UnityEngine.Vector3 u)
		{
			return global::mset.SHEncoding.sEquationConstants[4] * u.y * u.x;
		}

		private static float project_l2_mneg1(global::UnityEngine.Vector3 u)
		{
			return global::mset.SHEncoding.sEquationConstants[5] * u.y * u.z;
		}

		private static float project_l2_m0(global::UnityEngine.Vector3 u)
		{
			return global::mset.SHEncoding.sEquationConstants[6] * (3f * u.z * u.z - 1f);
		}

		private static float project_l2_m1(global::UnityEngine.Vector3 u)
		{
			return global::mset.SHEncoding.sEquationConstants[7] * u.z * u.x;
		}

		private static float project_l2_m2(global::UnityEngine.Vector3 u)
		{
			return global::mset.SHEncoding.sEquationConstants[8] * (u.x * u.x - u.y * u.y);
		}

		private static void scale(ref global::mset.SHEncoding sh, float s)
		{
			for (int i = 0; i < 27; i++)
			{
				sh.c[i] *= s;
			}
		}

		public static void projectCubeBuffer(ref global::mset.SHEncoding sh, global::mset.CubeBuffer cube)
		{
			sh.clearToBlack();
			float num = 0f;
			ulong num2 = (ulong)((long)cube.faceSize);
			float[] array = new float[9];
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			for (ulong num3 = 0UL; num3 < 6UL; num3 += 1UL)
			{
				for (ulong num4 = 0UL; num4 < num2; num4 += 1UL)
				{
					for (ulong num5 = 0UL; num5 < num2; num5 += 1UL)
					{
						float num6 = 1f;
						global::mset.Util.invCubeLookup(ref zero, ref num6, num3, num5, num4, num2);
						float num7 = 1.33333337f;
						ulong num8 = num3 * num2 * num2 + num4 * num2 + num5;
						global::UnityEngine.Color color = cube.pixels[(int)(checked((global::System.IntPtr)num8))];
						array[0] = global::mset.SHUtil.project_l0_m0(zero);
						array[1] = global::mset.SHUtil.project_l1_mneg1(zero);
						array[2] = global::mset.SHUtil.project_l1_m0(zero);
						array[3] = global::mset.SHUtil.project_l1_m1(zero);
						array[4] = global::mset.SHUtil.project_l2_mneg2(zero);
						array[5] = global::mset.SHUtil.project_l2_mneg1(zero);
						array[6] = global::mset.SHUtil.project_l2_m0(zero);
						array[7] = global::mset.SHUtil.project_l2_m1(zero);
						array[8] = global::mset.SHUtil.project_l2_m2(zero);
						for (int i = 0; i < 9; i++)
						{
							sh.c[3 * i] += num7 * num6 * color[0] * array[i];
							sh.c[3 * i + 1] += num7 * num6 * color[1] * array[i];
							sh.c[3 * i + 2] += num7 * num6 * color[2] * array[i];
						}
						num += num6;
					}
				}
			}
			global::mset.SHUtil.scale(ref sh, 16f / num);
		}

		public static void projectCube(ref global::mset.SHEncoding sh, global::UnityEngine.Cubemap cube, int mip, bool hdr)
		{
			sh.clearToBlack();
			float num = 0f;
			ulong num2 = (ulong)((long)cube.width);
			mip = global::UnityEngine.Mathf.Min(global::mset.QPow.Log2i(num2) + 1, mip);
			num2 >>= mip;
			float[] array = new float[9];
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			for (ulong num3 = 0UL; num3 < 6UL; num3 += 1UL)
			{
				global::UnityEngine.Color color = global::UnityEngine.Color.black;
				global::UnityEngine.Color[] pixels = cube.GetPixels((global::UnityEngine.CubemapFace)num3, mip);
				for (ulong num4 = 0UL; num4 < num2; num4 += 1UL)
				{
					for (ulong num5 = 0UL; num5 < num2; num5 += 1UL)
					{
						float num6 = 1f;
						global::mset.Util.invCubeLookup(ref zero, ref num6, num3, num5, num4, num2);
						float num7 = 1.33333337f;
						ulong num8 = num4 * num2 + num5;
						checked
						{
							if (hdr)
							{
								global::mset.RGB.fromRGBM(ref color, pixels[(int)((global::System.IntPtr)num8)], true);
							}
							else
							{
								color = pixels[(int)((global::System.IntPtr)num8)];
							}
							array[0] = global::mset.SHUtil.project_l0_m0(zero);
							array[1] = global::mset.SHUtil.project_l1_mneg1(zero);
							array[2] = global::mset.SHUtil.project_l1_m0(zero);
							array[3] = global::mset.SHUtil.project_l1_m1(zero);
							array[4] = global::mset.SHUtil.project_l2_mneg2(zero);
							array[5] = global::mset.SHUtil.project_l2_mneg1(zero);
							array[6] = global::mset.SHUtil.project_l2_m0(zero);
							array[7] = global::mset.SHUtil.project_l2_m1(zero);
							array[8] = global::mset.SHUtil.project_l2_m2(zero);
						}
						for (int i = 0; i < 9; i++)
						{
							sh.c[3 * i] += num7 * num6 * color[0] * array[i];
							sh.c[3 * i + 1] += num7 * num6 * color[1] * array[i];
							sh.c[3 * i + 2] += num7 * num6 * color[2] * array[i];
						}
						num += num6;
					}
				}
			}
			global::mset.SHUtil.scale(ref sh, 16f / num);
		}

		public static void convolve(ref global::mset.SHEncoding sh)
		{
			global::mset.SHUtil.convolve(ref sh, 1f, 0.6666667f, 0.25f);
		}

		public static void convolve(ref global::mset.SHEncoding sh, float conv0, float conv1, float conv2)
		{
			for (int i = 0; i < 27; i++)
			{
				if (i < 3)
				{
					sh.c[i] *= conv0;
				}
				else if (i < 12)
				{
					sh.c[i] *= conv1;
				}
				else
				{
					sh.c[i] *= conv2;
				}
			}
		}
	}
}
