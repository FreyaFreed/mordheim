using System;

namespace WellFired.Shared
{
	public class OSXOpen : global::WellFired.Shared.IOpen
	{
		public void OpenFolderToDisplayFile(string filePath)
		{
			filePath = "\"" + filePath + "\"";
			global::RuntimeProcessRunner runtimeProcessRunner = new global::RuntimeProcessRunner("open", string.Format("-n -R {0}", filePath));
			runtimeProcessRunner.Execute();
		}
	}
}
