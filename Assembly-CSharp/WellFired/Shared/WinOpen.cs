using System;

namespace WellFired.Shared
{
	public class WinOpen : global::WellFired.Shared.IOpen
	{
		public void OpenFolderToDisplayFile(string filePath)
		{
			filePath = "\"filePath\"";
			global::RuntimeProcessRunner runtimeProcessRunner = new global::RuntimeProcessRunner("explorer.exe", string.Format("/select,{0}", filePath));
			runtimeProcessRunner.Execute();
		}
	}
}
