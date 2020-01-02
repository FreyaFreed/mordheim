using System;

namespace mset
{
	public class QPow
	{
		public static float Pow1(float f)
		{
			return f;
		}

		public static float Pow2(float f)
		{
			return f * f;
		}

		public static float Pow4(float f)
		{
			f *= f;
			return f * f;
		}

		public static float Pow8(float f)
		{
			f *= f;
			f *= f;
			return f * f;
		}

		public static float Pow16(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		public static float Pow32(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		public static float Pow64(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		public static float Pow128(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		public static float Pow256(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		public static float Pow512(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		public static global::mset.QPow.PowFunc closestPowFunc(int exp)
		{
			if (exp + 128 >= 512)
			{
				return new global::mset.QPow.PowFunc(global::mset.QPow.Pow512);
			}
			if (exp + 64 >= 256)
			{
				return new global::mset.QPow.PowFunc(global::mset.QPow.Pow256);
			}
			if (exp + 32 >= 128)
			{
				return new global::mset.QPow.PowFunc(global::mset.QPow.Pow128);
			}
			if (exp + 16 >= 64)
			{
				return new global::mset.QPow.PowFunc(global::mset.QPow.Pow64);
			}
			if (exp + 8 >= 32)
			{
				return new global::mset.QPow.PowFunc(global::mset.QPow.Pow32);
			}
			if (exp + 4 >= 16)
			{
				return new global::mset.QPow.PowFunc(global::mset.QPow.Pow16);
			}
			if (exp + 2 >= 8)
			{
				return new global::mset.QPow.PowFunc(global::mset.QPow.Pow8);
			}
			if (exp + 1 >= 4)
			{
				return new global::mset.QPow.PowFunc(global::mset.QPow.Pow4);
			}
			if (exp >= 2)
			{
				return new global::mset.QPow.PowFunc(global::mset.QPow.Pow2);
			}
			return new global::mset.QPow.PowFunc(global::mset.QPow.Pow1);
		}

		public static int Log2i(int val)
		{
			int num = 0;
			while (val > 0)
			{
				num++;
				val >>= 1;
			}
			return num;
		}

		public static int Log2i(ulong val)
		{
			int num = 0;
			while (val > 0UL)
			{
				num++;
				val >>= 1;
			}
			return num;
		}

		public static int clampedDownShift(int val, int shift)
		{
			while (val > 0 && shift > 0)
			{
				val >>= 1;
				shift--;
			}
			return val;
		}

		public static int clampedDownShift(int val, int shift, int minVal)
		{
			while (val > minVal && shift > 0)
			{
				val >>= 1;
				shift--;
			}
			return val;
		}

		public delegate float PowFunc(float f);
	}
}
