using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamAppList
	{
		public static uint GetNumInstalledApps()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamAppList_GetNumInstalledApps();
		}

		public static uint GetInstalledApps(global::Steamworks.AppId_t[] pvecAppID, uint unMaxAppIDs)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamAppList_GetInstalledApps(pvecAppID, unMaxAppIDs);
		}

		public static int GetAppName(global::Steamworks.AppId_t nAppID, out string pchName, int cchNameMax)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(cchNameMax);
			int num = global::Steamworks.NativeMethods.ISteamAppList_GetAppName(nAppID, intPtr, cchNameMax);
			pchName = ((num == -1) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return num;
		}

		public static int GetAppInstallDir(global::Steamworks.AppId_t nAppID, out string pchDirectory, int cchNameMax)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(cchNameMax);
			int num = global::Steamworks.NativeMethods.ISteamAppList_GetAppInstallDir(nAppID, intPtr, cchNameMax);
			pchDirectory = ((num == -1) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return num;
		}

		public static int GetAppBuildId(global::Steamworks.AppId_t nAppID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamAppList_GetAppBuildId(nAppID);
		}
	}
}
