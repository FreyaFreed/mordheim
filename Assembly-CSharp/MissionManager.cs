using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

public class MissionManager : global::PandoraSingleton<global::MissionManager>
{
	public global::System.Collections.Generic.List<global::WarbandController> WarbandCtrlrs { get; private set; }

	public global::System.Collections.Generic.List<global::UnitController> InitiativeLadder { get; private set; }

	public global::CameraManager CamManager { get; private set; }

	public global::MessageManager MsgManager { get; private set; }

	public global::CheapStateMachine StateMachine { get; private set; }

	public int VictoriousTeamIdx { get; private set; }

	public global::MovementCircles MoveCircle { get; private set; }

	public global::NetworkManager NetworkMngr { get; private set; }

	public global::MissionEndDataSave MissionEndData { get; private set; }

	public global::Tyche NetworkTyche { get; private set; }

	public global::CombatLogger CombatLogger { get; private set; }

	public int CurrentLadderIdx { get; private set; }

	public global::UnitController focusedUnit { get; private set; }

	public bool transitionDone { get; private set; }

	public global::Seeker PathSeeker { get; set; }

	public global::System.Collections.Generic.List<global::MapImprint> MapImprints { get; private set; }

	public global::TurnTimer TurnTimer { get; private set; }

	private bool IsNavmeshWorking
	{
		get
		{
			return global::AstarPath.active.isScanning || global::AstarPath.active.IsAnyGraphUpdateQueued || global::AstarPath.active.IsAnyWorkItemInProgress || global::AstarPath.active.IsAnyGraphUpdateInProgress;
		}
	}

	public bool IsNavmeshUpdating
	{
		get
		{
			return this.navGraphNeedsRefresh || this.IsNavmeshWorking;
		}
	}

