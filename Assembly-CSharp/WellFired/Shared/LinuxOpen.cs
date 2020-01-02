using System;

namespace WellFired.Shared
{
	public class LinuxOpen : global::WellFired.Shared.IOpen
	{
		public void OpenFolderToDisplayFile(string filePath)
		{
			filePath = "\"filePath\"";
			global::RuntimeProcessRunner runtimeProcessRunner = new global::RuntimeProcessRunner("nautilus", string.Format("{0}", filePath));
			runtimeProcessRunner.Execute();
		}
	}
}
