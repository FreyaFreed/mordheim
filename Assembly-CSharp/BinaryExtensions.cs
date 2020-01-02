using System;
using System.Collections.Generic;
using System.IO;
using TNet;

public static class BinaryExtensions
{
	private static object[] GetTempBuffer(int count)
	{
		object[] array;
		if (!global::BinaryExtensions.mTemp.TryGetValue((byte)count, out array))
		{
			array = new object[count];
			global::BinaryExtensions.mTemp[(byte)count] = array;
		}
		return array;
	}

	public static void WriteArray(this global::System.IO.BinaryWriter bw, params object[] objs)
	{
		bw.WriteInt(objs.Length);
		if (objs.Length == 0)
		{
			return;
		}
		int i = 0;
		int num = objs.Length;
		while (i < num)
		{
			bw.WriteObject(objs[i]);
			i++;
		}
	}

	public static object[] ReadArray(this global::System.IO.BinaryReader reader)
	{
		int num = reader.ReadInt();
		if (num == 0)
		{
			return null;
		}
		object[] tempBuffer = global::BinaryExtensions.GetTempBuffer(num);
		for (int i = 0; i < num; i++)
		{
			tempBuffer[i] = reader.ReadObject();
		}
		return tempBuffer;
	}

	public static object[] ReadArray(this global::System.IO.BinaryReader reader, object obj)
	{
		int num = reader.ReadInt() + 1;
		object[] tempBuffer = global::BinaryExtensions.GetTempBuffer(num);
		tempBuffer[0] = obj;
		for (int i = 1; i < num; i++)
		{
			tempBuffer[i] = reader.ReadObject();
		}
		return tempBuffer;
	}

	public static object[] CombineArrays(object obj, params object[] objs)
	{
		int num = objs.Length;
		object[] tempBuffer = global::BinaryExtensions.GetTempBuffer(num + 1);
		tempBuffer[0] = obj;
		for (int i = 0; i < num; i++)
		{
			tempBuffer[i + 1] = objs[i];
		}
		return tempBuffer;
	}

	private static global::System.Collections.Generic.Dictionary<byte, object[]> mTemp = new global::System.Collections.Generic.Dictionary<byte, object[]>();
}
