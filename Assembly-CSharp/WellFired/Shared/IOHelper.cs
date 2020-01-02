using System;
using System.IO;

namespace WellFired.Shared
{
	public class IOHelper : global::WellFired.Shared.IIOHelper
	{
		public bool FileExists(string file)
		{
			return global::System.IO.File.Exists(file);
		}
	}
}
