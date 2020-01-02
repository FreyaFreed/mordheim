using System;
using System.IO;

namespace Pathfinding.Serialization.Zip
{
	public class ZipEntry
	{
		public ZipEntry(string name, byte[] bytes)
		{
			this.name = name;
			this.bytes = bytes;
		}

		public void Extract(global::System.IO.Stream stream)
		{
			stream.Write(this.bytes, 0, this.bytes.Length);
		}

		internal string name;

		internal byte[] bytes;
	}
}
