using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HideoutLobby : global::ICheapState
{
	public HideoutLobby(global::HideoutManager mng, global::HideoutCamAnchor anchor)
	{
		this.camAnchor = anchor;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.warbandChanged = true;
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SKIRMISH_OPPONENT_JOIN, new global::DelReceiveNotice(this.SetOpponent));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SKIRMISH_OPPONENT_LEAVE, new global::DelReceiveNotice(this.OpponentLeft));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SKIRMISH_OPPONENT_READY, new global::DelReceiveNotice(this.OpponentReady));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SKIRMISH_LOBBY_UPDATED, new global::DelReceiveNotice(this.RefreshLobbyData));
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.LOBBY_DETAIL
		});
		this.chatModule = null;
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline())
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
			{
				global::ModuleId.LOBBY_PLAYERS,
				global::ModuleId.CHAT
			});
			this.chatModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::LobbyChatModule>(global::ModuleId.CHAT);
			this.chatModule.Setup();
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
			{
				global::ModuleId.LOBBY_PLAYERS
			});
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.TITLE,
			global::ModuleId.DESC
		});
		this.titleModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE);
		this.descModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::DescriptionModule>(global::ModuleId.DESC);
		this.descModule.Show(false);
		this.lobbyDetail = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::LobbyDetailModule>(global::ModuleId.LOBBY_DETAIL);
		if (global::PandoraSingleton<global::Hermes>.Instance.IsHost())
		{
			this.lobbyDetail.SetLobbyData(global::PandoraSingleton<global::Hephaestus>.Instance.Lobby, global::PandoraSingleton<global::Hermes>.Instance.IsHost());
		}
		this.lobbyDetail.LinkDescriptions(new global::UnityEngine.Events.UnityAction<string, string>(this.descModule.desc.Set), new global::UnityEngine.Events.UnityAction<string, string>(this.descModule.desc.SetLocalized));
		this.playersMod = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::LobbyPlayersModule>(global::ModuleId.LOBBY_PLAYERS);
		this.playersMod.RefreshPlayers();
		this.swapModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandSwapModule>(global::ModuleId.SWAP);
		if (!this.swapModule.initialized)
		{
			this.swapModule.Init();
		}
		this.swapModule.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband, global::PandoraSingleton<global::SkirmishManager>.Instance.unitsPosition, global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.ratingMin, global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.ratingMax);
		global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[3].DestroyContent();
		global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[2].DestroyContent();
		this.lastIsExhibition = global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.isExhibition;
		this.SetTitle();
		global::PandoraSingleton<global::Hephaestus>.Instance.SetRichPresence((!global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.isExhibition) ? global::Hephaestus.RichPresenceId.LOBBY_CONTEST : global::Hephaestus.RichPresenceId.LOBBY_EXHIBITION, true);
		if (!global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[1].IsOccupied())
		{
			global::UnityEngine.Cloth cloth = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.banner.GetComponentsInChildren<global::UnityEngine.Cloth>(true)[0];
			cloth.enabled = false;
			global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[1].SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.banner);
			cloth.enabled = true;
			global::UnitMenuController leader = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetLeader();
			global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[0].SetContent(leader.gameObject);
			leader.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
		}
		this.SetOpponent();
		global::PandoraSingleton<global::Hermes>.Instance.DoNotDisconnectMode = false;
		this.SetButtons();
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.SKIRMISH_OPPONENT_JOIN, new global::DelReceiveNotice(this.SetOpponent));
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.SKIRMISH_OPPONENT_LEAVE, new global::DelReceiveNotice(this.OpponentLeft));
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.SKIRMISH_OPPONENT_READY, new global::DelReceiveNotice(this.OpponentReady));
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.SKIRMISH_LOBBY_UPDATED, new global::DelReceiveNotice(this.RefreshLobbyData));
		global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[3].DestroyContent();
		global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[2].DestroyContent();
		if (global::PandoraSingleton<global::LobbyPlayerPopup>.Exists())
		{
			global::PandoraSingleton<global::LobbyPlayerPopup>.Instance.gameObject.SetActive(false);
		}
	}

	void global::ICheapState.Update()
	{
		if (global::PandoraSingleton<global::Hephaestus>.Instance.Lobby == null)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(3);
			return;
		}
		if (!global::PandoraSingleton<global::Hermes>.Instance.IsHost() && !global::PandoraSingleton<global::Hermes>.Instance.IsConnected())
		{
			global::PandoraDebug.LogDebug("Client leaving lobby because there is no connection", "uncategorised", null);
			global::PandoraSingleton<global::Hephaestus>.Instance.LeaveLobby();
			return;
		}
		if (this.lastIsExhibition != global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.isExhibition)
		{
			this.lastIsExhibition = global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.isExhibition;
			this.titleModule.Set((!global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.isExhibition) ? "menu_skirmish_contest" : "menu_skirmish_exhibition", true);
			global::PandoraSingleton<global::Hephaestus>.Instance.SetRichPresence((!global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.isExhibition) ? global::Hephaestus.RichPresenceId.LOBBY_CONTEST : global::Hephaestus.RichPresenceId.LOBBY_EXHIBITION, true);
		}
		string empty = string.Empty;
		if (this.warbandChanged)
		{
			this.canLaunchMission = this.CanLaunchMission();
		}
		if (global::PandoraSingleton<global::Hermes>.Instance.IsHost())
		{
			bool flag = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.deployCount > 1 && global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.deployCount == global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count;
			bool visible = this.canLaunchMission && flag && global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].IsReady;
			this.lobbyDetail.SetLaunchButtonVisible(flag, false);
			if (flag)
			{
				this.lobbyDetail.SetLaunchButtonVisible(visible, true);
			}
		}
		else
		{
			this.lobbyDetail.SetLaunchButtonVisible(this.canLaunchMission, false);
		}
		if (this.chatModule != null)
		{
			float axis = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0);
			if (axis != 0f)
			{
				this.chatModule.messages.ForceScroll(axis < 0f, false);
			}
		}
		this.playersMod.SetErrorMessage(empty);
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::System.Collections.IEnumerator SetButtonsAsync()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.DeactivateAllButtons();
		yield return new global::UnityEngine.WaitForSeconds(1f);
		this.SetButtons();
		yield break;
	}

	private void SetTitle()
	{
		if (global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.privacy == global::Hephaestus.LobbyPrivacy.OFFLINE)
		{
			this.titleModule.Set("menu_skirmish_exhibition", true);
			global::PandoraSingleton<global::SkirmishManager>.Instance.AddAIOpponent(null);
		}
		else
		{
			this.titleModule.Set((!global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.isExhibition) ? "menu_skirmish_contest" : "menu_skirmish_exhibition", true);
		}
	}

	private void SetButtons()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "menu_leave", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(global::PandoraSingleton<global::SkirmishManager>.Instance.LeaveLobby), false, true);
		this.SetProfileButton();
		this.lobbyDetail.swapButton.SetAction(string.Empty, "hideout_swap_unit", 0, false, this.lobbyDetail.swapIcon, null);
		this.lobbyDetail.swapButton.OnAction(new global::UnityEngine.Events.UnityAction(this.ShowMissionPrep), false, true);
		if (global::PandoraSingleton<global::Hermes>.Instance.IsHost())
		{
			this.lobbyDetail.launchButton.SetAction(string.Empty, "menu_launch_mission", 0, false, this.lobbyDetail.launchIcon, null);
			this.lobbyDetail.launchButton.OnAction(new global::UnityEngine.Events.UnityAction(global::PandoraSingleton<global::SkirmishManager>.Instance.LaunchMission), false, true);
			this.lobbyDetail.SetLaunchButtonVisible(false, true);
		}
		else
		{
			this.UpdateReadyButton(global::PandoraSingleton<global::SkirmishManager>.Instance.ready);
			this.lobbyDetail.launchButton.OnAction(delegate
			{
				global::PandoraSingleton<global::SkirmishManager>.Instance.SendReady();
				this.SetButtons();
			}, false, true);
		}
		if (this.chatModule != null)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.SetAction("show_chat", "menu_chat", 0, false, null, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.OnAction(new global::UnityEngine.Events.UnityAction(this.ShowChat), false, true);
			if (global::PandoraSingleton<global::Hermes>.Instance.IsHost() && global::PandoraSingleton<global::Hermes>.Instance.IsConnected() && global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1)
			{
				global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.SetAction("rename_warband", "menu_skirmish_kick_player", 0, false, null, null);
				global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.OnAction(new global::UnityEngine.Events.UnityAction(this.KickPlayer), false, true);
			}
			else
			{
				global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
			}
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
		}
	}

	private void SetProfileButton()
	{
		this.lobbyDetail.displayProfile.gameObject.SetActive(false);
	}

	private void ShowChat()
	{
		if (this.chatModule != null)
		{
			this.chatModule.Select();
		}
	}

	private void OnChatTextEntered(bool success, string text)
	{
		if (success)
		{
			this.chatModule.SendChat(text);
		}
	}

	private void UpdateReadyButton(bool ready)
	{
		if (!ready)
		{
			this.lobbyDetail.launchButton.SetAction(string.Empty, "menu_set_ready", 0, false, this.lobbyDetail.launchIcon, null);
		}
		else
		{
			this.lobbyDetail.launchButton.SetAction(string.Empty, "menu_not_ready", 0, false, this.lobbyDetail.launchIcon, null);
		}
	}

	public bool CanLaunchMission()
	{
		string empty = string.Empty;
		return this.swapModule != null && this.swapModule.CanLaunchMission(out empty);
	}

	private void ShowMissionPrep()
	{
		if (global::PandoraSingleton<global::LobbyPlayerPopup>.Exists() && global::PandoraSingleton<global::LobbyPlayerPopup>.Instance.isActiveAndEnabled)
		{
			global::PandoraSingleton<global::LobbyPlayerPopup>.Instance.GetComponent<global::UnityEngine.CanvasGroup>().alpha = 0f;
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.SWAP,
			global::ModuleId.TITLE
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(false, new global::ModuleId[0]);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(false, new global::ModuleId[0]);
		this.swapModule.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband, new global::System.Action<bool>(this.OnPrepConfirm), false, false, !global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.isExhibition, global::PandoraSingleton<global::SkirmishManager>.Instance.unitsPosition, false, global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.ratingMin, global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.ratingMax);
	}

	private void OnPrepConfirm(bool confirm)
	{
		if (this.swapModule.HasChanged)
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.RefreshMyWarband(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr, global::PandoraSingleton<global::SkirmishManager>.Instance.unitsPosition);
			if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1 && global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerTypeId == global::PlayerTypeId.AI)
			{
				global::PandoraSingleton<global::SkirmishManager>.Instance.RemoveOpponent();
				global::PandoraSingleton<global::SkirmishManager>.Instance.AddAIOpponent(null);
			}
			this.warbandChanged = true;
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.LOBBY_DETAIL
		});
		this.chatModule = null;
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline() && global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.privacy != global::Hephaestus.LobbyPrivacy.OFFLINE)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
			{
				global::ModuleId.LOBBY_PLAYERS,
				global::ModuleId.CHAT
			});
			this.chatModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::LobbyChatModule>(global::ModuleId.CHAT);
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
			{
				global::ModuleId.LOBBY_PLAYERS
			});
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.DESC,
			global::ModuleId.TITLE
		});
		global::TitleModule moduleCenter = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE);
		moduleCenter.Set((!global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.isExhibition) ? "menu_skirmish_contest" : "menu_skirmish_exhibition", true);
		if (global::PandoraSingleton<global::LobbyPlayerPopup>.Exists() && global::PandoraSingleton<global::LobbyPlayerPopup>.Instance.isActiveAndEnabled)
		{
			global::PandoraSingleton<global::LobbyPlayerPopup>.Instance.GetComponent<global::UnityEngine.CanvasGroup>().alpha = 1f;
		}
		if (!global::PandoraSingleton<global::Hermes>.Instance.IsHost())
		{
			this.RefreshLobbyData();
		}
		this.playersMod.RefreshPlayers();
		this.descModule.Show(false);
		this.SetButtons();
		this.lobbyDetail.LinkDescriptions(new global::UnityEngine.Events.UnityAction<string, string>(this.descModule.desc.Set), new global::UnityEngine.Events.UnityAction<string, string>(this.descModule.desc.SetLocalized));
		this.lobbyDetail.ForceMapReselect();
		this.lobbyDetail.StartCoroutine(this.SelectOnNextFrame());
	}

	private global::System.Collections.IEnumerator SelectOnNextFrame()
	{
		yield return null;
		this.lobbyDetail.swapButton.SetSelected(true);
		yield break;
	}

	private void RefreshLobbyData()
	{
		if (this.lobbyDetail.isActiveAndEnabled)
		{
			this.lobbyDetail.SetLobbyData(global::PandoraSingleton<global::Hephaestus>.Instance.Lobby, false);
			this.lobbyDetail.LinkDescriptions(new global::UnityEngine.Events.UnityAction<string, string>(this.descModule.desc.Set), new global::UnityEngine.Events.UnityAction<string, string>(this.descModule.desc.SetLocalized));
		}
		this.SetTitle();
	}

	private void SetOpponent()
	{
		this.lobbyDetail.RefreshPlayerNames();
		this.playersMod.RefreshPlayers();
		if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1 || !global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.joinable)
		{
			if (global::PandoraSingleton<global::Hermes>.Instance.IsHost())
			{
				if (global::PandoraSingleton<global::Hermes>.Instance.IsConnected())
				{
					global::MissionWarbandSave missionWarbandSave = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1];
					global::PandoraSingleton<global::LobbyPlayerPopup>.Instance.Show(missionWarbandSave.PlayerName, "menu_not_ready");
				}
			}
			else
			{
				global::PandoraSingleton<global::LobbyPlayerPopup>.Instance.Show(null, "menu_not_ready");
			}
			global::WarbandMenuController warbandMenuController = new global::WarbandMenuController(global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[(!global::PandoraSingleton<global::Hermes>.Instance.IsHost()) ? 0 : 1].ToWarbandSave());
			global::MenuNode opponentNode = global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[2];
			opponentNode.DestroyContent();
			opponentNode.gameObject.SetActive(false);
			global::HideoutLobby.currentLoadedCharacterIdx++;
			int idx = global::HideoutLobby.currentLoadedCharacterIdx;
			global::Unit leaderUnit = warbandMenuController.GetLeaderUnit();
			if (leaderUnit != null)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.StartCoroutine(global::UnitMenuController.LoadUnitPrefabAsync(leaderUnit, delegate(global::UnityEngine.GameObject obj)
				{
					if (idx == global::HideoutLobby.currentLoadedCharacterIdx)
					{
						opponentNode.gameObject.SetActive(true);
						opponentNode.SetContent(obj);
					}
					else
					{
						global::UnityEngine.Object.Destroy(obj);
					}
				}, null));
			}
			global::MenuNode bannerNode = global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[3];
			bannerNode.DestroyContent();
			global::WarbandMenuController.GenerateBanner(global::Warband.GetBannerName(warbandMenuController.Warband.WarbandData.Id), delegate(global::UnityEngine.GameObject banner)
			{
				global::UnityEngine.Cloth componentInChildren = banner.GetComponentInChildren<global::UnityEngine.Cloth>(true);
				componentInChildren.enabled = false;
				bannerNode.SetContent(banner);
				componentInChildren.enabled = true;
			});
			if (global::PandoraSingleton<global::Hermes>.Instance.IsHost())
			{
				this.lobbyDetail.SetWarbandType(global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].WarbandId);
				if (global::PandoraSingleton<global::Hermes>.Instance.IsConnected())
				{
					this.lobbyDetail.LockAI(true);
					this.lobbyDetail.SetInviteButtonVisible(false);
					this.SetButtons();
				}
				else
				{
					this.lobbyDetail.LockAI(false);
					this.lobbyDetail.SetInviteButtonVisible(global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count < 2);
				}
			}
		}
		this.SetButtons();
	}

	private void OpponentLeft()
	{
		this.lobbyDetail.LockAI(false);
		this.lobbyDetail.SetInviteButtonVisible(true);
		this.playersMod.RefreshPlayers();
		this.lobbyDetail.RefreshPlayerNames();
		this.lobbyDetail.SetLaunchButtonVisible(false, false);
		global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[3].DestroyContent();
		global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[2].DestroyContent();
		global::PandoraSingleton<global::LobbyPlayerPopup>.Instance.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		this.SetButtons();
	}

	private void OpponentReady()
	{
		if (this.playersMod == null || global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count < 2)
		{
			return;
		}
		this.playersMod.RefreshPlayers();
		if (global::PandoraSingleton<global::Hermes>.Instance.IsHost())
		{
			if (global::PandoraSingleton<global::Hermes>.Instance.IsConnected())
			{
				global::MissionWarbandSave missionWarbandSave = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1];
				global::PandoraSingleton<global::LobbyPlayerPopup>.Instance.Show(missionWarbandSave.PlayerName, (!missionWarbandSave.IsReady) ? "menu_not_ready" : "menu_set_ready");
			}
		}
		else
		{
			global::MissionWarbandSave missionWarbandSave2 = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1];
			global::PandoraSingleton<global::LobbyPlayerPopup>.Instance.Show(null, (!missionWarbandSave2.IsReady) ? "menu_not_ready" : "menu_set_ready");
			this.UpdateReadyButton(missionWarbandSave2.IsReady);
		}
	}

	private void KickPlayer()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_skirmish_kick_player"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_skirmish_kick_player_desc", new string[]
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerName
		}), new global::System.Action<bool>(this.OnKickPlayer), false, false);
	}

	private void OnKickPlayer(bool confirm)
	{
		if (confirm)
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.KickPlayerFromLobby(12);
		}
	}

	private static int currentLoadedCharacterIdx;

	private global::HideoutCamAnchor camAnchor;

	private global::LobbyDetailModule lobbyDetail;

	private global::LobbyPlayersModule playersMod;

	private global::DescriptionModule descModule;

	private global::LobbyChatModule chatModule;

	private global::WarbandSwapModule swapModule;

	private bool canLaunchMission;

	private bool warbandChanged;

	private global::TitleModule titleModule;

	private bool lastIsExhibition;
}
