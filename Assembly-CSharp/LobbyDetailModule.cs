using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyDetailModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
		this.skirmishMap = null;
		this.skirmishManager = global::PandoraSingleton<global::SkirmishManager>.Instance;
		this.mapList.Setup(this.mapItem, false);
		this.mapList.gameObject.AddComponent<global::UnityEngine.EventSystems.EventTrigger>().AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.PointerExit, delegate(global::UnityEngine.EventSystems.BaseEventData eventData)
		{
			this.ToggleCurrentMap();
		});
		this.currentMapIndex = 0;
		this.position1.id = 0;
		this.position2.id = 1;
		this.objective1.id = 0;
		this.objective2.id = 1;
		this.inviteButton.SetAction(string.Empty, "lobby_invite_friend", 0, false, null, null);
		this.inviteButton.OnAction(new global::UnityEngine.Events.UnityAction(this.OpenInviteInterface), false, true);
	}

	public void SetLobbyData(global::Lobby data, bool isHost)
	{
		if (isHost)
		{
			this.SetHostLobby(data);
		}
		else
		{
			this.SetClientLobby(data);
		}
	}

	private void SetHostLobby(global::Lobby data)
	{
		this.lobbyName.text = data.name;
		this.privacy.transform.parent.gameObject.SetActive(true);
		this.lobbyPrivacy = data.privacy;
		string key = "lobby_privacy_" + data.privacy.ToLowerString();
		string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(key);
		this.privacy.selections.Clear();
		this.privacy.selections.Add(stringById);
		this.privacy.SetCurrentSel(0);
		this.privacy.SetButtonsVisible(false);
		this.AI.selections.Clear();
		this.AI.SetButtonsVisible(global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isSkirmish);
		if (data.privacy != global::Hephaestus.LobbyPrivacy.OFFLINE)
		{
			this.AI.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_player"));
			this.SetInviteButtonVisible(true);
		}
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isSkirmish)
		{
			this.AI.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnSetAI);
			global::System.Collections.Generic.List<global::ProcMissionRatingData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingData>();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Id != global::ProcMissionRatingId.NONE)
				{
					this.AI.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_ai_" + list[i].Id));
				}
			}
		}
		this.AI.SetCurrentSel(0);
		global::UnityEngine.UI.Selectable component = this.privacy.transform.parent.gameObject.GetComponent<global::UnityEngine.UI.Toggle>();
		global::UnityEngine.UI.Navigation navigation = this.AI.selection.navigation;
		navigation.selectOnUp = component;
		this.AI.selection.navigation = navigation;
		this.AIWarbandType.selections.Clear();
		global::System.Collections.Generic.List<global::WarbandData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>("basic", "1");
		for (int j = 0; j < list2.Count; j++)
		{
			this.AIWarbandType.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_wb_type_" + list2[j].Name.ToLowerInvariant()));
		}
		this.AIWarbandType.SetCurrentSel(0);
		if (data.privacy == global::Hephaestus.LobbyPrivacy.OFFLINE)
		{
			this.AIWarbandType.SetButtonsVisible(true);
		}
		else
		{
			this.AIWarbandType.SetButtonsVisible(false);
			this.AIWarbandType.selection.GetComponent<global::UnityEngine.UI.Text>().text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_name_none");
		}
		this.roaming.selections.Clear();
		this.roaming.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_random"));
		global::System.Collections.Generic.List<global::UnitId> roamingUnitIds = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.GetRoamingUnitIds();
		for (int k = 0; k < roamingUnitIds.Count; k++)
		{
			this.roaming.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_name_" + roamingUnitIds[k]));
		}
		this.roaming.SetCurrentSel(1);
		this.roaming.SetButtonsVisible(true);
		this.backtracking.selections.Clear();
		this.backtracking.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_no_backtracking"));
		for (int l = 1; l <= 10; l++)
		{
			this.backtracking.selections.Add(l.ToString());
		}
		this.backtracking.SetCurrentSel(0);
		this.backtracking.SetButtonsVisible(true);
		this.turnTimer.selections.Clear();
		for (int m = 0; m <= 180; m += 15)
		{
			this.turnTimer.selections.Add((m != 0) ? m.ToString() : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_no_timer"));
		}
		this.turnTimer.SetCurrentSel(0);
		this.turnTimer.SetButtonsVisible(true);
		this.deployTimer.selections.Clear();
		for (int m = 0; m <= 180; m += 15)
		{
			this.deployTimer.selections.Add((m != 0) ? m.ToString() : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_no_timer"));
		}
		this.deployTimer.SetCurrentSel(0);
		this.deployTimer.SetButtonsVisible(true);
		this.routThreshold.selections.Clear();
		float @float = global::Constant.GetFloat(global::ConstantId.ROUT_RATIO_ALIVE);
		for (int n = 0; n <= 95; n += 5)
		{
			this.routThreshold.selections.Add(n.ToConstantString() + "%");
		}
		this.routThreshold.SetCurrentSel((int)(@float * 100f / 5f));
		this.routThreshold.SetButtonsVisible(true);
		this.autodeploy.selections.Clear();
		this.autodeploy.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_off"));
		this.autodeploy.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_on"));
		this.autodeploy.SetButtonsVisible(true);
		this.autodeploy.SetCurrentSel(0);
		this.mapList.ClearList();
		this.mapName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_random");
		component = this.routThreshold.transform.parent.gameObject.GetComponent<global::UnityEngine.UI.Toggle>();
		global::UnityEngine.UI.Selectable component2 = this.gameplay.transform.parent.gameObject.GetComponent<global::UnityEngine.UI.Toggle>();
		global::UnityEngine.GameObject gameObject = this.mapList.AddToList(component, component2);
		global::UnityEngine.UI.Image component3 = gameObject.GetComponent<global::UnityEngine.UI.Image>();
		component3.sprite = global::UnityEngine.Resources.Load<global::UnityEngine.Sprite>("maps/img_map_random");
		gameObject.GetComponent<global::ToggleEffects>().onAction.AddListener(delegate()
		{
			this.OnMapSelect(0);
		});
		for (int num = 0; num < this.skirmishManager.skirmishMaps.Count; num++)
		{
			gameObject = this.mapList.AddToList(component, component2);
			component3 = gameObject.GetComponent<global::UnityEngine.UI.Image>();
			component3.sprite = global::UnityEngine.Resources.Load<global::UnityEngine.Sprite>("maps/img_map_" + this.skirmishManager.skirmishMaps[num].mapData.Name);
			int index = num + 1;
			gameObject.GetComponent<global::ToggleEffects>().onAction.AddListener(delegate()
			{
				this.OnMapSelect(index);
			});
		}
		this.inviteButton.gameObject.SetActive(true);
		component2 = this.swapButton.gameObject.GetComponent<global::UnityEngine.UI.Toggle>();
		navigation = this.objective2.selection.navigation;
		navigation.selectOnDown = component2;
		this.objective2.selection.navigation = navigation;
		this.roaming.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnRoamingChange);
		this.AIWarbandType.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnAiWarbandChanged);
		this.backtracking.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnBacktrackingChange);
		this.turnTimer.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnTurnTimerChange);
		this.deployTimer.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnDeployTimerChanged);
		this.routThreshold.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnRoutThresholdChanged);
		this.timeOfDay.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnTimeofDayChange);
		this.gameplay.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnGameplayChange);
		this.deployment.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnDeploymentChange);
		this.autodeploy.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnAutodeployChange);
		this.position1.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnPositionChange);
		this.position2.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnPositionChange);
		this.gameType.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnGameTypeChange);
		this.objective1.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnObjectiveChange);
		this.objective2.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnObjectiveChange);
		this.RefreshPlayerNames();
		this.currentMapIndex = -1;
		this.OnMapSelect(0);
		this.privacy.GetComponentInParent<global::ToggleEffects>().SetSelected(true);
	}

	private void SetClientLobby(global::Lobby data)
	{
		global::Mission currentMission = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission;
		this.lobbyName.text = data.name;
		this.privacy.transform.parent.gameObject.SetActive(false);
		this.AI.SetButtonsVisible(false);
		this.AI.selections.Clear();
		this.AI.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_player"));
		this.AI.SetCurrentSel(0);
		global::UnityEngine.UI.Selectable component = this.objective2.transform.parent.gameObject.GetComponent<global::UnityEngine.UI.Toggle>();
		global::UnityEngine.UI.Navigation navigation = this.AI.selection.navigation;
		navigation.selectOnUp = component;
		this.AI.selection.navigation = navigation;
		this.AIWarbandType.selections.Clear();
		this.AIWarbandType.SetButtonsVisible(false);
		this.AIWarbandType.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_wb_type_" + ((global::WarbandId)data.warbandId).ToLowerString()));
		this.AIWarbandType.SetCurrentSel(0);
		this.roaming.selections.Clear();
		this.roaming.SetButtonsVisible(false);
		this.roaming.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!currentMission.missionSave.randomRoaming) ? ("unit_name_" + (global::UnitId)currentMission.missionSave.roamingUnitId) : "lobby_title_random"));
		this.roaming.SetCurrentSel(0);
		this.backtracking.selections.Clear();
		this.backtracking.SetButtonsVisible(false);
		this.backtracking.selections.Add((currentMission.missionSave.beaconLimit != 0) ? currentMission.missionSave.beaconLimit.ToString() : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_no_backtracking"));
		this.backtracking.SetCurrentSel(0);
		this.turnTimer.selections.Clear();
		this.turnTimer.SetButtonsVisible(false);
		this.turnTimer.selections.Add((currentMission.missionSave.turnTimer != 0) ? currentMission.missionSave.turnTimer.ToString() : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_no_timer"));
		this.turnTimer.SetCurrentSel(0);
		this.deployTimer.selections.Clear();
		this.deployTimer.SetButtonsVisible(false);
		this.deployTimer.selections.Add((currentMission.missionSave.deployTimer != 0) ? currentMission.missionSave.deployTimer.ToString() : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_no_timer"));
		this.deployTimer.SetCurrentSel(0);
		this.routThreshold.selections.Clear();
		this.routThreshold.SetButtonsVisible(false);
		this.routThreshold.selections.Add(currentMission.missionSave.routThreshold * 100f + "%");
		this.routThreshold.SetCurrentSel(0);
		global::MissionMapId mapId = currentMission.GetMapId();
		this.mapName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((mapId == global::MissionMapId.NONE) ? "lobby_title_random" : (mapId.ToLowerString() + "_name"));
		component = this.turnTimer.transform.parent.gameObject.GetComponent<global::UnityEngine.UI.Toggle>();
		global::UnityEngine.UI.Selectable component2 = this.gameplay.transform.parent.gameObject.GetComponent<global::UnityEngine.UI.Toggle>();
		global::UnityEngine.GameObject gameObject;
		if (this.mapList.items.Count != 1)
		{
			this.mapList.ClearList();
			gameObject = this.mapList.AddToList(component, component2);
		}
		else
		{
			gameObject = this.mapList.items[0];
		}
		global::UnityEngine.UI.Image component3 = gameObject.GetComponent<global::UnityEngine.UI.Image>();
		component3.sprite = global::UnityEngine.Resources.Load<global::UnityEngine.Sprite>("maps/img_map_" + ((mapId == global::MissionMapId.NONE) ? "random" : mapId.ToLowerString()));
		this.RealignOnNextFrame(0);
		this.timeOfDay.SetButtonsVisible(false);
		this.timeOfDay.selections.Clear();
		string text = string.Empty;
		string skyName = currentMission.GetSkyName();
		if (skyName.Contains("day"))
		{
			text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_sky_day");
			text = text + " " + skyName[skyName.Length - 1];
		}
		else if (skyName.Contains("night"))
		{
			text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_sky_night");
			text = text + " " + skyName[skyName.Length - 1];
		}
		else
		{
			text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_random");
		}
		this.timeOfDay.selections.Add(text);
		this.timeOfDay.SetCurrentSel(0);
		this.gameplay.SetButtonsVisible(false);
		this.gameplay.selections.Clear();
		this.gameplay.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!currentMission.missionSave.randomGameplay) ? ("lobby_title_" + currentMission.GetMapGameplayId()) : "lobby_title_random"));
		this.gameplay.SetCurrentSel(0);
		this.deployment.SetButtonsVisible(false);
		this.deployment.selections.Clear();
		global::DeploymentScenarioId deploymentScenarioId = currentMission.GetDeploymentScenarioId();
		this.deployment.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((deploymentScenarioId == global::DeploymentScenarioId.NONE) ? "lobby_title_random" : ("lobby_title_" + deploymentScenarioId.ToLowerString())));
		this.deployment.SetCurrentSel(0);
		this.autodeploy.SetButtonsVisible(false);
		this.autodeploy.selections.Clear();
		this.autodeploy.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!currentMission.missionSave.autoDeploy) ? "menu_off" : "menu_on"));
		this.autodeploy.SetCurrentSel(0);
		this.position1.SetButtonsVisible(false);
		this.position1.selections.Clear();
		global::DeploymentId deploymentId = currentMission.GetDeploymentId(0);
		this.position1.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((deploymentId == global::DeploymentId.NONE) ? "lobby_title_random" : ("lobby_title_" + deploymentId.ToLowerString())));
		this.position1.SetCurrentSel(0);
		this.position2.SetButtonsVisible(false);
		this.position2.selections.Clear();
		deploymentId = currentMission.GetDeploymentId(1);
		this.position2.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((deploymentId == global::DeploymentId.NONE) ? "lobby_title_random" : ("lobby_title_" + deploymentId.ToLowerString())));
		this.position2.SetCurrentSel(0);
		this.gameType.SetButtonsVisible(false);
		this.gameType.selections.Clear();
		this.gameType.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!currentMission.HasObjectives()) ? "lobby_title_battleground_only" : "lobby_title_extra_objectives"));
		this.gameType.SetCurrentSel(0);
		this.objective1.SetButtonsVisible(false);
		this.objective1.selections.Clear();
		this.objective1.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.HasObjectives() ? ((!currentMission.IsObjectiveRandom(0)) ? ("lobby_title_" + currentMission.GetObjectiveTypeId(0).ToString()) : "lobby_title_random") : "lobby_title_no_extra_objectives"));
		this.objective1.SetCurrentSel(0);
		this.objective2.SetButtonsVisible(false);
		this.objective2.selections.Clear();
		this.objective2.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.HasObjectives() ? ((!currentMission.IsObjectiveRandom(1)) ? ("lobby_title_" + currentMission.GetObjectiveTypeId(1).ToString()) : "lobby_title_random") : "lobby_title_no_extra_objectives"));
		this.objective2.SetCurrentSel(0);
		this.inviteButton.gameObject.SetActive(false);
		component2 = this.AI.transform.parent.gameObject.GetComponent<global::UnityEngine.UI.Toggle>();
		navigation = this.objective2.selection.navigation;
		navigation.selectOnDown = component2;
		this.objective2.selection.navigation = navigation;
		this.SetInviteButtonVisible(false);
		this.launchButton.SetDisabled(false);
		this.launchButton.SetSelected(true);
		this.ForceMapReselect();
	}

	public void RefreshPlayerNames()
	{
		if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 0)
		{
			this.player1.text = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[0].PlayerName;
			this.player1Obj.text = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[0].PlayerName;
		}
		else
		{
			this.player1.text = global::PandoraSingleton<global::Hephaestus>.Instance.GetUserName();
			this.player1Obj.text = global::PandoraSingleton<global::Hephaestus>.Instance.GetUserName();
		}
		if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1)
		{
			this.player2.text = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerName;
			this.player2Obj.text = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerName;
		}
		else
		{
			this.player2.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_player") + " 2";
			this.player2Obj.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_player") + " 2";
		}
	}

	public void LockAI(bool lockButtons)
	{
		if (global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.privacy == global::Hephaestus.LobbyPrivacy.OFFLINE)
		{
			return;
		}
		this.AI.SetButtonsVisible(!lockButtons);
	}

	private void OnSetAI(int id, int index)
	{
		if (index != 0 || global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.privacy == global::Hephaestus.LobbyPrivacy.OFFLINE)
		{
			int ratingId = index + ((global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.privacy != global::Hephaestus.LobbyPrivacy.OFFLINE) ? 0 : 1);
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.ratingId = ratingId;
			if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count == 1)
			{
				global::PandoraSingleton<global::SkirmishManager>.Instance.AddAIOpponent(null);
			}
			else
			{
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SKIRMISH_OPPONENT_READY);
			}
			this.SetInviteButtonVisible(false);
			this.AIWarbandType.SetButtonsVisible(true);
		}
		else
		{
			global::PandoraSingleton<global::SkirmishManager>.Instance.RemoveOpponent();
			this.SetInviteButtonVisible(true);
			this.AIWarbandType.SetButtonsVisible(false);
			this.AIWarbandType.selection.GetComponent<global::UnityEngine.UI.Text>().text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_name_none");
		}
	}

	public void SetWarbandType(global::WarbandId wbId)
	{
		global::System.Collections.Generic.List<global::WarbandData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>("basic", "1");
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Id == wbId)
			{
				this.AIWarbandType.SetCurrentSel(i);
				break;
			}
		}
	}

	private void OnAiWarbandChanged(int id, int index)
	{
		global::System.Collections.Generic.List<global::WarbandData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>("basic", "1");
		global::PandoraSingleton<global::SkirmishManager>.Instance.RemoveOpponent();
		global::PandoraSingleton<global::SkirmishManager>.Instance.AddAIOpponent(list[index]);
	}

	public void SetInviteButtonVisible(bool visible)
	{
		if (!visible && global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == this.inviteButton.gameObject)
		{
			this.swapButton.SetSelected(true);
		}
		this.inviteButton.gameObject.SetActive(visible && global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.privacy != global::Hephaestus.LobbyPrivacy.OFFLINE);
	}

	public void SetLaunchButtonVisible(bool visible, bool disabled)
	{
		if (!visible && global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == this.launchButton.gameObject)
		{
			this.swapButton.SetSelected(true);
		}
		if (disabled)
		{
			this.launchButton.SetDisabled(!visible);
		}
		else
		{
			this.launchButton.gameObject.SetActive(visible);
		}
	}

	private void OnTurnTimerChange(int id, int index)
	{
		global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.turnTimer = index * 15;
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMission(false);
	}

	private void OnDeployTimerChanged(int id, int index)
	{
		global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.deployTimer = index * 15;
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMission(false);
	}

	private void OnRoutThresholdChanged(int id, int index)
	{
		global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.routThreshold = (float)(index * 5) / 100f;
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMission(false);
	}

	private void OnRoamingChange(int id, int index)
	{
		if (index == 0)
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.SetRandomRoaming(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche);
		}
		else
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.SetRoamingUnit(index - 1);
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMission(false);
	}

	private void OnBacktrackingChange(int id, int index)
	{
		global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.beaconLimit = index;
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMission(false);
	}

	private void ToggleCurrentMap()
	{
		if (global::PandoraSingleton<global::Hermes>.Instance.IsHost())
		{
			this.mapList.items[this.currentMapIndex].SetSelected(false);
		}
	}

	public void ForceMapReselect()
	{
		if (base.gameObject.activeSelf)
		{
			base.StartCoroutine(this.RealignOnNextFrame(this.currentMapIndex));
		}
	}

	private global::System.Collections.IEnumerator RealignOnNextFrame(int index)
	{
		yield return null;
		this.mapList.RealignList(true, index, true);
		yield break;
	}

	private void OnMapSelect(int index)
	{
		if (this.currentMapIndex == index)
		{
			return;
		}
		this.mapList.items[index].SetSelected(false);
		this.mapList.items[index].GetComponent<global::ToggleEffects>().SetOn();
		base.StartCoroutine(this.RealignOnNextFrame(index));
		this.currentMapIndex = index;
		global::PandoraDebug.LogDebug("Map Selected = " + this.currentMapIndex, "uncategorised", null);
		global::PandoraSingleton<global::Hephaestus>.Instance.SetLobbyData("map", this.currentMapIndex.ToLowerString());
		if (this.currentMapIndex == 0)
		{
			this.skirmishMap = null;
			this.mapName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_random");
			global::PandoraSingleton<global::MissionStartData>.Instance.RegenerateMission(global::MissionMapId.NONE, global::DeploymentScenarioId.NONE, false);
			global::PandoraDebug.LogInfo("Generating random map", "SKIRMISH", null);
		}
		else
		{
			this.skirmishMap = this.skirmishManager.skirmishMaps[this.currentMapIndex - 1];
			this.mapName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.skirmishMap.mapData.Name + "_name");
			global::PandoraDebug.LogInfo("Generating map : " + this.skirmishMap.mapData.Id, "SKIRMISH", null);
			global::PandoraSingleton<global::MissionStartData>.Instance.RegenerateMission(this.skirmishMap.mapData.Id, global::DeploymentScenarioId.NONE, false);
		}
		this.SetTimeOfDayChoices(index);
		this.SetGameplayChoices(index);
		this.SetDeploymentChoices(index);
	}

	private void SetTimeOfDayChoices(int mapIndex)
	{
		this.timeOfDay.selections.Clear();
		this.timeOfDay.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_random"));
		if (mapIndex != 0)
		{
			for (int i = 0; i < this.skirmishMap.layouts.Count; i++)
			{
				string text = string.Empty;
				string name = this.skirmishMap.layouts[i].Name;
				if (name.Contains("day"))
				{
					text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_sky_day");
				}
				else
				{
					text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_sky_night");
				}
				char c = name[name.Length - 1];
				if (!char.IsDigit(c))
				{
					c = '1';
				}
				text = text + " " + c;
				this.timeOfDay.selections.Add(text);
			}
		}
		this.timeOfDay.SetButtonsVisible(mapIndex != 0);
		this.timeOfDay.SetCurrentSel(0);
	}

	private void OnTimeofDayChange(int id, int index)
	{
		global::PandoraDebug.LogDebug("timeofDay cursel = " + this.timeOfDay.CurSel, "uncategorised", null);
		global::PandoraDebug.LogDebug("OnTimeofDayChange index = " + index, "uncategorised", null);
		bool flag = false;
		global::MissionMapLayoutId id2;
		if (index == 0)
		{
			flag = true;
			id2 = this.skirmishMap.layouts[global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.skirmishMap.layouts.Count)].Id;
		}
		else
		{
			id2 = this.skirmishMap.layouts[index - 1].Id;
		}
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Changing Map Layout :",
			id2,
			" random : ",
			flag
		}), "SKIRMISH", null);
		global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.randomLayout = flag;
		global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.mapLayoutId = (int)id2;
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMission(false);
	}

	private void SetGameplayChoices(int mapIndex)
	{
		this.gameplay.selections.Clear();
		if (mapIndex != 0 && this.skirmishMap.gameplays.Count > 0)
		{
			this.gameplay.SetButtonsVisible(true);
			this.gameplay.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_random"));
			for (int i = 0; i < this.skirmishMap.gameplays.Count; i++)
			{
				this.gameplay.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_" + this.skirmishMap.gameplays[i].Name));
			}
		}
		else
		{
			this.gameplay.SetButtonsVisible(false);
			this.gameplay.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_no_gameplay"));
		}
		this.gameplay.SetCurrentSel(0);
	}

	private void OnGameplayChange(int id, int index)
	{
		global::PandoraDebug.LogDebug("Gameplay cursel = " + this.gameplay.CurSel, "uncategorised", null);
		global::PandoraDebug.LogDebug("OnGameplayChanged index = " + index, "uncategorised", null);
		bool flag = index == 0;
		global::MissionMapGameplayId id2;
		if (flag)
		{
			id2 = this.skirmishMap.gameplays[global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.skirmishMap.gameplays.Count)].Id;
		}
		else
		{
			id2 = this.skirmishMap.gameplays[index - 1].Id;
		}
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Changing Map Gameplay :",
			id2,
			" random : ",
			flag
		}), "SKIRMISH", null);
		global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.randomGameplay = flag;
		global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.mapGameplayId = (int)id2;
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMission(false);
	}

	private void SetDeploymentChoices(int mapIndex)
	{
		this.deployment.selections.Clear();
		this.deployment.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_random"));
		if (mapIndex != 0)
		{
			for (int i = 0; i < this.skirmishMap.deployments.Count; i++)
			{
				string key = "lobby_title_" + this.skirmishMap.deployments[i].scenarioData.Name;
				this.deployment.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(key));
			}
		}
		this.deployment.SetButtonsVisible(mapIndex != 0);
		this.deployment.SetCurrentSel(0);
		this.SetPositionsChoices(0);
	}

	private void OnDeploymentChange(int id, int index)
	{
		global::DeploymentScenarioId scenarioId = (index != 0) ? this.skirmishMap.deployments[index - 1].scenarioData.Id : global::DeploymentScenarioId.NONE;
		global::PandoraSingleton<global::MissionStartData>.Instance.RegenerateMission(this.skirmishMap.mapData.Id, scenarioId, true);
		this.SetPositionsChoices(index);
	}

	private void OnAutodeployChange(int id, int index)
	{
		global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.autoDeploy = (index == 1);
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMission(false);
	}

	private void SetPositionsChoices(int deployIndex)
	{
		this.position1.selections.Clear();
		this.position1.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_random"));
		this.position2.selections.Clear();
		this.position2.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_random"));
		if (deployIndex != 0)
		{
			for (int i = 0; i < this.skirmishMap.deployments[deployIndex - 1].slots.Count; i++)
			{
				this.position1.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_" + this.skirmishMap.deployments[deployIndex - 1].slots[i].Name));
				this.position2.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_" + this.skirmishMap.deployments[deployIndex - 1].slots[i].Name));
			}
		}
		this.position1.SetButtonsVisible(deployIndex != 0);
		this.position1.SetCurrentSel(0);
		this.position2.SetButtonsVisible(deployIndex != 0);
		this.position2.SetCurrentSel(0);
		this.SetGameTypeChoices();
	}

	private void OnPositionChange(int id, int index)
	{
		if (index == 0)
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.SetRandomDeploySlots(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche);
			this.position1.SetCurrentSel(0);
			this.position2.SetCurrentSel(0);
		}
		else
		{
			int num = index - 1;
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.SetDeploySlots(id, num);
			if (id == 0)
			{
				this.position2.SetCurrentSel((num + 1) % 2 + 1);
			}
			else
			{
				this.position1.SetCurrentSel((num + 1) % 2 + 1);
			}
		}
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.HasObjectives())
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.RandomizeObjectives(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche);
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMission(false);
		this.SetObjectiveChoices();
	}

	private void SetGameTypeChoices()
	{
		this.gameType.selections.Clear();
		this.gameType.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_battleground_only"));
		this.gameType.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_extra_objectives"));
		this.gameType.SetCurrentSel(global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.HasObjectives() ? 1 : 0);
		this.gameType.SetButtonsVisible(true);
		this.SetObjectiveChoices();
	}

	private void OnGameTypeChange(int id, int index)
	{
		if (index == 0)
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.ClearObjectives();
		}
		else
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.RandomizeObjectives(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche);
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMission(false);
		this.SetObjectiveChoices();
	}

	private void SetObjectiveChoices()
	{
		bool flag = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.HasObjectives();
		this.objective1.selections.Clear();
		this.objective2.selections.Clear();
		if (!flag)
		{
			this.objective1.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_no_extra_objectives"));
			this.objective2.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_no_extra_objectives"));
		}
		else
		{
			this.SetPlayerObjectiveChoices(this.objective1, 0);
			this.SetPlayerObjectiveChoices(this.objective2, 1);
		}
		this.objective1.SetButtonsVisible(flag && this.deployment.CurSel != 0);
		this.objective1.SetCurrentSel(0);
		this.objective2.SetButtonsVisible(flag && this.deployment.CurSel != 0);
		this.objective2.SetCurrentSel(0);
	}

	private void SetPlayerObjectiveChoices(global::SelectorGroup objectiveGroup, int idx)
	{
		objectiveGroup.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_random"));
		global::System.Collections.Generic.List<global::PrimaryObjectiveTypeId> availableObjectiveTypes = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.GetAvailableObjectiveTypes(idx);
		for (int i = 0; i < availableObjectiveTypes.Count; i++)
		{
			objectiveGroup.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_" + availableObjectiveTypes[i].ToLowerString()));
		}
	}

	private void OnObjectiveChange(int id, int index)
	{
		if (index == 0)
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.SetRandomObjective(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, id);
		}
		else
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.SetObjective(id, index - 1);
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.SendMission(false);
	}

	public void OpenInviteInterface()
	{
		global::PandoraSingleton<global::Hephaestus>.Instance.InviteFriends();
	}

	public void LinkDescriptions(global::UnityEngine.Events.UnityAction<string, string> onSelect, global::UnityEngine.Events.UnityAction<string, string> onSelectLocalized)
	{
		if (this.privacy.transform.parent.gameObject.activeSelf)
		{
			this.SetDesc(this.privacy, onSelect, new global::LobbyDetailModule.GetLoc(this.GetPrivacy), "lobby_title_", "lobby_desc_");
		}
		this.SetDesc(this.AI, onSelect, new global::LobbyDetailModule.GetLoc(this.GetPlayerTypeLoc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.AIWarbandType, onSelect, new global::LobbyDetailModule.GetLoc(this.GetAiTypeLoc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.roaming, onSelect, new global::LobbyDetailModule.GetLoc(this.GetRoamingDesc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.backtracking, onSelect, new global::LobbyDetailModule.GetLoc(this.GetBacktrackingLoc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.turnTimer, onSelect, new global::LobbyDetailModule.GetLoc(this.GetTurnTimerLoc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.deployTimer, onSelect, new global::LobbyDetailModule.GetLoc(this.GetDeployTimerLoc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.routThreshold, onSelect, new global::LobbyDetailModule.GetLoc(this.GetRoutThresholdLoc), "lobby_title_", "lobby_desc_");
		foreach (global::UnityEngine.GameObject gameObject in this.mapList.items)
		{
			gameObject.GetComponent<global::ToggleEffects>().onSelect.AddListener(delegate()
			{
				onSelect(this.GetMapLoc() + "_name", this.GetMapLoc() + "_desc");
			});
		}
		this.SetDesc(this.timeOfDay, onSelect, new global::LobbyDetailModule.GetLoc(this.GetMapLayoutLoc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.gameplay, onSelect, new global::LobbyDetailModule.GetLoc(this.GetMapGameplayLoc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.deployment, onSelect, new global::LobbyDetailModule.GetLoc(this.GetDeploymentScenarLoc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.autodeploy, onSelect, new global::LobbyDetailModule.GetLoc(this.GetAutodeployLoc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.position1, onSelect, new global::LobbyDetailModule.GetLoc(this.GetDeploySlot1Loc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.position2, onSelect, new global::LobbyDetailModule.GetLoc(this.GetDeploySlot2Loc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.gameType, onSelect, new global::LobbyDetailModule.GetLoc(this.GetGameTypeLoc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.objective1, onSelect, new global::LobbyDetailModule.GetLoc(this.GetObj1Loc), "lobby_title_", "lobby_desc_");
		this.SetDesc(this.objective2, onSelect, new global::LobbyDetailModule.GetLoc(this.GetObj2Loc), "lobby_title_", "lobby_desc_");
	}

	private string GetPrivacy()
	{
		return this.lobbyPrivacy.ToLowerString();
	}

	private string GetPlayerTypeLoc()
	{
		return (this.AI.CurSel != 0) ? "ai" : "player";
	}

	private string GetAiTypeLoc()
	{
		return "opponent_warband";
	}

	private string GetRoamingDesc()
	{
		return "roaming";
	}

	private string GetBacktrackingLoc()
	{
		return "backtracking";
	}

	private string GetTurnTimerLoc()
	{
		return "timer";
	}

	private string GetDeployTimerLoc()
	{
		return "deploy_timer";
	}

	private string GetRoutThresholdLoc()
	{
		return "rout_threshold";
	}

	private string GetMapLoc()
	{
		global::MissionMapId mapId = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.GetMapId();
		return (mapId != global::MissionMapId.NONE) ? mapId.ToLowerString() : "lobby_title_random";
	}

	private string GetMapLayoutLoc()
	{
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.randomLayout)
		{
			return "random";
		}
		global::MissionMapLayoutData missionMapLayoutData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapLayoutData>((int)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.GetMapLayoutId());
		if (missionMapLayoutData.Name.Contains("day"))
		{
			return "sky_day";
		}
		return "sky_night";
	}

	private string GetMapGameplayLoc()
	{
		global::MissionMapGameplayData missionMapGameplayData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapGameplayData>((int)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.GetMapGameplayId());
		return (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.randomGameplay) ? missionMapGameplayData.Name : "random";
	}

	private string GetDeploymentScenarLoc()
	{
		return (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.randomDeployment) ? global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.GetDeploymentScenarioId().ToLowerString() : "random";
	}

	private string GetAutodeployLoc()
	{
		return (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.autoDeploy) ? "manualdeploy" : "autodeploy";
	}

	private string GetDeploySlot1Loc()
	{
		return (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.randomSlots) ? global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.GetDeploymentId(0).ToLowerString() : "random";
	}

	private string GetDeploySlot2Loc()
	{
		return (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.randomSlots) ? global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.GetDeploymentId(1).ToLowerString() : "random";
	}

	private string GetGameTypeLoc()
	{
		return (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.HasObjectives()) ? "battleground_only" : "extra_objectives";
	}

	private string GetObj1Loc()
	{
		if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.HasObjectives())
		{
			return "no_extra_objectives";
		}
		return (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.randomObjectives[0]) ? ((global::PrimaryObjectiveTypeId)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.objectiveTypeIds[0]).ToLowerString() : "random";
	}

	private string GetObj2Loc()
	{
		if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.HasObjectives())
		{
			return "no_extra_objectives";
		}
		return (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.randomObjectives[1]) ? ((global::PrimaryObjectiveTypeId)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.objectiveTypeIds[1]).ToLowerString() : "random";
	}

	private void SetDesc(global::SelectorGroup group, global::UnityEngine.Events.UnityAction<string, string> onSelect, global::LobbyDetailModule.GetLoc loc, string title = "lobby_title_", string desc = "lobby_desc_")
	{
		global::UnityEngine.Events.UnityAction action = delegate()
		{
			onSelect(title + loc(), desc + loc());
		};
		group.GetComponentInParent<global::ToggleEffects>().onSelect.AddListener(action);
		group.onValueChanged = (global::SelectorGroup.OnValueChanged)global::System.Delegate.Combine(group.onValueChanged, new global::SelectorGroup.OnValueChanged(delegate(int id, int index)
		{
			action();
		}));
	}

	private const int TURN_TIMER_MAX_VALUE = 180;

	private const int DELPOY_TIMER_MAX_VALUE = 180;

	private const int BACKTRACKING_MAX_VALUE = 10;

	private const int TURN_TIMER_INCREASE = 15;

	private const int ROUT_THRESHOLD_INCREASE = 5;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text lobbyName;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup privacy;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup AI;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup AIWarbandType;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup roaming;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup backtracking;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup turnTimer;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup deployTimer;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup routThreshold;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text mapName;

	[global::UnityEngine.SerializeField]
	public global::ScrollGroup mapList;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.GameObject mapItem;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup timeOfDay;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup gameplay;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup fow;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup deployment;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup autodeploy;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text player1;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text player2;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup position1;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup position2;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup gameType;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text player1Obj;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text player2Obj;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup objective1;

	[global::UnityEngine.SerializeField]
	private global::SelectorGroup objective2;

	public global::ButtonGroup inviteButton;

	public global::ButtonGroup swapButton;

	public global::ButtonGroup launchButton;

	public global::ButtonGroup displayProfile;

	public global::UnityEngine.Sprite swapIcon;

	public global::UnityEngine.Sprite launchIcon;

	private int currentMapIndex = -1;

	private global::SkirmishMap skirmishMap;

	private global::SkirmishManager skirmishManager;

	private global::Hephaestus.LobbyPrivacy lobbyPrivacy;

	private delegate string GetLoc();
}
