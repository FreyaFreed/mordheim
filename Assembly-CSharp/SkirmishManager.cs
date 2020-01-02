using System;
using System.Collections.Generic;
using UnityEngine;

public class SkirmishManager : global::PandoraSingleton<global::SkirmishManager>
{
	private void Start()
	{
		this.createPopup.gameObject.SetActive(false);
		this.warbandPopup.gameObject.SetActive(false);
		this.messagePopup.gameObject.SetActive(false);
		global::System.Collections.Generic.List<global::DeploymentScenarioMapLayoutData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>("skirmish", "1");
		this.skirmishMaps.Clear();
		global::System.Collections.Generic.List<global::MissionMapId> list2 = new global::System.Collections.Generic.List<global::MissionMapId>();
		foreach (global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData in list)
		{
			if (list2.IndexOf(deploymentScenarioMapLayoutData.MissionMapId, global::MissionMapIdComparer.Instance) == -1)
			{
				list2.Add(deploymentScenarioMapLayoutData.MissionMapId);
				global::SkirmishMap skirmishMap = new global::SkirmishMap();
				skirmishMap.mapData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapData>((int)deploymentScenarioMapLayoutData.MissionMapId);
				this.skirmishMaps.Add(skirmishMap);
			}
		}
		this.skirmishMaps.Sort(new global::SkirmishMapSorter());
		foreach (global::SkirmishMap skirmishMap2 in this.skirmishMaps)
		{
			skirmishMap2.layouts = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapLayoutData>("fk_mission_map_id", ((int)skirmishMap2.mapData.Id).ToString());
			skirmishMap2.gameplays = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapGameplayData>("fk_mission_map_id", ((int)skirmishMap2.mapData.Id).ToString());
			skirmishMap2.deployments = new global::System.Collections.Generic.List<global::DeploymentInfo>();
			foreach (global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData2 in list)
			{
				if (deploymentScenarioMapLayoutData2.MissionMapId == skirmishMap2.mapData.Id)
				{
					global::DeploymentInfo deploymentInfo = new global::DeploymentInfo();
					deploymentInfo.scenarioData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioData>((int)deploymentScenarioMapLayoutData2.DeploymentScenarioId);
					global::System.Collections.Generic.List<global::DeploymentScenarioSlotData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioSlotData>("fk_deployment_scenario_id", ((int)deploymentScenarioMapLayoutData2.DeploymentScenarioId).ToString());
					deploymentInfo.slots = new global::System.Collections.Generic.List<global::DeploymentData>();
					foreach (global::DeploymentScenarioSlotData deploymentScenarioSlotData in list3)
					{
						deploymentInfo.slots.Add(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentData>((int)deploymentScenarioSlotData.DeploymentId));
					}
					skirmishMap2.deployments.Add(deploymentInfo);
				}
			}
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.RegisterToHermes();
		global::PandoraSingleton<global::Pan>.Instance.GetSound("turn_begin", true, delegate(global::UnityEngine.AudioClip clip)
		{
			this.playerJoinSound = clip;
		});
		global::PandoraSingleton<global::Pan>.Instance.GetSound("turn_end", true, delegate(global::UnityEngine.AudioClip clip)
		{
			this.playerLeaveSound = clip;
		});
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SKIRMISH_LOBBY_JOINED, new global::DelReceiveNotice(this.OnLobbyEntered));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SKIRMISH_LOBBY_LEFT, new global::DelReceiveNotice(this.OnLobbyLeft));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SKIRMISH_LOBBY_KICKED, new global::DelReceiveNotice(this.LeaveLobby));
	}

	public void BuildUnitPosition()
	{
		global::Warband warband = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband;
		this.unitsPosition = new global::System.Collections.Generic.List<int>(new int[warband.Units.Count]);
		for (int i = 0; i < warband.Units.Count; i++)
		{
			this.unitsPosition[i] = warband.Units[i].UnitSave.warbandSlotIndex;
		}
	}

	private void OnDestroy()
	{
		global::PandoraSingleton<global::MissionStartData>.Instance.RemoveFromHermes();
	}

	private void Update()
	{
		if (this.joiningLobby && global::UnityEngine.Time.time > this.lobbyJoinTimer)
		{
			this.CancelJoinLobby();
			this.messagePopup.Show("join_lobby_title_timed_out", "join_lobby_desc_timed_out", null, false, false);
		}
	}

	public void OnCreateGame(bool exhibition = true, global::System.Action onCancelCreateGame = null, bool silent = false)
	{
		this.cancelPopupCallback = onCancelCreateGame;
		if (exhibition)
		{
			this.BuildUnitPosition();
			int skirmishRating = global::PandoraSingleton<global::HideoutManager>.instance.WarbandCtrlr.GetSkirmishRating(this.unitsPosition);
			if (!silent)
			{
				this.createPopup.Show("menu_skirmish_create_game", (!global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline()) ? "lobby_choose_privacy_and_name_offline" : "lobby_choose_privacy_and_name_online", true, skirmishRating, new global::System.Action<bool>(this.OnCreateExhibitionPopup), false);
			}
			else
			{
				string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_name_default", new string[]
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.GetUserName()
				});
				this.ShowJoinPopup(stringById);
				global::PandoraSingleton<global::Hephaestus>.Instance.CreateLobby(stringById, global::Hephaestus.LobbyPrivacy.PRIVATE, new global::Hephaestus.OnLobbyCreatedCallback(this.OnPlayTogetherExhibitionLobbyCreated));
			}
		}
		else
		{
			this.unitsPosition = null;
			int skirmishRating2 = global::PandoraSingleton<global::HideoutManager>.instance.WarbandCtrlr.GetSkirmishRating(this.unitsPosition);
			if (global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline())
			{
				if (!silent)
				{
					this.createPopup.Show("menu_skirmish_create_game", "lobby_choose_privacy_and_name_online", false, skirmishRating2, new global::System.Action<bool>(this.OnCreateContestPopup), false);
				}
				else
				{
					string stringById2 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_name_default", new string[]
					{
						global::PandoraSingleton<global::Hephaestus>.Instance.GetUserName()
					});
					this.ShowJoinPopup(stringById2);
					global::PandoraSingleton<global::Hephaestus>.Instance.CreateLobby(stringById2, global::Hephaestus.LobbyPrivacy.PRIVATE, new global::Hephaestus.OnLobbyCreatedCallback(this.OnPlayTogetherContestLobbyCreated));
				}
			}
			else
			{
				this.messagePopup.Show("menu_skirmish_create_game", "menu_skirmish_cant_contest_offline", delegate(bool confirm)
				{
					this.cancelPopupCallback();
				}, false, false);
			}
		}
	}

	private void OnPlayTogetherContestLobbyCreated(ulong lobbyid, bool success)
	{
		this.OnLobbyCreatedCallback(lobbyid, success, false, "100", "5000");
	}

	private void OnPlayTogetherExhibitionLobbyCreated(ulong lobbyid, bool success)
	{
		this.OnLobbyCreatedCallback(lobbyid, success, true, "100", "5000");
	}

	private void OnCreateExhibitionPopup(bool isConfirm)
	{
		this.OnCreatePopup(isConfirm, true, new global::Hephaestus.OnLobbyCreatedCallback(this.OnLobbyExhibitionCreatedCallback));
	}

	private void OnCreateContestPopup(bool isConfirm)
	{
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline())
		{
			this.OnCreatePopup(isConfirm, false, new global::Hephaestus.OnLobbyCreatedCallback(this.OnLobbyContestCreatedCallback));
		}
		else
		{
			this.messagePopup.Show("menu_skirmish_create_game", "menu_skirmish_cant_contest_offline", delegate(bool confirm)
			{
				this.cancelPopupCallback();
			}, false, false);
		}
	}

	private void OnCreatePopup(bool isConfirm, bool isExhibition, global::Hephaestus.OnLobbyCreatedCallback lobbyCreateCb)
	{
		if (isConfirm)
		{
			global::PandoraDebug.LogInfo("OnCreatePopup - confirmed = ", "SKIRMISH", null);
			int curSel = this.createPopup.lobbyPrivacy.CurSel;
			string text = this.createPopup.lobbyName.textComponent.text;
			global::PandoraSingleton<global::Hephaestus>.Instance.CreateLobby(text, (global::Hephaestus.LobbyPrivacy)curSel, lobbyCreateCb);
			if (global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline() && curSel != 3)
			{
				this.ShowJoinPopup(text);
			}
		}
		else if (this.cancelPopupCallback != null)
		{
			this.cancelPopupCallback();
		}
	}

	private void ShowJoinPopup(string lobbyName = null)
	{
		string text = lobbyName ?? this.createPopup.lobbyName.textComponent.text;
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_joining_desc", new string[]
		{
			text
		});
		string stringById2 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_joining_title");
		this.messagePopup.ShowLocalized(stringById2, stringById, null, true, false);
	}

	private void OnLobbyExhibitionCreatedCallback(ulong lobbyId, bool success)
	{
		string ratingMin = this.createPopup.GetRatingMin();
		string ratingMax = this.createPopup.GetRatingMax();
		this.OnLobbyCreatedCallback(lobbyId, success, true, ratingMin, ratingMax);
	}

	private void OnLobbyContestCreatedCallback(ulong lobbyId, bool success)
	{
		string ratingMin = this.createPopup.GetRatingMin();
		string ratingMax = this.createPopup.GetRatingMax();
		this.OnLobbyCreatedCallback(lobbyId, success, false, ratingMin, ratingMax);
	}

	private void OnLobbyCreatedCallback(ulong lobbyId, bool success, bool isExhibition, string ratingMin, string ratingMax)
	{
		if (success)
		{
			global::PandoraSingleton<global::Hermes>.Instance.StartHosting();
			if (isExhibition)
			{
				this.BuildUnitPosition();
			}
			else
			{
				this.unitsPosition = null;
			}
			global::PandoraSingleton<global::MissionStartData>.Instance.InitSkirmish(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr, this.unitsPosition, isExhibition);
			global::PandoraSingleton<global::Hephaestus>.Instance.SetLobbyData("warband", ((int)global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Id).ToLowerString());
			global::PandoraSingleton<global::Hephaestus>.Instance.SetLobbyData("exhibition", isExhibition.ToString());
			global::PandoraSingleton<global::Hephaestus>.Instance.SetLobbyData("rating_min", ratingMin);
			global::PandoraSingleton<global::Hephaestus>.Instance.SetLobbyData("rating_max", ratingMax);
			this.HideJoinPopup();
			global::PandoraSingleton<global::HideoutManager>.instance.StateMachine.ChangeState(4);
		}
		else
		{
			this.HideJoinPopup();
			this.messagePopup.Show("join_lobby_title_failed_to_create_lobby", "join_lobby_desc_failed_to_create_lobby", null, false, false);
		}
	}

	public void OnLobbyEntered()
	{
		ulong userId = (ulong)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		this.ready = false;
		global::PandoraSingleton<global::Hermes>.Instance.NewConnection(userId);
		if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1 || !global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.joinable)
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.SendKickPlayer(9);
		}
		else
		{
			global::PandoraDebug.LogDebug("OnLobbyEntered", "SKIRMISH", null);
			if (this.playerJoinSound != null)
			{
				global::PandoraSingleton<global::HideoutTabManager>.Instance.audioSource.PlayOneShot(this.playerJoinSound);
			}
			global::PandoraSingleton<global::Hephaestus>.Instance.SetLobbyJoinable(false);
		}
	}

	public void OnLobbyLeft()
	{
		global::PandoraDebug.LogDebug("OnLobbyLeft", "SKIRMISH", null);
		if (this.playerLeaveSound != null)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.audioSource.PlayOneShot(this.playerLeaveSound);
		}
		ulong num = (ulong)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		ulong userId = global::PandoraSingleton<global::Hephaestus>.Instance.GetUserId();
		if (global::PandoraSingleton<global::Hermes>.Instance.IsHost() && num != userId)
		{
			global::PandoraSingleton<global::Hermes>.Instance.RemoveConnection(num);
			if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1 && global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerTypeId == global::PlayerTypeId.PLAYER)
			{
				this.RemoveOpponent();
			}
		}
		else
		{
			this.LeaveLobby();
		}
	}

	public void OnKick(global::Hephaestus.LobbyConnexionResult result)
	{
		global::PandoraDebug.LogDebug("OnKick", "SKIRMISH", null);
		this.HideJoinPopup();
		this.ShowPopup(result);
		this.LeaveLobby();
	}

	public void LeaveLobby()
	{
		global::PandoraSingleton<global::Hephaestus>.Instance.LeaveLobby();
		global::PandoraSingleton<global::Hermes>.Instance.StopConnections(true);
		if (global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.GetActiveStateId() == 3)
		{
			this.HideJoinPopup();
		}
	}

	public void JoinLobby(int index)
	{
		this.JoinLobby(global::PandoraSingleton<global::Hephaestus>.Instance.Lobbies[index].id, global::PandoraSingleton<global::Hephaestus>.Instance.Lobbies[index].name, global::PandoraSingleton<global::Hephaestus>.Instance.Lobbies[index].isExhibition, global::PandoraSingleton<global::Hephaestus>.Instance.Lobbies[index].ratingMin, global::PandoraSingleton<global::Hephaestus>.Instance.Lobbies[index].ratingMax);
	}

	public void JoinLobby(ulong lobbyId, string lobbyName, bool isExhibition = false, int ratingMin = 0, int ratingMax = 5000)
	{
		string key;
		if (!isExhibition && !this.IsContestAvailable(out key))
		{
			this.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_joining_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(key), null, false, false);
		}
		else if (global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline())
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Joining lobbyId:",
				lobbyId,
				" (name:",
				lobbyName,
				")"
			}), "SKIRMISH", null);
			if (string.IsNullOrEmpty(lobbyName))
			{
				lobbyName = ".";
			}
			if (isExhibition)
			{
				this.BuildUnitPosition();
			}
			else
			{
				this.unitsPosition = null;
			}
			if (global::PandoraUtils.IsBetween(global::PandoraSingleton<global::HideoutManager>.instance.WarbandCtrlr.GetSkirmishRating(this.unitsPosition), ratingMin, ratingMax))
			{
				this.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_joining_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_joining_desc", new string[]
				{
					lobbyName
				}), new global::System.Action<bool>(this.OnJoiningLobbyPopupCancel), false, false);
				this.joiningLobby = true;
				this.lobbyJoinTimer = global::UnityEngine.Time.time + 30f;
				global::PandoraSingleton<global::Hephaestus>.Instance.JoinLobby(lobbyId, new global::Hephaestus.OnJoinLobbyCallback(this.OnJoinLobbyCallback), new global::Hermes.OnConnectedCallback(this.OnServerConnect));
			}
			else
			{
				this.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_joining_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_rating_range", new string[]
				{
					ratingMin.ToConstantString(),
					ratingMax.ToConstantString()
				}), null, false, false);
			}
		}
		else
		{
			this.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_joining_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_status_offline"), null, false, false);
		}
	}

	private void OnJoinLobbyCallback(global::Hephaestus.LobbyConnexionResult result)
	{
		if (result != global::Hephaestus.LobbyConnexionResult.SUCCESS)
		{
			this.HideJoinPopup();
			this.joiningLobby = false;
			this.ShowPopup(result);
		}
		else
		{
			this.ready = false;
		}
	}

	public void ShowPopup(global::Hephaestus.LobbyConnexionResult result)
	{
		string textId;
		string titleId;
		switch (result)
		{
		case global::Hephaestus.LobbyConnexionResult.DOES_NOT_EXIST:
			textId = "join_lobby_desc_no_longer_exist";
			titleId = "join_lobby_title_no_longer_exist";
			break;
		case global::Hephaestus.LobbyConnexionResult.NOT_ALLOWED:
			textId = "join_lobby_desc_not_allowed";
			titleId = "join_lobby_title_not_allowed";
			break;
		case global::Hephaestus.LobbyConnexionResult.BLOCKED_A_MEMBER:
			textId = "join_lobby_desc_blocked_a_member";
			titleId = "join_lobby_title_blocked_a_member";
			break;
		case global::Hephaestus.LobbyConnexionResult.MEMBER_BLOCKED_YOU:
			textId = "join_lobby_desc_member_blocked_you";
			titleId = "join_lobby_title_member_blocked_you";
			break;
		case global::Hephaestus.LobbyConnexionResult.LIMITED_USER:
			textId = "join_lobby_desc_limited_user";
			titleId = "join_lobby_title_limited_user";
			break;
		case global::Hephaestus.LobbyConnexionResult.COMMUNITY_BANNED:
			textId = "join_lobby_desc_community_banned";
			titleId = "join_lobby_title_community_banned";
			break;
		case global::Hephaestus.LobbyConnexionResult.CLAN_DISABLED:
			textId = "join_lobby_desc_clan_disabled";
			titleId = "join_lobby_title_clan_disabled";
			break;
		case global::Hephaestus.LobbyConnexionResult.BANNED:
			textId = "join_lobby_desc_banned";
			titleId = "join_lobby_title_banned";
			break;
		case global::Hephaestus.LobbyConnexionResult.FULL:
			textId = "join_lobby_desc_full";
			titleId = "join_lobby_title_full";
			break;
		case global::Hephaestus.LobbyConnexionResult.UNEXPECTED_ERROR:
			textId = "join_lobby_desc_unknown_error";
			titleId = "join_lobby_title_unknown_error";
			break;
		case global::Hephaestus.LobbyConnexionResult.VERSION_MISMATCH:
			textId = "join_lobby_desc_version_mismatch";
			titleId = "join_lobby_title_version_mismatch";
			break;
		case global::Hephaestus.LobbyConnexionResult.KICKED:
			textId = "join_lobby_desc_kicked";
			titleId = "join_lobby_title_kicked";
			break;
		default:
			textId = "join_lobby_desc_unknown_error";
			titleId = "join_lobby_title_unknown_error";
			break;
		}
		this.messagePopup.Show(titleId, textId, null, false, false);
	}

	private void OnJoiningLobbyPopupCancel(bool ok)
	{
		this.CancelJoinLobby();
	}

	private void CancelJoinLobby()
	{
		this.joiningLobby = false;
		this.HideJoinPopup();
		global::PandoraSingleton<global::Hephaestus>.Instance.CancelJoinLobby();
	}

	public void SendReady()
	{
		this.ready = !this.ready;
		global::PandoraSingleton<global::MissionStartData>.Instance.SendReady(this.ready);
	}

	private void OnServerConnect()
	{
		global::PandoraDebug.LogDebug("OnServerConnect", "uncategorised", null);
		this.joiningLobby = false;
		this.HideJoinPopup();
		global::PandoraSingleton<global::MissionStartData>.Instance.OnNetworkConnected(global::PandoraSingleton<global::HideoutManager>.instance.WarbandCtrlr, this.unitsPosition);
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(4);
	}

	private void HideJoinPopup()
	{
		this.messagePopup.Hide();
	}

	public void AddAIOpponent(global::WarbandData wbData = null)
	{
		global::PandoraSingleton<global::Hephaestus>.Instance.SetLobbyJoinable(false);
		bool impressive = false;
		int num = 0;
		int num2 = 0;
		global::MissionWarbandSave missionWarbandSave = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[0];
		for (int i = 0; i < missionWarbandSave.Units.Count; i++)
		{
			num2 = global::UnityEngine.Mathf.Max(num2, global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)missionWarbandSave.Units[i].rankId).Rank);
			int id = missionWarbandSave.Units[i].stats.id;
			global::UnitTypeId unitTypeId = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>(id).UnitTypeId;
			switch (global::Unit.GetUnitTypeId(missionWarbandSave.Units[i], unitTypeId))
			{
			case global::UnitTypeId.IMPRESSIVE:
				impressive = true;
				break;
			case global::UnitTypeId.HERO_1:
			case global::UnitTypeId.HERO_2:
			case global::UnitTypeId.HERO_3:
				num++;
				break;
			}
		}
		if (this.getProcWarbandCoroutine != null)
		{
			base.StopCoroutine(this.getProcWarbandCoroutine);
		}
		if (wbData == null)
		{
			global::System.Collections.Generic.List<global::WarbandData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>("basic", "1");
			int index = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, list.Count);
			wbData = list[index];
		}
		string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_loading");
		string stringById2 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_ai");
		global::PandoraSingleton<global::MissionStartData>.Instance.AddFightingWarband(wbData.Id, global::CampaignWarbandId.NONE, stringById, stringById, stringById2, missionWarbandSave.Rank, missionWarbandSave.Rating, 1, global::PlayerTypeId.AI, new string[0]);
		this.getProcWarbandCoroutine = base.StartCoroutine(global::Mission.GetProcWarband(missionWarbandSave.Rating, missionWarbandSave.Rank, missionWarbandSave.Units.Count, impressive, wbData, num, num2, new global::System.Action<global::WarbandSave>(this.OnProcWarbandGenerated)));
	}

	private void OnProcWarbandGenerated(global::WarbandSave warSave)
	{
		global::WarbandMenuController ctrlr = new global::WarbandMenuController(warSave);
		if (global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.GetActiveStateId() == 4 && global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1 && global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].Name == global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_loading"))
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.RemoveAt(1);
			global::PandoraSingleton<global::MissionStartData>.Instance.AddFightingWarband(ctrlr, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_ai"), global::PlayerTypeId.AI);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SKIRMISH_OPPONENT_JOIN);
		}
	}

	public void RemoveOpponent()
	{
		if (!global::PandoraSingleton<global::MissionStartData>.Instance.IsLocked)
		{
			if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1)
			{
				global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.RemoveAt(1);
			}
			global::PandoraSingleton<global::Hephaestus>.Instance.SetLobbyJoinable(true);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SKIRMISH_OPPONENT_LEAVE);
		}
	}

	public void LaunchMission()
	{
		global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.rating = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].Rating;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerTypeId == global::PlayerTypeId.PLAYER)
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.RefreshDifficulty(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetRating(), false);
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.Lock();
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMissionStartData();
		global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_MISSION, global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerTypeId == global::PlayerTypeId.PLAYER, false);
	}

	public bool IsContestAvailable(out string reason)
	{
		reason = string.Empty;
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lateShipmentCount >= global::Constant.GetInt(global::ConstantId.MAX_SHIPMENT_FAIL))
		{
			reason = "na_hideout_late_shipment_count";
			return false;
		}
		if (global::PandoraSingleton<global::HideoutManager>.Instance.IsPostMission())
		{
			reason = "na_hideout_post_mission";
			return false;
		}
		if (!global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.HasLeader(true))
		{
			reason = "na_hideout_active_leader";
			return false;
		}
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetActiveUnitsCount() < global::Constant.GetInt(global::ConstantId.MIN_MISSION_UNITS))
		{
			reason = "na_hideout_min_active_unit";
			return false;
		}
		return true;
	}

	private const float MAX_LOBBY_CONNEXION_DELAY = 30f;

	public global::SkirmishCreatePopup createPopup;

	public global::SkirmishWarbandPopup warbandPopup;

	public global::ConfirmationPopupView messagePopup;

	public global::System.Collections.Generic.List<global::SkirmishMap> skirmishMaps = new global::System.Collections.Generic.List<global::SkirmishMap>();

	public bool ready;

	private global::System.Action cancelPopupCallback;

	private float lobbyJoinTimer;

	private bool joiningLobby;

	private global::UnityEngine.AudioClip playerJoinSound;

	private global::UnityEngine.AudioClip playerLeaveSound;

	public global::System.Collections.Generic.List<int> unitsPosition;

	private global::UnityEngine.Coroutine getProcWarbandCoroutine;
}
