using System;
using UnityEngine;
using UnityEngine.Events;

public class HideoutSkirmish : global::ICheapState
{
	public HideoutSkirmish(global::HideoutManager ctrl, global::HideoutCamAnchor anchor)
	{
		this.controller = ctrl;
		this.camAnchor = anchor;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.ClearLookAtFocus();
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.CancelTransition();
		this.controller.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		this.controller.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.LOBBY_CREATE
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.LOBBY_LIST
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.WARBAND_TABS,
			global::ModuleId.TITLE,
			global::ModuleId.NOTIFICATION
		});
		this.lobbyCreateMod = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::LobbyCreateModule>(global::ModuleId.LOBBY_CREATE);
		this.lobbyCreateMod.Setup();
		this.lobbyCreateMod.createExhibitionGame.SetDisabled(true);
		this.lobbyCreateMod.createContestGame.SetDisabled(true);
		this.lobbyListMod = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::LobbyListModule>(global::ModuleId.LOBBY_LIST);
		this.lobbyListMod.Setup(new global::UnityEngine.Events.UnityAction<bool, int>(this.OnServerSelect), new global::UnityEngine.Events.UnityAction<int>(global::PandoraSingleton<global::SkirmishManager>.Instance.JoinLobby));
		this.lobbyListMod.ClearServersList();
		this.warbandTabs = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandTabsModule>(global::ModuleId.WARBAND_TABS);
		this.warbandTabs.Setup(global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE));
		this.warbandTabs.SetCurrentTab(global::HideoutManager.State.SKIRMISH);
		this.warbandTabs.Refresh();
		global::UnityEngine.Cloth cloth = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.banner.GetComponentsInChildren<global::UnityEngine.Cloth>(true)[0];
		cloth.enabled = false;
		global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[1].SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.banner);
		cloth.enabled = true;
		global::UnitMenuController leader = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetLeader();
		global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[0].SetContent(leader, null);
		leader.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
		this.opponentBannerNode = global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[3];
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(leader.gameObject.transform, 0f);
		global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[3].DestroyContent();
		global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[2].DestroyContent();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_camp", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(this.GoToCamp), false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
		this.GetNumberOfPlayers();
		if (iFrom == 4)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.SetRichPresence(global::Hephaestus.RichPresenceId.HIDEOUT, true);
		}
		global::PandoraSingleton<global::Hermes>.Instance.StopConnections(true);
		global::PandoraSingleton<global::Hephaestus>.Instance.ResetNetwork();
		this.isCheckingNetwork = false;
		this.networkChecked = false;
		this.once = true;
		this.lobbyCreateMod.createExhibitionGame.effects.toggle.isOn = false;
		this.lobbyCreateMod.createContestGame.effects.toggle.isOn = false;
	}

	void global::ICheapState.Exit(int iTo)
	{
		this.opponentBannerNode.DestroyContent();
	}

	void global::ICheapState.Update()
	{
		if (this.networkChecked && global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline() && global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer != 1000 && global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer != 1 && !global::PandoraSingleton<global::HideoutManager>.Instance.showingTuto && !global::PandoraSingleton<global::HideoutManager>.Instance.IsCheckingInvite())
		{
			if (global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite())
			{
				if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.IsSkirmishAvailable(out this.unavailbleReason))
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.JoinInvite();
				}
				else
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.Show("lobby_joining_title", this.unavailbleReason, new global::System.Action<bool>(this.GoToCamp), false, true);
				}
			}
			else if (global::PandoraSingleton<global::Hephaestus>.Instance.IsPlayTogether() || global::PandoraSingleton<global::Hephaestus>.Instance.IsPlayTogetherPassive())
			{
				string text;
				bool flag = global::PandoraSingleton<global::SkirmishManager>.Instance.IsContestAvailable(out text);
				if (flag)
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.playTogetherPopup.Show("popup_play_together_title", "popup_play_together_desc", new global::System.Action(this.OnPlayTogetherExhibition), new global::System.Action(this.OnPlayTogetherContest), new global::System.Action(this.OnPlayTogetherCancel));
				}
				else
				{
					this.OnPlayTogetherExhibition();
				}
			}
		}
	}

	void global::ICheapState.FixedUpdate()
	{
		if (this.once && !global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite() && !global::PandoraSingleton<global::Hephaestus>.Instance.IsPlayTogether() && !global::PandoraSingleton<global::Hephaestus>.Instance.IsPlayTogetherPassive() && global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == 0)
		{
			this.once = false;
			global::PandoraSingleton<global::HideoutManager>.Instance.ShowHideoutTuto(global::HideoutManager.HideoutTutoType.SKIRMISH);
		}
		else if (!this.isCheckingNetwork && !this.networkChecked && !global::PandoraSingleton<global::HideoutManager>.Instance.showingTuto && global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == 0)
		{
			this.isCheckingNetwork = true;
			global::PandoraSingleton<global::Hephaestus>.Instance.CheckNetworkServicesAvailability(new global::System.Action<bool, string>(this.OnNetworkCheck));
		}
	}

	private void GoToCamp(bool confirm)
	{
		this.GoToCamp();
	}

	private void GoToCamp()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
	}

	private void OnPlayTogetherExhibition()
	{
		this.lobbyCreateMod.CreateExhibitionGamePopup(true);
	}

	private void OnPlayTogetherContest()
	{
		this.lobbyCreateMod.CreateContestGamePopup(true);
	}

	private void OnPlayTogetherCancel()
	{
		global::PandoraSingleton<global::Hephaestus>.Instance.ResetPlayTogether(false);
		this.lobbyListMod.btnRefresh.SetSelected(true);
	}

	private void OnNetworkCheck(bool result, string reason)
	{
		this.isCheckingNetwork = false;
		this.networkChecked = true;
		if (global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.GetActiveStateId() != 3)
		{
			return;
		}
		this.lobbyCreateMod.createExhibitionGame.SetDisabled(false);
		this.lobbyCreateMod.createExhibitionGame.SetSelected(true);
		if (result)
		{
			if (!global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite())
			{
				string reasonTag;
				if (global::PandoraSingleton<global::SkirmishManager>.Instance.IsContestAvailable(out reasonTag))
				{
					this.lobbyCreateMod.createContestGame.SetDisabled(false);
				}
				else
				{
					this.lobbyCreateMod.LockContest(reasonTag);
				}
				this.lobbyListMod.LookForGames();
			}
		}
		else
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.ResetInvite();
			global::PandoraSingleton<global::Hephaestus>.Instance.ResetPlayTogether(false);
			if (!string.IsNullOrEmpty(reason))
			{
				this.lobbyCreateMod.LockContest(reason);
				global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.CONNECTION_VALIDATION, "console_offline_error_title", reason, null, null, false);
			}
		}
	}

	private void GetNumberOfPlayers()
	{
		this.lobbyListMod.availableGames.text = string.Empty;
		global::PandoraSingleton<global::Hephaestus>.Instance.RequestNumberOfCurrentPlayers(new global::Hephaestus.OnNumberOfPlayersCallback(this.OnNumberOfPlayers));
	}

	private void OnNumberOfPlayers(int number)
	{
		if (number > 0)
		{
			this.lobbyListMod.availableGames.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_skirmish_players_online", new string[]
			{
				number.ToString()
			});
		}
	}

	private void OnServerSelect(bool isOn, int index)
	{
		if (isOn)
		{
			this.opponentBannerNode.DestroyContent();
			global::WarbandMenuController.GenerateBanner(global::Warband.GetBannerName((global::WarbandId)global::PandoraSingleton<global::Hephaestus>.Instance.Lobbies[index].warbandId), delegate(global::UnityEngine.GameObject banner)
			{
				global::UnityEngine.Cloth componentInChildren = banner.GetComponentInChildren<global::UnityEngine.Cloth>(true);
				componentInChildren.enabled = false;
				this.opponentBannerNode.SetContent(banner);
				componentInChildren.enabled = true;
			});
		}
	}

	private global::HideoutManager controller;

	private global::HideoutCamAnchor camAnchor;

	private global::LobbyCreateModule lobbyCreateMod;

	private global::LobbyListModule lobbyListMod;

	private global::WarbandTabsModule warbandTabs;

	private global::MenuNode opponentBannerNode;

	private bool once = true;

	private bool isCheckingNetwork;

	private bool networkChecked;

	private string unavailbleReason;

	public enum NodeSlot
	{
		PLAYER_1,
		BANNER_1,
		PLAYER_2,
		BANNER_2
	}
}
