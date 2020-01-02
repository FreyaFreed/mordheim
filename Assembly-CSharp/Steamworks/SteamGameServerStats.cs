using System;

namespace Steamworks
{
	public static class SteamGameServerStats
	{
		public static global::Steamworks.SteamAPICall_t RequestUserStats(global::Steamworks.CSteamID steamIDUser)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamGameServerStats_RequestUserStats(steamIDUser);
		}

		public static bool GetUserStat(global::Steamworks.CSteamID steamIDUser, string pchName, out int pData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamGameServerStats_GetUserStat(steamIDUser, utf8StringHandle, out pData);
			}
			return result;
		}

		public static bool GetUserStat(global::Steamworks.CSteamID steamIDUser, string pchName, out float pData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamGameServerStats_GetUserStat_(steamIDUser, utf8StringHandle, out pData);
			}
			return result;
		}

		public static bool GetUserAchievement(global::Steamworks.CSteamID steamIDUser, string pchName, out bool pbAchieved)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamGameServerStats_GetUserAchievement(steamIDUser, utf8StringHandle, out pbAchieved);
			}
			return result;
		}

		public static bool SetUserStat(global::Steamworks.CSteamID steamIDUser, string pchName, int nData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamGameServerStats_SetUserStat(steamIDUser, utf8StringHandle, nData);
			}
			return result;
		}

		public static bool SetUserStat(global::Steamworks.CSteamID steamIDUser, string pchName, float fData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamGameServerStats_SetUserStat_(steamIDUser, utf8StringHandle, fData);
			}
			return result;
		}

		public static bool UpdateUserAvgRateStat(global::Steamworks.CSteamID steamIDUser, string pchName, float flCountThisSession, double dSessionLength)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamGameServerStats_UpdateUserAvgRateStat(steamIDUser, utf8StringHandle, flCountThisSession, dSessionLength);
			}
			return result;
		}

		public static bool SetUserAchievement(global::Steamworks.CSteamID steamIDUser, string pchName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamGameServerStats_SetUserAchievement(steamIDUser, utf8StringHandle);
			}
			return result;
		}

		public static bool ClearUserAchievement(global::Steamworks.CSteamID steamIDUser, string pchName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamGameServerStats_ClearUserAchievement(steamIDUser, utf8StringHandle);
			}
			return result;
		}

		public static global::Steamworks.SteamAPICall_t StoreUserStats(global::Steamworks.CSteamID steamIDUser)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamGameServerStats_StoreUserStats(steamIDUser);
		}
	}
}
