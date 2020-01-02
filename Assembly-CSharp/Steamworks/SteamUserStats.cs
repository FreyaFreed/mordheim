using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamUserStats
	{
		public static bool RequestCurrentStats()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUserStats_RequestCurrentStats();
		}

		public static bool GetStat(string pchName, out int pData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetStat(utf8StringHandle, out pData);
			}
			return result;
		}

		public static bool GetStat(string pchName, out float pData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetStat_(utf8StringHandle, out pData);
			}
			return result;
		}

		public static bool SetStat(string pchName, int nData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_SetStat(utf8StringHandle, nData);
			}
			return result;
		}

		public static bool SetStat(string pchName, float fData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_SetStat_(utf8StringHandle, fData);
			}
			return result;
		}

		public static bool UpdateAvgRateStat(string pchName, float flCountThisSession, double dSessionLength)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_UpdateAvgRateStat(utf8StringHandle, flCountThisSession, dSessionLength);
			}
			return result;
		}

		public static bool GetAchievement(string pchName, out bool pbAchieved)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetAchievement(utf8StringHandle, out pbAchieved);
			}
			return result;
		}

		public static bool SetAchievement(string pchName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_SetAchievement(utf8StringHandle);
			}
			return result;
		}

		public static bool ClearAchievement(string pchName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_ClearAchievement(utf8StringHandle);
			}
			return result;
		}

		public static bool GetAchievementAndUnlockTime(string pchName, out bool pbAchieved, out uint punUnlockTime)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetAchievementAndUnlockTime(utf8StringHandle, out pbAchieved, out punUnlockTime);
			}
			return result;
		}

		public static bool StoreStats()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUserStats_StoreStats();
		}

		public static int GetAchievementIcon(string pchName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			int result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetAchievementIcon(utf8StringHandle);
			}
			return result;
		}

		public static string GetAchievementDisplayAttribute(string pchName, string pchKey)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			string result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
				{
					result = global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamUserStats_GetAchievementDisplayAttribute(utf8StringHandle, utf8StringHandle2));
				}
			}
			return result;
		}

		public static bool IndicateAchievementProgress(string pchName, uint nCurProgress, uint nMaxProgress)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_IndicateAchievementProgress(utf8StringHandle, nCurProgress, nMaxProgress);
			}
			return result;
		}

		public static uint GetNumAchievements()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUserStats_GetNumAchievements();
		}

		public static string GetAchievementName(uint iAchievement)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamUserStats_GetAchievementName(iAchievement));
		}

		public static global::Steamworks.SteamAPICall_t RequestUserStats(global::Steamworks.CSteamID steamIDUser)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUserStats_RequestUserStats(steamIDUser);
		}

		public static bool GetUserStat(global::Steamworks.CSteamID steamIDUser, string pchName, out int pData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetUserStat(steamIDUser, utf8StringHandle, out pData);
			}
			return result;
		}

		public static bool GetUserStat(global::Steamworks.CSteamID steamIDUser, string pchName, out float pData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetUserStat_(steamIDUser, utf8StringHandle, out pData);
			}
			return result;
		}

		public static bool GetUserAchievement(global::Steamworks.CSteamID steamIDUser, string pchName, out bool pbAchieved)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetUserAchievement(steamIDUser, utf8StringHandle, out pbAchieved);
			}
			return result;
		}

		public static bool GetUserAchievementAndUnlockTime(global::Steamworks.CSteamID steamIDUser, string pchName, out bool pbAchieved, out uint punUnlockTime)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetUserAchievementAndUnlockTime(steamIDUser, utf8StringHandle, out pbAchieved, out punUnlockTime);
			}
			return result;
		}

		public static bool ResetAllStats(bool bAchievementsToo)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUserStats_ResetAllStats(bAchievementsToo);
		}

		public static global::Steamworks.SteamAPICall_t FindOrCreateLeaderboard(string pchLeaderboardName, global::Steamworks.ELeaderboardSortMethod eLeaderboardSortMethod, global::Steamworks.ELeaderboardDisplayType eLeaderboardDisplayType)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchLeaderboardName))
			{
				result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUserStats_FindOrCreateLeaderboard(utf8StringHandle, eLeaderboardSortMethod, eLeaderboardDisplayType);
			}
			return result;
		}

		public static global::Steamworks.SteamAPICall_t FindLeaderboard(string pchLeaderboardName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchLeaderboardName))
			{
				result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUserStats_FindLeaderboard(utf8StringHandle);
			}
			return result;
		}

		public static string GetLeaderboardName(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamUserStats_GetLeaderboardName(hSteamLeaderboard));
		}

		public static int GetLeaderboardEntryCount(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUserStats_GetLeaderboardEntryCount(hSteamLeaderboard);
		}

		public static global::Steamworks.ELeaderboardSortMethod GetLeaderboardSortMethod(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUserStats_GetLeaderboardSortMethod(hSteamLeaderboard);
		}

		public static global::Steamworks.ELeaderboardDisplayType GetLeaderboardDisplayType(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUserStats_GetLeaderboardDisplayType(hSteamLeaderboard);
		}

		public static global::Steamworks.SteamAPICall_t DownloadLeaderboardEntries(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard, global::Steamworks.ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUserStats_DownloadLeaderboardEntries(hSteamLeaderboard, eLeaderboardDataRequest, nRangeStart, nRangeEnd);
		}

		public static global::Steamworks.SteamAPICall_t DownloadLeaderboardEntriesForUsers(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard, global::Steamworks.CSteamID[] prgUsers, int cUsers)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUserStats_DownloadLeaderboardEntriesForUsers(hSteamLeaderboard, prgUsers, cUsers);
		}

		public static bool GetDownloadedLeaderboardEntry(global::Steamworks.SteamLeaderboardEntries_t hSteamLeaderboardEntries, int index, out global::Steamworks.LeaderboardEntry_t pLeaderboardEntry, int[] pDetails, int cDetailsMax)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUserStats_GetDownloadedLeaderboardEntry(hSteamLeaderboardEntries, index, out pLeaderboardEntry, pDetails, cDetailsMax);
		}

		public static global::Steamworks.SteamAPICall_t UploadLeaderboardScore(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard, global::Steamworks.ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, int[] pScoreDetails, int cScoreDetailsCount)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUserStats_UploadLeaderboardScore(hSteamLeaderboard, eLeaderboardUploadScoreMethod, nScore, pScoreDetails, cScoreDetailsCount);
		}

		public static global::Steamworks.SteamAPICall_t AttachLeaderboardUGC(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard, global::Steamworks.UGCHandle_t hUGC)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUserStats_AttachLeaderboardUGC(hSteamLeaderboard, hUGC);
		}

		public static global::Steamworks.SteamAPICall_t GetNumberOfCurrentPlayers()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUserStats_GetNumberOfCurrentPlayers();
		}

		public static global::Steamworks.SteamAPICall_t RequestGlobalAchievementPercentages()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUserStats_RequestGlobalAchievementPercentages();
		}

		public static int GetMostAchievedAchievementInfo(out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal((int)unNameBufLen);
			int num = global::Steamworks.NativeMethods.ISteamUserStats_GetMostAchievedAchievementInfo(intPtr, unNameBufLen, out pflPercent, out pbAchieved);
			pchName = ((num == -1) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return num;
		}

		public static int GetNextMostAchievedAchievementInfo(int iIteratorPrevious, out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal((int)unNameBufLen);
			int num = global::Steamworks.NativeMethods.ISteamUserStats_GetNextMostAchievedAchievementInfo(iIteratorPrevious, intPtr, unNameBufLen, out pflPercent, out pbAchieved);
			pchName = ((num == -1) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return num;
		}

		public static bool GetAchievementAchievedPercent(string pchName, out float pflPercent)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetAchievementAchievedPercent(utf8StringHandle, out pflPercent);
			}
			return result;
		}

		public static global::Steamworks.SteamAPICall_t RequestGlobalStats(int nHistoryDays)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUserStats_RequestGlobalStats(nHistoryDays);
		}

		public static bool GetGlobalStat(string pchStatName, out long pData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetGlobalStat(utf8StringHandle, out pData);
			}
			return result;
		}

		public static bool GetGlobalStat(string pchStatName, out double pData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetGlobalStat_(utf8StringHandle, out pData);
			}
			return result;
		}

		public static int GetGlobalStatHistory(string pchStatName, long[] pData, uint cubData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			int result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetGlobalStatHistory(utf8StringHandle, pData, cubData);
			}
			return result;
		}

		public static int GetGlobalStatHistory(string pchStatName, double[] pData, uint cubData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			int result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = global::Steamworks.NativeMethods.ISteamUserStats_GetGlobalStatHistory_(utf8StringHandle, pData, cubData);
			}
			return result;
		}
	}
}
