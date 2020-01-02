using System;

namespace Steamworks
{
	public static class SteamVideo
	{
		public static void GetVideoURL(global::Steamworks.AppId_t unVideoAppID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamVideo_GetVideoURL(unVideoAppID);
		}

		public static bool IsBroadcasting(out int pnNumViewers)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamVideo_IsBroadcasting(out pnNumViewers);
		}
	}
}
