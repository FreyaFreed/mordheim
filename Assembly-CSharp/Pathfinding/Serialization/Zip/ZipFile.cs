using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pathfinding.Serialization.Zip
{
	public class ZipFile
	{
		public void AddEntry(string name, byte[] bytes)
		{
			this.dict[name] = new global::Pathfinding.Serialization.Zip.ZipEntry(name, bytes);
		}

		public bool ContainsEntry(string name)
		{
			return this.dict.ContainsKey(name);
		}

		public void Save(global::System.IO.Stream stream)
		{
			global::System.IO.BinaryWriter binaryWriter = new global::System.IO.BinaryWriter(stream);
			binaryWriter.Write(this.dict.Count);
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::Pathfinding.Serialization.Zip.ZipEntry> keyValuePair in this.dict)
			{
				binaryWriter.Write(keyValuePair.Key);
				binaryWriter.Write(keyValuePair.Value.bytes.Length);
				binaryWriter.Write(keyValuePair.Value.bytes);
			}
		}

		public static global::Pathfinding.Serialization.Zip.ZipFile Read(global::System.IO.Stream stream)
		{
			global::Pathfinding.Serialization.Zip.ZipFile zipFile = new global::Pathfinding.Serialization.Zip.ZipFile();
			global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(stream);
			int num = binaryReader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string text = binaryReader.ReadString();
				int count = binaryReader.ReadInt32();
				byte[] bytes = binaryReader.ReadBytes(count);
				zipFile.dict[text] = new global::Pathfinding.Serialization.Zip.ZipEntry(text, bytes);
			}
			return zipFile;
		}

		public global::Pathfinding.Serialization.Zip.ZipEntry this[string index]
		{
			get
			{
				global::Pathfinding.Serialization.Zip.ZipEntry result;
				this.dict.TryGetValue(index, out result);
				return result;
			}
		}

		public void Dispose()
		{
		}

		public global::System.Text.Encoding AlternateEncoding;

		public global::Pathfinding.Serialization.Zip.ZipOption AlternateEncodingUsage;

		private global::System.Collections.Generic.Dictionary<string, global::Pathfinding.Serialization.Zip.ZipEntry> dict = new global::System.Collections.Generic.Dictionary<string, global::Pathfinding.Serialization.Zip.ZipEntry>();
	}
}
