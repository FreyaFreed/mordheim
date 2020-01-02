using System;

namespace Steamworks
{
	public static class SteamGameServer
	{
		public static bool InitGameServer(uint unIP, ushort usGamePort, ushort usQueryPort, uint unFlags, global::Steamworks.AppId_t nGameAppId, string pchVersionString)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVersionString))
			{
				result = global::Steamworks.NativeMethods.ISteamGameServer_InitGameServer(unIP, usGamePort, usQueryPort, unFlags, nGameAppId, utf8StringHandle);
			}
			return result;
		}

		public static void SetProduct(string pszProduct)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszProduct))
			{
				global::Steamworks.NativeMethods.ISteamGameServer_SetProduct(utf8StringHandle);
			}
		}

		public static void SetGameDescription(string pszGameDescription)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszGameDescription))
			{
				global::Steamworks.NativeMethods.ISteamGameServer_SetGameDescription(utf8StringHandle);
			}
		}

		public static void SetModDir(string pszModDir)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszModDir))
			{
				global::Steamworks.NativeMethods.ISteamGameServer_SetModDir(utf8StringHandle);
			}
		}

		public static void SetDedicatedServer(bool bDedicated)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_SetDedicatedServer(bDedicated);
		}

		public static void LogOn(string pszToken)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszToken))
			{
				global::Steamworks.NativeMethods.ISteamGameServer_LogOn(utf8StringHandle);
			}
		}

		public static void LogOnAnonymous()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_LogOnAnonymous();
		}

		public static void LogOff()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_LogOff();
		}

		public static bool BLoggedOn()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServer_BLoggedOn();
		}

		public static bool BSecure()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServer_BSecure();
		}

		public static global::Steamworks.CSteamID GetSteamID()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamGameServer_GetSteamID();
		}

		public static bool WasRestartRequested()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServer_WasRestartRequested();
		}

		public static void SetMaxPlayerCount(int cPlayersMax)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_SetMaxPlayerCount(cPlayersMax);
		}

		public static void SetBotPlayerCount(int cBotplayers)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_SetBotPlayerCount(cBotplayers);
		}

		public static void SetServerName(string pszServerName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszServerName))
			{
				global::Steamworks.NativeMethods.ISteamGameServer_SetServerName(utf8StringHandle);
			}
		}

		public static void SetMapName(string pszMapName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszMapName))
			{
				global::Steamworks.NativeMethods.ISteamGameServer_SetMapName(utf8StringHandle);
			}
		}

		public static void SetPasswordProtected(bool bPasswordProtected)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_SetPasswordProtected(bPasswordProtected);
		}

		public static void SetSpectatorPort(ushort unSpectatorPort)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_SetSpectatorPort(unSpectatorPort);
		}

		public static void SetSpectatorServerName(string pszSpectatorServerName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszSpectatorServerName))
			{
				global::Steamworks.NativeMethods.ISteamGameServer_SetSpectatorServerName(utf8StringHandle);
			}
		}

		public static void ClearAllKeyValues()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_ClearAllKeyValues();
		}

		public static void SetKeyValue(string pKey, string pValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pKey))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pValue))
				{
					global::Steamworks.NativeMethods.ISteamGameServer_SetKeyValue(utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		public static void SetGameTags(string pchGameTags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchGameTags))
			{
				global::Steamworks.NativeMethods.ISteamGameServer_SetGameTags(utf8StringHandle);
			}
		}

		public static void SetGameData(string pchGameData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchGameData))
			{
				global::Steamworks.NativeMethods.ISteamGameServer_SetGameData(utf8StringHandle);
			}
		}

		public static void SetRegion(string pszRegion)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszRegion))
			{
				global::Steamworks.NativeMethods.ISteamGameServer_SetRegion(utf8StringHandle);
			}
		}

		public static bool SendUserConnectAndAuthenticate(uint unIPClient, byte[] pvAuthBlob, uint cubAuthBlobSize, out global::Steamworks.CSteamID pSteamIDUser)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServer_SendUserConnectAndAuthenticate(unIPClient, pvAuthBlob, cubAuthBlobSize, out pSteamIDUser);
		}

		public static global::Steamworks.CSteamID CreateUnauthenticatedUserConnection()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamGameServer_CreateUnauthenticatedUserConnection();
		}

		public static void SendUserDisconnect(global::Steamworks.CSteamID steamIDUser)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_SendUserDisconnect(steamIDUser);
		}

		public static bool BUpdateUserData(global::Steamworks.CSteamID steamIDUser, string pchPlayerName, uint uScore)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchPlayerName))
			{
				result = global::Steamworks.NativeMethods.ISteamGameServer_BUpdateUserData(steamIDUser, utf8StringHandle, uScore);
			}
			return result;
		}

		public static global::Steamworks.HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return (global::Steamworks.HAuthTicket)global::Steamworks.NativeMethods.ISteamGameServer_GetAuthSessionTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		public static global::Steamworks.EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, global::Steamworks.CSteamID steamID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServer_BeginAuthSession(pAuthTicket, cbAuthTicket, steamID);
		}

		public static void EndAuthSession(global::Steamworks.CSteamID steamID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_EndAuthSession(steamID);
		}

		public static void CancelAuthTicket(global::Steamworks.HAuthTicket hAuthTicket)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_CancelAuthTicket(hAuthTicket);
		}

		public static global::Steamworks.EUserHasLicenseForAppResult UserHasLicenseForApp(global::Steamworks.CSteamID steamID, global::Steamworks.AppId_t appID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServer_UserHasLicenseForApp(steamID, appID);
		}

		public static bool RequestUserGroupStatus(global::Steamworks.CSteamID steamIDUser, global::Steamworks.CSteamID steamIDGroup)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServer_RequestUserGroupStatus(steamIDUser, steamIDGroup);
		}

		public static void GetGameplayStats()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_GetGameplayStats();
		}

		public static global::Steamworks.SteamAPICall_t GetServerReputation()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamGameServer_GetServerReputation();
		}

		public static uint GetPublicIP()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServer_GetPublicIP();
		}

		public static bool HandleIncomingPacket(byte[] pData, int cbData, uint srcIP, ushort srcPort)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServer_HandleIncomingPacket(pData, cbData, srcIP, srcPort);
		}

		public static int GetNextOutgoingPacket(byte[] pOut, int cbMaxOut, out uint pNetAdr, out ushort pPort)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServer_GetNextOutgoingPacket(pOut, cbMaxOut, out pNetAdr, out pPort);
		}

		public static void EnableHeartbeats(bool bActive)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_EnableHeartbeats(bActive);
		}

		public static void SetHeartbeatInterval(int iHeartbeatInterval)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_SetHeartbeatInterval(iHeartbeatInterval);
		}

		public static void ForceHeartbeat()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServer_ForceHeartbeat();
		}

		public static global::Steamworks.SteamAPICall_t AssociateWithClan(global::Steamworks.CSteamID steamIDClan)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamGameServer_AssociateWithClan(steamIDClan);
		}

		public static global::Steamworks.SteamAPICall_t ComputeNewPlayerCompatibility(global::Steamworks.CSteamID steamIDNewPlayer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamGameServer_ComputeNewPlayerCompatibility(steamIDNewPlayer);
		}
	}
}
