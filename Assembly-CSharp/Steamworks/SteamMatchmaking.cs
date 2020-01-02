using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamMatchmaking
	{
		public static int GetFavoriteGameCount()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_GetFavoriteGameCount();
		}

		public static bool GetFavoriteGame(int iGame, out global::Steamworks.AppId_t pnAppID, out uint pnIP, out ushort pnConnPort, out ushort pnQueryPort, out uint punFlags, out uint pRTime32LastPlayedOnServer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_GetFavoriteGame(iGame, out pnAppID, out pnIP, out pnConnPort, out pnQueryPort, out punFlags, out pRTime32LastPlayedOnServer);
		}

		public static int AddFavoriteGame(global::Steamworks.AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags, uint rTime32LastPlayedOnServer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_AddFavoriteGame(nAppID, nIP, nConnPort, nQueryPort, unFlags, rTime32LastPlayedOnServer);
		}

		public static bool RemoveFavoriteGame(global::Steamworks.AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_RemoveFavoriteGame(nAppID, nIP, nConnPort, nQueryPort, unFlags);
		}

		public static global::Steamworks.SteamAPICall_t RequestLobbyList()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamMatchmaking_RequestLobbyList();
		}

		public static void AddRequestLobbyListStringFilter(string pchKeyToMatch, string pchValueToMatch, global::Steamworks.ELobbyComparison eComparisonType)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKeyToMatch))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchValueToMatch))
				{
					global::Steamworks.NativeMethods.ISteamMatchmaking_AddRequestLobbyListStringFilter(utf8StringHandle, utf8StringHandle2, eComparisonType);
				}
			}
		}

		public static void AddRequestLobbyListNumericalFilter(string pchKeyToMatch, int nValueToMatch, global::Steamworks.ELobbyComparison eComparisonType)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKeyToMatch))
			{
				global::Steamworks.NativeMethods.ISteamMatchmaking_AddRequestLobbyListNumericalFilter(utf8StringHandle, nValueToMatch, eComparisonType);
			}
		}

		public static void AddRequestLobbyListNearValueFilter(string pchKeyToMatch, int nValueToBeCloseTo)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKeyToMatch))
			{
				global::Steamworks.NativeMethods.ISteamMatchmaking_AddRequestLobbyListNearValueFilter(utf8StringHandle, nValueToBeCloseTo);
			}
		}

		public static void AddRequestLobbyListFilterSlotsAvailable(int nSlotsAvailable)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(nSlotsAvailable);
		}

		public static void AddRequestLobbyListDistanceFilter(global::Steamworks.ELobbyDistanceFilter eLobbyDistanceFilter)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamMatchmaking_AddRequestLobbyListDistanceFilter(eLobbyDistanceFilter);
		}

		public static void AddRequestLobbyListResultCountFilter(int cMaxResults)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamMatchmaking_AddRequestLobbyListResultCountFilter(cMaxResults);
		}

		public static void AddRequestLobbyListCompatibleMembersFilter(global::Steamworks.CSteamID steamIDLobby)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(steamIDLobby);
		}

		public static global::Steamworks.CSteamID GetLobbyByIndex(int iLobby)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamMatchmaking_GetLobbyByIndex(iLobby);
		}

		public static global::Steamworks.SteamAPICall_t CreateLobby(global::Steamworks.ELobbyType eLobbyType, int cMaxMembers)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamMatchmaking_CreateLobby(eLobbyType, cMaxMembers);
		}

		public static global::Steamworks.SteamAPICall_t JoinLobby(global::Steamworks.CSteamID steamIDLobby)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamMatchmaking_JoinLobby(steamIDLobby);
		}

		public static void LeaveLobby(global::Steamworks.CSteamID steamIDLobby)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamMatchmaking_LeaveLobby(steamIDLobby);
		}

		public static bool InviteUserToLobby(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.CSteamID steamIDInvitee)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_InviteUserToLobby(steamIDLobby, steamIDInvitee);
		}

		public static int GetNumLobbyMembers(global::Steamworks.CSteamID steamIDLobby)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_GetNumLobbyMembers(steamIDLobby);
		}

		public static global::Steamworks.CSteamID GetLobbyMemberByIndex(global::Steamworks.CSteamID steamIDLobby, int iMember)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamMatchmaking_GetLobbyMemberByIndex(steamIDLobby, iMember);
		}

		public static string GetLobbyData(global::Steamworks.CSteamID steamIDLobby, string pchKey)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			string result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
			{
				result = global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamMatchmaking_GetLobbyData(steamIDLobby, utf8StringHandle));
			}
			return result;
		}

		public static bool SetLobbyData(global::Steamworks.CSteamID steamIDLobby, string pchKey, string pchValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchValue))
				{
					result = global::Steamworks.NativeMethods.ISteamMatchmaking_SetLobbyData(steamIDLobby, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		public static int GetLobbyDataCount(global::Steamworks.CSteamID steamIDLobby)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_GetLobbyDataCount(steamIDLobby);
		}

		public static bool GetLobbyDataByIndex(global::Steamworks.CSteamID steamIDLobby, int iLobbyData, out string pchKey, int cchKeyBufferSize, out string pchValue, int cchValueBufferSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(cchKeyBufferSize);
			global::System.IntPtr intPtr2 = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(cchValueBufferSize);
			bool flag = global::Steamworks.NativeMethods.ISteamMatchmaking_GetLobbyDataByIndex(steamIDLobby, iLobbyData, intPtr, cchKeyBufferSize, intPtr2, cchValueBufferSize);
			pchKey = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			pchValue = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr2));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		public static bool DeleteLobbyData(global::Steamworks.CSteamID steamIDLobby, string pchKey)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
			{
				result = global::Steamworks.NativeMethods.ISteamMatchmaking_DeleteLobbyData(steamIDLobby, utf8StringHandle);
			}
			return result;
		}

		public static string GetLobbyMemberData(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.CSteamID steamIDUser, string pchKey)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			string result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
			{
				result = global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamMatchmaking_GetLobbyMemberData(steamIDLobby, steamIDUser, utf8StringHandle));
			}
			return result;
		}

		public static void SetLobbyMemberData(global::Steamworks.CSteamID steamIDLobby, string pchKey, string pchValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchValue))
				{
					global::Steamworks.NativeMethods.ISteamMatchmaking_SetLobbyMemberData(steamIDLobby, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		public static bool SendLobbyChatMsg(global::Steamworks.CSteamID steamIDLobby, byte[] pvMsgBody, int cubMsgBody)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_SendLobbyChatMsg(steamIDLobby, pvMsgBody, cubMsgBody);
		}

		public static int GetLobbyChatEntry(global::Steamworks.CSteamID steamIDLobby, int iChatID, out global::Steamworks.CSteamID pSteamIDUser, byte[] pvData, int cubData, out global::Steamworks.EChatEntryType peChatEntryType)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_GetLobbyChatEntry(steamIDLobby, iChatID, out pSteamIDUser, pvData, cubData, out peChatEntryType);
		}

		public static bool RequestLobbyData(global::Steamworks.CSteamID steamIDLobby)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_RequestLobbyData(steamIDLobby);
		}

		public static void SetLobbyGameServer(global::Steamworks.CSteamID steamIDLobby, uint unGameServerIP, ushort unGameServerPort, global::Steamworks.CSteamID steamIDGameServer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamMatchmaking_SetLobbyGameServer(steamIDLobby, unGameServerIP, unGameServerPort, steamIDGameServer);
		}

		public static bool GetLobbyGameServer(global::Steamworks.CSteamID steamIDLobby, out uint punGameServerIP, out ushort punGameServerPort, out global::Steamworks.CSteamID psteamIDGameServer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_GetLobbyGameServer(steamIDLobby, out punGameServerIP, out punGameServerPort, out psteamIDGameServer);
		}

		public static bool SetLobbyMemberLimit(global::Steamworks.CSteamID steamIDLobby, int cMaxMembers)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_SetLobbyMemberLimit(steamIDLobby, cMaxMembers);
		}

		public static int GetLobbyMemberLimit(global::Steamworks.CSteamID steamIDLobby)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_GetLobbyMemberLimit(steamIDLobby);
		}

		public static bool SetLobbyType(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.ELobbyType eLobbyType)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_SetLobbyType(steamIDLobby, eLobbyType);
		}

		public static bool SetLobbyJoinable(global::Steamworks.CSteamID steamIDLobby, bool bLobbyJoinable)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_SetLobbyJoinable(steamIDLobby, bLobbyJoinable);
		}

		public static global::Steamworks.CSteamID GetLobbyOwner(global::Steamworks.CSteamID steamIDLobby)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamMatchmaking_GetLobbyOwner(steamIDLobby);
		}

		public static bool SetLobbyOwner(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.CSteamID steamIDNewOwner)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_SetLobbyOwner(steamIDLobby, steamIDNewOwner);
		}

		public static bool SetLinkedLobby(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.CSteamID steamIDLobbyDependent)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMatchmaking_SetLinkedLobby(steamIDLobby, steamIDLobbyDependent);
		}
	}
}
