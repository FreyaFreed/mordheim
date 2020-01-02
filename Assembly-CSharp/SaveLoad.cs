using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad
{
	public static bool Exists(string filePath)
	{
		return global::System.IO.File.Exists(filePath);
	}

	public static void Delete(string filePath)
	{
		if (global::SaveLoad.Exists(filePath))
		{
			global::System.IO.File.Delete(filePath);
		}
	}

	public static void Save(string filePath, object data)
	{
		global::System.IO.Directory.CreateDirectory(global::System.IO.Path.GetDirectoryName(filePath));
		global::System.IO.Stream stream = global::System.IO.File.Open(filePath, global::System.IO.FileMode.Create);
		new global::System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
		{
			Binder = new global::VersionDeserializationBinderEdge()
		}.Serialize(stream, data);
		stream.Close();
	}

	public static T Load<T>(string filePath)
	{
		global::System.IO.Stream stream = global::System.IO.File.Open(filePath, global::System.IO.FileMode.Open);
		T result = (T)((object)new global::System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
		{
			Binder = new global::VersionDeserializationBinderEdge()
		}.Deserialize(stream));
		stream.Close();
		return result;
	}
}
