using System;
using System.Collections;
using System.Collections.Generic;

public interface IHephaestus
{
	void OnDestroy();

	void Update();

	bool IsInitialized();

	global::System.Collections.IEnumerator Init();

	void Reset();

	bool IsOnline();

	string GetOfflineReason();

	void CreateLobby(string name, global::Hephaestus.LobbyPrivacy privacy);

	void LeaveLobby();

	void JoinLobby(ulong lobbyId);

	void CancelJoinLobby();

	void SetLobbyData(string key, string value);

	void SetLobbyJoinable(bool joinable);

	void InviteFriends();

	void SearchLobbies();

	void OpenStore(global::Hephaestus.DlcId DLCId);

	void OpenCommunity();

	string GetUserName();

	ulong GetUserId();

	void DisplayOtherPlayerProfile();

	void RefreshSaveInfo();

	long GetFileTimeStamp(string fileName);

	bool FileExists(string fileName);

	void FileRead(string fileName);

	void FileWrite(string fileName, byte[] data);

	void FileDelete(string fileName);

	void DisconnectFromUser(ulong id);

	void ResetNetwork();

	void SetDataReceivedCallback(global::Hephaestus.OnDataReceivedCallback cb);

	void Send(bool reliable, ulong uId, byte[] data);

	global::System.Collections.Generic.List<global::SupportedLanguage> GetAvailableLanguages();

	void GetDefaultLocale(global::System.Action<global::SupportedLanguage> callback);

	void IncrementStat(global::Hephaestus.StatId stat, int increment);

	void UnlockAchievement(global::Hephaestus.TrophyId achievement);

	bool IsAchievementUnlocked(global::Hephaestus.TrophyId achievement);

	void UpdateGameProgress();

	void RequestNumberOfCurrentPlayers();

	bool OwnsDLC(global::Hephaestus.DlcId dlcId);

	bool ShowVirtualKeyboard(bool multiLine, string title, uint maxChar, string oldText, bool validateString = true);

	void LockUserEngagement();

	void EngageUser();

	void SetRichPresence(global::Hephaestus.RichPresenceId presId, bool active);

	void GetUserPicture(global::Hephaestus.UserPictureSize sizeId);

	bool IsPrivilegeRestricted(global::Hephaestus.RestrictionId restrictionId);

	void CheckNetworkServicesAvailability(global::System.Action<bool, string> callback);

	void MultiplayerRoundStart();

	void MultiplayerRoundEnd();

	void CheckPendingInvite();

	string GetOpponentUserName();

	bool IsPlayTogether();

	void CanReceiveMessages(global::System.Action<bool> cb);

	void ResetPlayTogether(bool setPassive);

	bool IsPlayTogetherPassive();

	void InitVoiceChat();
}
