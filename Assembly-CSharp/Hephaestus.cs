using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hephaestus : global::PandoraSingleton<global::Hephaestus>
{
	public global::Lobby Lobby { get; private set; }

	public global::System.Collections.Generic.List<global::Lobby> Lobbies { get; private set; }

	private void Awake()
	{
		this.Init();
		this.SaveUI = base.GetComponent<global::UnityEngine.Canvas>();
		this.SaveUI.enabled = false;
	}

	public void Init()
	{
		this.client = new global::SteamManager();
	}

	public bool ClientLoaded()
	{
		return true;
	}

	private void OnDestroy()
	{
		this.client.OnDestroy();
	}

	public global::System.Collections.IEnumerator InitializeClient()
	{
		this.Lobby = null;
		this.Lobbies = new global::System.Collections.Generic.List<global::Lobby>();
		yield return null;
		base.StopCoroutine(this.client.Init());
		yield return base.StartCoroutine(this.client.Init());
		yield break;
	}

	public bool IsInitialized()
	{
		return this.client.IsInitialized();
	}

	public void Reset()
	{
		this.client.Reset();
	}

	public bool UpdateLobby(global::Lobby lobby)
	{
		bool result = true;
		if (lobby.version != "1.4.4.4" || (lobby.privacy == global::Hephaestus.LobbyPrivacy.FRIENDS && lobby.hostId == 0UL))
		{
			global::PandoraDebug.LogInfo(string.Concat(new object[]
			{
				"removing lobby because it is invalid ",
				lobby.version,
				" ",
				lobby.privacy,
				" ",
				lobby.hostId
			}), "uncategorised", null);
			this.RemoveLobby(lobby.id, false);
			result = false;
		}
		else
		{
			bool flag = false;
			for (int i = 0; i < this.Lobbies.Count; i++)
			{
				if (this.Lobbies[i].id == lobby.id)
				{
					global::PandoraDebug.LogInfo("found lobby " + this.Lobbies[i].id, "uncategorised", null);
					this.Lobbies[i].name = lobby.name;
					this.Lobbies[i].privacy = lobby.privacy;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				global::PandoraDebug.LogInfo("adding lobby " + lobby.id, "uncategorised", null);
				this.Lobbies.Add(lobby);
			}
		}
		if ((long)this.Lobbies.Count == (long)((ulong)this.numLobbies) && this.searchLobbiesCb != null)
		{
			this.searchLobbiesCb();
			this.searchLobbiesCb = null;
		}
		return result;
	}

	public void RemoveLobby(ulong lobbyId, bool check = false)
	{
		uint num = this.numLobbies;
		for (int i = this.Lobbies.Count - 1; i >= 0; i--)
		{
			if (this.Lobbies[i].id == lobbyId)
			{
				this.Lobbies.RemoveAt(i);
				this.numLobbies -= 1U;
			}
		}
		if (num == this.numLobbies && this.numLobbies > 0U)
		{
			this.numLobbies -= 1U;
		}
		if (check && (long)this.Lobbies.Count == (long)((ulong)this.numLobbies) && this.searchLobbiesCb != null)
		{
			this.searchLobbiesCb();
			this.searchLobbiesCb = null;
		}
	}

	public void OnKickFromLobby()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SKIRMISH_LOBBY_KICKED);
	}

	public bool IsOnline()
	{
		return this.client.IsOnline();
	}

	public string GetOfflineReason()
	{
		return this.client.GetOfflineReason();
	}

	public void CreateLobby(string name, global::Hephaestus.LobbyPrivacy privacy, global::Hephaestus.OnLobbyCreatedCallback callback)
	{
		this.Lobby = null;
		this.lobbyCreatedCb = callback;
		if (!this.IsOnline())
		{
			privacy = global::Hephaestus.LobbyPrivacy.OFFLINE;
		}
		this.client.CreateLobby(name, privacy);
	}

	public void OnCreateLobby(ulong lobbyId, bool success)
	{
		if (success)
		{
			for (int i = 0; i < this.Lobbies.Count; i++)
			{
				if (this.Lobbies[i].id == lobbyId)
				{
					this.Lobby = this.Lobbies[i];
				}
			}
		}
		this.lobbyCreatedCb(lobbyId, success);
		this.lobbyCreatedCb = null;
	}

	public void LeaveLobby()
	{
		if (this.Lobby != null)
		{
			if (global::PandoraSingleton<global::Hermes>.Instance.IsHost())
			{
				this.client.SetLobbyJoinable(false);
			}
			this.client.LeaveLobby();
			this.Lobby = null;
		}
	}

	public void JoinLobby(ulong lobbyId, global::Hephaestus.OnJoinLobbyCallback callback, global::Hermes.OnConnectedCallback hermesCb)
	{
		global::PandoraSingleton<global::Hermes>.Instance.connectedCallback = hermesCb;
		this.joinLobbyCb = callback;
		for (int i = 0; i < this.Lobbies.Count; i++)
		{
			if (this.Lobbies[i].id == lobbyId)
			{
				this.Lobby = this.Lobbies[i];
				break;
			}
		}
		if (this.Lobby == null)
		{
			this.Lobby = new global::Lobby();
			this.Lobby.id = lobbyId;
		}
		this.client.JoinLobby(lobbyId);
	}

	public void CancelJoinLobby()
	{
		global::PandoraSingleton<global::Hermes>.instance.connectedCallback = null;
		this.client.CancelJoinLobby();
	}

	public void OnJoinLobby(ulong ownerId, global::Hephaestus.LobbyConnexionResult connexionResult)
	{
		if (this.Lobby != null)
		{
			this.Lobby.hostId = ownerId;
		}
		if (this.joinLobbyCb != null)
		{
			this.joinLobbyCb(connexionResult);
			this.joinLobbyCb = null;
		}
	}

	public void SetLobbyData(string key, string value)
	{
		this.client.SetLobbyData(key, value);
	}

	public void SetLobbyJoinable(bool joinable)
	{
		this.client.SetLobbyJoinable(joinable);
	}

	public void SearchLobbies(global::Hephaestus.OnSearchLobbiesCallback callback)
	{
		this.searchLobbiesCb = callback;
		this.Lobbies.Clear();
		if (this.IsOnline())
		{
			this.client.SearchLobbies();
		}
	}

	public void OnSearchLobbies(uint lobbiesCount)
	{
		global::PandoraDebug.LogInfo("OnSearchLobbies to call callback", "Hephaestus", null);
		this.numLobbies = lobbiesCount;
		if (this.numLobbies == 0U && this.searchLobbiesCb != null)
		{
			this.searchLobbiesCb();
			this.searchLobbiesCb = null;
		}
	}

	public void InviteFriends()
	{
		if (this.Lobby != null)
		{
			this.client.InviteFriends();
		}
	}

	public void InitVoiceChat()
	{
		this.client.InitVoiceChat();
	}

	public void OpenStore(global::Hephaestus.DlcId appId)
	{
		this.client.OpenStore(appId);
	}

	public void OpenCommunity()
	{
		this.client.OpenCommunity();
	}

	public string GetUserName()
	{
		return this.client.GetUserName();
	}

	public string GetOpponentUserName()
	{
		return this.client.GetOpponentUserName();
	}

	public ulong GetUserId()
	{
		return this.client.GetUserId();
	}

	public void DisplayOtherPlayerProfile()
	{
		this.client.DisplayOtherPlayerProfile();
	}

	public void CanReceiveMessages(global::System.Action<bool> cb)
	{
		this.client.CanReceiveMessages(cb);
	}

	public void RefreshSaveData(global::Hephaestus.OnSaveDataRefreshed cb)
	{
		this.saveRefreshCb = cb;
		this.client.RefreshSaveInfo();
	}

	public void OnRefreshSaveDataDone()
	{
		global::PandoraDebug.LogInfo("Save Data Refreshed", "HEPHAESTUS", null);
		if (this.saveRefreshCb != null)
		{
			this.saveRefreshCb();
			this.saveRefreshCb = null;
		}
	}

	public void FileWrite(string fileName, byte[] data, global::Hephaestus.OnFileWriteCallback callback)
	{
		this.fileWriteCb = callback;
		this.client.FileWrite(fileName, data);
		this.DisplaySaveLogo();
	}

	public void OnFileWrite(bool success)
	{
		this.fileWriteCb(success);
	}

	public void FileRead(string fileName, global::Hephaestus.OnFileReadCallback callback)
	{
		this.fileReadCb = callback;
		this.client.FileRead(fileName);
	}

	public void OnFileRead(byte[] data, bool success)
	{
		global::PandoraDebug.LogInfo("On File Read " + success, "HEPHAESTUS", null);
		if (this.fileReadCb != null)
		{
			this.fileReadCb(data, success);
		}
	}

	public void FileDelete(string fileName, global::Hephaestus.OnFileDeleteCallback callback)
	{
		this.fileDeleteCb = callback;
		this.client.FileDelete(fileName);
	}

	public void OnFileDelete(bool success)
	{
		if (this.fileDeleteCb != null)
		{
			this.fileDeleteCb(success);
		}
	}

	public bool FileExists(string fileName)
	{
		return this.client.FileExists(fileName);
	}

	public global::System.DateTime GetFileTimeStamp(string fileName)
	{
		global::System.DateTime result = new global::System.DateTime(this.client.GetFileTimeStamp(fileName), global::System.DateTimeKind.Local);
		return result;
	}

	public void DisconnectFromUser(ulong userId)
	{
		this.client.DisconnectFromUser(userId);
	}

	public void ResetNetwork()
	{
		this.client.ResetNetwork();
	}

	public void SetDataReceivedCallback(global::Hephaestus.OnDataReceivedCallback cb)
	{
		if (this.client != null)
		{
			this.client.SetDataReceivedCallback(cb);
		}
	}

	public void SendData(bool reliable, ulong toid, byte[] data)
	{
		this.client.Send(reliable, toid, data);
	}

	public void InitDefaultLocale()
	{
		this.client.GetDefaultLocale(delegate(global::SupportedLanguage lang)
		{
			global::PandoraDebug.LogDebug("Changing Language to : " + lang.ToString(), "HEPHAESTUS", null);
			global::PandoraSingleton<global::LocalizationManager>.Instance.SetLanguage(lang, true);
			global::PandoraSingleton<global::GameManager>.Instance.Options.language = (int)lang;
		});
	}

	public global::System.Collections.Generic.List<global::SupportedLanguage> GetAvailableLanguages()
	{
		return this.client.GetAvailableLanguages();
	}

	private void Update()
	{
		if (this.client != null)
		{
			this.client.Update();
		}
		if (this.saveTimer > 0f)
		{
			this.SaveUI.enabled = true;
			this.saveTimer -= global::UnityEngine.Time.smoothDeltaTime;
			if (this.saveTimer <= 0f)
			{
				this.SaveUI.enabled = false;
			}
		}
	}

	private void DisplaySaveLogo()
	{
		this.saveTimer = 2f;
	}

	public void IncrementStat(global::Hephaestus.StatId stat, int increment)
	{
		this.client.IncrementStat(stat, increment);
	}

	public void UnlockAchievement(global::Hephaestus.TrophyId achievement)
	{
		this.client.UnlockAchievement(achievement);
	}

	public bool IsAchievementUnlocked(global::Hephaestus.TrophyId achievement)
	{
		return this.client.IsAchievementUnlocked(achievement);
	}

	public void UpdateGameProgress()
	{
		this.client.UpdateGameProgress();
	}

	public void RequestNumberOfCurrentPlayers(global::Hephaestus.OnNumberOfPlayersCallback callback)
	{
		this.nbPlayersCb = callback;
		this.client.RequestNumberOfCurrentPlayers();
	}

	public void OnNumberOfCurrentPlayersReceived(int number)
	{
		if (this.nbPlayersCb != null)
		{
			this.nbPlayersCb(number);
		}
	}

	public bool OwnsDLC(global::Hephaestus.DlcId dlcId)
	{
		return this.client.OwnsDLC(dlcId);
	}

	public void SetDLCBoughtCb(global::Hephaestus.OnDLCBoughtCallback cb)
	{
		this.dlcBoughtCb = cb;
	}

	public void OnDLCBought()
	{
		if (this.dlcBoughtCb != null)
		{
			this.dlcBoughtCb();
		}
	}

	public bool ShowVirtualKeyboard(string title, string oldText, uint maxChar, bool multiLine, global::Hephaestus.OnVirtualKeyboardCallback vkCb, bool validateString = true)
	{
		if (this.client.ShowVirtualKeyboard(multiLine, title, maxChar, oldText, validateString))
		{
			this.vkCallback = vkCb;
			return true;
		}
		return false;
	}

	public void OnVirtualKeyboardCB(bool success, string str)
	{
		if (this.vkCallback != null)
		{
			this.vkCallback(success, str);
			this.vkCallback = null;
		}
	}

	public void LockUserEngagement()
	{
		this.client.LockUserEngagement();
	}

	public void EngageUser(global::UnityEngine.Events.UnityAction cb)
	{
		this.OnEngagedCallback = cb;
		this.client.EngageUser();
	}

	public void OnUserEngaged()
	{
		if (this.OnEngagedCallback != null)
		{
			global::UnityEngine.Events.UnityAction onEngagedCallback = this.OnEngagedCallback;
			this.OnEngagedCallback = null;
			onEngagedCallback();
		}
	}

	public void SetRichPresence(global::Hephaestus.RichPresenceId presId, bool active)
	{
		this.client.SetRichPresence(presId, active);
	}

	public void GetUserPicture(global::Hephaestus.UserPictureSize size, global::UnityEngine.Events.UnityAction<global::UnityEngine.Sprite> pictureLoaded)
	{
		this.OnPlayerPictureLoaded = pictureLoaded;
		this.client.GetUserPicture(size);
	}

	public void PlayerPictureLoaded(global::UnityEngine.Sprite sprite)
	{
		if (this.OnPlayerPictureLoaded != null)
		{
			this.OnPlayerPictureLoaded(sprite);
		}
	}

	public bool IsPrivilegeRestricted(global::Hephaestus.RestrictionId restrictionId)
	{
		return this.client.IsPrivilegeRestricted(restrictionId);
	}

	public void CheckNetworkServicesAvailability(global::System.Action<bool, string> callback)
	{
		this.client.CheckNetworkServicesAvailability(callback);
	}

	public void Delay(global::Hephaestus.WaitDelegate whileCondition, global::System.Action cb)
	{
		base.StartCoroutine(this.Delayer(whileCondition, cb));
	}

	private global::System.Collections.IEnumerator Delayer(global::Hephaestus.WaitDelegate whileCondition, global::System.Action cb)
	{
		while (whileCondition())
		{
			yield return null;
		}
		cb();
		yield break;
	}

	public void MultiplayerRoundStart()
	{
		this.client.MultiplayerRoundStart();
	}

	public void MultiplayerRoundEnd()
	{
		this.client.MultiplayerRoundEnd();
	}

	public bool ValidateWarbandDLC(global::WarbandSave warband)
	{
		this.dlcNeeded.Clear();
		this.dlcNeededLoc.Clear();
		global::WarbandId id = (global::WarbandId)warband.id;
		if (id == global::WarbandId.WITCH_HUNTERS)
		{
			if (!global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.WITCH_HUNTERS))
			{
				this.dlcNeeded.Add(global::Hephaestus.DlcId.WITCH_HUNTERS);
				this.dlcNeededLoc.Add("Witch Hunters");
			}
		}
		else if (id == global::WarbandId.UNDEAD && !global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.UNDEAD))
		{
			this.dlcNeeded.Add(global::Hephaestus.DlcId.UNDEAD);
			this.dlcNeededLoc.Add("Undead");
		}
		for (int i = 0; i < warband.units.Count; i++)
		{
			global::UnitSave unitSave = warband.units[i];
			switch (unitSave.stats.id)
			{
			case 98:
				if (!global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.GLOBADIER) && !this.dlcNeeded.Contains(global::Hephaestus.DlcId.GLOBADIER))
				{
					this.dlcNeeded.Add(global::Hephaestus.DlcId.GLOBADIER);
					this.dlcNeededLoc.Add("Globadier");
				}
				break;
			case 99:
				if (!global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.SMUGGLER) && !this.dlcNeeded.Contains(global::Hephaestus.DlcId.SMUGGLER))
				{
					this.dlcNeeded.Add(global::Hephaestus.DlcId.SMUGGLER);
					this.dlcNeededLoc.Add("Smuggler");
				}
				break;
			case 101:
				if (!global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.PRIEST_OF_ULRIC) && !this.dlcNeeded.Contains(global::Hephaestus.DlcId.PRIEST_OF_ULRIC))
				{
					this.dlcNeeded.Add(global::Hephaestus.DlcId.PRIEST_OF_ULRIC);
					this.dlcNeededLoc.Add("Wolf-Priest of Ulric");
				}
				break;
			case 102:
				if (!global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.DOOMWEAVER) && !this.dlcNeeded.Contains(global::Hephaestus.DlcId.DOOMWEAVER))
				{
					this.dlcNeeded.Add(global::Hephaestus.DlcId.DOOMWEAVER);
					this.dlcNeededLoc.Add("Doomweaver");
				}
				break;
			}
		}
		if (this.dlcNeededLoc.Count > 0)
		{
			global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.DLC, "com_wb_title_dlc", "popup_dlc_needed_desc", string.Join(", ", this.dlcNeededLoc.ToArray()), null, false);
		}
		return this.dlcNeededLoc.Count == 0;
	}

	public void JoinInvite()
	{
		if (this.joiningLobby != null)
		{
			this.Lobby = this.joiningLobby;
			this.ResetInvite();
			global::PandoraSingleton<global::SkirmishManager>.Instance.JoinLobby(this.Lobby.id, this.Lobby.name, this.Lobby.isExhibition, this.Lobby.ratingMin, this.Lobby.ratingMax);
		}
	}

	public void CheckPendingInvite()
	{
		this.client.CheckPendingInvite();
	}

	public void ReceiveInvite(global::Lobby lobby)
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"Receive invite ",
			lobby.name,
			" ",
			lobby.isExhibition
		}), "uncategorised", null);
		this.isJoiningInvite = true;
		this.joiningLobby = lobby;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SKIRMISH_INVITE_ACCEPTED);
	}

	public void ReceiveInvite(ulong lobbyId, string lobbyName, bool exhibition, int ratingMin, int ratingMax)
	{
		this.ReceiveInvite(new global::Lobby
		{
			id = lobbyId,
			isExhibition = exhibition,
			name = lobbyName,
			ratingMin = ratingMin,
			ratingMax = ratingMax
		});
	}

	public void ResetInvite()
	{
		this.isJoiningInvite = false;
		this.joiningLobby = null;
	}

	public bool IsJoiningInvite()
	{
		return this.isJoiningInvite;
	}

	public global::Lobby GetJoiningLobby()
	{
		return this.joiningLobby;
	}

	public bool IsPlayTogether()
	{
		return this.client.IsPlayTogether();
	}

	public bool IsPlayTogetherPassive()
	{
		return this.client.IsPlayTogetherPassive();
	}

	public void ResetPlayTogether(bool setPassive)
	{
		this.client.ResetPlayTogether(setPassive);
	}

	public void OnResume()
	{
		if (global::PandoraSingleton<global::HideoutManager>.Exists())
		{
			if (global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.GetActiveStateId() == 3 || global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.GetActiveStateId() == 4)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(3);
			}
		}
		else if (global::PandoraSingleton<global::MissionManager>.Exists() && global::PandoraSingleton<global::MissionManager>.Instance.GetPlayersCount() == 2)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.OnConnectionLost(true);
		}
	}

	private const float SAVE_DISPLAY_TIME = 2f;

	private global::IHephaestus client;

	private global::Hephaestus.OnLobbyCreatedCallback lobbyCreatedCb;

	private global::Hephaestus.OnLobbyEnteredCallback lobbyEnteredCb;

	private global::Hephaestus.OnSearchLobbiesCallback searchLobbiesCb;

	private global::Hephaestus.OnJoinLobbyCallback joinLobbyCb;

	private global::Hephaestus.OnSaveDataRefreshed saveRefreshCb;

	private global::Hephaestus.OnFileWriteCallback fileWriteCb;

	private global::Hephaestus.OnFileReadCallback fileReadCb;

	private global::Hephaestus.OnFileDeleteCallback fileDeleteCb;

	private global::Hephaestus.OnNumberOfPlayersCallback nbPlayersCb;

	private global::Hephaestus.OnDLCBoughtCallback dlcBoughtCb;

	private global::Hephaestus.OnVirtualKeyboardCallback vkCallback;

	private global::UnityEngine.Events.UnityAction OnEngagedCallback;

	private global::UnityEngine.Events.UnityAction<global::UnityEngine.Sprite> OnPlayerPictureLoaded;

	private uint numLobbies;

	private global::UnityEngine.Canvas SaveUI;

	private float saveTimer;

	private readonly global::System.Collections.Generic.List<global::Hephaestus.DlcId> dlcNeeded = new global::System.Collections.Generic.List<global::Hephaestus.DlcId>();

	private readonly global::System.Collections.Generic.List<string> dlcNeededLoc = new global::System.Collections.Generic.List<string>();

	private bool isJoiningInvite;

	public global::Lobby joiningLobby;

	private bool isJoiningLobbyExhibition;

	public enum RestrictionId
	{
		CHAT,
		UGC,
		VOICE_CHAT,
		PROFILE_VIEWING
	}

	public enum DlcId
	{
		GLOBADIER,
		SMUGGLER,
		PRIEST_OF_ULRIC,
		DOOMWEAVER,
		WITCH_HUNTERS,
		UNDEAD
	}

	public enum RichPresenceId
	{
		MAIN_MENU,
		HIDEOUT,
		LOBBY_EXHIBITION,
		LOBBY_CONTEST,
		CAMPAIGN_MISSION,
		TUTORIAL_MISSION,
		PROC_MISSION,
		EXHIBITION_AI,
		EXHIBITION_PLAYER,
		CONTEST
	}

	public enum UserPictureSize
	{
		SMALL,
		MEDIUM,
		LARGE,
		EXTRA_LARGE
	}

	public enum LobbyPrivacy
	{
		PRIVATE,
		FRIENDS,
		PUBLIC,
		OFFLINE,
		COUNT
	}

	public enum LobbyConnexionResult
	{
		SUCCESS,
		DOES_NOT_EXIST,
		NOT_ALLOWED,
		BLOCKED_A_MEMBER,
		MEMBER_BLOCKED_YOU,
		LIMITED_USER,
		COMMUNITY_BANNED,
		CLAN_DISABLED,
		BANNED,
		FULL,
		UNEXPECTED_ERROR,
		VERSION_MISMATCH,
		KICKED
	}

	public enum TrophyId
	{
		STORY_SKAVEN_1,
		STORY_MERC_1,
		STORY_POSSESSED_1,
		STORY_SISTERS_1,
		STORY_ALL_1,
		STORY_SKAVEN_2,
		STORY_MERC_2,
		STORY_POSSESSED_2,
		STORY_SISTERS_2,
		STORY_ALL_2,
		SKAVEN_RANK_10,
		MERC_RANK_10,
		POSSESSED_RANK_10,
		SISTERS_RANK_10,
		ALL_RANK_10,
		LEADER_RANK_10_1,
		LEADER_RANK_10_2,
		LEADER_RANK_10_3,
		HERO_RANK_10_1,
		HERO_RANK_10_2,
		HERO_RANK_10_3,
		HENCHMEN_RANK_10_1,
		HENCHMEN_RANK_10_2,
		HENCHMEN_RANK_10_3,
		IMPRESSIVE_RANK_10_1,
		IMPRESSIVE_RANK_10_2,
		IMPRESSIVE_RANK_10_3,
		LEADER_NO_INJURY,
		HERO_NO_INJURY,
		HENCHMEN_NO_INJURY,
		IMPRESSIVE_NO_INJURY,
		SHIPMENT_1,
		SHIPMENT_2,
		SHIPMENT_3,
		WYRDSTONES,
		WYRDSTONE_GOLD,
		WYRDSTONE_WEIGHT,
		SHOP_BUY,
		NORMAL_EQUIP,
		GOOD_EQUIP,
		BEST_EQUIP,
		ENCHANT_EQUIP_1,
		ENCHANT_EQUIP_2,
		RENAME,
		YEAR_1,
		YEAR_5,
		GAME_OVER,
		HIRE_1,
		HIRE_2,
		HIRE_3,
		HIRE_4,
		HIRE_5,
		RECIPIES,
		MULTIPLE_INJURED,
		INJURED_FIRE,
		TREATMENT_NOT_PAID,
		UPKEEP_NOT_PAID,
		MULTIPLE_INJURIES,
		MUTATION_1,
		MUTATION_2,
		RANGE_9M,
		WIN_ALONE,
		WIN_CRIPPLED,
		ONE_SHOT,
		TUTO_1,
		TUTO_2,
		ALTF4,
		WITCH_HUNTERS_10,
		STORY_WITCH_HUNTERS_1,
		STORY_WITCH_HUNTERS_2,
		UNDEAD_10,
		STORY_UNDEAD_1,
		STORY_UNDEAD_2
	}

	public enum StatId
	{
		LEADER_RANK_10,
		HERO_RANK_10,
		HENCHMEN_RANK_10,
		IMPRESSIVE_RANK_10,
		SHIPMENTS,
		WYRDSTONE_SELL,
		WYRDSTONE_GATHER,
		SHOP_GOLD,
		HIRED_WARRIORS,
		UNLOCKED_RECIPES,
		MUTATIONS,
		STUNNED_OOAS,
		OPENED_CHESTS,
		IMPRESSIVE_OOAS,
		STUN_ENEMIES,
		CRITICALS,
		SPELLS_CAST,
		SPELLS_CURSES,
		MY_TOTAL_OOA,
		LOOT_ENEMIES,
		TRAPS,
		MULTI_WINS,
		MULTI_PLAY,
		ENEMIES_OOA,
		GOLD_EARNED,
		MY_TOTAL_INJURIES
	}

	public delegate void OnLobbyCreatedCallback(ulong lobbyId, bool success);

	public delegate void OnLobbyEnteredCallback(ulong lobbyId, bool success);

	public delegate void OnSearchLobbiesCallback();

	public delegate void OnJoinLobbyCallback(global::Hephaestus.LobbyConnexionResult result);

	public delegate void OnSaveDataRefreshed();

	public delegate void OnFileWriteCallback(bool success);

	public delegate void OnFileReadCallback(byte[] data, bool success);

	public delegate void OnFileDeleteCallback(bool success);

	public delegate void OnDataReceivedCallback(ulong fromId, byte[] data);

	public delegate void OnNumberOfPlayersCallback(int number);

	public delegate void OnDLCBoughtCallback();

	public delegate void OnVirtualKeyboardCallback(bool success, string newString);

	public delegate bool WaitDelegate();
}
