using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	public class DllCheck
	{
		[global::System.Runtime.InteropServices.DllImport("kernel32.dll")]
		public static extern global::System.IntPtr GetModuleHandle(string lpModuleName);

		[global::System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = global::System.Runtime.InteropServices.CharSet.Auto)]
		private static extern int GetModuleFileName(global::System.IntPtr hModule, global::System.Text.StringBuilder strFullPath, int nSize);

		public static bool Test()
		{
			return true;
		}

		private static bool CheckSteamAPIDLL()
		{
			string lpModuleName;
			int num;
			if (global::System.IntPtr.Size == 4)
			{
				lpModuleName = "steam_api.dll";
				num = 217168;
			}
			else
			{
				lpModuleName = "steam_api64.dll";
				num = 239184;
			}
			global::System.IntPtr moduleHandle = global::Steamworks.DllCheck.GetModuleHandle(lpModuleName);
			if (moduleHandle == global::System.IntPtr.Zero)
			{
				return true;
			}
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder(256);
			global::Steamworks.DllCheck.GetModuleFileName(moduleHandle, stringBuilder, stringBuilder.Capacity);
			string text = stringBuilder.ToString();
			if (global::System.IO.File.Exists(text))
			{
				global::System.IO.FileInfo fileInfo = new global::System.IO.FileInfo(text);
				if (fileInfo.Length != (long)num)
				{
					return false;
				}
				if (global::System.Diagnostics.FileVersionInfo.GetVersionInfo(text).FileVersion != "03.27.76.74")
				{
					return false;
				}
			}
			return true;
		}
	}
}
