using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Steamworks;
using UnityEngine;

internal class SteamManager : global::IHephaestus
{
	public SteamManager()
	{
		try
		{
			if (global::Steamworks.SteamAPI.RestartAppIfNecessary((global::Steamworks.AppId_t)276810U))
			{
				global::PandoraDebug.LogError("Steam is not started... start in now!\n", "HEPHAESTUS-STEAMWORKS", null);
				global::UnityEngine.Application.Quit();
				return;
			}
		}
		catch (global::System.DllNotFoundException arg)
		{
			global::PandoraDebug.LogError("[Steamworks] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, "HEPHAESTUS-STEAMWORKS", null);
			global::UnityEngine.Application.Quit();
			return;
		}
		this.Initialized = global::Steamworks.SteamAPI.Init();
		if (!this.Initialized)
		{
			global::PandoraDebug.LogError("SteamAPI_Init() failed", "HEPHAESTUS-STEAMWORKS", null);
			global::UnityEngine.Application.Quit();
			return;
		}
		this.SteamAPIWarningMessageHook = new global::Steamworks.SteamAPIWarningMessageHook_t(global::SteamManager.SteamAPIDebugTextHook);
		global::Steamworks.SteamClient.SetWarningMessageHook(this.SteamAPIWarningMessageHook);
		global::Steamworks.SteamUtils.SetOverlayNotificationPosition(global::Steamworks.ENotificationPosition.k_EPositionTopRight);
		if (!global::Steamworks.SteamApps.BIsSubscribed())
		{
			global::PandoraDebug.LogError("Steam user must own the game in order to play this game (SteamApps.BIsSubscribed() returned false).\n", "HEPHAESTUS-STEAMWORKS", null);
			global::UnityEngine.Application.Quit();
		}
		this.matchmaking = new global::Matchmaking();
		this.networking = new global::Networking();
		new global::Steamworks.Callback<global::Steamworks.UserStatsReceived_t>(new global::Steamworks.Callback<global::Steamworks.UserStatsReceived_t>.DispatchDelegate(this.OnSteamUserStatsReceived), false);
		new global::Steamworks.Callback<global::Steamworks.UserStatsStored_t>(new global::Steamworks.Callback<global::Steamworks.UserStatsStored_t>.DispatchDelegate(this.OnSteamUserStatsStored), false);
		new global::Steamworks.Callback<global::Steamworks.UserAchievementStored_t>(new global::Steamworks.Callback<global::Steamworks.UserAchievementStored_t>.DispatchDelegate(this.OnSteamAchievementStored), false);
		new global::Steamworks.Callback<global::Steamworks.DlcInstalled_t>(new global::Steamworks.Callback<global::Steamworks.DlcInstalled_t>.DispatchDelegate(this.OnDLCInstalled), false);
		this.steamCbNumberOfPlayers = new global::Steamworks.CallResult<global::Steamworks.NumberOfCurrentPlayers_t>(null);
		this.steamCbVKClosed = global::Steamworks.Callback<global::Steamworks.GamepadTextInputDismissed_t>.Create(new global::Steamworks.Callback<global::Steamworks.GamepadTextInputDismissed_t>.DispatchDelegate(this.OnVirtualKeyboardClosed));
		global::Steamworks.SteamApps.GetAvailableGameLanguages();
		this.GetAvailableLanguages();
		global::Steamworks.ControllerHandle_t[] array = new global::Steamworks.ControllerHandle_t[25];
		int connectedControllers = global::Steamworks.SteamController.GetConnectedControllers(array);
		for (int i = 0; i < connectedControllers; i++)
		{
			global::PandoraDebug.LogInfo("FOUND CONTROLLER " + array[i].m_ControllerHandle, "HEPHAESTUS-STEAMWORKS", null);
		}
	}

	void global::IHephaestus.CreateLobby(string name, global::Hephaestus.LobbyPrivacy privacy)
	{
		global::Steamworks.ELobbyType type = global::Steamworks.ELobbyType.k_ELobbyTypePublic;
		switch (privacy)
		{
		case global::Hephaestus.LobbyPrivacy.PRIVATE:
			type = global::Steamworks.ELobbyType.k_ELobbyTypeInvisible;
			break;
		case global::Hephaestus.LobbyPrivacy.FRIENDS:
			type = global::Steamworks.ELobbyType.k_ELobbyTypeFriendsOnly;
			break;
		case global::Hephaestus.LobbyPrivacy.PUBLIC:
			type = global::Steamworks.ELobbyType.k_ELobbyTypePublic;
			break;
		case global::Hephaestus.LobbyPrivacy.OFFLINE:
			type = global::Steamworks.ELobbyType.k_ELobbyTypePrivate;
			break;
		}
		this.matchmaking.CreateLobby(name, type);
	}

	public bool Initialized { get; private set; }

	private static void SteamAPIDebugTextHook(int nSeverity, global::System.Text.StringBuilder pchDebugText)
	{
		global::UnityEngine.Debug.LogWarning(pchDebugText);
	}

	public void OnDestroy()
	{
		if (this.Initialized)
		{
			global::PandoraDebug.LogInfo("OnDestroy(), ShuttingDown SteamAPI!", "HEPHAESTUS-STEAMWORKS", null);
			this.LeaveLobby();
			global::Steamworks.SteamAPI.Shutdown();
		}
	}

	public global::System.Collections.IEnumerator Init()
	{
		global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(this.ProcessCommandLineArguments());
		yield return null;
		yield break;
	}

	public void Reset()
	{
	}

	public bool IsInitialized()
	{
		return this.Initialized;
	}

	private global::System.Collections.IEnumerator ProcessCommandLineArguments()
	{
		string[] args = global::System.Environment.GetCommandLineArgs();
		while (!global::PandoraSingleton<global::Hephaestus>.Instance.IsInitialized())
		{
			yield return null;
		}
		if (args.Length > 1)
		{
			for (int i = 1; i < args.Length; i++)
			{
				if (args[i].Contains("+connect_lobby"))
				{
					ulong lobbyId = ulong.Parse(args[i + 1]);
					string exhibition = null;
					while (string.IsNullOrEmpty(exhibition))
					{
						exhibition = global::Steamworks.SteamMatchmaking.GetLobbyData((global::Steamworks.CSteamID)lobbyId, "exhibition");
						yield return null;
					}
					string lobbyName = global::Steamworks.SteamMatchmaking.GetLobbyData((global::Steamworks.CSteamID)lobbyId, "name");
					bool isExhibition = bool.Parse(exhibition);
					global::PandoraSingleton<global::Hephaestus>.Instance.ReceiveInvite(lobbyId, lobbyName, isExhibition, 0, 5000);
				}
			}
		}
		yield break;
	}

	public void GetDefaultLocale(global::System.Action<global::SupportedLanguage> callback)
	{
		if (this.GetAvailableLanguages()[0] == global::SupportedLanguage.ruRU)
		{
			callback(global::SupportedLanguage.ruRU);
			return;
		}
		if (this.GetAvailableLanguages()[0] == global::SupportedLanguage.plPL)
		{
			callback(global::SupportedLanguage.plPL);
			return;
		}
		string currentGameLanguage = global::Steamworks.SteamApps.GetCurrentGameLanguage();
		global::PandoraDebug.LogDebug("Steam default language : " + currentGameLanguage, "STEAM MANAGER", null);
		global::SupportedLanguage obj = global::SupportedLanguage.enUS;
		string text = currentGameLanguage;
		switch (text)
		{
		case "english":
			obj = global::SupportedLanguage.enUS;
			break;
		case "french":
			obj = global::SupportedLanguage.frFR;
			break;
		case "german":
			obj = global::SupportedLanguage.deDE;
			break;
		case "spanish":
			obj = global::SupportedLanguage.esES;
			break;
		case "italian":
			obj = global::SupportedLanguage.itIT;
			break;
		case "polish":
			obj = global::SupportedLanguage.plPL;
			break;
		case "russian":
			obj = global::SupportedLanguage.ruRU;
			break;
		}
		callback(obj);
	}

	public global::System.Collections.Generic.List<global::SupportedLanguage> GetAvailableLanguages()
	{
		if (this.availableLangs.Count == 0)
		{
			global::Steamworks.DepotId_t[] array = new global::Steamworks.DepotId_t[25];
			uint installedDepots = global::Steamworks.SteamApps.GetInstalledDepots((global::Steamworks.AppId_t)276810U, array, 25U);
			int num = 0;
			while ((long)num < (long)((ulong)installedDepots))
			{
				global::PandoraDebug.LogDebug("Steam depot : " + array[num].m_DepotId, "STEAM MANAGER", null);
				if (array[num].m_DepotId == 276822U)
				{
					for (int i = 0; i < 7; i++)
					{
						this.availableLangs.Add((global::SupportedLanguage)i);
					}
					return this.availableLangs;
				}
				num++;
			}
			string currentGameLanguage = global::Steamworks.SteamApps.GetCurrentGameLanguage();
			global::PandoraDebug.LogDebug("Steam current languages : " + currentGameLanguage, "STEAM MANAGER", null);
			string text = currentGameLanguage;
			switch (text)
			{
			case "english":
				this.availableLangs.Add(global::SupportedLanguage.enUS);
				break;
			case "french":
				this.availableLangs.Add(global::SupportedLanguage.frFR);
				break;
			case "german":
				this.availableLangs.Add(global::SupportedLanguage.deDE);
				break;
			case "spanish":
				this.availableLangs.Add(global::SupportedLanguage.esES);
				break;
			case "italian":
				this.availableLangs.Add(global::SupportedLanguage.itIT);
				break;
			case "polish":
				this.availableLangs.Add(global::SupportedLanguage.plPL);
				break;
			case "russian":
				this.availableLangs.Add(global::SupportedLanguage.ruRU);
				break;
			}
			if (this.availableLangs.Count == 0)
			{
				for (int j = 0; j < 7; j++)
				{
					this.availableLangs.Add((global::SupportedLanguage)j);
				}
			}
		}
		return this.availableLangs;
	}

	public bool IsOnline()
	{
		return global::Steamworks.SteamFriends.GetPersonaState() != global::Steamworks.EPersonaState.k_EPersonaStateOffline;
	}

	public string GetOfflineReason()
	{
		return string.Empty;
	}

	public void LeaveLobby()
	{
		this.matchmaking.LeaveLobby();
	}

	public void JoinLobby(ulong lobbyId)
	{
		this.matchmaking.JoinLobby(lobbyId);
	}

	public void CancelJoinLobby()
	{
		this.matchmaking.CancelJoinLobby();
	}

	public void SetLobbyData(string key, string value)
	{
		this.matchmaking.SetLobbyData(key, value);
	}

	public void SetLobbyJoinable(bool joinable)
	{
		this.matchmaking.SetLobbyJoinable(joinable);
	}

	public void InviteFriends()
	{
		global::PandoraDebug.LogInfo("InviteFriends", "HEPHAESTUS-STEAMWORKS", null);
		global::Steamworks.SteamFriends.ActivateGameOverlay("LobbyInvite");
	}

	public void SearchLobbies()
	{
		if (global::Steamworks.SteamUser.BLoggedOn())
		{
			this.matchmaking.SearchLobbies();
		}
	}

	public void OpenStore(global::Hephaestus.DlcId dlcId)
	{
		if (global::Steamworks.SteamUtils.IsOverlayEnabled())
		{
			switch (dlcId)
			{
			case global::Hephaestus.DlcId.GLOBADIER:
				global::Steamworks.SteamFriends.ActivateGameOverlayToStore((global::Steamworks.AppId_t)434040U, global::Steamworks.EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
				break;
			case global::Hephaestus.DlcId.SMUGGLER:
				global::Steamworks.SteamFriends.ActivateGameOverlayToStore((global::Steamworks.AppId_t)434041U, global::Steamworks.EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
				break;
			case global::Hephaestus.DlcId.PRIEST_OF_ULRIC:
				global::Steamworks.SteamFriends.ActivateGameOverlayToStore((global::Steamworks.AppId_t)450810U, global::Steamworks.EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
				break;
			case global::Hephaestus.DlcId.DOOMWEAVER:
				global::Steamworks.SteamFriends.ActivateGameOverlayToStore((global::Steamworks.AppId_t)450811U, global::Steamworks.EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
				break;
			case global::Hephaestus.DlcId.WITCH_HUNTERS:
				global::Steamworks.SteamFriends.ActivateGameOverlayToStore((global::Steamworks.AppId_t)450812U, global::Steamworks.EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
				break;
			case global::Hephaestus.DlcId.UNDEAD:
				global::Steamworks.SteamFriends.ActivateGameOverlayToStore((global::Steamworks.AppId_t)534990U, global::Steamworks.EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
				break;
			}
		}
	}

	public void OpenCommunity()
	{
		if (global::Steamworks.SteamUtils.IsOverlayEnabled())
		{
			global::Steamworks.SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/app/276810");
		}
	}

	public string GetUserName()
	{
		return global::Steamworks.SteamFriends.GetPersonaName();
	}

	public ulong GetUserId()
	{
		return (ulong)global::Steamworks.SteamUser.GetSteamID();
	}

	public string GetOpponentUserName()
	{
		if (global::PandoraSingleton<global::MissionStartData>.Exists() && global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1)
		{
			return global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerName;
		}
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_player") + " 2";
	}

	public void DisplayOtherPlayerProfile()
	{
	}

	public bool FileExists(string fileName)
	{
		return global::Steamworks.SteamRemoteStorage.FileExists(fileName) && global::Steamworks.SteamRemoteStorage.GetFileSize(fileName) > 0;
	}

	public long GetFileTimeStamp(string fileName)
	{
		global::System.DateTime dateTime = new global::System.DateTime(1970, 1, 1, 0, 0, 0, global::System.DateTimeKind.Utc);
		return dateTime.AddSeconds((double)global::Steamworks.SteamRemoteStorage.GetFileTimestamp(fileName)).Ticks;
	}

	public void RefreshSaveInfo()
	{
		global::PandoraSingleton<global::Hephaestus>.Instance.OnRefreshSaveDataDone();
	}

	public void FileWrite(string fileName, byte[] data)
	{
		global::PandoraSingleton<global::Hephaestus>.Instance.OnFileWrite(global::Steamworks.SteamRemoteStorage.FileWrite(fileName, data, data.Length));
	}

	public void FileDelete(string fileName)
	{
		global::PandoraSingleton<global::Hephaestus>.Instance.OnFileDelete(global::Steamworks.SteamRemoteStorage.FileDelete(fileName));
	}

	public void FileRead(string fileName)
	{
		byte[] array = null;
		int num = 0;
		int num2 = 0;
		if (global::Steamworks.SteamRemoteStorage.FileExists(fileName))
		{
			num = global::Steamworks.SteamRemoteStorage.GetFileSize(fileName);
			array = new byte[num];
			num2 = global::Steamworks.SteamRemoteStorage.FileRead(fileName, array, num);
		}
		global::PandoraSingleton<global::Hephaestus>.Instance.OnFileRead(array, num == num2);
	}

	public void DisconnectFromUser(ulong steamID)
	{
		this.networking.CloseP2PSessionWithUser(steamID);
	}

	public void ResetNetwork()
	{
	}

	public void Send(bool reliable, ulong steamID, byte[] data)
	{
		this.networking.Send(reliable, (global::Steamworks.CSteamID)steamID, data);
	}

	public void SetDataReceivedCallback(global::Hephaestus.OnDataReceivedCallback cb)
	{
		this.networking.SetDataReceivedCallback(cb);
	}

	public void Update()
	{
		if (!this.requestedStats)
		{
			global::PandoraDebug.LogDebug("RequestCurrentStats", "STEAM MANAGER", null);
			this.requestedStats = global::Steamworks.SteamUserStats.RequestCurrentStats();
		}
		global::Steamworks.SteamAPI.RunCallbacks();
		this.StoreStatsIfNecessary();
		this.networking.ReadPackets();
	}

	public void UnlockAchievement(global::Hephaestus.TrophyId achievement)
	{
		global::Steamworks.SteamUserStats.SetAchievement(achievement.ToLowerString());
		this.shouldUploadStats = true;
	}

	public void IncrementStat(global::Hephaestus.StatId stat, int increment)
	{
		int num = 0;
		global::Steamworks.SteamUserStats.GetStat(stat.ToLowerString(), out num);
		global::Steamworks.SteamUserStats.SetStat(stat.ToLowerString(), num + increment);
		this.shouldUploadStats = true;
	}

	public bool IsAchievementUnlocked(global::Hephaestus.TrophyId achievement)
	{
		bool result = false;
		global::Steamworks.SteamUserStats.GetAchievement(achievement.ToLowerString(), out result);
		return result;
	}

	private void StoreStatsIfNecessary()
	{
		if (this.shouldUploadStats)
		{
			bool flag = global::Steamworks.SteamUserStats.StoreStats();
			this.shouldUploadStats = !flag;
		}
	}

	private void OnSteamUserStatsReceived(global::Steamworks.UserStatsReceived_t pCallback)
	{
		global::PandoraDebug.LogInfo("OnSteamUserStatsReceived", "STEAM MANAGER", null);
	}

	private void OnSteamUserStatsStored(global::Steamworks.UserStatsStored_t pCallback)
	{
		global::PandoraDebug.LogInfo("OnSteamUserStatsStored", "STEAM MANAGER", null);
		if (pCallback.m_nGameID == 276810UL)
		{
			if (pCallback.m_eResult == global::Steamworks.EResult.k_EResultOK)
			{
				global::PandoraDebug.LogInfo("OnSteamUserStatsStored - Success", "STEAM MANAGER", null);
			}
			else if (pCallback.m_eResult == global::Steamworks.EResult.k_EResultInvalidParam)
			{
				global::PandoraDebug.LogInfo("OnSteamUserStatsStored - Some stats failed to validate", "STEAM MANAGER", null);
				this.OnSteamUserStatsReceived(new global::Steamworks.UserStatsReceived_t
				{
					m_eResult = global::Steamworks.EResult.k_EResultOK,
					m_nGameID = 276810UL
				});
			}
		}
		else
		{
			global::PandoraDebug.LogInfo("OnSteamUserStatsStored - Received event for wrong game_id", "STEAM MANAGER", null);
		}
	}

	private void OnSteamAchievementStored(global::Steamworks.UserAchievementStored_t pCallback)
	{
		global::PandoraDebug.LogInfo("OnSteamAchievementStored", "STEAM MANAGER", null);
		if (pCallback.m_nGameID == 276810UL)
		{
		}
	}

	public void RequestNumberOfCurrentPlayers()
	{
		global::Steamworks.SteamAPICall_t numberOfCurrentPlayers = global::Steamworks.SteamUserStats.GetNumberOfCurrentPlayers();
		this.steamCbNumberOfPlayers.Set(numberOfCurrentPlayers, new global::Steamworks.CallResult<global::Steamworks.NumberOfCurrentPlayers_t>.APIDispatchDelegate(this.OnNumberOfCurrentPlayers));
	}

	private void OnNumberOfCurrentPlayers(global::Steamworks.NumberOfCurrentPlayers_t data, bool failure)
	{
		int number = (data.m_bSuccess != 1) ? 0 : data.m_cPlayers;
		global::PandoraSingleton<global::Hephaestus>.Instance.OnNumberOfCurrentPlayersReceived(number);
	}

	private void OnDLCInstalled(global::Steamworks.DlcInstalled_t pCallback)
	{
		global::PandoraDebug.LogInfo("New DLC bought " + pCallback.m_nAppID, "STEAM MANAGER", null);
		global::PandoraSingleton<global::Hephaestus>.Instance.OnDLCBought();
	}

	public bool OwnsDLC(global::Hephaestus.DlcId dlcId)
	{
		switch (dlcId)
		{
		case global::Hephaestus.DlcId.GLOBADIER:
			return global::Steamworks.SteamApps.BIsDlcInstalled((global::Steamworks.AppId_t)434040U);
		case global::Hephaestus.DlcId.SMUGGLER:
			return global::Steamworks.SteamApps.BIsDlcInstalled((global::Steamworks.AppId_t)434041U);
		case global::Hephaestus.DlcId.PRIEST_OF_ULRIC:
			return global::Steamworks.SteamApps.BIsDlcInstalled((global::Steamworks.AppId_t)450810U);
		case global::Hephaestus.DlcId.DOOMWEAVER:
			return global::Steamworks.SteamApps.BIsDlcInstalled((global::Steamworks.AppId_t)450811U);
		case global::Hephaestus.DlcId.WITCH_HUNTERS:
			return global::Steamworks.SteamApps.BIsDlcInstalled((global::Steamworks.AppId_t)450812U);
		case global::Hephaestus.DlcId.UNDEAD:
			return global::Steamworks.SteamApps.BIsDlcInstalled((global::Steamworks.AppId_t)534990U);
		default:
			return false;
		}
	}

	public bool IsDLCExists(global::Hephaestus.DlcId dlcId)
	{
		return true;
	}

	public bool ShowVirtualKeyboard(bool multiLine, string title, uint maxChar, string oldText, bool validateString = true)
	{
		this.oldInputText = oldText;
		global::Steamworks.EGamepadTextInputLineMode eLineInputMode = global::Steamworks.EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine;
		if (multiLine)
		{
			eLineInputMode = global::Steamworks.EGamepadTextInputLineMode.k_EGamepadTextInputLineModeMultipleLines;
		}
		return global::Steamworks.SteamUtils.ShowGamepadTextInput(global::Steamworks.EGamepadTextInputMode.k_EGamepadTextInputModeNormal, eLineInputMode, title, maxChar, oldText);
	}

	private void OnVirtualKeyboardClosed(global::Steamworks.GamepadTextInputDismissed_t callback)
	{
		if (callback.m_bSubmitted)
		{
			uint enteredGamepadTextLength = global::Steamworks.SteamUtils.GetEnteredGamepadTextLength();
			if (enteredGamepadTextLength == 0U)
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.OnVirtualKeyboardCB(false, this.oldInputText);
				return;
			}
			string str;
			if (global::Steamworks.SteamUtils.GetEnteredGamepadTextInput(out str, enteredGamepadTextLength + 1U))
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.OnVirtualKeyboardCB(true, str);
				return;
			}
		}
		global::PandoraSingleton<global::Hephaestus>.Instance.OnVirtualKeyboardCB(false, this.oldInputText);
	}

	public void EngageUser()
	{
	}

	public void LockUserEngagement()
	{
	}

	public void SetRichPresence(global::Hephaestus.RichPresenceId presId, bool active)
	{
	}

	public void GetUserPicture(global::Hephaestus.UserPictureSize sizeId)
	{
	}

	public void UpdateGameProgress()
	{
	}

	public bool IsChatRestricted()
	{
		return false;
	}

	public void CheckNetworkServicesAvailability(global::System.Action<bool> callback)
	{
		callback(true);
	}

	public bool IsPrivilegeRestricted(global::Hephaestus.RestrictionId restrictionId)
	{
		return false;
	}

	public void CheckNetworkServicesAvailability(global::System.Action<bool, string> callback)
	{
		callback(true, null);
	}

	public void MultiplayerRoundStart()
	{
	}

	public void MultiplayerRoundEnd()
	{
	}

	public void CheckPendingInvite()
	{
		throw new global::System.NotImplementedException();
	}

	public void CanReceiveMessages(global::System.Action<bool> cb)
	{
		if (cb != null)
		{
			cb(true);
		}
	}

	public bool IsPlayTogether()
	{
		return false;
	}

	public bool IsPlayTogetherPassive()
	{
		return false;
	}

	public void ResetPlayTogether(bool setPassive)
	{
	}

	public void InitVoiceChat()
	{
	}

	public const uint GAME_ID = 276810U;

	public const uint ALL_LANGS = 276822U;

	public const uint GLOBADIER_DLC_ID = 434040U;

	public const uint SMUGGLER_DLC_ID = 434041U;

	public const uint PRIEST_OF_ULRICH_DLC_ID = 450810U;

	public const uint DOOMWEAVER_DLC_ID = 450811U;

	public const uint WITCH_HUNTERS_DLC_ID = 450812U;

	public const uint UNDEAD_DLC_ID = 534990U;

	private global::Matchmaking matchmaking;

	private global::Networking networking;

	private bool shouldUploadStats;

	private bool requestedStats;

	public string opponentName;

	private global::Steamworks.SteamAPIWarningMessageHook_t SteamAPIWarningMessageHook;

	private global::System.Collections.Generic.List<global::SupportedLanguage> availableLangs = new global::System.Collections.Generic.List<global::SupportedLanguage>();

	private global::Steamworks.CallResult<global::Steamworks.NumberOfCurrentPlayers_t> steamCbNumberOfPlayers;

	private global::Steamworks.Callback<global::Steamworks.GamepadTextInputDismissed_t> steamCbVKClosed;

	private string oldInputText;
}
