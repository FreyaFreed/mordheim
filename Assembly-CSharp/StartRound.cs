using System;
using System.Collections;
using System.Collections.Generic;

public class StartRound : global::ICheapState
{
	public StartRound(global::MissionManager mission)
	{
		this.missionMngr = mission;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.missionMngr.CombatLogger.AddLog(global::CombatLogger.LogMessage.ROUND_START, new string[]
		{
			(this.missionMngr.currentTurn + 1).ToConstantString()
		});
		this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.FIXED, null, true, false, true, false);
		if (this.missionMngr.campaignId != global::CampaignMissionId.NONE && !global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
		{
			this.missionMngr.StartCoroutine(this.CheckReinforcements());
		}
		else
		{
			this.StartRoundAction();
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::System.Collections.IEnumerator CheckReinforcements()
	{
		bool noticeSent = false;
		int totalUnitSpawned = 0;
		if (this.spawnsData == null)
		{
			this.spawnsData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionSpawnData>("fk_campaign_mission_id", ((int)this.missionMngr.campaignId).ToConstantString());
		}
		for (int i = 0; i < this.spawnsData.Count; i++)
		{
			int teamIdx = this.spawnsData[i].Team;
			this.teamWarbands.Clear();
			int aliveUnits = 0;
			for (int w = 0; w < this.missionMngr.WarbandCtrlrs.Count; w++)
			{
				if (this.missionMngr.WarbandCtrlrs[w].teamIdx == teamIdx)
				{
					this.teamWarbands.Add(this.missionMngr.WarbandCtrlrs[w]);
					for (int u = 0; u < this.missionMngr.WarbandCtrlrs[w].unitCtrlrs.Count; u++)
					{
						aliveUnits += ((this.missionMngr.WarbandCtrlrs[w].unitCtrlrs[u].unit.Status == global::UnitStateId.OUT_OF_ACTION) ? 0 : 1);
					}
				}
			}
			if (aliveUnits < this.spawnsData[i].MinUnit)
			{
				global::System.Collections.Generic.List<global::DecisionPoint> availableSpawnPoints = this.missionMngr.GetAvailableSpawnPoints(false, true, null, null);
				int requiredUnits = this.spawnsData[i].MinUnit - aliveUnits;
				int spawnedUnits = 0;
				global::System.Collections.Generic.List<global::CampaignMissionSpawnQueueData> queuesData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionSpawnQueueData>("fk_campaign_mission_spawn_id", ((int)this.spawnsData[i].Id).ToConstantString());
				queuesData.Sort((global::CampaignMissionSpawnQueueData x, global::CampaignMissionSpawnQueueData y) => x.Order.CompareTo(y.Order));
				int q = 0;
				while (q < queuesData.Count && spawnedUnits < requiredUnits)
				{
					int queueSpawned = 0;
					global::System.Collections.Generic.List<global::UnitController> unitTypeSrcs = new global::System.Collections.Generic.List<global::UnitController>();
					for (int w2 = 0; w2 < this.teamWarbands.Count; w2++)
					{
						for (int u2 = 0; u2 < this.teamWarbands[w2].unitCtrlrs.Count; u2++)
						{
							if (this.teamWarbands[w2].unitCtrlrs[u2].unit.Data.UnitTypeId == queuesData[q].UnitTypeId)
							{
								unitTypeSrcs.Add(this.teamWarbands[w2].unitCtrlrs[u2]);
							}
						}
					}
					while (unitTypeSrcs.Count > 0 && availableSpawnPoints.Count > 0 && queueSpawned < queuesData[q].Amount && spawnedUnits < requiredUnits)
					{
						if (!noticeSent)
						{
							noticeSent = true;
							global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<int>(global::Notices.MISSION_REINFORCEMENTS, this.missionMngr.currentTurn);
						}
						spawnedUnits++;
						queueSpawned++;
						totalUnitSpawned++;
						global::UnitController srcCtrlr = unitTypeSrcs[global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, unitTypeSrcs.Count)];
						global::DecisionPoint spawnPoint = availableSpawnPoints[0];
						availableSpawnPoints.RemoveAt(0);
						yield return this.missionMngr.StartCoroutine(global::PandoraSingleton<global::UnitFactory>.Instance.CloneUnitCtrlr(srcCtrlr, queuesData[q].Rank, queuesData[q].Rating, spawnPoint.transform.position, spawnPoint.transform.rotation));
					}
					q++;
				}
			}
		}
		this.StartRoundAction();
		yield break;
	}

	private void StartRoundAction()
	{
		if (global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
		{
			this.missionMngr.currentTurn = global::PandoraSingleton<global::MissionStartData>.Instance.currentTurn;
			this.missionMngr.ReloadLadder();
			for (int i = 0; i < global::PandoraSingleton<global::MissionStartData>.Instance.morals.Count; i++)
			{
				global::WarbandController warbandController = this.missionMngr.WarbandCtrlrs[i];
				warbandController.idolMoralRemoved = global::PandoraSingleton<global::MissionStartData>.Instance.morals[i].Item3;
				warbandController.OldMoralValue = global::PandoraSingleton<global::MissionStartData>.Instance.morals[i].Item2;
				warbandController.MoralValue = global::PandoraSingleton<global::MissionStartData>.Instance.morals[i].Item1;
			}
		}
		else if (this.missionMngr.currentTurn != 0)
		{
			this.missionMngr.ResetLadderIdx(true);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<int>(global::Notices.MISSION_ROUND_START, this.missionMngr.currentTurn);
		}
		for (int j = 0; j < this.missionMngr.WarbandCtrlrs.Count; j++)
		{
			global::WarbandController warbandController2 = this.missionMngr.WarbandCtrlrs[j];
			for (int k = 0; k < warbandController2.unitCtrlrs.Count; k++)
			{
				global::UnitController unitController = warbandController2.unitCtrlrs[k];
				if (unitController.StateMachine.GetActiveStateId() == 39 || this.missionMngr.currentTurn == 0)
				{
					if (unitController.unit.IsAvailable())
					{
						if (unitController.unit.OverwatchLeft > 0 && unitController.HasRange())
						{
							unitController.StateMachine.ChangeState(36);
							goto IL_1BE;
						}
						if (unitController.unit.AmbushLeft > 0 && unitController.HasClose())
						{
							unitController.StateMachine.ChangeState(37);
							goto IL_1BE;
						}
					}
					unitController.StateMachine.ChangeState(9);
				}
				IL_1BE:;
			}
			if (warbandController2.SquadManager != null)
			{
				warbandController2.SquadManager.RefreshSquads();
			}
		}
		if (!global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
		{
			this.missionMngr.RestoreUnitWeapons();
		}
		if (this.missionMngr.DissolveDeadUnits())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<int>(global::Notices.MISSION_DEAD_UNIT_FLEE, this.missionMngr.currentTurn);
		}
		this.missionMngr.UpdateObjectivesUI(false);
		this.missionMngr.SendTurnReady();
	}

	private global::MissionManager missionMngr;

	private global::System.Collections.Generic.List<global::CampaignMissionSpawnData> spawnsData;

	private global::System.Collections.Generic.List<global::WarbandController> teamWarbands = new global::System.Collections.Generic.List<global::WarbandController>();
}
