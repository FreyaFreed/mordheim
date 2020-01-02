using System;
using System.Collections;
using Steamworks;
using UnityEngine;

public class Matchmaking
{
	public Matchmaking()
	{
		this.StateChangeCb = global::Steamworks.Callback<global::Steamworks.PersonaStateChange_t>.Create(new global::Steamworks.Callback<global::Steamworks.PersonaStateChange_t>.DispatchDelegate(this.OnPersonaStateChange));
		this.LobbyDataUpdateCb = global::Steamworks.Callback<global::Steamworks.LobbyDataUpdate_t>.Create(new global::Steamworks.Callback<global::Steamworks.LobbyDataUpdate_t>.DispatchDelegate(this.OnLobbyDataUpdate));
		this.LobbyChatUpdateCb = global::Steamworks.Callback<global::Steamworks.LobbyChatUpdate_t>.Create(new global::Steamworks.Callback<global::Steamworks.LobbyChatUpdate_t>.DispatchDelegate(this.OnLobbyChatUpdate));
		this.LobbyKickedCb = global::Steamworks.Callback<global::Steamworks.LobbyKicked_t>.Create(new global::Steamworks.Callback<global::Steamworks.LobbyKicked_t>.DispatchDelegate(this.OnLobbyKicked));
		this.LobbyJoinRequestCb = global::Steamworks.Callback<global::Steamworks.GameLobbyJoinRequested_t>.Create(new global::Steamworks.Callback<global::Steamworks.GameLobbyJoinRequested_t>.DispatchDelegate(this.OnLobbyJoinRequest));
		this.steamCbLobbyCreated = new global::Steamworks.CallResult<global::Steamworks.LobbyCreated_t>(null);
		this.steamCbLobbyEnter = new global::Steamworks.CallResult<global::Steamworks.LobbyEnter_t>(null);
		this.steamCbLobbyMatchList = new global::Steamworks.CallResult<global::Steamworks.LobbyMatchList_t>(null);
	}

	public void CreateLobby(string name, global::Steamworks.ELobbyType type)
	{
		if (!this.steamCbLobbyCreated.IsActive())
		{
			global::PandoraDebug.LogInfo("Create Lobby = " + type, "HEPHAESTUS-STEAMWORKS", null);
			if (type != global::Steamworks.ELobbyType.k_ELobbyTypePrivate)
			{
				global::Steamworks.SteamAPICall_t hAPICall = global::Steamworks.SteamMatchmaking.CreateLobby((type != global::Steamworks.ELobbyType.k_ELobbyTypeInvisible) ? type : global::Steamworks.ELobbyType.k_ELobbyTypeFriendsOnly, 2);
				this.steamCbLobbyCreated.Set(hAPICall, new global::Steamworks.CallResult<global::Steamworks.LobbyCreated_t>.APIDispatchDelegate(this.OnLobbyCreated));
				this.lobby = new global::Lobby();
				this.lobby.name = name;
				this.lobby.SetPrivacy(type);
			}
			else
			{
				this.lobby = new global::Lobby();
				this.lobby.name = name;
				this.lobby.SetPrivacy(type);
				this.lobby.id = 0UL;
				this.lobby.version = "1.4.4.4";
				global::PandoraSingleton<global::Hephaestus>.Instance.UpdateLobby(this.lobby);
				global::PandoraSingleton<global::Hephaestus>.Instance.OnCreateLobby(this.lobby.id, true);
			}
		}
	}

