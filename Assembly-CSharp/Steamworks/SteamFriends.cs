using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamFriends
	{
		public static string GetPersonaName()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamFriends_GetPersonaName());
		}

		public static global::Steamworks.SteamAPICall_t SetPersonaName(string pchPersonaName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchPersonaName))
			{
				result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamFriends_SetPersonaName(utf8StringHandle);
			}
			return result;
		}

		public static global::Steamworks.EPersonaState GetPersonaState()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetPersonaState();
		}

		public static int GetFriendCount(global::Steamworks.EFriendFlags iFriendFlags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetFriendCount(iFriendFlags);
		}

		public static global::Steamworks.CSteamID GetFriendByIndex(int iFriend, global::Steamworks.EFriendFlags iFriendFlags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamFriends_GetFriendByIndex(iFriend, iFriendFlags);
		}

		public static global::Steamworks.EFriendRelationship GetFriendRelationship(global::Steamworks.CSteamID steamIDFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetFriendRelationship(steamIDFriend);
		}

		public static global::Steamworks.EPersonaState GetFriendPersonaState(global::Steamworks.CSteamID steamIDFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetFriendPersonaState(steamIDFriend);
		}

		public static string GetFriendPersonaName(global::Steamworks.CSteamID steamIDFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamFriends_GetFriendPersonaName(steamIDFriend));
		}

		public static bool GetFriendGamePlayed(global::Steamworks.CSteamID steamIDFriend, out global::Steamworks.FriendGameInfo_t pFriendGameInfo)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetFriendGamePlayed(steamIDFriend, out pFriendGameInfo);
		}

		public static string GetFriendPersonaNameHistory(global::Steamworks.CSteamID steamIDFriend, int iPersonaName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamFriends_GetFriendPersonaNameHistory(steamIDFriend, iPersonaName));
		}

		public static int GetFriendSteamLevel(global::Steamworks.CSteamID steamIDFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetFriendSteamLevel(steamIDFriend);
		}

		public static string GetPlayerNickname(global::Steamworks.CSteamID steamIDPlayer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamFriends_GetPlayerNickname(steamIDPlayer));
		}

		public static int GetFriendsGroupCount()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetFriendsGroupCount();
		}

		public static global::Steamworks.FriendsGroupID_t GetFriendsGroupIDByIndex(int iFG)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.FriendsGroupID_t)global::Steamworks.NativeMethods.ISteamFriends_GetFriendsGroupIDByIndex(iFG);
		}

		public static string GetFriendsGroupName(global::Steamworks.FriendsGroupID_t friendsGroupID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamFriends_GetFriendsGroupName(friendsGroupID));
		}

		public static int GetFriendsGroupMembersCount(global::Steamworks.FriendsGroupID_t friendsGroupID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetFriendsGroupMembersCount(friendsGroupID);
		}

		public static void GetFriendsGroupMembersList(global::Steamworks.FriendsGroupID_t friendsGroupID, global::Steamworks.CSteamID[] pOutSteamIDMembers, int nMembersCount)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamFriends_GetFriendsGroupMembersList(friendsGroupID, pOutSteamIDMembers, nMembersCount);
		}

		public static bool HasFriend(global::Steamworks.CSteamID steamIDFriend, global::Steamworks.EFriendFlags iFriendFlags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_HasFriend(steamIDFriend, iFriendFlags);
		}

		public static int GetClanCount()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetClanCount();
		}

		public static global::Steamworks.CSteamID GetClanByIndex(int iClan)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamFriends_GetClanByIndex(iClan);
		}

		public static string GetClanName(global::Steamworks.CSteamID steamIDClan)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamFriends_GetClanName(steamIDClan));
		}

		public static string GetClanTag(global::Steamworks.CSteamID steamIDClan)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamFriends_GetClanTag(steamIDClan));
		}

		public static bool GetClanActivityCounts(global::Steamworks.CSteamID steamIDClan, out int pnOnline, out int pnInGame, out int pnChatting)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetClanActivityCounts(steamIDClan, out pnOnline, out pnInGame, out pnChatting);
		}

		public static global::Steamworks.SteamAPICall_t DownloadClanActivityCounts(global::Steamworks.CSteamID[] psteamIDClans, int cClansToRequest)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamFriends_DownloadClanActivityCounts(psteamIDClans, cClansToRequest);
		}

		public static int GetFriendCountFromSource(global::Steamworks.CSteamID steamIDSource)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetFriendCountFromSource(steamIDSource);
		}

		public static global::Steamworks.CSteamID GetFriendFromSourceByIndex(global::Steamworks.CSteamID steamIDSource, int iFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamFriends_GetFriendFromSourceByIndex(steamIDSource, iFriend);
		}

		public static bool IsUserInSource(global::Steamworks.CSteamID steamIDUser, global::Steamworks.CSteamID steamIDSource)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_IsUserInSource(steamIDUser, steamIDSource);
		}

		public static void SetInGameVoiceSpeaking(global::Steamworks.CSteamID steamIDUser, bool bSpeaking)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamFriends_SetInGameVoiceSpeaking(steamIDUser, bSpeaking);
		}

		public static void ActivateGameOverlay(string pchDialog)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchDialog))
			{
				global::Steamworks.NativeMethods.ISteamFriends_ActivateGameOverlay(utf8StringHandle);
			}
		}

		public static void ActivateGameOverlayToUser(string pchDialog, global::Steamworks.CSteamID steamID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchDialog))
			{
				global::Steamworks.NativeMethods.ISteamFriends_ActivateGameOverlayToUser(utf8StringHandle, steamID);
			}
		}

		public static void ActivateGameOverlayToWebPage(string pchURL)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchURL))
			{
				global::Steamworks.NativeMethods.ISteamFriends_ActivateGameOverlayToWebPage(utf8StringHandle);
			}
		}

		public static void ActivateGameOverlayToStore(global::Steamworks.AppId_t nAppID, global::Steamworks.EOverlayToStoreFlag eFlag)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamFriends_ActivateGameOverlayToStore(nAppID, eFlag);
		}

		public static void SetPlayedWith(global::Steamworks.CSteamID steamIDUserPlayedWith)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamFriends_SetPlayedWith(steamIDUserPlayedWith);
		}

		public static void ActivateGameOverlayInviteDialog(global::Steamworks.CSteamID steamIDLobby)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamFriends_ActivateGameOverlayInviteDialog(steamIDLobby);
		}

		public static int GetSmallFriendAvatar(global::Steamworks.CSteamID steamIDFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetSmallFriendAvatar(steamIDFriend);
		}

		public static int GetMediumFriendAvatar(global::Steamworks.CSteamID steamIDFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetMediumFriendAvatar(steamIDFriend);
		}

		public static int GetLargeFriendAvatar(global::Steamworks.CSteamID steamIDFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetLargeFriendAvatar(steamIDFriend);
		}

		public static bool RequestUserInformation(global::Steamworks.CSteamID steamIDUser, bool bRequireNameOnly)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_RequestUserInformation(steamIDUser, bRequireNameOnly);
		}

		public static global::Steamworks.SteamAPICall_t RequestClanOfficerList(global::Steamworks.CSteamID steamIDClan)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamFriends_RequestClanOfficerList(steamIDClan);
		}

		public static global::Steamworks.CSteamID GetClanOwner(global::Steamworks.CSteamID steamIDClan)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamFriends_GetClanOwner(steamIDClan);
		}

		public static int GetClanOfficerCount(global::Steamworks.CSteamID steamIDClan)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetClanOfficerCount(steamIDClan);
		}

		public static global::Steamworks.CSteamID GetClanOfficerByIndex(global::Steamworks.CSteamID steamIDClan, int iOfficer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamFriends_GetClanOfficerByIndex(steamIDClan, iOfficer);
		}

		public static uint GetUserRestrictions()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetUserRestrictions();
		}

		public static bool SetRichPresence(string pchKey, string pchValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchValue))
				{
					result = global::Steamworks.NativeMethods.ISteamFriends_SetRichPresence(utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		public static void ClearRichPresence()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamFriends_ClearRichPresence();
		}

		public static string GetFriendRichPresence(global::Steamworks.CSteamID steamIDFriend, string pchKey)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			string result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
			{
				result = global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamFriends_GetFriendRichPresence(steamIDFriend, utf8StringHandle));
			}
			return result;
		}

		public static int GetFriendRichPresenceKeyCount(global::Steamworks.CSteamID steamIDFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetFriendRichPresenceKeyCount(steamIDFriend);
		}

		public static string GetFriendRichPresenceKeyByIndex(global::Steamworks.CSteamID steamIDFriend, int iKey)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamFriends_GetFriendRichPresenceKeyByIndex(steamIDFriend, iKey));
		}

		public static void RequestFriendRichPresence(global::Steamworks.CSteamID steamIDFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamFriends_RequestFriendRichPresence(steamIDFriend);
		}

		public static bool InviteUserToGame(global::Steamworks.CSteamID steamIDFriend, string pchConnectString)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchConnectString))
			{
				result = global::Steamworks.NativeMethods.ISteamFriends_InviteUserToGame(steamIDFriend, utf8StringHandle);
			}
			return result;
		}

		public static int GetCoplayFriendCount()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetCoplayFriendCount();
		}

		public static global::Steamworks.CSteamID GetCoplayFriend(int iCoplayFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamFriends_GetCoplayFriend(iCoplayFriend);
		}

		public static int GetFriendCoplayTime(global::Steamworks.CSteamID steamIDFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetFriendCoplayTime(steamIDFriend);
		}

		public static global::Steamworks.AppId_t GetFriendCoplayGame(global::Steamworks.CSteamID steamIDFriend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.AppId_t)global::Steamworks.NativeMethods.ISteamFriends_GetFriendCoplayGame(steamIDFriend);
		}

		public static global::Steamworks.SteamAPICall_t JoinClanChatRoom(global::Steamworks.CSteamID steamIDClan)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamFriends_JoinClanChatRoom(steamIDClan);
		}

		public static bool LeaveClanChatRoom(global::Steamworks.CSteamID steamIDClan)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_LeaveClanChatRoom(steamIDClan);
		}

		public static int GetClanChatMemberCount(global::Steamworks.CSteamID steamIDClan)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_GetClanChatMemberCount(steamIDClan);
		}

		public static global::Steamworks.CSteamID GetChatMemberByIndex(global::Steamworks.CSteamID steamIDClan, int iUser)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.CSteamID)global::Steamworks.NativeMethods.ISteamFriends_GetChatMemberByIndex(steamIDClan, iUser);
		}

		public static bool SendClanChatMessage(global::Steamworks.CSteamID steamIDClanChat, string pchText)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchText))
			{
				result = global::Steamworks.NativeMethods.ISteamFriends_SendClanChatMessage(steamIDClanChat, utf8StringHandle);
			}
			return result;
		}

		public static int GetClanChatMessage(global::Steamworks.CSteamID steamIDClanChat, int iMessage, out string prgchText, int cchTextMax, out global::Steamworks.EChatEntryType peChatEntryType, out global::Steamworks.CSteamID psteamidChatter)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(cchTextMax);
			int num = global::Steamworks.NativeMethods.ISteamFriends_GetClanChatMessage(steamIDClanChat, iMessage, intPtr, cchTextMax, out peChatEntryType, out psteamidChatter);
			prgchText = ((num == 0) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return num;
		}

		public static bool IsClanChatAdmin(global::Steamworks.CSteamID steamIDClanChat, global::Steamworks.CSteamID steamIDUser)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_IsClanChatAdmin(steamIDClanChat, steamIDUser);
		}

		public static bool IsClanChatWindowOpenInSteam(global::Steamworks.CSteamID steamIDClanChat)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_IsClanChatWindowOpenInSteam(steamIDClanChat);
		}

		public static bool OpenClanChatWindowInSteam(global::Steamworks.CSteamID steamIDClanChat)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_OpenClanChatWindowInSteam(steamIDClanChat);
		}

		public static bool CloseClanChatWindowInSteam(global::Steamworks.CSteamID steamIDClanChat)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_CloseClanChatWindowInSteam(steamIDClanChat);
		}

		public static bool SetListenForFriendsMessages(bool bInterceptEnabled)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamFriends_SetListenForFriendsMessages(bInterceptEnabled);
		}

		public static bool ReplyToFriendMessage(global::Steamworks.CSteamID steamIDFriend, string pchMsgToSend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchMsgToSend))
			{
				result = global::Steamworks.NativeMethods.ISteamFriends_ReplyToFriendMessage(steamIDFriend, utf8StringHandle);
			}
			return result;
		}

		public static int GetFriendMessage(global::Steamworks.CSteamID steamIDFriend, int iMessageID, out string pvData, int cubData, out global::Steamworks.EChatEntryType peChatEntryType)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(cubData);
			int num = global::Steamworks.NativeMethods.ISteamFriends_GetFriendMessage(steamIDFriend, iMessageID, intPtr, cubData, out peChatEntryType);
			pvData = ((num == 0) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return num;
		}

		public static global::Steamworks.SteamAPICall_t GetFollowerCount(global::Steamworks.CSteamID steamID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamFriends_GetFollowerCount(steamID);
		}

		public static global::Steamworks.SteamAPICall_t IsFollowing(global::Steamworks.CSteamID steamID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamFriends_IsFollowing(steamID);
		}

		public static global::Steamworks.SteamAPICall_t EnumerateFollowingList(uint unStartIndex)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamFriends_EnumerateFollowingList(unStartIndex);
		}
	}
}
