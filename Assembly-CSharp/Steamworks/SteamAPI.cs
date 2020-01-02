using System;

namespace Steamworks
{
	public static class SteamAPI
	{
		public static bool RestartAppIfNecessary(global::Steamworks.AppId_t unOwnAppID)
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return global::Steamworks.NativeMethods.SteamAPI_RestartAppIfNecessary(unOwnAppID);
		}

		public static bool InitSafe()
		{
			return global::Steamworks.SteamAPI.Init();
		}

		public static bool Init()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return global::Steamworks.NativeMethods.SteamAPI_InitSafe();
		}

		public static void Shutdown()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			global::Steamworks.NativeMethods.SteamAPI_Shutdown();
		}

		public static void ReleaseCurrentThreadMemory()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			global::Steamworks.NativeMethods.SteamAPI_ReleaseCurrentThreadMemory();
		}

		public static void RunCallbacks()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			global::Steamworks.NativeMethods.SteamAPI_RunCallbacks();
		}

		public static bool IsSteamRunning()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return global::Steamworks.NativeMethods.SteamAPI_IsSteamRunning();
		}

		public static global::Steamworks.HSteamUser GetHSteamUserCurrent()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return (global::Steamworks.HSteamUser)global::Steamworks.NativeMethods.Steam_GetHSteamUserCurrent();
		}

		public static global::Steamworks.HSteamPipe GetHSteamPipe()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return (global::Steamworks.HSteamPipe)global::Steamworks.NativeMethods.SteamAPI_GetHSteamPipe();
		}

		public static global::Steamworks.HSteamUser GetHSteamUser()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return (global::Steamworks.HSteamUser)global::Steamworks.NativeMethods.SteamAPI_GetHSteamUser();
		}
	}
}