	private void OnLobbyCreated(global::Steamworks.LobbyCreated_t callback, bool failure)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"OnLobbyCreated Result = ",
			callback.m_eResult,
			" FAIL = ",
			failure,
			"LobbyId = ",
			this.lobby.id
		}), "HEPHAESTUS-STEAMWORKS", null);
		if (callback.m_eResult == global::Steamworks.EResult.k_EResultOK)
		{
			this.lobby.id = callback.m_ulSteamIDLobby;
			this.lobby.hostId = (ulong)global::Steamworks.SteamUser.GetSteamID();
			this.lobby.version = "1.4.4.4";
			this.lobby.mapName = 0;
			this.lobby.warbandId = 0;
			this.lobby.isExhibition = true;
			if (string.IsNullOrEmpty(this.lobby.name))
			{
				this.lobby.name = global::Steamworks.SteamFriends.GetPersonaName() + "'s lobby";
			}
			this.SetLobbyData(null, null);
			global::PandoraDebug.LogInfo(string.Concat(new object[]
			{
				"OnLobbyCreated = ",
				this.lobby.id,
				" name = ",
				this.lobby.name
			}), "HEPHAESTUS-STEAMWORKS", null);
			global::Steamworks.SteamMatchmaking.SetLobbyMemberLimit((global::Steamworks.CSteamID)this.lobby.id, 2);
			global::PandoraSingleton<global::Hephaestus>.Instance.UpdateLobby(this.lobby);
			global::PandoraSingleton<global::Hephaestus>.Instance.OnCreateLobby(this.lobby.id, true);
		}
		else
		{
			global::PandoraDebug.LogInfo("OnLobbyCreated Not OK. forget lobby", "HEPHAESTUS-STEAMWORKS", null);
			global::PandoraSingleton<global::Hephaestus>.Instance.OnCreateLobby(this.lobby.id, false);
		}
	}

	public void LeaveLobby()
	{
		if (this.lobby != null)
		{
			global::PandoraDebug.LogInfo("Left Lobby = " + this.lobby.id, "HEPHAESTUS-STEAMWORKS", null);
			global::Steamworks.SteamMatchmaking.LeaveLobby((global::Steamworks.CSteamID)this.lobby.id);
			if (global::Steamworks.SteamMatchmaking.GetNumLobbyMembers((global::Steamworks.CSteamID)this.lobby.id) == 0)
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.RemoveLobby(this.lobby.id, false);
			}
			this.lobby = null;
		}
	}

	public void JoinLobby(ulong lobbyId)
	{
		if (!this.steamCbLobbyEnter.IsActive())
		{
			global::PandoraDebug.LogInfo("Join Lobby " + lobbyId, "HEPHAESTUS-STEAMWORKS", null);
			this.pendingLobbyJoinId = (global::Steamworks.CSteamID)lobbyId;
			global::Steamworks.SteamAPICall_t hAPICall = global::Steamworks.SteamMatchmaking.JoinLobby(this.pendingLobbyJoinId);
			this.steamCbLobbyEnter.Set(hAPICall, new global::Steamworks.CallResult<global::Steamworks.LobbyEnter_t>.APIDispatchDelegate(this.OnLobbyEntered));
		}
	}

	public void CancelJoinLobby()
	{
		if (this.steamCbLobbyEnter.IsActive())
		{
			this.steamCbLobbyEnter.Cancel();
		}
		global::Steamworks.SteamMatchmaking.LeaveLobby(this.pendingLobbyJoinId);
	}

	private void OnLobbyEntered(global::Steamworks.LobbyEnter_t callback, bool failure)
	{
		global::Hephaestus.LobbyConnexionResult lobbyConnexionResult;
		switch (callback.m_EChatRoomEnterResponse)
		{
		case 1U:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.SUCCESS;
			break;
		case 2U:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.DOES_NOT_EXIST;
			break;
		case 3U:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.NOT_ALLOWED;
			break;
		case 4U:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.FULL;
			break;
		case 5U:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.UNEXPECTED_ERROR;
			break;
		case 6U:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.BANNED;
			break;
		case 7U:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.LIMITED_USER;
			break;
		case 8U:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.CLAN_DISABLED;
			break;
		case 9U:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.COMMUNITY_BANNED;
			break;
		case 10U:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.MEMBER_BLOCKED_YOU;
			break;
		case 11U:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.BLOCKED_A_MEMBER;
			break;
		default:
			lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.UNEXPECTED_ERROR;
			break;
		}
		if (lobbyConnexionResult == global::Hephaestus.LobbyConnexionResult.SUCCESS)
		{
			this.lobby = new global::Lobby();
			this.lobby.id = callback.m_ulSteamIDLobby;
			string lobbyData = global::Steamworks.SteamMatchmaking.GetLobbyData((global::Steamworks.CSteamID)callback.m_ulSteamIDLobby, "version");
			if (lobbyData != "1.4.4.4")
			{
				lobbyConnexionResult = global::Hephaestus.LobbyConnexionResult.VERSION_MISMATCH;
				global::PandoraSingleton<global::Hephaestus>.Instance.LeaveLobby();
			}
		}
		if (lobbyConnexionResult == global::Hephaestus.LobbyConnexionResult.SUCCESS)
		{
			global::PandoraDebug.LogInfo("Lobby Enter Successful", "HEPHAESTUS-STEAMWORKS", null);
			global::PandoraSingleton<global::Hephaestus>.Instance.OnJoinLobby((ulong)global::Steamworks.SteamMatchmaking.GetLobbyOwner((global::Steamworks.CSteamID)callback.m_ulSteamIDLobby), lobbyConnexionResult);
		}
		else
		{
			global::PandoraDebug.LogInfo("Lobby Enter Unsuccessful, probably full!", "HEPHAESTUS-STEAMWORKS", null);
			global::PandoraSingleton<global::Hephaestus>.Instance.OnJoinLobby(0UL, lobbyConnexionResult);
		}
	}

	public void SetLobbyData(string key, string value)
	{
		if (this.lobby != null && this.lobby.id != 0UL)
		{
			if (!string.IsNullOrEmpty(key))
			{
				switch (key)
				{
				case "privacy":
					this.lobby.privacy = (global::Hephaestus.LobbyPrivacy)int.Parse(value);
					goto IL_1A3;
				case "name":
					this.lobby.name = value;
					goto IL_1A3;
				case "version":
					this.lobby.version = value;
					goto IL_1A3;
				case "map":
					this.lobby.mapName = int.Parse(value);
					goto IL_1A3;
				case "warband":
					this.lobby.warbandId = int.Parse(value);
					goto IL_1A3;
				case "exhibition":
					this.lobby.isExhibition = bool.Parse(value);
					goto IL_1A3;
				case "rating_min":
					this.lobby.ratingMin = int.Parse(value);
					goto IL_1A3;
				case "rating_max":
					this.lobby.ratingMax = int.Parse(value);
					goto IL_1A3;
				}
				global::PandoraDebug.LogWarning("Setting Unknown key in lobby data:" + key, "HEPHAESTUS-STEAMWORKS", null);
			}
			IL_1A3:
			string key2 = "privacy";
			int privacy = (int)this.lobby.privacy;
			this.SetSingleLobbyDataKeyValue(key2, privacy.ToString());
			this.SetSingleLobbyDataKeyValue("name", this.lobby.name);
			this.SetSingleLobbyDataKeyValue("version", this.lobby.version);
			this.SetSingleLobbyDataKeyValue("map", this.lobby.mapName.ToLowerString());
			this.SetSingleLobbyDataKeyValue("warband", this.lobby.warbandId.ToLowerString());
			this.SetSingleLobbyDataKeyValue("exhibition", this.lobby.isExhibition.ToLowerString());
			this.SetSingleLobbyDataKeyValue("rating_min", this.lobby.ratingMin.ToLowerString());
			this.SetSingleLobbyDataKeyValue("rating_max", this.lobby.ratingMax.ToLowerString());
		}
	}

	private void SetSingleLobbyDataKeyValue(string key, string value)
	{
		if (!global::Steamworks.SteamMatchmaking.SetLobbyData((global::Steamworks.CSteamID)this.lobby.id, key, value))
		{
			global::PandoraDebug.LogWarning("Unable to set Lobby Data:" + key + " to value:" + value, "HEPHAESTUS-STEAMWORKS", null);
		}
	}

	public void SetLobbyJoinable(bool joinable)
	{
		if (this.lobby != null)
		{
			global::PandoraDebug.LogDebug("Setting Lobby Joinable = " + joinable, "uncategorised", null);
			global::Steamworks.SteamMatchmaking.SetLobbyJoinable((global::Steamworks.CSteamID)this.lobby.id, joinable);
		}
	}

	public void SearchLobbies()
	{
		global::PandoraDebug.LogInfo("Call Search Lobbies", "HEPHAESTUS-STEAMWORKS", null);
		global::Steamworks.SteamAPICall_t hAPICall = global::Steamworks.SteamMatchmaking.RequestLobbyList();
		this.steamCbLobbyMatchList.Set(hAPICall, new global::Steamworks.CallResult<global::Steamworks.LobbyMatchList_t>.APIDispatchDelegate(this.OnLobbyMatchList));
	}

	private void OnPersonaStateChange(global::Steamworks.PersonaStateChange_t callback)
	{
	}

	private void OnLobbyDataUpdate(global::Steamworks.LobbyDataUpdate_t callback)
	{
		global::PandoraDebug.LogInfo("Update Lobby data begin", "HEPHAESTUS-STEAMWORKS", null);
		if (callback.m_bSuccess > 0)
		{
			int lobbyMemberLimit = global::Steamworks.SteamMatchmaking.GetLobbyMemberLimit((global::Steamworks.CSteamID)callback.m_ulSteamIDLobby);
			int numLobbyMembers = global::Steamworks.SteamMatchmaking.GetNumLobbyMembers((global::Steamworks.CSteamID)callback.m_ulSteamIDLobby);
			if (numLobbyMembers < lobbyMemberLimit || lobbyMemberLimit != 0)
			{
				global::Lobby lobby = new global::Lobby();
				lobby.id = callback.m_ulSteamIDLobby;
				int lobbyDataCount = global::Steamworks.SteamMatchmaking.GetLobbyDataCount((global::Steamworks.CSteamID)lobby.id);
				string lobbyData = global::Steamworks.SteamMatchmaking.GetLobbyData((global::Steamworks.CSteamID)lobby.id, "privacy");
				string lobbyData2 = global::Steamworks.SteamMatchmaking.GetLobbyData((global::Steamworks.CSteamID)lobby.id, "name");
				string lobbyData3 = global::Steamworks.SteamMatchmaking.GetLobbyData((global::Steamworks.CSteamID)lobby.id, "map");
				string lobbyData4 = global::Steamworks.SteamMatchmaking.GetLobbyData((global::Steamworks.CSteamID)lobby.id, "version");
				string lobbyData5 = global::Steamworks.SteamMatchmaking.GetLobbyData((global::Steamworks.CSteamID)lobby.id, "warband");
				string lobbyData6 = global::Steamworks.SteamMatchmaking.GetLobbyData((global::Steamworks.CSteamID)lobby.id, "exhibition");
				string lobbyData7 = global::Steamworks.SteamMatchmaking.GetLobbyData((global::Steamworks.CSteamID)lobby.id, "rating_min");
				string lobbyData8 = global::Steamworks.SteamMatchmaking.GetLobbyData((global::Steamworks.CSteamID)lobby.id, "rating_max");
				ulong num = (ulong)global::Steamworks.SteamMatchmaking.GetLobbyOwner((global::Steamworks.CSteamID)lobby.id);
				if (!string.IsNullOrEmpty(lobbyData))
				{
					lobby.privacy = (global::Hephaestus.LobbyPrivacy)int.Parse(lobbyData);
				}
				lobby.hostId = num;
				lobby.name = lobbyData2;
				lobby.version = lobbyData4;
				if (!string.IsNullOrEmpty(lobbyData6))
				{
					lobby.isExhibition = bool.Parse(lobbyData6);
				}
				if (!string.IsNullOrEmpty(lobbyData7))
				{
					lobby.ratingMin = int.Parse(lobbyData7);
				}
				if (!string.IsNullOrEmpty(lobbyData8))
				{
					lobby.ratingMax = int.Parse(lobbyData8);
				}
				global::PandoraDebug.LogInfo(string.Concat(new object[]
				{
					"Update Lobby Id = ",
					lobby.id,
					" lobbyId of cb:",
					callback.m_ulSteamIDLobby,
					" paramCount:",
					lobbyDataCount,
					" name = ",
					lobbyData2,
					" host Id = ",
					num,
					" map = ",
					lobbyData3,
					" warband = ",
					lobbyData5
				}), "HEPHAESTUS-STEAMWORKS", null);
				if (!string.IsNullOrEmpty(lobbyData3))
				{
					lobby.mapName = int.Parse(lobbyData3);
				}
				if (!string.IsNullOrEmpty(lobbyData5))
				{
					lobby.warbandId = int.Parse(lobbyData5);
				}
				global::PandoraSingleton<global::Hephaestus>.Instance.UpdateLobby(lobby);
			}
		}
		else
		{
			global::PandoraDebug.LogInfo("FAIL Update Lobby Id", "HEPHAESTUS-STEAMWORKS", null);
		}
	}

	private void OnLobbyJoinRequest(global::Steamworks.GameLobbyJoinRequested_t callback)
	{
		global::PandoraDebug.LogDebug("OnLobbyJoinRequest LobbyId = " + callback.m_steamIDLobby, "HEPHAESTUS-STEAMWORKS", null);
		this.OnLobbyJoinRequest(callback.m_steamIDLobby);
	}

	public void OnLobbyJoinRequest(global::Steamworks.CSteamID lobbyId)
	{
		global::Steamworks.SteamMatchmaking.RequestLobbyData(lobbyId);
		if (this.pendingLobbyInvite != null)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.StopCoroutine(this.pendingLobbyInvite);
		}
		this.pendingLobbyInvite = global::PandoraSingleton<global::Hephaestus>.Instance.StartCoroutine(this.AsyncLobbyJoin(lobbyId));
	}

	private global::System.Collections.IEnumerator AsyncLobbyJoin(global::Steamworks.CSteamID lobbyId)
	{
		string exhibition = string.Empty;
		while (string.IsNullOrEmpty(exhibition))
		{
			exhibition = global::Steamworks.SteamMatchmaking.GetLobbyData(lobbyId, "exhibition");
			yield return null;
		}
		string lobbyName = global::Steamworks.SteamMatchmaking.GetLobbyData(lobbyId, "name");
		bool isExhibition = bool.Parse(exhibition);
		global::PandoraSingleton<global::Hephaestus>.Instance.ReceiveInvite((ulong)lobbyId, lobbyName, isExhibition, 0, 5000);
		yield break;
	}

	private void OnLobbyChatUpdate(global::Steamworks.LobbyChatUpdate_t callback)
	{
		if (global::PandoraSingleton<global::Hephaestus>.Instance.Lobby != null && callback.m_ulSteamIDLobby == global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.id)
		{
			global::PandoraDebug.LogInfo(string.Concat(new object[]
			{
				"OnLobbyChatUpdate lobby = ",
				global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.id,
				" Callback lobby = ",
				callback.m_ulSteamIDLobby,
				" Message = ",
				(global::Steamworks.EChatMemberStateChange)callback.m_rgfChatMemberStateChange
			}), "HEPHAESTUS-STEAMWORKS", null);
			uint rgfChatMemberStateChange = callback.m_rgfChatMemberStateChange;
			if ((rgfChatMemberStateChange & 1U) > 0U)
			{
				global::PandoraDebug.LogInfo(callback.m_ulSteamIDUserChanged + " has entered the Lobby", "HEPHAESTUS-STEAMWORKS", null);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<ulong>(global::Notices.SKIRMISH_LOBBY_JOINED, callback.m_ulSteamIDUserChanged);
			}
			if ((rgfChatMemberStateChange & 2U) > 0U)
			{
				global::PandoraDebug.LogInfo(callback.m_ulSteamIDUserChanged + " has Left the Lobby", "HEPHAESTUS-STEAMWORKS", null);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<ulong>(global::Notices.SKIRMISH_LOBBY_LEFT, callback.m_ulSteamIDUserChanged);
			}
			if ((rgfChatMemberStateChange & 4U) > 0U)
			{
				global::PandoraDebug.LogInfo(callback.m_ulSteamIDUserChanged + " has Disconnected from steam", "HEPHAESTUS-STEAMWORKS", null);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<ulong>(global::Notices.SKIRMISH_LOBBY_LEFT, callback.m_ulSteamIDUserChanged);
			}
			if ((rgfChatMemberStateChange & 8U) > 0U)
			{
				global::PandoraDebug.LogInfo(callback.m_ulSteamIDUserChanged + " was kicked by" + callback.m_ulSteamIDMakingChange, "HEPHAESTUS-STEAMWORKS", null);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<ulong>(global::Notices.SKIRMISH_LOBBY_LEFT, callback.m_ulSteamIDUserChanged);
			}
			if ((rgfChatMemberStateChange & 16U) > 0U)
			{
				global::PandoraDebug.LogInfo(callback.m_ulSteamIDUserChanged + " was banned by" + callback.m_ulSteamIDMakingChange, "HEPHAESTUS-STEAMWORKS", null);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<ulong>(global::Notices.SKIRMISH_LOBBY_LEFT, callback.m_ulSteamIDUserChanged);
			}
		}
		if (global::Steamworks.SteamMatchmaking.GetNumLobbyMembers((global::Steamworks.CSteamID)callback.m_ulSteamIDLobby) == 0)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.RemoveLobby(callback.m_ulSteamIDLobby, false);
		}
	}

	private void OnLobbyKicked(global::Steamworks.LobbyKicked_t callback)
	{
		if (this.lobby != null)
		{
			this.lobby = null;
			global::PandoraSingleton<global::Hephaestus>.Instance.OnKickFromLobby();
		}
	}

	private void OnLobbyMatchList(global::Steamworks.LobbyMatchList_t callback, bool failure)
	{
		global::PandoraDebug.LogInfo("OnLobbyMatchList " + ((!failure) ? "SUCCESS" : "FAIL"), "HEPHAESTUS-STEAMWORKS", null);
		uint num = callback.m_nLobbiesMatching;
		global::PandoraDebug.LogInfo("OnLobbyMatchList num lobbies = " + num, "HEPHAESTUS-STEAMWORKS", null);
		int num2 = 0;
		while ((long)num2 < (long)((ulong)num))
		{
			global::Steamworks.CSteamID lobbyByIndex = global::Steamworks.SteamMatchmaking.GetLobbyByIndex(num2);
			global::PandoraDebug.LogInfo("Lobby Id = " + (ulong)lobbyByIndex, "HEPHAESTUS-STEAMWORKS", null);
			string lobbyData = global::Steamworks.SteamMatchmaking.GetLobbyData(lobbyByIndex, "name");
			global::Steamworks.SteamMatchmaking.RequestLobbyData(lobbyByIndex);
			num2++;
		}
		int friendCount = global::Steamworks.SteamFriends.GetFriendCount(global::Steamworks.EFriendFlags.k_EFriendFlagImmediate);
		for (int i = 0; i < friendCount; i++)
		{
			global::Steamworks.FriendGameInfo_t friendGameInfo_t = default(global::Steamworks.FriendGameInfo_t);
			global::Steamworks.CSteamID friendByIndex = global::Steamworks.SteamFriends.GetFriendByIndex(i, global::Steamworks.EFriendFlags.k_EFriendFlagImmediate);
			if (global::Steamworks.SteamFriends.GetFriendGamePlayed(friendByIndex, out friendGameInfo_t) && friendGameInfo_t.m_steamIDLobby.IsValid() && (uint)((ulong)friendGameInfo_t.m_gameID) == 276810U)
			{
				global::PandoraDebug.LogInfo("Found Friend = " + friendByIndex + " Limit = ", "HEPHAESTUS-STEAMWORKS", null);
				bool flag = false;
				int num3 = 0;
				while ((long)num3 < (long)((ulong)num) && !flag)
				{
					if (global::Steamworks.SteamMatchmaking.GetLobbyByIndex(num3) == friendGameInfo_t.m_steamIDLobby)
					{
						flag = true;
					}
					num3++;
				}
				if (!flag)
				{
					global::Steamworks.SteamMatchmaking.RequestLobbyData(friendGameInfo_t.m_steamIDLobby);
					num += 1U;
					string friendPersonaName = global::Steamworks.SteamFriends.GetFriendPersonaName(friendByIndex);
					global::PandoraDebug.LogInfo(string.Concat(new object[]
					{
						"Found Friend Requesting Data = ",
						friendByIndex,
						" Name = ",
						friendPersonaName,
						" Lobby = ",
						friendGameInfo_t.m_steamIDLobby
					}), "HEPHAESTUS-STEAMWORKS", null);
				}
			}
		}
		global::PandoraSingleton<global::Hephaestus>.Instance.OnSearchLobbies(num);
	}

	private global::Lobby lobby;

	private global::Steamworks.CallResult<global::Steamworks.LobbyCreated_t> steamCbLobbyCreated;

	private global::Steamworks.CallResult<global::Steamworks.LobbyEnter_t> steamCbLobbyEnter;

	private global::Steamworks.CallResult<global::Steamworks.LobbyMatchList_t> steamCbLobbyMatchList;

	private global::Steamworks.Callback<global::Steamworks.PersonaStateChange_t> StateChangeCb;

	private global::Steamworks.Callback<global::Steamworks.LobbyDataUpdate_t> LobbyDataUpdateCb;

	private global::Steamworks.Callback<global::Steamworks.LobbyChatUpdate_t> LobbyChatUpdateCb;

	private global::Steamworks.Callback<global::Steamworks.LobbyKicked_t> LobbyKickedCb;

	private global::Steamworks.Callback<global::Steamworks.GameLobbyJoinRequested_t> LobbyJoinRequestCb;

	private global::Steamworks.CSteamID pendingLobbyJoinId;

	private global::UnityEngine.Coroutine pendingLobbyInvite;
}
