using System;
using System.IO;

public class Thoth
{
	public static string WriteToString(global::IThoth thoth)
	{
		string result;
		using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream())
		{
			using (global::System.IO.BinaryWriter binaryWriter = new global::System.IO.BinaryWriter(memoryStream))
			{
				thoth.Write(binaryWriter);
				result = global::System.Convert.ToBase64String(memoryStream.ToArray());
			}
		}
		return result;
	}

	public static bool ReadFromString(string source, global::IThoth target)
	{
		bool result = true;
		try
		{
			using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream(global::System.Convert.FromBase64String(source)))
			{
				using (global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(memoryStream))
				{
					target.Read(binaryReader);
					result = (memoryStream.Length == memoryStream.Position);
				}
			}
		}
		catch (global::System.IO.IOException ex)
		{
			global::PandoraDebug.LogWarning("Unable to read string: " + ex.ToString(), "THOTH", null);
			return false;
		}
		return result;
	}

	public static byte[] WriteToArray(global::IThoth thoth)
	{
		byte[] result;
		using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream())
		{
			using (global::System.IO.BinaryWriter binaryWriter = new global::System.IO.BinaryWriter(memoryStream))
			{
				thoth.Write(binaryWriter);
				result = memoryStream.ToArray();
			}
		}
		return result;
	}

	public static bool ReadFromArray(byte[] bytes, global::IThoth thoth)
	{
		if (bytes == null)
		{
			return false;
		}
		if (bytes.Length == 0)
		{
			return false;
		}
		bool result = true;
		try
		{
			using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream(bytes))
			{
				using (global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(memoryStream))
				{
					thoth.Read(binaryReader);
					result = (memoryStream.Length == memoryStream.Position);
				}
			}
		}
		catch (global::System.IO.IOException ex)
		{
			global::PandoraDebug.LogWarning("Unable to read array: " + ex.ToString(), "THOTH", null);
			return false;
		}
		return result;
	}

	public static void WriteToFile(string filePath, global::IThoth thoth)
	{
		global::System.IO.Directory.CreateDirectory(global::System.IO.Path.GetDirectoryName(filePath));
		try
		{
			using (global::System.IO.Stream stream = global::System.IO.File.Open(filePath, global::System.IO.FileMode.Create))
			{
				using (global::System.IO.BinaryWriter binaryWriter = new global::System.IO.BinaryWriter(stream))
				{
					thoth.Write(binaryWriter);
				}
				stream.Close();
			}
		}
		catch (global::System.IO.IOException ex)
		{
			global::PandoraDebug.LogWarning("Unable to write file: " + ex.ToString(), "THOTH", null);
		}
	}

	public static bool ReadFromFile(string filePath, global::IThoth thoth)
	{
		bool result = true;
		try
		{
			using (global::System.IO.Stream stream = global::System.IO.File.Open(filePath, global::System.IO.FileMode.Open))
			{
				using (global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(stream))
				{
					thoth.Read(binaryReader);
					result = (stream.Length == stream.Position);
				}
				stream.Close();
			}
		}
		catch (global::System.IO.IOException ex)
		{
			global::PandoraDebug.LogWarning("Unable to read from file: " + ex.ToString(), "THOTH", null);
			return false;
		}
		return result;
	}

	public static void Copy(global::IThoth src, global::IThoth dst)
	{
		try
		{
			using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream())
			{
				using (global::System.IO.BinaryWriter binaryWriter = new global::System.IO.BinaryWriter(memoryStream))
				{
					src.Write(binaryWriter);
					memoryStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
					using (global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(memoryStream))
					{
						dst.Read(binaryReader);
					}
				}
			}
		}
		catch (global::System.IO.IOException ex)
		{
			global::PandoraDebug.LogWarning("Unable to copy file: " + ex.ToString(), "THOTH", null);
		}
	}

	public static void Write(global::System.IO.BinaryWriter writer, bool b)
	{
		writer.Write(b);
	}

	public static void Write(global::System.IO.BinaryWriter writer, byte b)
	{
		writer.Write(b);
	}

	public static void Write(global::System.IO.BinaryWriter writer, byte[] b)
	{
		writer.Write(b.Length);
		writer.Write(b);
	}

	public static void Write(global::System.IO.BinaryWriter writer, char c)
	{
		writer.Write(c);
	}

	public static void Write(global::System.IO.BinaryWriter writer, char[] c)
	{
		writer.Write(c.Length);
		writer.Write(c);
	}

	public static void Write(global::System.IO.BinaryWriter writer, decimal d)
	{
		writer.Write(d);
	}

	public static void Write(global::System.IO.BinaryWriter writer, double d)
	{
		writer.Write(d);
	}

	public static void Write(global::System.IO.BinaryWriter writer, short i)
	{
		writer.Write(i);
	}

	public static void Write(global::System.IO.BinaryWriter writer, int i)
	{
		writer.Write(i);
	}

	public static void Write(global::System.IO.BinaryWriter writer, long i)
	{
		writer.Write(i);
	}

	public static void Write(global::System.IO.BinaryWriter writer, sbyte b)
	{
		writer.Write(b);
	}

	public static void Write(global::System.IO.BinaryWriter writer, float s)
	{
		writer.Write(s);
	}

	public static void Write(global::System.IO.BinaryWriter writer, string s)
	{
		writer.Write(s);
	}

	public static void Write(global::System.IO.BinaryWriter writer, ushort i)
	{
		writer.Write(i);
	}

	public static void Write(global::System.IO.BinaryWriter writer, uint i)
	{
		writer.Write(i);
	}

	public static void Write(global::System.IO.BinaryWriter writer, ulong i)
	{
		writer.Write(i);
	}

	public static void Read(global::System.IO.BinaryReader reader, out bool b)
	{
		b = reader.ReadBoolean();
	}

	public static void Read(global::System.IO.BinaryReader reader, out byte b)
	{
		b = reader.ReadByte();
	}

	public static void Read(global::System.IO.BinaryReader reader, out byte[] b)
	{
		int count = 0;
		global::Thoth.Read(reader, out count);
		b = reader.ReadBytes(count);
	}

	public static void Read(global::System.IO.BinaryReader reader, out char c)
	{
		c = reader.ReadChar();
	}

	public static void Read(global::System.IO.BinaryReader reader, out char[] c)
	{
		int count = 0;
		global::Thoth.Read(reader, out count);
		c = reader.ReadChars(count);
	}

	public static void Read(global::System.IO.BinaryReader reader, out decimal d)
	{
		d = reader.ReadDecimal();
	}

	public static void Read(global::System.IO.BinaryReader reader, out double d)
	{
		d = reader.ReadDouble();
	}

	public static void Read(global::System.IO.BinaryReader reader, out short i)
	{
		i = reader.ReadInt16();
	}

	public static void Read(global::System.IO.BinaryReader reader, out int i)
	{
		i = reader.ReadInt32();
	}

	public static void Read(global::System.IO.BinaryReader reader, out long i)
	{
		i = reader.ReadInt64();
	}

	public static void Read(global::System.IO.BinaryReader reader, out sbyte b)
	{
		b = reader.ReadSByte();
	}

	public static void Read(global::System.IO.BinaryReader reader, out float s)
	{
		s = reader.ReadSingle();
	}

	public static void Read(global::System.IO.BinaryReader reader, out string s)
	{
		s = reader.ReadString();
	}

	public static void Read(global::System.IO.BinaryReader reader, out ushort i)
	{
		i = reader.ReadUInt16();
	}

	public static void Read(global::System.IO.BinaryReader reader, out uint i)
	{
		i = reader.ReadUInt32();
	}

	public static void Read(global::System.IO.BinaryReader reader, out ulong i)
	{
		i = reader.ReadUInt64();
	}
}
