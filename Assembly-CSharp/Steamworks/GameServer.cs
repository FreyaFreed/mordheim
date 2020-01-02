using System;

namespace Steamworks
{
	public static class GameServer
	{
		public static bool InitSafe(uint unIP, ushort usSteamPort, ushort usGamePort, ushort usQueryPort, global::Steamworks.EServerMode eServerMode, string pchVersionString)
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersionString))
			{
				result = global::Steamworks.NativeMethods.SteamGameServer_InitSafe(unIP, usSteamPort, usGamePort, usQueryPort, eServerMode, utf8StringHandle);
			}
			return result;
		}

		public static bool Init(uint unIP, ushort usSteamPort, ushort usGamePort, ushort usQueryPort, global::Steamworks.EServerMode eServerMode, string pchVersionString)
		{
			return global::Steamworks.GameServer.InitSafe(unIP, usSteamPort, usGamePort, usQueryPort, eServerMode, pchVersionString);
		}

		public static void Shutdown()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			global::Steamworks.NativeMethods.SteamGameServer_Shutdown();
		}

		public static void RunCallbacks()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			global::Steamworks.NativeMethods.SteamGameServer_RunCallbacks();
		}

		public static bool BSecure()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return global::Steamworks.NativeMethods.SteamGameServer_BSecure();
		}

		public static global::Steamworks.CSteamID GetSteamID()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.SteamGameServer_GetSteamID();
		}

		public static global::Steamworks.HSteamPipe GetHSteamPipe()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return (global::Steamworks.HSteamPipe)global::Steamworks.NativeMethods.SteamGameServer_GetHSteamPipe();
		}

		public static global::Steamworks.HSteamUser GetHSteamUser()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return (global::Steamworks.HSteamUser)global::Steamworks.NativeMethods.SteamGameServer_GetHSteamUser();
		}
	}
}