	private void Awake()
	{
		this.WarbandCtrlrs = new global::System.Collections.Generic.List<global::WarbandController>();
		this.InitiativeLadder = new global::System.Collections.Generic.List<global::UnitController>();
		this.allEnemiesList = new global::System.Collections.Generic.List<global::UnitController>();
		this.allLiveEnemiesList = new global::System.Collections.Generic.List<global::UnitController>();
		this.allLiveAlliesList = new global::System.Collections.Generic.List<global::UnitController>();
		this.allLiveMyUnitsList = new global::System.Collections.Generic.List<global::UnitController>();
		this.allLiveUnits = new global::System.Collections.Generic.List<global::UnitController>();
		this.allMyUnitsList = new global::System.Collections.Generic.List<global::UnitController>();
		this.allUnitsList = new global::System.Collections.Generic.List<global::UnitController>();
		this.MapImprints = new global::System.Collections.Generic.List<global::MapImprint>();
		this.interactivePoints = new global::System.Collections.Generic.List<global::InteractivePoint>();
		this.triggerPoints = new global::System.Collections.Generic.List<global::TriggerPoint>();
		this.decisionPoints = new global::System.Collections.Generic.List<global::DecisionPoint>();
		this.locateZones = new global::System.Collections.Generic.List<global::LocateZone>();
		this.patrolRoutes = new global::System.Collections.Generic.Dictionary<string, global::PatrolRoute>();
		this.zoneAoes = new global::System.Collections.Generic.List<global::ZoneAoe>();
		this.spawnNodes = new global::System.Collections.Generic.List<global::SpawnNode>();
		this.delayedUnits = new global::System.Collections.Generic.Stack<global::UnitController>();
		this.MsgManager = new global::MessageManager();
		this.externalUpdators = new global::System.Collections.Generic.List<global::ExternalUpdator>();
		this.extUpdaterIndex = 0;
		this.VictoriousTeamIdx = -1;
		this.numWyrdstones = 0;
		this.envGuid = 0U;
		this.rtGuid = 200000000U;
		this.CamManager = global::UnityEngine.GameObject.Find("game_camera").GetComponent<global::CameraManager>();
		this.NetworkMngr = new global::NetworkManager();
		this.NetworkTyche = new global::Tyche(global::PandoraSingleton<global::MissionStartData>.Instance.Seed, false);
		this.MsgManager.Init((global::CampaignMissionId)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.campaignId);
		this.CombatLogger = new global::CombatLogger();
		this.InitBeacons();
		this.InitTargetingAssets();
		global::PandoraDebug.LogInfo("Set NetworkTyche Seed = " + global::PandoraSingleton<global::MissionStartData>.Instance.Seed, "TYCHE", this);
		this.MissionEndData = new global::MissionEndDataSave();
		if (global::PandoraSingleton<global::GameManager>.Instance.currentSave != null)
		{
			global::PandoraSingleton<global::GameManager>.Instance.currentSave.endMission = this.MissionEndData;
		}
		this.MoveCircle = base.GetComponent<global::MovementCircles>();
		global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MISSION, false);
		this.StateMachine = new global::CheapStateMachine(9);
		this.StateMachine.AddState(new global::StartGame(this), 0);
		this.StateMachine.AddState(new global::Deployment(this), 1);
		this.StateMachine.AddState(new global::WaitGameSetup(this), 3);
		this.StateMachine.AddState(new global::StartRound(this), 2);
		this.StateMachine.AddState(new global::UnitMovement(this), 5);
		this.StateMachine.AddState(new global::WatchUnit(this), 6);
		this.StateMachine.AddState(new global::EndRound(this), 7);
		this.StateMachine.AddState(new global::Rout(this), 4);
		this.StateMachine.AddState(new global::EndGame(this), 8);
		this.StateMachine.SetBlockingStateIdx(8);
		global::PandoraSingleton<global::PandoraInput>.instance.SetActive(false);
		this.nearestNodeConstraint = global::Pathfinding.NNConstraint.Default;
		this.nearestNodeConstraint.graphMask = 1;
		this.lastWarbandIdx = -1;
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SKIRMISH_INVITE_ACCEPTED, new global::DelReceiveNotice(this.InviteReceived));
	}

	public void RegisterExternalUpdator(global::ExternalUpdator up)
	{
		this.externalUpdators.Add(up);
	}

	public void ReleaseExternalUpdator(global::ExternalUpdator up)
	{
		this.externalUpdators.Remove(up);
	}

	public uint GetNextEnvGUID()
	{
		return this.envGuid++;
	}

	public uint GetNextRTGUID()
	{
		return this.rtGuid++;
	}

	public void OnDestroy()
	{
		global::UnityEngine.Time.timeScale = 1f;
		this.StateMachine.Destroy();
		this.NetworkMngr.Remove();
		this.NetworkTyche = null;
		if (global::PandoraSingleton<global::NoticeManager>.Instance != null)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.GAME_TUTO_MESSAGE, new global::DelReceiveNotice(this.OnTutoMessageShown));
		}
	}

	private void Start()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.TRANSITION_DONE, new global::DelReceiveNotice(this.OnTransitionDone));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.GAME_TUTO_MESSAGE, new global::DelReceiveNotice(this.OnTutoMessageShown));
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/", global::AssetBundleId.PROPS, "loot_bag.prefab", delegate(global::UnityEngine.Object prefab)
		{
			this.lootbagPrefab = (global::UnityEngine.GameObject)prefab;
		});
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, "fx_pointing_range.prefab", delegate(global::UnityEngine.Object prefab)
		{
			for (int i = 0; i < 10; i++)
			{
				global::UnityEngine.GameObject item = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(prefab);
				this.pointingArrows.Add(item);
			}
		});
	}

	private void Update()
	{
		if (global::PandoraSingleton<global::TransitionManager>.Instance.GameLoadingDone && global::PandoraSingleton<global::TransitionManager>.Instance.IsDone() && this.checkInvite)
		{
			this.CheckInvite();
		}
		if (this.checkMultiPlayerConnection && this.GetPlayersCount() == 2 && (!global::PandoraSingleton<global::Hermes>.Instance.IsConnected() || !global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline()))
		{
			this.checkMultiPlayerConnection = false;
			global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.TRANSITION_DONE, new global::DelReceiveNotice(this.OnTransitionDone));
			global::PandoraSingleton<global::TransitionManager>.Instance.SetGameLoadingDone(true);
			this.OnConnectionLost(false);
		}
		if (!this.lockNavRefresh && !this.IsNavmeshWorking && this.navGraphNeedsRefresh)
		{
			this.UpdateGraph();
		}
		if (this.TurnTimer != null)
		{
			this.TurnTimer.Update();
		}
		if (this.StateMachine.GetActiveStateId() != -1)
		{
			int count = this.externalUpdators.Count;
			int num = 0;
			while (num < 150 && num < count)
			{
				if (this.extUpdaterIndex >= count)
				{
					this.extUpdaterIndex = 0;
				}
				this.externalUpdators[this.extUpdaterIndex].ExternalUpdate();
				num++;
				this.extUpdaterIndex++;
			}
		}
		this.StateMachine.Update();
	}

	private void FixedUpdate()
	{
		this.StateMachine.FixedUpdate();
		for (int i = this.MapImprints.Count - 1; i >= 0; i--)
		{
			global::MapImprint mapImprint = this.MapImprints[i];
			if (mapImprint != null)
			{
				if (mapImprint.UnitCtrlr == null || mapImprint.UnitCtrlr.isInLadder)
				{
					mapImprint.RefreshPosition();
				}
			}
			else
			{
				this.MapImprints.RemoveAt(i);
			}
		}
		if (this.resendLadder)
		{
			this.resendLadder = false;
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.LADDER_CHANGED);
		}
	}

	public int GetPlayersCount()
	{
		if (this.WarbandCtrlrs == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlrs[i].playerTypeId == global::PlayerTypeId.PLAYER)
			{
				num++;
			}
		}
		return num;
	}

	private void InviteReceived()
	{
		this.checkInvite = true;
	}

	private void CheckInvite()
	{
		this.checkInvite = false;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto)
		{
			global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "invite_mission_quit_tuto_title", "invite_mission_quit_tuto_desc", new global::System.Action<bool>(this.OnMissionReceiveInviteShouldSaveAndQuit), true);
			return;
		}
		global::Warband warband = new global::Warband(global::PandoraSingleton<global::GameManager>.instance.currentSave);
		if (this.MissionEndData.isSkirmish || this.GetPlayersCount() == 2)
		{
			if (!warband.ValidateWarbandForInvite(false))
			{
				global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "invite_skirmish_quit_title", "invite_skirmish_quit_desc", new global::System.Action<bool>(this.OnMissionReceiveInviteShouldSaveAndQuit), true);
			}
			else
			{
				global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "invite_skirmish_load_title", "invite_skirmish_load_desc", new global::System.Action<bool>(this.OnMissionReceiveInviteShouldForfeit), true);
			}
		}
		else if (!warband.ValidateWarbandForInvite(false))
		{
			global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "invite_mission_quit_title", "invite_mission_quit_desc", new global::System.Action<bool>(this.OnMissionReceiveInviteShouldSaveAndQuit), true);
		}
		else
		{
			global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "invite_mission_forfeit_title", "invite_mission_forfeit_desc", new global::System.Action<bool>(this.OnMissionReceiveInviteShouldForfeit), true);
		}
	}

	private void OnMissionReceiveInviteShouldSaveAndQuit(bool confirm)
	{
		if (confirm)
		{
			this.SaveAndQuit();
		}
		else
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.ResetInvite();
		}
	}

	private void OnMissionReceiveInviteShouldForfeit(bool confirm)
	{
		if (confirm)
		{
			this.NetworkMngr.SendForfeitMission(this.GetMyWarbandCtrlr().idx);
		}
		else
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.ResetInvite();
		}
	}

	public void CreateMissionEndData()
	{
		global::MissionSave missionSave = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave;
		this.MissionEndData.missionSave = missionSave;
		this.MissionEndData.ratingId = (global::ProcMissionRatingId)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.ratingId;
		this.MissionEndData.isCampaign = missionSave.isCampaign;
		this.MissionEndData.isSkirmish = missionSave.isSkirmish;
		this.MissionEndData.seed = global::PandoraSingleton<global::MissionStartData>.Instance.Seed;
		this.MissionEndData.missionWarbands = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands;
		int playersCount = this.GetPlayersCount();
		this.MissionEndData.isVsAI = (playersCount == 1);
		if (!this.MissionEndData.isVsAI)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.MULTI_PLAY, 1);
		}
		global::System.Collections.Generic.List<global::UnitController> list = new global::System.Collections.Generic.List<global::UnitController>();
		list.AddRange(this.allUnitsList);
		list.AddRange(this.excludedUnits);
		this.MissionEndData.AddUnits(list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			this.MissionEndData.UpdateUnit(list[i]);
		}
		this.MissionEndData.warbandMorals.Clear();
		for (int j = 0; j < this.WarbandCtrlrs.Count; j++)
		{
			global::Tuple<int, int, bool> item = new global::Tuple<int, int, bool>(this.WarbandCtrlrs[j].MoralValue, this.WarbandCtrlrs[j].OldMoralValue, this.WarbandCtrlrs[j].idolMoralRemoved);
			this.MissionEndData.warbandMorals.Add(item);
		}
		this.MissionEndData.destroyedTraps.Clear();
		this.MissionEndData.destroyedTraps.AddRange(global::PandoraSingleton<global::MissionStartData>.Instance.usedTraps);
		this.MissionEndData.aoeZones.Clear();
		this.MissionEndData.aoeZones.AddRange(global::PandoraSingleton<global::MissionStartData>.Instance.aoeZones);
		this.MissionEndData.objectives.Clear();
		this.MissionEndData.objectives.AddRange(global::PandoraSingleton<global::MissionStartData>.Instance.objectives);
		this.MissionEndData.converters.Clear();
		this.MissionEndData.converters.AddRange(global::PandoraSingleton<global::MissionStartData>.Instance.converters);
		this.MissionEndData.activaters.Clear();
		this.MissionEndData.activaters.AddRange(global::PandoraSingleton<global::MissionStartData>.Instance.activaters);
		if (global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
		{
			this.MissionEndData.myrtilusLadder.Clear();
			this.MissionEndData.myrtilusLadder.AddRange(global::PandoraSingleton<global::MissionStartData>.Instance.myrtilusLadder);
			this.MissionEndData.currentLadderIdx = global::PandoraSingleton<global::MissionStartData>.Instance.currentLadderIdx;
			this.MissionEndData.currentTurn = global::PandoraSingleton<global::MissionStartData>.Instance.currentTurn;
		}
		if (!this.MissionEndData.missionSave.isTuto && global::PandoraSingleton<global::GameManager>.Instance.currentSave != null)
		{
			global::PandoraSingleton<global::GameManager>.Instance.currentSave.inMission = true;
			global::PandoraSingleton<global::GameManager>.Instance.Save.SaveCampaign(global::PandoraSingleton<global::GameManager>.Instance.currentSave, global::PandoraSingleton<global::GameManager>.Instance.campaign);
		}
	}

	public void SaveAndQuit()
	{
		if (global::PandoraSingleton<global::GameManager>.Instance.currentSave != null)
		{
			global::PandoraSingleton<global::GameManager>.Instance.Save.SaveCampaign(global::PandoraSingleton<global::GameManager>.Instance.currentSave, global::PandoraSingleton<global::GameManager>.Instance.campaign);
		}
		global::PandoraSingleton<global::Hermes>.Instance.StopConnections(true);
		global::PandoraSingleton<global::Hephaestus>.Instance.LeaveLobby();
		global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.OPTIONS_QUIT_GAME, false, true);
	}

	public void ForceQuitMission()
	{
		base.StartCoroutine(this.QuitMissionRoutine());
	}

	private global::System.Collections.IEnumerator QuitMissionRoutine()
	{
		yield return new global::UnityEngine.WaitForFixedUpdate();
		global::PandoraDebug.LogDebug("Mission Manager::QuitMission", "uncategorised", null);
		bool wasOnline = global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline();
		bool wasConnected = global::PandoraSingleton<global::Hermes>.Instance.IsConnected();
		global::PandoraSingleton<global::Hermes>.Instance.StopConnections(true);
		global::PandoraSingleton<global::Hephaestus>.Instance.LeaveLobby();
		this.MissionEndData.missionSave = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave;
		if (this.MissionEndData.missionSave.isTuto)
		{
			global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.OPTIONS_QUIT_GAME, false, true);
		}
		else if (this.StateMachine.GetActiveStateId() <= 3 && this.GetPlayersCount() == 2)
		{
			global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_HIDEOUT, false, true);
		}
		else
		{
			global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_END_GAME, false, true);
		}
		yield break;
	}

	public void SendLoadingDone()
	{
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlrs[i].playerIdx == global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex)
			{
				this.NetworkMngr.SendLoadingDone(global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex);
			}
		}
	}

	public void SetLoadingDone()
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"SetLoadingDone = ",
			this.syncDone + 1,
			"Numwabands = ",
			this.WarbandCtrlrs.Count
		}), "HERMES", this);
		this.syncDone++;
		if (this.syncDone >= 2 && this.syncDone == this.WarbandCtrlrs.Count)
		{
			if (global::PandoraSingleton<global::GameManager>.Instance.IsFastForwarded)
			{
				global::UnityEngine.Time.timeScale = 1.15f;
			}
			global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SYSTEM_RESUME, new global::DelReceiveNotice(this.CheckConnection));
			global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.HERMES_CONNECTION_LOST, new global::DelReceiveNotice(this.OnConnectionLostNoResume));
			this.checkMultiPlayerConnection = false;
			this.syncDone = 0;
			for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
			{
				global::WarbandController warbandController = this.WarbandCtrlrs[i];
				this.InitiativeLadder.AddRange(warbandController.unitCtrlrs);
				for (int j = warbandController.unitCtrlrs.Count - 1; j >= 0; j--)
				{
					global::UnitController unitController = warbandController.unitCtrlrs[j];
					if (unitController.unit.CampaignData != null)
					{
						if (unitController.unit.CampaignData.StartHidden)
						{
							this.ExcludeUnit(unitController);
						}
						else if (unitController.unit.CampaignData.StartInactive)
						{
							this.RemoveUnitFromLadder(unitController);
						}
					}
					if (unitController.unit.Id == global::UnitId.BLUE_HORROR)
					{
						this.ExcludeUnit(unitController);
					}
				}
			}
			this.InitWalkability();
			this.StateMachine.ChangeState(0);
			global::PandoraSingleton<global::Hephaestus>.Instance.LeaveLobby();
			global::PandoraSingleton<global::Hermes>.Instance.DoNotDisconnectMode = false;
			global::PandoraSingleton<global::FlyingTextManager>.Instance.Init();
			int num = 6;
			global::System.Collections.Generic.List<global::UnitController> allEnemies = this.GetAllEnemies(this.GetMyWarbandCtrlr().idx);
			for (int k = 0; k < allEnemies.Count; k++)
			{
				if (allEnemies[k].unit.Data.Id == global::UnitId.MANTICORE && num > 1)
				{
					num = 1;
					break;
				}
				if (num > 2 && allEnemies[k].unit.Data.Id == global::UnitId.CURATOR)
				{
					num = 2;
				}
				else if (num > 3 && allEnemies[k].unit.Data.Id == global::UnitId.ALLURESS)
				{
					num = 3;
				}
				else if (num > 3 && allEnemies[k].unit.Data.Id == global::UnitId.CHAOS_OGRE)
				{
					num = 4;
				}
				else if (num > 3 && allEnemies[k].unit.RaceId == global::RaceId.DAEMON)
				{
					num = 5;
				}
			}
			if (num == 1)
			{
				global::PandoraSingleton<global::Pan>.Instance.PlayMusic("boss", true);
			}
			else if (num == 2)
			{
				global::PandoraSingleton<global::Pan>.Instance.PlayMusic("generic_01", true);
			}
			else if (num == 3)
			{
				global::PandoraSingleton<global::Pan>.Instance.PlayMusic("generic_04", true);
			}
			else if (num == 4)
			{
				global::PandoraSingleton<global::Pan>.Instance.PlayMusic("generic_02", true);
			}
			else if (num == 5)
			{
				global::PandoraSingleton<global::Pan>.Instance.PlayMusic("generic_03", true);
			}
			else
			{
				bool flag = false;
				for (int l = 0; l < this.WarbandCtrlrs.Count; l++)
				{
					if (!this.WarbandCtrlrs[l].IsPlayed() && this.WarbandCtrlrs[l].WarData.Basic)
					{
						flag = true;
						global::PandoraSingleton<global::Pan>.Instance.PlayMusic(this.WarbandCtrlrs[l].WarData.Id.ToLowerString(), true);
						break;
					}
				}
				if (!flag)
				{
					global::PandoraSingleton<global::Pan>.Instance.PlayMusic(this.GetMyWarbandCtrlr().WarData.Id.ToLowerString(), true);
				}
			}
		}
		else if (this.GetPlayersCount() == 2)
		{
			this.checkMultiPlayerConnection = true;
		}
	}

	public void SendTurnReady()
	{
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlrs[i].playerIdx == global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex)
			{
				this.NetworkMngr.SendTurnReady();
			}
		}
	}

	public void SetTurnReady()
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"SetTurnReady = ",
			this.syncDone + 1,
			"Numwabands = ",
			this.WarbandCtrlrs.Count
		}), "HERMES", this);
		if (++this.syncDone == this.WarbandCtrlrs.Count)
		{
			this.syncDone = 0;
			if ((this.currentTurn == 0 || global::PandoraSingleton<global::MissionStartData>.Instance.isReload) && (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.autoDeploy))
			{
				global::PandoraSingleton<global::MissionStartData>.Instance.isReload = false;
				global::PandoraSingleton<global::UIMissionManager>.Instance.messagePopup.Hide();
			}
			else
			{
				this.SelectNextLadderUnit(1);
			}
		}
	}

	public void EndLoading()
	{
		this.gameFinished = false;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.autoDeploy)
		{
			this.StateMachine.ChangeState(1);
		}
		else
		{
			this.StateMachine.ChangeState(3);
			global::PandoraSingleton<global::TransitionManager>.Instance.SetGameLoadingDone(false);
		}
	}

	private void OnTransitionDone()
	{
		global::PandoraDebug.LogInfo("OnTransitionDone", "FLOW", this);
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.TRANSITION_DONE, new global::DelReceiveNotice(this.OnTransitionDone));
		this.transitionDone = true;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.autoDeploy)
		{
			this.StateMachine.ChangeState(2);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<int>(global::Notices.MISSION_ROUND_START, this.currentTurn);
			this.SelectNextLadderUnit(1);
			this.resendLadder = true;
		}
		else
		{
			this.StateMachine.ChangeState(1);
		}
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(true);
	}

	public bool CheckUnitTurnFinished()
	{
		int activeStateId = this.GetCurrentUnit().StateMachine.GetActiveStateId();
		if (activeStateId == 39)
		{
			global::PandoraDebug.LogDebug("Unit " + this.GetCurrentUnit().name + " has finished it's turn, calling SelectNextLadderUnit.", "FLOW", null);
			this.SelectNextLadderUnit(1);
			return true;
		}
		return false;
	}

	public void OnTutoMessageShown()
	{
		bool flag = (bool)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[2];
		if (flag)
		{
			global::CampaignMissionData campaignMissionData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionData>(global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.campaignId);
			int idx = campaignMissionData.Idx - 1;
			if (global::PandoraSingleton<global::MissionStartData>.instance.CurrentMission.missionSave.isCampaign && !global::PandoraSingleton<global::MissionStartData>.instance.CurrentMission.missionSave.isTuto)
			{
				idx = 4;
			}
			global::PandoraSingleton<global::GameManager>.Instance.Profile.CompleteTutorial(idx);
			global::PandoraSingleton<global::GameManager>.Instance.SaveProfile();
		}
	}

	private void OnSkirmishInviteAccepted()
	{
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isSkirmish)
		{
			if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto && global::PandoraSingleton<global::GameManager>.Instance.campaign == -1)
			{
				global::PandoraSingleton<global::GameManager>.Instance.campaign = global::PandoraSingleton<global::GameManager>.Instance.Profile.LastPlayedCampaign;
				if (global::PandoraSingleton<global::GameManager>.Instance.campaign == -1)
				{
					global::PandoraSingleton<global::UIMissionManager>.Instance.messagePopup.Show("join_lobby_title_no_warband", "join_lobby_desc_no_warband", null, false, false);
					global::PandoraSingleton<global::UIMissionManager>.Instance.messagePopup.HideCancelButton();
				}
			}
			if (global::PandoraSingleton<global::GameManager>.Instance.campaign != -1)
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.messagePopup.Show("join_lobby_title_quit_game", "join_lobby_desc_quit_game_safe", new global::System.Action<bool>(this.OnQuitGameResult), false, false);
			}
		}
	}

	private void OnQuitGameResult(bool confirm)
	{
		if (confirm)
		{
			this.NetworkMngr.SendForfeitMission(this.GetMyWarbandCtrlr().idx);
		}
	}

	private global::System.Collections.IEnumerator ConnectionLostWaitLoading(bool isResume)
	{
		while (global::PandoraSingleton<global::MissionLoader>.Exists())
		{
			yield return null;
		}
		this.OnConnectionLost(isResume);
		yield break;
	}

	public void CheckConnection()
	{
		if (this.GetPlayersCount() == 2 && !global::PandoraSingleton<global::Hermes>.Instance.IsConnected())
		{
			this.OnConnectionLost(false);
		}
	}

	private void OnConnectionLostNoResume()
	{
		this.OnConnectionLost(false);
	}

	public void OnConnectionLost(bool isResume = false)
	{
		if (global::PandoraSingleton<global::MissionLoader>.Exists())
		{
			base.StartCoroutine(this.ConnectionLostWaitLoading(isResume));
			return;
		}
		if (this.StateMachine.GetActiveStateId() == 8)
		{
			return;
		}
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(true);
		if (!global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline() || isResume)
		{
			global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.LOST_CONNECTION, "connection_error_title", global::PandoraSingleton<global::Hephaestus>.Instance.GetOfflineReason(), new global::System.Action<bool>(this.ConnectionLost), false);
		}
		else
		{
			global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.LOST_CONNECTION, "connection_error_title", "connection_error_opponent_left", new global::System.Action<bool>(this.ConnectionLost), false);
		}
	}

	private void ConnectionLost(bool confirm)
	{
		if (!global::PandoraSingleton<global::MissionManager>.Exists() || this.StateMachine == null || this.StateMachine.GetActiveStateId() == 8)
		{
			return;
		}
		if (this.StateMachine.GetActiveStateId() <= 3)
		{
			this.ForceQuitMission();
		}
		else if (global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline())
		{
			global::System.Collections.Generic.List<global::WarbandController> enemyWarbandCtrlrs = this.GetEnemyWarbandCtrlrs();
			this.ForfeitMission(enemyWarbandCtrlrs[0].idx);
		}
		else
		{
			this.ForfeitMission(this.GetMyWarbandCtrlr().idx);
		}
	}

	public void ForfeitMission(int warbandidx)
	{
		this.WarbandCtrlrs[warbandidx].defeated = true;
		if (this.WarbandCtrlrs[warbandidx].MoralRatio > global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.routThreshold)
		{
			this.MissionEndData.crushed = true;
			for (int i = 0; i < this.WarbandCtrlrs[warbandidx].unitCtrlrs.Count; i++)
			{
				this.WarbandCtrlrs[warbandidx].unitCtrlrs[i].unit.CurrentWound = 0;
				this.WarbandCtrlrs[warbandidx].unitCtrlrs[i].unit.SetStatus(global::UnitStateId.OUT_OF_ACTION);
			}
		}
		if (!global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying)
		{
			this.CheckEndGame();
		}
	}

	public bool CheckEndGame()
	{
		this.MissionEndData.playerMVUIdx = this.GetMyWarbandCtrlr().GetMVU(global::PandoraSingleton<global::GameManager>.instance.LocalTyche, false);
		this.MissionEndData.enemyMVUIdx = this.GetEnemyWarbandCtrlrs()[0].GetMVU(global::PandoraSingleton<global::GameManager>.instance.LocalTyche, true);
		global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::WarbandController>> dictionary = new global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::WarbandController>>();
		int num = 0;
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlrs[i].IsPlayed() && this.WarbandCtrlrs[i].MoralRatio <= global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.routThreshold)
			{
				this.MissionEndData.routable = true;
			}
			this.WarbandCtrlrs[i].CheckObjectives(false, false);
			if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.VictoryTypeId == 2 && this.WarbandCtrlrs[i].HasFailedMandatoryObjective())
			{
				this.WarbandCtrlrs[i].defeated = true;
			}
			if (this.WarbandCtrlrs[i].playerTypeId == global::PlayerTypeId.PLAYER)
			{
				num++;
			}
			if (!this.WarbandCtrlrs[i].IsRoaming())
			{
				this.MissionEndData.crushed = false;
				if (!this.WarbandCtrlrs[i].defeated)
				{
					bool flag = false;
					for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
					{
						if (this.WarbandCtrlrs[i].unitCtrlrs[j].unit.Status != global::UnitStateId.OUT_OF_ACTION)
						{
							flag = true;
							break;
						}
					}
					this.WarbandCtrlrs[i].defeated = (!flag || this.WarbandCtrlrs[i].MoralValue <= 0);
					this.MissionEndData.crushed |= !flag;
				}
				if (!dictionary.ContainsKey(this.WarbandCtrlrs[i].teamIdx))
				{
					dictionary[this.WarbandCtrlrs[i].teamIdx] = new global::System.Collections.Generic.List<global::WarbandController>();
				}
				dictionary[this.WarbandCtrlrs[i].teamIdx].Add(this.WarbandCtrlrs[i]);
			}
		}
		int victoriousTeamIdx = -1;
		int num2 = 0;
		int num3 = 0;
		foreach (int num4 in dictionary.Keys)
		{
			global::System.Collections.Generic.List<global::WarbandController> list = dictionary[num4];
			bool flag2 = true;
			for (int k = 0; k < list.Count; k++)
			{
				if (list[k].BlackList != 0)
				{
					flag2 &= list[k].defeated;
				}
			}
			if (!flag2)
			{
				victoriousTeamIdx = num4;
				num2++;
			}
			else
			{
				num3++;
			}
		}
		bool flag3 = false;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.VictoryTypeId == 2)
		{
			for (int l = 0; l < this.WarbandCtrlrs.Count; l++)
			{
				if (this.WarbandCtrlrs[l].IsPlayed())
				{
					if (this.WarbandCtrlrs[l].AllObjectivesCompleted)
					{
						victoriousTeamIdx = l;
						flag3 = true;
						break;
					}
					if (this.WarbandCtrlrs[l].defeated)
					{
						flag3 = true;
						break;
					}
				}
			}
		}
		else
		{
			flag3 = (num2 == 1 || num3 == this.WarbandCtrlrs.Count || (num == 1 && this.GetMyWarbandCtrlr().defeated));
		}
		if (flag3)
		{
			this.VictoriousTeamIdx = victoriousTeamIdx;
			global::WarbandController myWarbandCtrlr = this.GetMyWarbandCtrlr();
			if (myWarbandCtrlr.teamIdx == this.VictoriousTeamIdx)
			{
				int num5 = 0;
				bool flag4 = false;
				for (int m = 0; m < myWarbandCtrlr.unitCtrlrs.Count; m++)
				{
					if (myWarbandCtrlr.unitCtrlrs[m].unit.Status != global::UnitStateId.OUT_OF_ACTION)
					{
						num5++;
					}
					flag4 |= myWarbandCtrlr.unitCtrlrs[m].unit.HasInjury(global::InjuryId.SEVERED_ARM);
					flag4 |= myWarbandCtrlr.unitCtrlrs[m].unit.HasInjury(global::InjuryId.SEVERED_LEG);
				}
				if (num5 == 1)
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.WIN_ALONE);
				}
				if (flag4)
				{
					global::PandoraSingleton<global::Hephaestus>.instance.UnlockAchievement(global::Hephaestus.TrophyId.WIN_CRIPPLED);
				}
				if (!this.MissionEndData.isVsAI)
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.MULTI_WINS, 1);
				}
			}
			this.StateMachine.ChangeState(8);
		}
		return flag3;
	}

	public void SetTurnTimer(float turnDuration, global::UnityEngine.Events.UnityAction onDone = null)
	{
		if (onDone == null)
		{
			onDone = new global::UnityEngine.Events.UnityAction(this.OnTurnTimerDone);
		}
		if (this.TurnTimer != null)
		{
			this.TurnTimer.Pause();
		}
		this.TurnTimer = new global::TurnTimer(turnDuration, onDone);
	}

	public void OnTurnTimerDone()
	{
		global::PandoraDebug.LogInfo("UNIT TURN FORCED TO END BY THE TIMER", "FLOW", this.GetCurrentUnit());
		if (this.IsCurrentPlayer())
		{
			this.GetCurrentUnit().SendSkill(global::SkillId.BASE_END_TURN);
		}
	}

	public void UpdateObjectivesUI(bool flyCam = false)
	{
		global::WarbandController myWarbandCtrlr = this.GetMyWarbandCtrlr();
		float percSearch = myWarbandCtrlr.percSearch;
		float percWyrd = myWarbandCtrlr.percWyrd;
		global::UnityEngine.Vector2 v = new global::UnityEngine.Vector2((float)myWarbandCtrlr.openedSearch.Count, (float)this.GetSearchPoints().Count);
		int num = 0;
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			num += this.WarbandCtrlrs[i].GetCollectedWyrdStone();
		}
		global::UnityEngine.Vector2 v2 = new global::UnityEngine.Vector2((float)num, (float)this.numWyrdstones);
		myWarbandCtrlr.percSearch = (float)myWarbandCtrlr.openedSearch.Count / (float)this.GetSearchPoints().Count;
		myWarbandCtrlr.percWyrd = (float)num / (float)this.numWyrdstones;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
		{
			if (percSearch < 1f && myWarbandCtrlr.percSearch >= 1f)
			{
				global::PandoraSingleton<global::Pan>.Instance.Narrate("search_points4");
			}
			else if ((double)percSearch < 0.75 && (double)myWarbandCtrlr.percSearch >= 0.75)
			{
				global::PandoraSingleton<global::Pan>.Instance.Narrate("search_points3");
			}
			else if ((double)percSearch < 0.5 && (double)myWarbandCtrlr.percSearch >= 0.5)
			{
				global::PandoraSingleton<global::Pan>.Instance.Narrate("search_points2");
			}
			else if ((double)percSearch < 0.25 && (double)myWarbandCtrlr.percSearch >= 0.25)
			{
				global::PandoraSingleton<global::Pan>.Instance.Narrate("search_points1");
			}
			if (percWyrd < 1f && myWarbandCtrlr.percWyrd >= 1f)
			{
				global::PandoraSingleton<global::Pan>.Instance.Narrate("wyrdstone_claimed4");
			}
			else if ((double)percWyrd < 0.75 && (double)myWarbandCtrlr.percWyrd >= 0.75)
			{
				global::PandoraSingleton<global::Pan>.Instance.Narrate("wyrdstone_claimed3");
			}
			else if ((double)percWyrd < 0.5 && (double)myWarbandCtrlr.percWyrd >= 0.5)
			{
				global::PandoraSingleton<global::Pan>.Instance.Narrate("wyrdstone_claimed2");
			}
			else if ((double)percWyrd < 0.25 && (double)myWarbandCtrlr.percWyrd >= 0.25)
			{
				global::PandoraSingleton<global::Pan>.Instance.Narrate("wyrdstone_claimed1");
			}
		}
		if (myWarbandCtrlr != null)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::System.Collections.Generic.List<global::Objective>, global::UnityEngine.Vector2, global::UnityEngine.Vector2>(global::Notices.GAME_OBJECTIVE_UPDATE, myWarbandCtrlr.objectives, v, v2);
		}
	}

	public bool IsCurrentPlayer()
	{
		global::UnitController currentUnit = this.GetCurrentUnit();
		return currentUnit != null && currentUnit.GetWarband().playerIdx == global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex;
	}

	public void SetDepoyLadderIndex(int index)
	{
		this.CurrentLadderIdx = index;
	}

	public global::UnitController GetCurrentUnit()
	{
		if (this.InitiativeLadder != null && this.CurrentLadderIdx >= 0 && this.CurrentLadderIdx < this.InitiativeLadder.Count)
		{
			return this.InitiativeLadder[this.CurrentLadderIdx];
		}
		return null;
	}

	public global::UnitController GetUnitController(global::Unit unit, bool includeExclude = false)
	{
		for (int i = 0; i < this.allUnitsList.Count; i++)
		{
			if (this.allUnitsList[i].unit == unit)
			{
				return this.allUnitsList[i];
			}
		}
		if (includeExclude)
		{
			for (int j = 0; j < this.excludedUnits.Count; j++)
			{
				if (this.excludedUnits[j].unit == unit)
				{
					return this.excludedUnits[j];
				}
			}
		}
		return null;
	}

	public global::UnitController GetUnitController(uint uid)
	{
		for (int i = 0; i < this.allUnitsList.Count; i++)
		{
			if (this.allUnitsList[i].uid == uid)
			{
				return this.allUnitsList[i];
			}
		}
		return null;
	}

	public bool DissolveDeadUnits()
	{
		bool result = false;
		for (int i = 0; i < this.allUnitsList.Count; i++)
		{
			if (this.allUnitsList[i] != null && this.allUnitsList[i].lootBagPoint != null)
			{
				result = true;
				if (this.allUnitsList[i].lootBagPoint.visual != null && !this.allUnitsList[i].lootBagPoint.IsEmpty())
				{
					this.allUnitsList[i].lootBagPoint.visual.SetActive(true);
				}
				this.allUnitsList[i].Imprint.alwaysHide = true;
				this.allUnitsList[i].Hide(true, false, null);
			}
		}
		return result;
	}

	public void ExcludeUnit(global::UnitController ctrlr)
	{
		this.excludedUnits.Add(ctrlr);
		ctrlr.GetWarband().unitCtrlrs.Remove(ctrlr);
		this.allUnitsList.Remove(ctrlr);
		this.RemoveUnitFromLadder(ctrlr);
		this.ResetAllUnitsTargetData();
		ctrlr.gameObject.SetActive(false);
	}

	public void IncludeUnit(global::UnitController ctrlr, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot)
	{
		this.IncludeUnit(ctrlr, this.WarbandCtrlrs[ctrlr.unit.warbandIdx], pos, rot);
	}

	public void IncludeUnit(global::UnitController ctrlr, global::WarbandController warCtrlr, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot)
	{
		ctrlr.gameObject.SetActive(true);
		ctrlr.Imprint.alwaysHide = false;
		this.excludedUnits.Remove(ctrlr);
		warCtrlr.unitCtrlrs.Add(ctrlr);
		this.MissionEndData.UpdateUnit(ctrlr);
		this.allUnitsList.Add(ctrlr);
		this.AddUnitToLadder(ctrlr);
		this.ResetAllUnitsTargetData();
		ctrlr.transform.position = pos;
		ctrlr.transform.rotation = rot;
		ctrlr.StartGameInitialization();
		ctrlr.Deployed(true);
	}

	public void ResetAllUnitsTargetData()
	{
		for (int i = 0; i < this.allUnitsList.Count; i++)
		{
			this.allUnitsList[i].InitTargetsData();
		}
	}

	public void ForceUnitVisibilityCheck(global::UnitController unitCtrlr)
	{
		global::System.Collections.Generic.List<global::UnitController> myAliveUnits = this.GetMyAliveUnits();
		for (int i = 0; i < myAliveUnits.Count; i++)
		{
			myAliveUnits[i].UpdateTargetData(unitCtrlr);
		}
		this.RefreshFoWTargetMoving(unitCtrlr);
	}

	public void RegisterInteractivePoint(global::InteractivePoint point)
	{
		this.interactivePoints.Add(point);
	}

	public global::System.Collections.Generic.List<global::InteractivePoint> GetSearchPoints()
	{
		if (this.initialSearchPoints == null)
		{
			this.initialSearchPoints = new global::System.Collections.Generic.List<global::InteractivePoint>();
			for (int i = 0; i < this.interactivePoints.Count; i++)
			{
				if (this.interactivePoints[i] != null && this.interactivePoints[i] is global::SearchPoint && !((global::SearchPoint)this.interactivePoints[i]).isWyrdstone)
				{
					this.initialSearchPoints.Add(this.interactivePoints[i]);
				}
			}
		}
		return this.initialSearchPoints;
	}

	public global::System.Collections.Generic.List<global::SearchPoint> GetWyrdstonePoints()
	{
		global::System.Collections.Generic.List<global::SearchPoint> list = new global::System.Collections.Generic.List<global::SearchPoint>();
		for (int i = 0; i < this.interactivePoints.Count; i++)
		{
			if (this.interactivePoints[i] is global::SearchPoint && ((global::SearchPoint)this.interactivePoints[i]).isWyrdstone)
			{
				list.Add((global::SearchPoint)this.interactivePoints[i]);
			}
		}
		return list;
	}

	public int GetInitialWyrdstoneCount()
	{
		int num = 0;
		for (int i = 0; i < this.interactivePoints.Count; i++)
		{
			if (this.interactivePoints[i] is global::SearchPoint)
			{
				global::SearchPoint searchPoint = (global::SearchPoint)this.interactivePoints[i];
				for (int j = 0; j < searchPoint.items.Count; j++)
				{
					if (searchPoint.items[j].IsWyrdStone)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	public void GetUnclaimedLootableItems(ref global::System.Collections.Generic.List<global::Item> wyrdstones, ref global::System.Collections.Generic.List<global::Item> search)
	{
		for (int i = 0; i < this.interactivePoints.Count; i++)
		{
			global::SearchPoint searchPoint = this.interactivePoints[i] as global::SearchPoint;
			if (searchPoint != null && searchPoint.warbandController == null && searchPoint.unitController == null && !searchPoint.IsEmpty())
			{
				for (int j = 0; j < searchPoint.items.Count; j++)
				{
					if (searchPoint.items[j].TypeData.Id != global::ItemTypeId.QUEST_ITEM && searchPoint.items[j].Id != global::ItemId.NONE)
					{
						if (searchPoint.items[j].IsWyrdStone)
						{
							wyrdstones.Add(searchPoint.items[j]);
						}
						else
						{
							search.Add(searchPoint.items[j]);
						}
					}
				}
			}
		}
	}

	public void UnregisterInteractivePoint(global::InteractivePoint point)
	{
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
			{
				this.WarbandCtrlrs[i].unitCtrlrs[j].interactivePoints.Remove(point);
				if (this.WarbandCtrlrs[i].unitCtrlrs[j].AICtrlr != null && point is global::SearchPoint && this.WarbandCtrlrs[i].unitCtrlrs[j].AICtrlr.targetSearchPoint == (global::SearchPoint)point)
				{
					this.WarbandCtrlrs[i].unitCtrlrs[j].AICtrlr.targetSearchPoint = null;
				}
			}
		}
		this.interactivePoints.Remove(point);
		this.interactivePointsTrash.Add(point);
		if (point is global::SearchPoint)
		{
			global::SearchPoint searchPoint = (global::SearchPoint)point;
			for (int k = 0; k < this.MissionEndData.searches.Count; k++)
			{
				if (this.MissionEndData.searches[k].Key == searchPoint.guid)
				{
					global::SearchSave value = this.MissionEndData.searches[k].Value;
					value.items = null;
					this.MissionEndData.searches[k] = new global::System.Collections.Generic.KeyValuePair<uint, global::SearchSave>(searchPoint.guid, value);
				}
			}
		}
	}

	public int GetInteractivePointIndex(global::InteractivePoint point)
	{
		return this.interactivePoints.IndexOf(point);
	}

	public global::InteractivePoint GetInteractivePoint(int index)
	{
		return this.interactivePoints[index];
	}

	public global::System.Collections.Generic.List<global::SearchPoint> GetSearchPoints(string name)
	{
		global::System.Collections.Generic.List<global::SearchPoint> list = new global::System.Collections.Generic.List<global::SearchPoint>();
		for (int i = 0; i < this.interactivePoints.Count; i++)
		{
			if (this.interactivePoints[i] != null && this.interactivePoints[i].name == name && this.interactivePoints[i] is global::SearchPoint)
			{
				list.Add((global::SearchPoint)this.interactivePoints[i]);
			}
		}
		for (int j = 0; j < this.interactivePointsTrash.Count; j++)
		{
			if (this.interactivePointsTrash[j] != null && this.interactivePointsTrash[j].name == name && this.interactivePointsTrash[j] is global::SearchPoint)
			{
				list.Add((global::SearchPoint)this.interactivePointsTrash[j]);
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::ActivatePoint> GetActivatePoints(string name)
	{
		global::System.Collections.Generic.List<global::ActivatePoint> list = new global::System.Collections.Generic.List<global::ActivatePoint>();
		for (int i = 0; i < this.interactivePoints.Count; i++)
		{
			if (this.interactivePoints[i] != null && this.interactivePoints[i] is global::ActivatePoint && this.interactivePoints[i].name == name)
			{
				list.Add((global::ActivatePoint)this.interactivePoints[i]);
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::SearchPoint> GetSearchPointInRadius(global::UnityEngine.Vector3 pos, float dist, global::UnitActionId actionId)
	{
		global::System.Collections.Generic.List<global::SearchPoint> list = new global::System.Collections.Generic.List<global::SearchPoint>();
		for (int i = 0; i < this.interactivePoints.Count; i++)
		{
			if (this.interactivePoints[i] != null && this.interactivePoints[i].unitActionId == actionId && this.interactivePoints[i] is global::SearchPoint && !((global::SearchPoint)this.interactivePoints[i]).IsEmpty() && global::UnityEngine.Vector3.SqrMagnitude(pos - this.interactivePoints[i].transform.position) < dist * dist)
			{
				list.Add((global::SearchPoint)this.interactivePoints[i]);
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::Destructible> GetDestructibles(string name)
	{
		global::System.Collections.Generic.List<global::Destructible> list = new global::System.Collections.Generic.List<global::Destructible>();
		for (int i = 0; i < this.triggerPoints.Count; i++)
		{
			if (this.triggerPoints[i] != null && this.triggerPoints[i] is global::Destructible && this.triggerPoints[i].name == name)
			{
				list.Add((global::Destructible)this.triggerPoints[i]);
			}
		}
		return list;
	}

	public global::SearchPoint SpawnLootBag(global::UnitController owner, global::UnityEngine.Vector3 pos, global::System.Collections.Generic.List<global::ItemSave> itemSaves, bool visible, bool wasSearched)
	{
		global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
		for (int i = 0; i < itemSaves.Count; i++)
		{
			list.Add(new global::Item(itemSaves[i]));
		}
		return this.SpawnLootBag(owner, pos, list, visible, wasSearched);
	}

	public global::SearchPoint SpawnLootBag(global::UnitController owner, global::UnityEngine.Vector3 pos, global::System.Collections.Generic.List<global::Item> items, bool visible, bool wasSearched)
	{
		if (this.lootbagPrefab == null)
		{
			this.lootbagPrefab = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAsset<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/", global::AssetBundleId.PROPS, "loot_bag.prefab");
		}
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.lootbagPrefab);
		gameObject.name = "loot_" + owner.unit.Name;
		gameObject.transform.position = pos;
		gameObject.transform.rotation = global::UnityEngine.Quaternion.identity;
		global::SearchPoint componentInChildren = gameObject.GetComponentInChildren<global::SearchPoint>();
		componentInChildren.unitController = owner;
		componentInChildren.loc_name = "search_body";
		componentInChildren.Init(this.GetNextRTGUID());
		componentInChildren.visual.SetActive(visible);
		for (int i = 0; i < items.Count; i++)
		{
			this.ResetItemOwnership(items[i], owner);
			componentInChildren.AddItem(items[i], false);
		}
		owner.lootBagPoint = componentInChildren;
		this.MissionEndData.UpdateSearches(componentInChildren.guid, owner.uid, pos, items.ConvertAll<global::ItemSave>((global::Item x) => x.Save), wasSearched);
		return componentInChildren;
	}

	public global::System.Collections.Generic.List<global::Item> FindObjectivesInSearch(global::ItemId itemId)
	{
		global::System.Collections.Generic.List<global::Item> result = new global::System.Collections.Generic.List<global::Item>();
		this.ExtractItemsFromSearch(this.interactivePoints, itemId, ref result);
		this.ExtractItemsFromSearch(this.interactivePointsTrash, itemId, ref result);
		return result;
	}

	private void ExtractItemsFromSearch(global::System.Collections.Generic.List<global::InteractivePoint> points, global::ItemId itemId, ref global::System.Collections.Generic.List<global::Item> itemsFound)
	{
		for (int i = 0; i < points.Count; i++)
		{
			if (points[i] != null && points[i] is global::SearchPoint)
			{
				global::System.Collections.Generic.List<global::Item> objectiveItems = ((global::SearchPoint)points[i]).GetObjectiveItems();
				for (int j = 0; j < objectiveItems.Count; j++)
				{
					if (objectiveItems[j].Id == itemId)
					{
						itemsFound.Add(objectiveItems[j]);
					}
				}
			}
		}
	}

	public void FindObjectiveInUnits(global::ItemId itemId, ref global::System.Collections.Generic.List<global::Item> foundItems)
	{
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
			{
				for (int k = 0; k < this.WarbandCtrlrs[i].unitCtrlrs[j].unit.Items.Count; k++)
				{
					if (this.WarbandCtrlrs[i].unitCtrlrs[j].unit.Items[k].Id == itemId)
					{
						foundItems.Add(this.WarbandCtrlrs[i].unitCtrlrs[j].unit.Items[k]);
					}
				}
			}
		}
	}

	public void RestoreUnitWeapons()
	{
		for (int i = 0; i < this.interactivePoints.Count; i++)
		{
			global::SearchPoint searchPoint = this.interactivePoints[i] as global::SearchPoint;
			if (searchPoint != null && searchPoint.unitController != null)
			{
				for (int j = 0; j < searchPoint.items.Count; j++)
				{
					global::Item item = searchPoint.items[j];
					if (item.owner == searchPoint.unitController.unit && (item.Save.oldSlot == 2 || item.Save.oldSlot == 3 || item.Save.oldSlot == 4 || item.Save.oldSlot == 5))
					{
						searchPoint.SetItem(j, global::ItemId.NONE, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE);
						item.owner.EquipItem((global::UnitSlotId)item.Save.oldSlot, item, true);
					}
				}
				searchPoint.SortItems();
				searchPoint.Close(true);
				this.MissionEndData.UpdateUnit(searchPoint.unitController);
			}
		}
	}

	public void ResetItemOwnership(global::Item item, global::UnitController owner)
	{
		if (owner != null && item.Save.ownerMyrtilus == owner.uid)
		{
			item.owner = owner.unit;
		}
		if (item.Save.ownerMyrtilus >= 200000000U)
		{
			uint num = item.Save.ownerMyrtilus - 200000000U;
			for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
			{
				if ((long)this.WarbandCtrlrs[i].saveIdx == (long)((ulong)num))
				{
					this.WarbandCtrlrs[i].ItemIdol = item;
					break;
				}
			}
		}
		else if (item.Save.ownerMyrtilus > 0U)
		{
			for (int j = 0; j < this.allUnitsList.Count; j++)
			{
				if (item.Save.ownerMyrtilus == this.allUnitsList[j].uid)
				{
					item.owner = this.allUnitsList[j].unit;
					break;
				}
			}
		}
		else
		{
			item.owner = ((!(owner != null)) ? null : owner.unit);
			item.Save.ownerMyrtilus = ((!(owner != null)) ? 0U : owner.uid);
		}
		if (item.IsTrophy)
		{
			for (int k = 0; k < this.allUnitsList.Count; k++)
			{
				if (this.allUnitsList[k].unit == item.owner)
				{
					this.allUnitsList[k].unit.deathTrophy = item;
				}
			}
		}
	}

	public void RegisterDecisionPoint(global::DecisionPoint point)
	{
		this.decisionPoints.Add(point);
	}

	public global::System.Collections.Generic.List<global::DecisionPoint> GetDecisionPoints(global::UnitController target, global::DecisionPointId pointId, float distance, bool excludeCloseToUnits = true)
	{
		global::System.Collections.Generic.List<global::DecisionPoint> list = new global::System.Collections.Generic.List<global::DecisionPoint>();
		global::System.Collections.Generic.List<global::UnitController> allAliveUnits = this.GetAllAliveUnits();
		float num = global::Constant.GetFloat(global::ConstantId.MELEE_RANGE_NORMAL);
		num *= num;
		float num2 = global::Constant.GetFloat(global::ConstantId.MELEE_RANGE_LARGE);
		num2 *= num2;
		for (int i = 0; i < this.decisionPoints.Count; i++)
		{
			if (this.decisionPoints[i].id == pointId && global::UnityEngine.Vector3.SqrMagnitude(this.decisionPoints[i].transform.position - target.transform.position) < distance * distance)
			{
				bool flag = true;
				if (excludeCloseToUnits)
				{
					for (int j = 0; j < allAliveUnits.Count; j++)
					{
						if (global::UnityEngine.Vector3.SqrMagnitude(allAliveUnits[j].transform.position - this.decisionPoints[i].transform.position) < ((allAliveUnits[j].unit.Data.UnitSizeId != global::UnitSizeId.LARGE) ? num : num2))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					list.Add(this.decisionPoints[i]);
				}
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::DecisionPoint> GetAvailableSpawnPoints(bool visible, bool asc = false, global::UnityEngine.Transform referencePoint = null, global::System.Collections.Generic.List<global::DecisionPoint> forcedSpawnPoints = null)
	{
		global::System.Collections.Generic.List<global::UnitController> allAliveUnits = this.GetAllAliveUnits();
		global::System.Collections.Generic.List<global::UnitController> unitCtrlrs = this.GetMyWarbandCtrlr().unitCtrlrs;
		global::System.Collections.Generic.List<global::DecisionPoint> list = new global::System.Collections.Generic.List<global::DecisionPoint>();
		global::System.Collections.Generic.List<global::DecisionPoint> list2 = (forcedSpawnPoints == null) ? this.decisionPoints : forcedSpawnPoints;
		list.AddRange(list2.FindAll((global::DecisionPoint x) => x.id == global::DecisionPointId.SPAWN));
		for (int i = list.Count - 1; i >= 0; i--)
		{
			bool flag = visible;
			for (int j = 0; j < unitCtrlrs.Count; j++)
			{
				global::UnitController unitController = unitCtrlrs[j];
				global::UnityEngine.Vector3 position = unitController.transform.position;
				position.y += 1.5f;
				global::UnityEngine.Vector3 position2 = list[i].transform.position;
				position2.y += 1.25f;
				global::UnityEngine.Physics.Raycast(position, global::UnityEngine.Vector3.Normalize(position2 - position), out this.hitInfo, (float)unitController.unit.ViewDistance, global::LayerMaskManager.fowMask);
				float num = global::UnityEngine.Vector3.Distance(position, position2);
				bool flag2 = this.hitInfo.collider == null || this.hitInfo.distance > num;
				if ((!visible && flag2) || (visible && flag2))
				{
					flag = !visible;
					break;
				}
			}
			if (flag)
			{
				list.RemoveAt(i);
			}
		}
		if (list.Count == 0)
		{
			list.AddRange(this.decisionPoints.FindAll((global::DecisionPoint x) => x.id == global::DecisionPointId.SPAWN));
		}
		for (int k = list.Count - 1; k >= 0; k--)
		{
			bool flag3 = true;
			list[k].closestUnitSqrDist = float.PositiveInfinity;
			int num2 = 0;
			while (num2 < allAliveUnits.Count && flag3)
			{
				if (allAliveUnits[num2].isInLadder)
				{
					float num3 = global::UnityEngine.Vector3.SqrMagnitude(list[k].transform.position - allAliveUnits[num2].transform.position);
					if (referencePoint == null)
					{
						list[k].closestUnitSqrDist = global::UnityEngine.Mathf.Min(list[k].closestUnitSqrDist, num3);
					}
					flag3 = (num3 > 6.25f);
				}
				num2++;
			}
			if (!flag3)
			{
				list.RemoveAt(k);
			}
			else if (referencePoint != null)
			{
				list[k].closestUnitSqrDist = global::UnityEngine.Vector3.SqrMagnitude(list[k].transform.position - referencePoint.position);
			}
		}
		int mod = (!asc) ? -1 : 1;
		list.Sort((global::DecisionPoint x, global::DecisionPoint y) => x.closestUnitSqrDist.CompareTo(y.closestUnitSqrDist) * mod);
		return list;
	}

	public void RegisterLocateZone(global::LocateZone zone)
	{
		this.locateZones.Add(zone);
	}

	public global::System.Collections.Generic.List<global::LocateZone> GetLocateZones(string name)
	{
		global::System.Collections.Generic.List<global::LocateZone> list = new global::System.Collections.Generic.List<global::LocateZone>();
		for (int i = 0; i < this.locateZones.Count; i++)
		{
			if (this.locateZones[i].name == name)
			{
				list.Add(this.locateZones[i]);
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::LocateZone> GetLocateZones()
	{
		return this.locateZones;
	}

	public void RegisterPatrolRoute(global::PatrolRoute route)
	{
		this.patrolRoutes[route.name] = route;
	}

	public void RegisterZoneAoe(global::ZoneAoe zone)
	{
		this.zoneAoes.Add(zone);
	}

	public void UpdateZoneAoeDurations(global::UnitController unitCtrlr)
	{
		for (int i = 0; i < this.zoneAoes.Count; i++)
		{
			if (this.zoneAoes[i].Owner == unitCtrlr)
			{
				this.zoneAoes[i].UpdateDuration();
			}
		}
	}

	public void ClearZoneAoes()
	{
		for (int i = this.zoneAoes.Count - 1; i >= 0; i--)
		{
			if (!this.zoneAoes[i].gameObject.activeSelf && !this.zoneAoes[i].indestructible)
			{
				global::UnityEngine.Object.Destroy(this.zoneAoes[i].gameObject);
				this.zoneAoes.RemoveAt(i);
			}
		}
	}

	public int ZoneAoeIdx(global::ZoneAoe zone)
	{
		return this.zoneAoes.IndexOf(zone);
	}

	public global::ZoneAoe GetZoneAoe(int idx)
	{
		if (idx >= 0 && idx < this.zoneAoes.Count)
		{
			return this.zoneAoes[idx];
		}
		return null;
	}

	public global::WarbandController GetMyWarbandCtrlr()
	{
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlrs[i].playerIdx == global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex && this.WarbandCtrlrs[i].playerTypeId == global::PlayerTypeId.PLAYER)
			{
				return this.WarbandCtrlrs[i];
			}
		}
		return this.WarbandCtrlrs[0];
	}

	public int GetCampaignWarbandIdx(global::CampaignWarbandId campWarbandId)
	{
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlrs[i].CampaignWarbandId == campWarbandId)
			{
				return i;
			}
		}
		return -1;
	}

	public global::System.Collections.Generic.List<global::WarbandController> GetEnemyWarbandCtrlrs()
	{
		global::System.Collections.Generic.List<global::WarbandController> list = new global::System.Collections.Generic.List<global::WarbandController>();
		global::WarbandController myWarbandCtrlr = this.GetMyWarbandCtrlr();
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlrs[i].teamIdx != myWarbandCtrlr.teamIdx)
			{
				list.Add(this.WarbandCtrlrs[i]);
			}
		}
		return list;
	}

	public global::WarbandController GetMainEnemyWarbandCtrlr()
	{
		global::WarbandController myWarbandCtrlr = this.GetMyWarbandCtrlr();
		global::WarbandController warbandController = null;
		int num = 0;
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlrs[i].teamIdx != myWarbandCtrlr.teamIdx && this.WarbandCtrlrs[i].MaxMoralValue > num)
			{
				warbandController = this.WarbandCtrlrs[i];
				num = warbandController.MaxMoralValue;
			}
		}
		return warbandController;
	}

	public global::System.Collections.Generic.List<global::UnitController> GetAllMyUnits()
	{
		this.allMyUnitsList.Clear();
		global::WarbandController myWarbandCtrlr = this.GetMyWarbandCtrlr();
		for (int i = 0; i < myWarbandCtrlr.unitCtrlrs.Count; i++)
		{
			this.allMyUnitsList.Add(myWarbandCtrlr.unitCtrlrs[i]);
		}
		return this.allMyUnitsList;
	}

	public global::System.Collections.Generic.List<global::UnitController> GetMyAliveUnits()
	{
		this.allLiveMyUnitsList.Clear();
		global::WarbandController myWarbandCtrlr = this.GetMyWarbandCtrlr();
		for (int i = 0; i < myWarbandCtrlr.unitCtrlrs.Count; i++)
		{
			if (myWarbandCtrlr.unitCtrlrs[i].unit.Status != global::UnitStateId.OUT_OF_ACTION)
			{
				this.allLiveMyUnitsList.Add(myWarbandCtrlr.unitCtrlrs[i]);
			}
		}
		return this.allLiveMyUnitsList;
	}

	public global::System.Collections.Generic.List<global::UnitController> GetAliveAllies(int warbandIdx)
	{
		this.allLiveAlliesList.Clear();
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlrs[i].teamIdx == this.WarbandCtrlrs[warbandIdx].teamIdx)
			{
				for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
				{
					if (this.WarbandCtrlrs[i].unitCtrlrs[j].unit.Status != global::UnitStateId.OUT_OF_ACTION)
					{
						this.allLiveAlliesList.Add(this.WarbandCtrlrs[i].unitCtrlrs[j]);
					}
				}
			}
		}
		return this.allLiveAlliesList;
	}

	public global::System.Collections.Generic.List<global::UnitController> GetAllEnemies(int warbandIdx)
	{
		this.allEnemiesList.Clear();
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlrs[i].idx != warbandIdx && this.WarbandCtrlrs[i].teamIdx != this.WarbandCtrlrs[warbandIdx].teamIdx && this.WarbandCtrlrs[warbandIdx].BlackListed(this.WarbandCtrlrs[i].idx))
			{
				this.allEnemiesList.AddRange(this.WarbandCtrlrs[i].unitCtrlrs);
			}
		}
		return this.allEnemiesList;
	}

	public global::System.Collections.Generic.List<global::UnitController> GetAliveEnemies(int warbandIdx)
	{
		this.allLiveEnemiesList.Clear();
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlrs[i].idx != warbandIdx && this.WarbandCtrlrs[i].teamIdx != this.WarbandCtrlrs[warbandIdx].teamIdx && this.WarbandCtrlrs[warbandIdx].BlackListed(this.WarbandCtrlrs[i].idx))
			{
				for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
				{
					if (this.WarbandCtrlrs[i].unitCtrlrs[j].unit.Status != global::UnitStateId.OUT_OF_ACTION)
					{
						this.allLiveEnemiesList.Add(this.WarbandCtrlrs[i].unitCtrlrs[j]);
					}
				}
			}
		}
		return this.allLiveEnemiesList;
	}

	public global::System.Collections.Generic.List<global::UnitController> GetAllUnits()
	{
		return this.allUnitsList;
	}

	public global::System.Collections.Generic.List<global::UnitController> GetAllAliveUnits()
	{
		this.allLiveUnits.Clear();
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
			{
				if (this.WarbandCtrlrs[i].unitCtrlrs[j] != null && this.WarbandCtrlrs[i].unitCtrlrs[j].unit.Status != global::UnitStateId.OUT_OF_ACTION)
				{
					this.allLiveUnits.Add(this.WarbandCtrlrs[i].unitCtrlrs[j]);
				}
			}
		}
		return this.allLiveUnits;
	}

	public global::UnitController OwnUnitInvolved(global::UnitController unit1, global::UnitController unit2)
	{
		if (unit1 != null && unit1.GetWarband().playerIdx == global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex && unit1.AICtrlr == null)
		{
			return unit1;
		}
		if (unit2 != null && unit2.GetWarband().playerIdx == global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex && unit2.AICtrlr == null)
		{
			return unit2;
		}
		return null;
	}

	public int GetLadderLastValidPosition()
	{
		int result = 0;
		for (int i = 0; i < this.InitiativeLadder.Count; i++)
		{
			if (this.InitiativeLadder[i].unit.Status != global::UnitStateId.OUT_OF_ACTION)
			{
				result = i;
			}
		}
		return result;
	}

	public void SendUnitBack(int positionCount)
	{
		int index = global::UnityEngine.Mathf.Min(this.CurrentLadderIdx + positionCount + 1, this.InitiativeLadder.Count);
		global::UnitController currentUnit = this.GetCurrentUnit();
		this.InitiativeLadder.Insert(index, currentUnit);
		this.InitiativeLadder.RemoveAt(this.CurrentLadderIdx);
		this.SaveLadder();
		this.SelectNextLadderUnit(0);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.LADDER_CHANGED);
	}

	public void ResetLadder()
	{
		this.CurrentLadderIdx = -1;
		int count = this.InitiativeLadder.Count;
		for (int i = 0; i < count; i++)
		{
			this.InitiativeLadder[i].ladderVisible = (this.InitiativeLadder[i].unit.Status != global::UnitStateId.OUT_OF_ACTION);
		}
	}

	public void ResetLadderIdx(bool updateUI = true)
	{
		this.ResetLadder();
		this.InitiativeLadder.Sort(new global::LadderSorter());
		if (updateUI)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.LADDER_CHANGED);
		}
		this.SaveLadder();
	}

	private void SaveLadder()
	{
		this.MissionEndData.myrtilusLadder.Clear();
		for (int i = 0; i < this.InitiativeLadder.Count; i++)
		{
			this.MissionEndData.myrtilusLadder.Add(this.InitiativeLadder[i].uid);
		}
		this.MissionEndData.currentLadderIdx = this.CurrentLadderIdx;
		this.MissionEndData.currentTurn = this.currentTurn;
	}

	public void ReloadLadder()
	{
		global::System.Collections.Generic.List<uint> myrtilusLadder = global::PandoraSingleton<global::MissionStartData>.Instance.myrtilusLadder;
		for (int i = 0; i < myrtilusLadder.Count; i++)
		{
			global::UnitController unitController = null;
			for (int j = 0; j < this.excludedUnits.Count; j++)
			{
				if (myrtilusLadder[i] == this.excludedUnits[j].uid)
				{
					unitController = this.excludedUnits[j];
					break;
				}
			}
			if (unitController == null)
			{
				for (int k = 0; k < this.allUnitsList.Count; k++)
				{
					if (myrtilusLadder[i] == this.allUnitsList[k].uid)
					{
						unitController = this.allUnitsList[k];
						break;
					}
				}
			}
			if (unitController != null && !unitController.isInLadder)
			{
				if (this.allUnitsList.IndexOf(unitController) == -1)
				{
					this.IncludeUnit(unitController, unitController.transform.position, unitController.transform.rotation);
				}
				else
				{
					this.AddUnitToLadder(unitController);
				}
			}
		}
		this.InitiativeLadder = new global::System.Collections.Generic.List<global::UnitController>(myrtilusLadder.Count);
		for (int l = 0; l < myrtilusLadder.Count; l++)
		{
			for (int m = 0; m < this.allUnitsList.Count; m++)
			{
				if (myrtilusLadder[l] == this.allUnitsList[m].uid)
				{
					this.InitiativeLadder.Add(this.allUnitsList[m]);
					break;
				}
			}
		}
		this.CurrentLadderIdx = -1;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.currentLadderIdx > -1)
		{
			this.CurrentLadderIdx = global::PandoraSingleton<global::MissionStartData>.Instance.currentLadderIdx - 1;
			if (this.CurrentLadderIdx >= -1)
			{
				this.InitiativeLadder[this.CurrentLadderIdx + 1].TurnStarted = true;
			}
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.LADDER_CHANGED);
		this.SaveLadder();
	}

	public void RemoveUnitFromLadder(global::UnitController ctrlr)
	{
		this.InitiativeLadder.Remove(ctrlr);
		ctrlr.Imprint.alwaysVisible = false;
		ctrlr.Imprint.alwaysHide = true;
		ctrlr.Imprint.Hide();
		ctrlr.isInLadder = false;
		ctrlr.Hide(true, true, null);
		this.SaveLadder();
	}

	public void AddUnitToLadder(global::UnitController ctrlr)
	{
		if (this.InitiativeLadder.IndexOf(ctrlr) == -1)
		{
			this.InitiativeLadder.Add(ctrlr);
		}
		ctrlr.Imprint.alwaysVisible = ctrlr.IsPlayed();
		ctrlr.Imprint.alwaysHide = false;
		ctrlr.isInLadder = true;
		ctrlr.ladderVisible = true;
		this.resendLadder = true;
		ctrlr.Hide(ctrlr.IsPlayed(), true, null);
		this.SaveLadder();
	}

	public void SelectNextLadderUnit(int modifier = 1)
	{
		global::PandoraDebug.LogDebug("SelectNextLadderUnit", "FLOW", this);
		this.MoveCircle.Hide();
		if (this.CheckEndGame())
		{
			return;
		}
		if (this.CurrentLadderIdx != -1)
		{
			this.lastWarbandIdx = this.InitiativeLadder[this.CurrentLadderIdx].GetWarband().idx;
		}
		else
		{
			this.lastWarbandIdx = -1;
		}
		this.CurrentLadderIdx += modifier;
		if (this.CurrentLadderIdx < this.InitiativeLadder.Count)
		{
			global::UnitController currentUnit = this.InitiativeLadder[this.CurrentLadderIdx];
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.CURRENT_UNIT_CHANGED, currentUnit);
			this.MissionEndData.currentLadderIdx = this.CurrentLadderIdx;
			if (currentUnit.TurnStarted)
			{
				this.SetCombatCircles(currentUnit, delegate
				{
					this.TurnTimer.Reset(currentUnit.lastTimer);
					currentUnit.lastTimer = 0f;
					currentUnit.nextState = global::UnitController.State.START_MOVE;
					this.WatchOrMove();
				});
			}
			else
			{
				this.StateMachine.ChangeState(4);
			}
			return;
		}
		this.StateMachine.ChangeState(7);
	}

	public global::UnitController GetLastPlayedAliveUnit(int warbandIdx)
	{
		for (int i = this.CurrentLadderIdx - 1; i >= 0; i--)
		{
			global::UnitController unitController = this.InitiativeLadder[i];
			if (unitController.unit.warbandIdx == warbandIdx && unitController.unit.Status != global::UnitStateId.OUT_OF_ACTION)
			{
				return unitController;
			}
		}
		return null;
	}

	public void WatchOrMove()
	{
		if (this.GetCurrentUnit().IsPlayed())
		{
			if (this.StateMachine.GetActiveStateId() != 5)
			{
				this.CamManager.Locked = false;
				this.StateMachine.ChangeState(5);
			}
		}
		else
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.MISSION_SHOW_ENEMY, true);
			if (this.StateMachine.GetActiveStateId() != 6)
			{
				this.CamManager.Locked = true;
				this.StateMachine.ChangeState(6);
			}
		}
	}

	private void InitWalkability()
	{
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
			{
				this.WarbandCtrlrs[i].unitCtrlrs[j].SetGraphWalkability(false);
			}
		}
	}

	public void RefreshGraph()
	{
		this.navGraphNeedsRefresh = true;
	}

	private void UpdateGraph()
	{
		global::PandoraDebug.LogDebug("UpdateGraph", "MISSION MANAGER", null);
		this.navGraphNeedsRefresh = false;
		float floatSqr = global::Constant.GetFloatSqr(global::ConstantId.MELEE_RANGE_LARGE);
		float floatSqr2 = global::Constant.GetFloatSqr(global::ConstantId.MELEE_RANGE_NORMAL);
		for (int i = 0; i < this.nodeLinks.Count; i++)
		{
			global::Pathfinding.NodeLink2 nodeLink = this.nodeLinks[i];
			nodeLink.startNode.Walkable = true;
			nodeLink.endNode.Walkable = true;
			nodeLink.oneWay = true;
			for (int j = 0; j < this.WarbandCtrlrs.Count; j++)
			{
				global::WarbandController warbandController = this.WarbandCtrlrs[j];
				for (int k = 0; k < warbandController.unitCtrlrs.Count; k++)
				{
					global::UnitController unitController = warbandController.unitCtrlrs[k];
					if (unitController != this.GetCurrentUnit() && unitController.unit.Status != global::UnitStateId.OUT_OF_ACTION)
					{
						float num = (unitController.unit.Data.UnitSizeId != global::UnitSizeId.LARGE) ? floatSqr2 : floatSqr;
						if (global::UnityEngine.Vector3.SqrMagnitude(unitController.transform.position - nodeLink.StartTransform.position) < num || global::UnityEngine.Vector3.SqrMagnitude(unitController.transform.position - nodeLink.EndTransform.position) < num)
						{
							nodeLink.startNode.Walkable = false;
							nodeLink.endNode.Walkable = false;
						}
					}
				}
			}
		}
		this.tileHandlerHelper.ForceUpdate();
	}

	public global::UnityEngine.Vector3 ClampToNavMesh(global::UnityEngine.Vector3 pos)
	{
		if (!this.IsOnNavmesh(pos))
		{
			this.nearestNodeInfo = global::AstarPath.active.GetNearest(pos + global::UnityEngine.Vector3.up * 0.07f, this.nearestNodeConstraint);
			return new global::UnityEngine.Vector3(this.nearestNodeInfo.position.x, pos.y, this.nearestNodeInfo.position.z);
		}
		return pos;
	}

	public bool IsOnNavmesh(global::UnityEngine.Vector3 pos)
	{
		return global::AstarPath.active.astarData.recastGraph.PointOnNavmesh(pos, this.nearestNodeConstraint) != null;
	}

	private void CheckFoWVisibility(global::UnitController ctrlr, global::MapImprint imprint)
	{
		if (imprint == null)
		{
			return;
		}
		global::UnityEngine.Transform transform = imprint.transform;
		global::UnityEngine.Vector3 position = ctrlr.transform.position;
		position.y += 1.5f;
		global::UnityEngine.Vector3 position2 = transform.position;
		position2.y += 1.25f;
		float num = global::UnityEngine.Vector3.SqrMagnitude(position - position2);
		if (num >= (float)(ctrlr.unit.ViewDistance * ctrlr.unit.ViewDistance))
		{
			imprint.RemoveViewer(ctrlr);
			return;
		}
		if (imprint.UnitCtrlr != null)
		{
			if (ctrlr.IsInRange(imprint.UnitCtrlr, 0f, (float)ctrlr.unit.ViewDistance, global::Constant.GetFloat(global::ConstantId.RANGE_SPELL_REQUIRED_PERC), false, false, global::BoneId.NONE))
			{
				imprint.AddViewer(ctrlr);
				if (imprint.UnitCtrlr.AICtrlr != null)
				{
					imprint.UnitCtrlr.GetWarband().SquadManager.UnitSpotted(ctrlr);
					imprint.UnitCtrlr.AICtrlr.hasSeenEnemy = true;
				}
				if (imprint.UnitCtrlr != null && !imprint.UnitCtrlr.isInLadder)
				{
					this.AddUnitToLadder(imprint.UnitCtrlr);
				}
			}
			else
			{
				imprint.RemoveViewer(ctrlr);
			}
		}
		else if (imprint.Flag == global::MapImprint.currentFlagChecked)
		{
			int num2 = global::UnityEngine.Physics.RaycastNonAlloc(position, position2 - position, global::PandoraUtils.hits, (float)ctrlr.unit.ViewDistance, global::LayerMaskManager.fowMask);
			global::UnityEngine.Collider collider = global::PandoraUtils.hits[0].collider;
			if (num2 == 0 || collider == null || (collider != null && collider.transform == transform) || (collider != null && collider.transform.parent != null && collider.transform.parent == transform) || global::PandoraUtils.hits[0].distance * global::PandoraUtils.hits[0].distance > num)
			{
				imprint.AddViewer(ctrlr);
			}
			else
			{
				imprint.RemoveViewer(ctrlr);
			}
		}
	}

	public void InitFoW()
	{
		global::System.Collections.Generic.List<global::UnitController> allMyUnits = this.GetAllMyUnits();
		for (int i = 0; i < this.MapImprints.Count; i++)
		{
			for (int j = 0; j < allMyUnits.Count; j++)
			{
				if (allMyUnits[j].isInLadder && this.MapImprints[i].UnitCtrlr != allMyUnits[j])
				{
					allMyUnits[j].UpdateTargetsData();
					if (!this.MapImprints[i].alwaysVisible && !this.MapImprints[i].alwaysHide)
					{
						this.CheckFoWVisibility(allMyUnits[j], this.MapImprints[i]);
					}
				}
			}
			this.MapImprints[i].needsRefresh = true;
		}
	}

	public void RefreshFoWOwnMoving(global::UnitController ctrlr)
	{
		for (int i = 0; i < this.MapImprints.Count; i++)
		{
			if (this.MapImprints[i].UnitCtrlr == null || !this.MapImprints[i].UnitCtrlr.IsPlayed() || !this.MapImprints[i].UnitCtrlr.isInLadder)
			{
				this.CheckFoWVisibility(ctrlr, this.MapImprints[i]);
			}
		}
		if (global::MapImprint.maxFlag > 0)
		{
			global::MapImprint.currentFlagChecked = (global::MapImprint.currentFlagChecked + 1) % (global::MapImprint.maxFlag + 1);
		}
	}

	public void RefreshFoWTargetMoving(global::UnitController target)
	{
		if (target.Imprint.alwaysVisible)
		{
			return;
		}
		float @float = global::Constant.GetFloat(global::ConstantId.RANGE_SPELL_REQUIRED_PERC);
		global::WarbandController myWarbandCtrlr = this.GetMyWarbandCtrlr();
		for (int i = 0; i < myWarbandCtrlr.unitCtrlrs.Count; i++)
		{
			if (myWarbandCtrlr.unitCtrlrs[i].unit.Status != global::UnitStateId.OUT_OF_ACTION && myWarbandCtrlr.unitCtrlrs[i].isInLadder && target.IsInRange(myWarbandCtrlr.unitCtrlrs[i], 0f, (float)target.unit.ViewDistance, @float, false, false, global::BoneId.NONE))
			{
				target.Imprint.AddViewer(myWarbandCtrlr.unitCtrlrs[i]);
			}
			else
			{
				target.Imprint.RemoveViewer(myWarbandCtrlr.unitCtrlrs[i]);
			}
		}
	}

	public void SetAccessibleActionZones(global::UnitController ctrlr, global::System.Action zonesSet)
	{
		base.StartCoroutine(this.CheckAllZonesAccessibility(ctrlr, zonesSet));
	}

	private global::System.Collections.IEnumerator CheckAllZonesAccessibility(global::UnitController ctrlr, global::System.Action zonesSet)
	{
		int counter = 0;
		float maxDist = (float)(ctrlr.unit.CurrentStrategyPoints * ctrlr.unit.Movement);
		maxDist *= maxDist;
		this.accessibleActionZones.Clear();
		global::System.Collections.Generic.List<global::UnitController> allAliveUnits = this.GetAllAliveUnits();
		for (int i = 0; i < this.actionZones.Count; i++)
		{
			global::ActionZone zone = this.actionZones[i];
			if (zone != null)
			{
				if (global::UnityEngine.Vector3.SqrMagnitude(ctrlr.transform.position - zone.transform.position) <= maxDist)
				{
					zone.EnableFx(true);
					this.accessibleActionZones.Add(zone);
					zone.PointsChecker.UpdateControlPoints(ctrlr, allAliveUnits);
					counter++;
					if (counter >= 10)
					{
						counter = 0;
						yield return null;
					}
				}
				else
				{
					zone.EnableFx(false);
				}
			}
		}
		for (int j = 0; j < this.triggerPoints.Count; j++)
		{
			global::Teleporter teleport = this.triggerPoints[j] as global::Teleporter;
			if (teleport != null && global::UnityEngine.Vector3.SqrMagnitude(ctrlr.transform.position - teleport.transform.position) <= maxDist)
			{
				for (int pc = 0; pc < teleport.PointsCheckers.Count; pc++)
				{
					teleport.PointsCheckers[pc].UpdateControlPoints(ctrlr, allAliveUnits);
					counter++;
					if (counter >= 10)
					{
						counter = 0;
						yield return null;
					}
				}
			}
		}
		if (zonesSet != null)
		{
			zonesSet();
		}
		yield break;
	}

	public void UnregisterDestructible(global::Destructible dest)
	{
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
			{
				this.WarbandCtrlrs[i].unitCtrlrs[j].triggeredDestructibles.Remove(dest);
			}
		}
		for (int k = this.triggerPoints.Count - 1; k >= 0; k--)
		{
			if (this.triggerPoints[k] == dest)
			{
				this.triggerPoints.RemoveAt(k);
			}
		}
	}

	public void RefreshActionZones(global::UnitController ctrlr)
	{
		float num = (float)(ctrlr.unit.Movement * 3);
		num *= num;
		for (int i = 0; i < this.accessibleActionZones.Count; i++)
		{
			global::ActionZone actionZone = this.accessibleActionZones[i];
			float num2 = global::UnityEngine.Vector3.SqrMagnitude(ctrlr.transform.position - actionZone.transform.position);
			float num3 = global::UnityEngine.Mathf.Min(num2, num) / num;
			actionZone.EnableFx(num2 < num);
		}
	}

	public void TurnOffActionZones()
	{
		for (int i = 0; i < this.actionZones.Count; i++)
		{
			global::ActionZone actionZone = this.actionZones[i];
			if (actionZone != null)
			{
				if (actionZone.destinations.Count > 0)
				{
					actionZone.SetupFX();
				}
				actionZone.EnableFx(false);
			}
		}
	}

	public void MoveUnitsOnActionZone(global::UnitController currentUnit, global::PointsChecker pointsChecker, global::System.Collections.Generic.List<global::UnitController> units, bool isEnemy)
	{
		global::System.Collections.Generic.List<global::UnityEngine.Vector3> validPoints = pointsChecker.validPoints;
		for (int i = 0; i < units.Count; i++)
		{
			float num = 9999f;
			global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
			for (int j = 0; j < validPoints.Count; j++)
			{
				float num2 = global::UnityEngine.Vector3.SqrMagnitude(units[i].transform.position - validPoints[j]);
				if (num2 < num)
				{
					num = num2;
					vector = validPoints[j];
				}
			}
			validPoints.Remove(vector);
			if (isEnemy)
			{
				global::UnityEngine.Vector3 value = vector - units[i].transform.position;
				vector += global::UnityEngine.Vector3.Normalize(value) * -0.22f;
			}
			currentUnit.SendMoveAndUpdateCircle(units[i].uid, vector, units[i].transform.rotation);
		}
	}

	public void PlaySequence(string sequence, global::UnitController target, global::DelSequenceDone onFinishDel = null)
	{
		target.SetKinemantic(true);
		this.focusedUnit = target;
		global::PandoraSingleton<global::SequenceManager>.Instance.PlaySequence(sequence, onFinishDel);
		this.TurnTimer.Pause();
	}

	public void ForceFocusedUnit(global::UnitController ctrlr)
	{
		this.focusedUnit = ctrlr;
	}

	public void SetBeaconLimit(int limit)
	{
		this.beaconLimit = limit;
	}

	public global::Beacon SpawnBeacon(global::UnityEngine.Vector3 position)
	{
		global::Beacon beacon = null;
		for (int i = 0; i < this.beacons.Count; i++)
		{
			if (!this.beacons[i].gameObject.activeSelf)
			{
				beacon = this.beacons[i];
				beacon.gameObject.SetActive(true);
				break;
			}
		}
		if (beacon == null)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.beaconPrefab);
			beacon = gameObject.GetComponent<global::Beacon>();
			this.beacons.Add(beacon);
		}
		beacon.transform.position = position;
		beacon.transform.rotation = global::UnityEngine.Quaternion.identity;
		beacon.idx = ++this.beaconIdx;
		if (this.beaconLimit != 0)
		{
			this.lastBeaconIdx = this.beaconIdx - this.beaconLimit;
		}
		this.RefreshBeacons();
		return beacon;
	}

	public void RevertBeacons(global::Beacon keavin)
	{
		this.beaconIdx = keavin.idx;
		this.RefreshBeacons();
	}

	public int ActiveBeacons()
	{
		return this.beaconIdx;
	}

	public void ClearBeacons()
	{
		for (int i = this.beacons.Count - 1; i >= 0; i--)
		{
			if (this.beacons[i] != null && this.beacons[i].gameObject != null)
			{
				this.beacons[i].gameObject.SetActive(false);
			}
		}
		this.beaconIdx = 0;
		this.lastBeaconIdx = 0;
		this.RefreshBeacons();
	}

	private void RefreshBeacons()
	{
		for (int i = 0; i < this.beacons.Count; i++)
		{
			this.beacons[i].gameObject.SetActive(this.beacons[i].idx > this.lastBeaconIdx && this.beacons[i].idx <= this.beaconIdx);
		}
	}

	private void InitBeacons()
	{
		this.mapBeacons = new global::System.Collections.Generic.List<global::MapBeacon>();
		int i;
		for (i = 0; i < 5; i++)
		{
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/beacon/", global::AssetBundleId.FX, "map_beacon_" + i + ".prefab", delegate(global::UnityEngine.Object prefab)
			{
				global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(prefab);
				global::MapBeacon component = gameObject.GetComponent<global::MapBeacon>();
				this.mapBeacons.Add(component);
				gameObject.SetActive(false);
			});
		}
		global::PandoraSingleton<global::AssetBundleLoader>.instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/beacon/", global::AssetBundleId.FX, "beacon.prefab", delegate(global::UnityEngine.Object prefab)
		{
			this.beaconPrefab = (global::UnityEngine.GameObject)prefab;
		});
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, "fx_unit_placement_blue_01.prefab", delegate(global::UnityEngine.Object prefab)
		{
			this.deployBeaconPrefab = (global::UnityEngine.GameObject)prefab;
		});
	}

	public global::System.Collections.Generic.List<global::MapBeacon> GetMapBeacons()
	{
		return this.mapBeacons;
	}

	private global::MapBeacon GetFirstAvailableMapBeacon()
	{
		for (int i = 0; i < this.mapBeacons.Count; i++)
		{
			if (!this.mapBeacons[i].gameObject.activeSelf)
			{
				return this.mapBeacons[i];
			}
		}
		return null;
	}

	public global::MapBeacon SpawnMapBeacon(global::UnityEngine.Vector3 pos, global::System.Action spawnCb)
	{
		global::MapBeacon firstAvailableMapBeacon = this.GetFirstAvailableMapBeacon();
		if (firstAvailableMapBeacon != null)
		{
			firstAvailableMapBeacon.transform.position = pos;
			firstAvailableMapBeacon.transform.rotation = global::UnityEngine.Quaternion.identity;
			base.StartCoroutine(this.ActivateBeaconNextFrame(firstAvailableMapBeacon, spawnCb));
			return firstAvailableMapBeacon;
		}
		return null;
	}

	private global::System.Collections.IEnumerator ActivateBeaconNextFrame(global::MapBeacon beacon, global::System.Action spawnCb)
	{
		yield return false;
		beacon.gameObject.SetActive(true);
		if (spawnCb != null)
		{
			spawnCb();
		}
		yield break;
	}

	public void RemoveMapBecon(global::MapBeacon mapBeacon)
	{
		mapBeacon.gameObject.SetActive(false);
	}

	public int GetAvailableMapBeacons()
	{
		int num = 0;
		for (int i = 0; i < this.mapBeacons.Count; i++)
		{
			if (this.mapBeacons[i].gameObject.activeSelf)
			{
				num++;
			}
		}
		return num;
	}

	public void ActivateMapObjectiveZones(bool activate)
	{
		global::System.Collections.Generic.List<global::Objective> objectives = this.GetMyWarbandCtrlr().objectives;
		for (int i = 0; i < this.mapObjectiveZones.Count; i++)
		{
			bool flag = activate;
			if (flag && this.mapObjectiveZones[i].objectiveId != global::PrimaryObjectiveId.NONE)
			{
				for (int j = 0; j < objectives.Count; j++)
				{
					if (objectives[j].Id == this.mapObjectiveZones[i].objectiveId)
					{
						flag = (!objectives[j].done && !objectives[j].Locked);
					}
				}
			}
			this.mapObjectiveZones[i].gameObject.SetActive(flag);
		}
	}

	private void InitTargetingAssets()
	{
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, "fx_zone_target_attack_aoe_01.prefab", delegate(global::UnityEngine.Object go)
		{
			this.aoeTargetSphere = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go);
			this.aoeTargetSphere.SetActive(false);
		});
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, "fx_zone_top_attack_aoe_01.prefab", delegate(global::UnityEngine.Object go)
		{
			this.aoeGroundTargetSphere = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go);
			this.aoeGroundTargetSphere.SetActive(false);
		});
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, "fx_aoe_cone_01.prefab", delegate(global::UnityEngine.Object go)
		{
			this.coneTarget = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go);
			this.coneTarget.SetActive(false);
		});
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, "fx_aoe_arc_01.prefab", delegate(global::UnityEngine.Object go)
		{
			this.arcTarget = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go);
			this.arcTarget.SetActive(false);
		});
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, "fx_aoe_cylindre_01.prefab", delegate(global::UnityEngine.Object go)
		{
			this.lineTarget = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go);
			this.lineTarget.SetActive(false);
		});
	}

	public void InitSphereTarget(global::UnityEngine.Transform src, float radius, global::TargetingId targetingId, out global::UnityEngine.Vector3 sphereRaySrc, out global::UnityEngine.Vector3 sphereDir)
	{
		if (this.sphereTarget != null)
		{
			this.sphereTarget.SetActive(false);
		}
		sphereRaySrc = global::UnityEngine.Vector3.zero;
		sphereDir = global::UnityEngine.Vector3.zero;
		if (targetingId != global::TargetingId.AREA)
		{
			if (targetingId != global::TargetingId.AREA_GROUND)
			{
				global::PandoraDebug.LogError("Targeting " + targetingId + " is not supported in InitSphereTargeting", "uncategorised", null);
			}
			else
			{
				this.sphereTarget = this.aoeGroundTargetSphere;
				sphereRaySrc = src.position + global::UnityEngine.Vector3.up * 1.25f / 2f;
				sphereDir = src.forward;
			}
		}
		else
		{
			this.sphereTarget = this.aoeTargetSphere;
			sphereRaySrc = src.position + global::UnityEngine.Vector3.up * 1.25f;
			sphereDir = global::UnityEngine.Vector3.Normalize(src.forward + src.up * -0.5f);
		}
		this.sphereTarget.SetActive(false);
		this.sphereTarget.transform.localScale = global::UnityEngine.Vector3.one * radius * 2f;
		this.sphereTarget.transform.position = sphereRaySrc + sphereDir;
	}

	public void InitConeTarget(global::UnityEngine.Transform src, float radius, float range, out global::UnityEngine.Vector3 coneSrc, out global::UnityEngine.Vector3 coneDir)
	{
		coneSrc = src.position + global::UnityEngine.Vector3.up;
		coneDir = global::UnityEngine.Vector3.Normalize(src.forward);
		this.coneTarget.transform.position = coneSrc;
		global::UnityEngine.Quaternion rotation = global::UnityEngine.Quaternion.LookRotation(coneDir);
		this.coneTarget.transform.rotation = rotation;
		this.coneTarget.transform.GetChild(0).localScale = new global::UnityEngine.Vector3(radius * 2f, range, radius * 2f);
	}

	public void InitArcTarget(global::UnityEngine.Transform src, out global::UnityEngine.Vector3 arcSrc, out global::UnityEngine.Vector3 arcDir)
	{
		arcSrc = src.position + global::UnityEngine.Vector3.up;
		this.arcTarget.transform.position = arcSrc;
		arcDir = src.transform.forward;
		this.arcTarget.transform.rotation = global::UnityEngine.Quaternion.LookRotation(arcDir);
	}

	public void InitLineTarget(global::UnityEngine.Transform src, float radius, float range, out global::UnityEngine.Vector3 lineSrc, out global::UnityEngine.Vector3 lineDir)
	{
		lineSrc = src.position + global::UnityEngine.Vector3.up;
		lineDir = global::UnityEngine.Vector3.Normalize(src.forward);
		this.lineTarget.transform.position = lineSrc;
		global::UnityEngine.Quaternion rotation = global::UnityEngine.Quaternion.LookRotation(lineDir);
		this.lineTarget.transform.rotation = rotation;
		this.lineTarget.transform.GetChild(0).localScale = new global::UnityEngine.Vector3(radius * 2f, range, radius * 2f);
	}

	public global::UnitController ActivateHiddenUnit(global::CampaignUnitId campaignUnitId, bool spawnVisible, string loc = "mission_unit_spawn")
	{
		global::UnitController unitController = this.excludedUnits.Find((global::UnitController x) => x.unit.CampaignData.Id == campaignUnitId);
		if (unitController != null)
		{
			global::UnitController unitController2 = this.GetCurrentUnit();
			unitController2 = ((!(unitController2 == null)) ? unitController2 : unitController);
			global::System.Collections.Generic.List<global::DecisionPoint> availableSpawnPoints = this.GetAvailableSpawnPoints(spawnVisible, true, unitController2.transform, unitController.forcedSpawnPoints);
			if (availableSpawnPoints.Count > 0)
			{
				this.IncludeUnit(unitController, availableSpawnPoints[0].transform.position, availableSpawnPoints[0].transform.rotation);
				this.ForceUnitVisibilityCheck(unitController);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<string, string>(global::Notices.MISSION_UNIT_SPAWN, loc, unitController.unit.Name);
			}
			else
			{
				global::PandoraDebug.LogWarning("Trying to spawn " + campaignUnitId + " but there is no spawn points available", "INTERACTIVE POINT", null);
			}
		}
		else
		{
			global::PandoraDebug.LogWarning("Trying to spawn campaign unit " + campaignUnitId + " but it's not in the hiddent list", "INTERACTIVE POINT", null);
		}
		return unitController;
	}

	public void SetCombatCircles(global::UnitController ctrlr, global::System.Action onCirclesSet)
	{
		base.StartCoroutine(this.SetCombatAllCircles(ctrlr, onCirclesSet));
	}

	private global::System.Collections.IEnumerator SetCombatAllCircles(global::UnitController ctrlr, global::System.Action onCirclesSet)
	{
		this.lockNavRefresh = true;
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
			{
				this.WarbandCtrlrs[i].unitCtrlrs[j].SetCombatCircle(ctrlr, false);
				yield return null;
			}
		}
		this.lockNavRefresh = false;
		if (onCirclesSet != null)
		{
			onCirclesSet();
		}
		yield break;
	}

	public void UpdateCombatCirclesAlpha(global::UnitController ctrlr)
	{
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
			{
				this.WarbandCtrlrs[i].unitCtrlrs[j].SetCombatCircleAlpha(ctrlr);
			}
		}
	}

	public void ShowCombatCircles(global::UnitController currentCtrlr)
	{
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
			{
				global::UnitController unitController = this.WarbandCtrlrs[i].unitCtrlrs[j];
				if (unitController != this && unitController.unit.Status != global::UnitStateId.OUT_OF_ACTION)
				{
					unitController.combatCircle.Show(true);
					unitController.combatCircle.SetMaterial(currentCtrlr.IsEnemy(unitController), unitController.Engaged);
				}
			}
		}
		this.UpdateCombatCirclesAlpha(currentCtrlr);
	}

	public void HideCombatCircles()
	{
		for (int i = 0; i < this.WarbandCtrlrs.Count; i++)
		{
			for (int j = 0; j < this.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
			{
				this.WarbandCtrlrs[i].unitCtrlrs[j].combatCircle.Show(false);
			}
		}
	}

	public const int POINTING_ARROWS_COUNT = 10;

	public const int MAPS_BEACON_MAX = 5;

	private const int MAX_EXTERNAL_PER_PRAME = 150;

	private const int MAX_ZONE_BY_FRAME = 10;

	public global::UnityEngine.Color colorSelected;

	public global::UnityEngine.Color colorUnselected;

	public int currentTurn;

	private int syncDone;

	public global::Pathfinding.TileHandlerHelper tileHandlerHelper;

	private bool navGraphNeedsRefresh;

	[global::UnityEngine.HideInInspector]
	public bool lockNavRefresh;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::Pathfinding.NodeLink2> nodeLinks;

	private global::Pathfinding.NNConstraint nearestNodeConstraint;

	private global::Pathfinding.NNInfo nearestNodeInfo;

	[global::UnityEngine.HideInInspector]
	public global::Pathfinding.RayPathModifier pathRayModifier;

	private global::System.Collections.Generic.List<global::UnitController> allEnemiesList;

	private global::System.Collections.Generic.List<global::UnitController> allLiveEnemiesList;

	private global::System.Collections.Generic.List<global::UnitController> allLiveAlliesList;

	private global::System.Collections.Generic.List<global::UnitController> allLiveMyUnitsList;

	private global::System.Collections.Generic.List<global::UnitController> allLiveUnits;

	private global::System.Collections.Generic.List<global::UnitController> allMyUnitsList;

	public global::System.Collections.Generic.List<global::UnitController> allUnitsList;

	public global::CampaignMissionId campaignId;

	public global::System.Collections.Generic.List<global::UnitController> excludedUnits;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::ActionZone> actionZones;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::ActionZone> accessibleActionZones = new global::System.Collections.Generic.List<global::ActionZone>();

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::InteractivePoint> interactivePoints;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::InteractivePoint> interactivePointsTrash = new global::System.Collections.Generic.List<global::InteractivePoint>();

	private global::System.Collections.Generic.List<global::InteractivePoint> initialSearchPoints;

	public int numWyrdstones;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::TriggerPoint> triggerPoints;

	private global::System.Collections.Generic.List<global::DecisionPoint> decisionPoints;

	private global::System.Collections.Generic.List<global::LocateZone> locateZones;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::ZoneAoe> zoneAoes;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::SpawnNode> spawnNodes;

	private global::System.Collections.Generic.Dictionary<string, global::PatrolRoute> patrolRoutes;

	public global::MissionMapData mapData;

	public global::MapContour mapContour;

	public global::MapOrigin mapOrigin;

	public global::System.Collections.Generic.List<global::MapObjectiveZone> mapObjectiveZones;

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> pointingArrows = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	[global::UnityEngine.HideInInspector]
	public bool resendLadder;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.Stack<global::UnitController> delayedUnits;

	[global::UnityEngine.HideInInspector]
	public global::UnitController interruptingUnit;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::SearchPoint> lootedEnemies = new global::System.Collections.Generic.List<global::SearchPoint>();

	public global::UnityEngine.GameObject sphereTarget;

	private global::UnityEngine.GameObject aoeTargetSphere;

	private global::UnityEngine.GameObject aoeGroundTargetSphere;

	public global::UnityEngine.GameObject coneTarget;

	public global::UnityEngine.GameObject lineTarget;

	public global::UnityEngine.GameObject dummyTargeter;

	public global::UnityEngine.GameObject arcTarget;

	private global::System.Collections.Generic.List<global::Beacon> beacons = new global::System.Collections.Generic.List<global::Beacon>();

	private global::UnityEngine.GameObject beaconPrefab;

	private int beaconLimit;

	private int beaconIdx;

	public global::UnityEngine.GameObject deployBeaconPrefab;

	private global::System.Collections.Generic.List<global::MapBeacon> mapBeacons;

	[global::UnityEngine.HideInInspector]
	public bool gameFinished;

	private global::System.Collections.Generic.List<global::ExternalUpdator> externalUpdators;

	private int extUpdaterIndex;

	private uint envGuid;

	private uint rtGuid;

	public int lastWarbandIdx;

	private global::UnityEngine.GameObject lootbagPrefab;

	private bool checkMultiPlayerConnection;

	private bool checkInvite;

	[global::UnityEngine.HideInInspector]
	public bool isDeploying;

	private global::UnityEngine.RaycastHit hitInfo;

	private int lastBeaconIdx;

	public enum TurnPhase
	{
		START_OF_GAME,
		DEPLOYMENT,
		START_OF_ROUND,
		WAIT_GAME_SETUP,
		ROUT,
		UNIT_MOVEMENT,
		WATCH_UNIT,
		END_OF_ROUND,
		END_OF_GAME,
		COUNT
	}
}
