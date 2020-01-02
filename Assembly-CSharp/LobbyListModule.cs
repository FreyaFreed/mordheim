using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyListModule : global::UIModule
{
	public void Setup(global::UnityEngine.Events.UnityAction<bool, int> OnServerSelect, global::UnityEngine.Events.UnityAction<int> OnServerConfirm)
	{
		this.onServerSelect = OnServerSelect;
		this.onServerConfirm = OnServerConfirm;
		this.serverList.Setup(this.serverItem.gameObject, true);
		this.btnRefresh.SetAction(string.Empty, "lobby_refresh", 0, false, null, null);
		this.btnRefresh.OnAction(new global::UnityEngine.Events.UnityAction(this.LookForGames), false, true);
		this.btnRefresh.effects.toggle.isOn = false;
	}

	public void ClearServersList()
	{
		this.serverList.ClearList();
		this.btnRefresh.SetSelected(false);
	}

	public void FillServersList(global::System.Collections.Generic.List<global::Lobby> lobbies, global::System.Collections.Generic.List<global::SkirmishMap> skirmishMaps)
	{
		for (int i = 0; i < lobbies.Count; i++)
		{
			if (lobbies[i].hostId != global::PandoraSingleton<global::Hephaestus>.Instance.GetUserId() && lobbies[i].privacy != global::Hephaestus.LobbyPrivacy.PRIVATE)
			{
				global::UnityEngine.UI.Toggle up = null;
				global::UnityEngine.UI.Selectable down = null;
				if (i == lobbies.Count - 1)
				{
					down = this.btnRefresh.effects.toggle;
				}
				int mapName = lobbies[i].mapName;
				string stringById;
				if (lobbies[i].mapName > 0)
				{
					stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(skirmishMaps[mapName - 1].mapData.Name + "_name");
				}
				else
				{
					stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_random");
				}
				global::UILobbyEntry component = this.serverList.AddToList(up, down).GetComponent<global::UILobbyEntry>();
				int a = 0;
				global::System.Collections.Generic.List<global::ProcMissionRatingData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingData>();
				for (int j = 0; j < list.Count; j++)
				{
					a = global::UnityEngine.Mathf.Max(a, list[j].MaxValue);
				}
				component.Set(lobbies[i].name, stringById, lobbies[i].ratingMin, lobbies[i].ratingMax, !lobbies[i].isExhibition);
				int index = i;
				component.GetComponent<global::ToggleEffects>().toggle.onValueChanged.AddListener(delegate(bool isOn)
				{
					this.onServerSelect(isOn, index);
				});
				component.GetComponent<global::ToggleEffects>().onAction.AddListener(delegate()
				{
					if (global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer != 1000)
					{
						this.onServerConfirm(index);
					}
				});
			}
		}
	}

	public void LookForGames()
	{
		this.ClearServersList();
		global::PandoraSingleton<global::HideoutManager>.Instance.skirmishNodeGroup.nodes[3].DestroyContent();
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline())
		{
			this.btnRefresh.SetDisabled(true);
			global::PandoraSingleton<global::Hephaestus>.Instance.SearchLobbies(new global::Hephaestus.OnSearchLobbiesCallback(this.OnSearchLobbiesCallback));
		}
	}

	private void OnSearchLobbiesCallback()
	{
		global::PandoraDebug.LogInfo("OnSearchLobbiesCallback - Lobby Count = " + global::PandoraSingleton<global::Hephaestus>.Instance.Lobbies.Count, "HEPHAESTUS", null);
		this.btnRefresh.SetDisabled(false);
		if (global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
		{
			this.btnRefresh.SetSelected(true);
		}
		this.FillServersList(global::PandoraSingleton<global::Hephaestus>.Instance.Lobbies, global::PandoraSingleton<global::SkirmishManager>.Instance.skirmishMaps);
	}

	public global::ScrollGroup serverList;

	public global::UILobbyEntry serverItem;

	public global::ButtonGroup btnRefresh;

	public global::UnityEngine.UI.Text availableGames;

	private global::UnityEngine.Events.UnityAction<bool, int> onServerSelect;

	private global::UnityEngine.Events.UnityAction<int> onServerConfirm;
}
