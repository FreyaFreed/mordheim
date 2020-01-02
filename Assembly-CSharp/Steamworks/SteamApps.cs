using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamApps
	{
		public static bool BIsSubscribed()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_BIsSubscribed();
		}

		public static bool BIsLowViolence()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_BIsLowViolence();
		}

		public static bool BIsCybercafe()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_BIsCybercafe();
		}

		public static bool BIsVACBanned()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_BIsVACBanned();
		}

		public static string GetCurrentGameLanguage()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamApps_GetCurrentGameLanguage());
		}

		public static string GetAvailableGameLanguages()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamApps_GetAvailableGameLanguages());
		}

		public static bool BIsSubscribedApp(global::Steamworks.AppId_t appID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_BIsSubscribedApp(appID);
		}

		public static bool BIsDlcInstalled(global::Steamworks.AppId_t appID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_BIsDlcInstalled(appID);
		}

		public static uint GetEarliestPurchaseUnixTime(global::Steamworks.AppId_t nAppID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_GetEarliestPurchaseUnixTime(nAppID);
		}

		public static bool BIsSubscribedFromFreeWeekend()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_BIsSubscribedFromFreeWeekend();
		}

		public static int GetDLCCount()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_GetDLCCount();
		}

		public static bool BGetDLCDataByIndex(int iDLC, out global::Steamworks.AppId_t pAppID, out bool pbAvailable, out string pchName, int cchNameBufferSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(cchNameBufferSize);
			bool flag = global::Steamworks.NativeMethods.ISteamApps_BGetDLCDataByIndex(iDLC, out pAppID, out pbAvailable, intPtr, cchNameBufferSize);
			pchName = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		public static void InstallDLC(global::Steamworks.AppId_t nAppID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamApps_InstallDLC(nAppID);
		}

		public static void UninstallDLC(global::Steamworks.AppId_t nAppID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamApps_UninstallDLC(nAppID);
		}

		public static void RequestAppProofOfPurchaseKey(global::Steamworks.AppId_t nAppID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamApps_RequestAppProofOfPurchaseKey(nAppID);
		}

		public static bool GetCurrentBetaName(out string pchName, int cchNameBufferSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(cchNameBufferSize);
			bool flag = global::Steamworks.NativeMethods.ISteamApps_GetCurrentBetaName(intPtr, cchNameBufferSize);
			pchName = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		public static bool MarkContentCorrupt(bool bMissingFilesOnly)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_MarkContentCorrupt(bMissingFilesOnly);
		}

		public static uint GetInstalledDepots(global::Steamworks.AppId_t appID, global::Steamworks.DepotId_t[] pvecDepots, uint cMaxDepots)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_GetInstalledDepots(appID, pvecDepots, cMaxDepots);
		}

		public static uint GetAppInstallDir(global::Steamworks.AppId_t appID, out string pchFolder, uint cchFolderBufferSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal((int)cchFolderBufferSize);
			uint num = global::Steamworks.NativeMethods.ISteamApps_GetAppInstallDir(appID, intPtr, cchFolderBufferSize);
			pchFolder = ((num == 0U) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return num;
		}

		public static bool BIsAppInstalled(global::Steamworks.AppId_t appID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_BIsAppInstalled(appID);
		}

		public static global::Steamworks.CSteamID GetAppOwner()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamApps_GetAppOwner();
		}

		public static string GetLaunchQueryParam(string pchKey)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			string result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
			{
				result = global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamApps_GetLaunchQueryParam(utf8StringHandle));
			}
			return result;
		}

		public static bool GetDlcDownloadProgress(global::Steamworks.AppId_t nAppID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_GetDlcDownloadProgress(nAppID, out punBytesDownloaded, out punBytesTotal);
		}

		public static int GetAppBuildId()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamApps_GetAppBuildId();
		}
	}
}
