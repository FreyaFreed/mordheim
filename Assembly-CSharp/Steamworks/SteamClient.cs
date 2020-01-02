using System;

namespace Steamworks
{
	public static class SteamClient
	{
		public static global::Steamworks.HSteamPipe CreateSteamPipe()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.HSteamPipe)global::Steamworks.NativeMethods.ISteamClient_CreateSteamPipe();
		}

		public static bool BReleaseSteamPipe(global::Steamworks.HSteamPipe hSteamPipe)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamClient_BReleaseSteamPipe(hSteamPipe);
		}

		public static global::Steamworks.HSteamUser ConnectToGlobalUser(global::Steamworks.HSteamPipe hSteamPipe)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.HSteamUser)global::Steamworks.NativeMethods.ISteamClient_ConnectToGlobalUser(hSteamPipe);
		}

		public static global::Steamworks.HSteamUser CreateLocalUser(out global::Steamworks.HSteamPipe phSteamPipe, global::Steamworks.EAccountType eAccountType)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.HSteamUser)global::Steamworks.NativeMethods.ISteamClient_CreateLocalUser(out phSteamPipe, eAccountType);
		}

		public static void ReleaseUser(global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.HSteamUser hUser)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamClient_ReleaseUser(hSteamPipe, hUser);
		}

		public static global::System.IntPtr GetISteamUser(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamUser(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamGameServer(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamGameServer(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static void SetLocalIPBinding(uint unIP, ushort usPort)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamClient_SetLocalIPBinding(unIP, usPort);
		}

		public static global::System.IntPtr GetISteamFriends(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamFriends(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamUtils(global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamUtils(hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamMatchmaking(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamMatchmaking(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamMatchmakingServers(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamMatchmakingServers(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamGenericInterface(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamGenericInterface(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamUserStats(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamUserStats(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamGameServerStats(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamGameServerStats(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamApps(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamApps(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamNetworking(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamNetworking(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamRemoteStorage(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamRemoteStorage(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamScreenshots(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamScreenshots(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static uint GetIPCCallCount()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamClient_GetIPCCallCount();
		}

		public static void SetWarningMessageHook(global::Steamworks.SteamAPIWarningMessageHook_t pFunction)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamClient_SetWarningMessageHook(pFunction);
		}

		public static bool BShutdownIfAllPipesClosed()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamClient_BShutdownIfAllPipesClosed();
		}

		public static global::System.IntPtr GetISteamHTTP(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamHTTP(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamUnifiedMessages(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamUnifiedMessages(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamController(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamController(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamUGC(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamUGC(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamAppList(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamAppList(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamMusic(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamMusic(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamMusicRemote(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamMusicRemote(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamHTMLSurface(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamHTMLSurface(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamInventory(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamInventory(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		public static global::System.IntPtr GetISteamVideo(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, string pchVersion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = global::Steamworks.NativeMethods.ISteamClient_GetISteamVideo(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}
	}
}
