using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamUser
	{
		public static global::Steamworks.HSteamUser GetHSteamUser()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.HSteamUser)global::Steamworks.NativeMethods.ISteamUser_GetHSteamUser();
		}

		public static bool BLoggedOn()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_BLoggedOn();
		}

		public static global::Steamworks.CSteamID GetSteamID()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamUser_GetSteamID();
		}

		public static int InitiateGameConnection(byte[] pAuthBlob, int cbMaxAuthBlob, global::Steamworks.CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, bool bSecure)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_InitiateGameConnection(pAuthBlob, cbMaxAuthBlob, steamIDGameServer, unIPServer, usPortServer, bSecure);
		}

		public static void TerminateGameConnection(uint unIPServer, ushort usPortServer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamUser_TerminateGameConnection(unIPServer, usPortServer);
		}

		public static void TrackAppUsageEvent(global::Steamworks.CGameID gameID, int eAppUsageEvent, string pchExtraInfo = "")
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchExtraInfo))
			{
				global::Steamworks.NativeMethods.ISteamUser_TrackAppUsageEvent(gameID, eAppUsageEvent, utf8StringHandle);
			}
		}

		public static bool GetUserDataFolder(out string pchBuffer, int cubBuffer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(cubBuffer);
			bool flag = global::Steamworks.NativeMethods.ISteamUser_GetUserDataFolder(intPtr, cubBuffer);
			pchBuffer = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		public static void StartVoiceRecording()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamUser_StartVoiceRecording();
		}

		public static void StopVoiceRecording()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamUser_StopVoiceRecording();
		}

		public static global::Steamworks.EVoiceResult GetAvailableVoice(out uint pcbCompressed, out uint pcbUncompressed, uint nUncompressedVoiceDesiredSampleRate)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_GetAvailableVoice(out pcbCompressed, out pcbUncompressed, nUncompressedVoiceDesiredSampleRate);
		}

		public static global::Steamworks.EVoiceResult GetVoice(bool bWantCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, bool bWantUncompressed, byte[] pUncompressedDestBuffer, uint cbUncompressedDestBufferSize, out uint nUncompressBytesWritten, uint nUncompressedVoiceDesiredSampleRate)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_GetVoice(bWantCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, bWantUncompressed, pUncompressedDestBuffer, cbUncompressedDestBufferSize, out nUncompressBytesWritten, nUncompressedVoiceDesiredSampleRate);
		}

		public static global::Steamworks.EVoiceResult DecompressVoice(byte[] pCompressed, uint cbCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_DecompressVoice(pCompressed, cbCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, nDesiredSampleRate);
		}

		public static uint GetVoiceOptimalSampleRate()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_GetVoiceOptimalSampleRate();
		}

		public static global::Steamworks.HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.HAuthTicket)global::Steamworks.NativeMethods.ISteamUser_GetAuthSessionTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		public static global::Steamworks.EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, global::Steamworks.CSteamID steamID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_BeginAuthSession(pAuthTicket, cbAuthTicket, steamID);
		}

		public static void EndAuthSession(global::Steamworks.CSteamID steamID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamUser_EndAuthSession(steamID);
		}

		public static void CancelAuthTicket(global::Steamworks.HAuthTicket hAuthTicket)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamUser_CancelAuthTicket(hAuthTicket);
		}

		public static global::Steamworks.EUserHasLicenseForAppResult UserHasLicenseForApp(global::Steamworks.CSteamID steamID, global::Steamworks.AppId_t appID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_UserHasLicenseForApp(steamID, appID);
		}

		public static bool BIsBehindNAT()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_BIsBehindNAT();
		}

		public static void AdvertiseGame(global::Steamworks.CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamUser_AdvertiseGame(steamIDGameServer, unIPServer, usPortServer);
		}

		public static global::Steamworks.SteamAPICall_t RequestEncryptedAppTicket(byte[] pDataToInclude, int cbDataToInclude)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUser_RequestEncryptedAppTicket(pDataToInclude, cbDataToInclude);
		}

		public static bool GetEncryptedAppTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_GetEncryptedAppTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		public static int GetGameBadgeLevel(int nSeries, bool bFoil)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_GetGameBadgeLevel(nSeries, bFoil);
		}

		public static int GetPlayerSteamLevel()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUser_GetPlayerSteamLevel();
		}

		public static global::Steamworks.SteamAPICall_t RequestStoreAuthURL(string pchRedirectURL)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchRedirectURL))
			{
				result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUser_RequestStoreAuthURL(utf8StringHandle);
			}
			return result;
		}
	}
}
