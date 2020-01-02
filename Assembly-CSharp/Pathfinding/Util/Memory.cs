﻿using System;

namespace Pathfinding.Util
{
	public static class Memory
	{
		public static void MemSet(byte[] array, byte value)
		{
			if (array == null)
			{
				throw new global::System.ArgumentNullException("array");
			}
			int num = 32;
			int i = 0;
			int num2 = global::System.Math.Min(num, array.Length);
			while (i < num2)
			{
				array[i++] = value;
			}
			num2 = array.Length;
			while (i < num2)
			{
				global::System.Buffer.BlockCopy(array, 0, array, i, global::System.Math.Min(num, num2 - i));
				i += num;
				num *= 2;
			}
		}

		public static void MemSet<T>(T[] array, T value, int byteSize) where T : struct
		{
			if (array == null)
			{
				throw new global::System.ArgumentNullException("array");
			}
			int num = 32;
			int i = 0;
			int num2 = global::System.Math.Min(num, array.Length);
			while (i < num2)
			{
				array[i] = value;
				i++;
			}
			num2 = array.Length;
			while (i < num2)
			{
				global::System.Buffer.BlockCopy(array, 0, array, i * byteSize, global::System.Math.Min(num, num2 - i) * byteSize);
				i += num;
				num *= 2;
			}
		}

		public static void MemSet<T>(T[] array, T value, int totalSize, int byteSize) where T : struct
		{
			if (array == null)
			{
				throw new global::System.ArgumentNullException("array");
			}
			int num = 32;
			int i = 0;
			int num2 = global::System.Math.Min(num, totalSize);
			while (i < num2)
			{
				array[i] = value;
				i++;
			}
			while (i < totalSize)
			{
				global::System.Buffer.BlockCopy(array, 0, array, i * byteSize, global::System.Math.Min(num, totalSize - i) * byteSize);
				i += num;
				num *= 2;
			}
		}
	}
}
