using System;

namespace Steamworks
{
	public static class SteamScreenshots
	{
		public static global::Steamworks.ScreenshotHandle WriteScreenshot(byte[] pubRGB, uint cubRGB, int nWidth, int nHeight)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.ScreenshotHandle)global::Steamworks.NativeMethods.ISteamScreenshots_WriteScreenshot(pubRGB, cubRGB, nWidth, nHeight);
		}

		public static global::Steamworks.ScreenshotHandle AddScreenshotToLibrary(string pchFilename, string pchThumbnailFilename, int nWidth, int nHeight)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.ScreenshotHandle result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFilename))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchThumbnailFilename))
				{
					result = (global::Steamworks.ScreenshotHandle)global::Steamworks.NativeMethods.ISteamScreenshots_AddScreenshotToLibrary(utf8StringHandle, utf8StringHandle2, nWidth, nHeight);
				}
			}
			return result;
		}

		public static void TriggerScreenshot()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamScreenshots_TriggerScreenshot();
		}

		public static void HookScreenshots(bool bHook)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamScreenshots_HookScreenshots(bHook);
		}

		public static bool SetLocation(global::Steamworks.ScreenshotHandle hScreenshot, string pchLocation)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchLocation))
			{
				result = global::Steamworks.NativeMethods.ISteamScreenshots_SetLocation(hScreenshot, utf8StringHandle);
			}
			return result;
		}

		public static bool TagUser(global::Steamworks.ScreenshotHandle hScreenshot, global::Steamworks.CSteamID steamID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamScreenshots_TagUser(hScreenshot, steamID);
		}

		public static bool TagPublishedFile(global::Steamworks.ScreenshotHandle hScreenshot, global::Steamworks.PublishedFileId_t unPublishedFileID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamScreenshots_TagPublishedFile(hScreenshot, unPublishedFileID);
		}
	}
}
