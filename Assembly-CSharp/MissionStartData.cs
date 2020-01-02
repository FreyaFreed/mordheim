using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStartData : global::PandoraSingleton<global::MissionStartData>, global::IMyrtilus
{
	public int Seed { get; private set; }

	public global::Mission CurrentMission { get; private set; }

	public global::System.Collections.Generic.List<global::MissionWarbandSave> FightingWarbands { get; private set; }

	private void Awake()
	{
		global::UnityEngine.Object.DontDestroyOnLoad(this);
		this.Clear();
	}

	public void Clear()
	{
		this.CurrentMission = new global::Mission(new global::MissionSave(global::Constant.GetFloat(global::ConstantId.ROUT_RATIO_ALIVE)));
		this.FightingWarbands = new global::System.Collections.Generic.List<global::MissionWarbandSave>();
		this.ResetSeed();
		this.isReload = false;
		this.locked = false;
		this.usedTraps.Clear();
		this.searches.Clear();
		this.aoeZones.Clear();
		this.units.Clear();
		this.morals.Clear();
		this.reinforcementsIdx.Clear();
		this.objectives.Clear();
		this.converters.Clear();
		this.activaters.Clear();
		this.dynamicTraps.Clear();
		this.destructibles.Clear();
		this.myrtilusLadder = new global::System.Collections.Generic.List<uint>();
		this.currentLadderIdx = 0;
		this.currentTurn = 0;
	}

	public void ResetSeed()
	{
		this.Seed = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, int.MaxValue);
	}

	public void Lock()
	{
		this.locked = true;
	}

	public void ResetWarbandsReady()
	{
		for (int i = 1; i < this.FightingWarbands.Count; i++)
		{
			this.FightingWarbands[i].ResetReady();
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SKIRMISH_OPPONENT_READY);
	}

	public void InitSkirmish(global::WarbandMenuController warband, global::System.Collections.Generic.List<int> unitsPosition, bool isExhibition)
	{
		this.ResetSeed();
		this.SetMission(global::Mission.GenerateSkirmishMission(global::MissionMapId.NONE, global::DeploymentScenarioId.NONE));
		this.FightingWarbands.Clear();
		int rating = 0;
		string[] array;
		warband.GetSkirmishInfo(unitsPosition, out rating, out array);
		this.AddFightingWarband(warband.Warband.Id, global::CampaignWarbandId.NONE, warband.Warband.GetWarbandSave().name, warband.Warband.GetWarbandSave().overrideName, global::PandoraSingleton<global::Hephaestus>.Instance.GetUserName(), warband.Warband.Rank, rating, global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex, global::PlayerTypeId.PLAYER, array);
		this.FightingWarbands[0].IsReady = true;
		this.CurrentMission.missionSave.isSkirmish = isExhibition;
	}

	public void RegenerateMission(global::MissionMapId mapId = global::MissionMapId.NONE, global::DeploymentScenarioId scenarioId = global::DeploymentScenarioId.NONE, bool keepLayout = false)
	{
		bool randomLayout = false;
		int mapLayoutId = 0;
		bool randomGameplay = false;
		int mapGameplayId = 0;
		if (keepLayout)
		{
			randomLayout = this.CurrentMission.missionSave.randomLayout;
			mapLayoutId = this.CurrentMission.missionSave.mapLayoutId;
			randomGameplay = this.CurrentMission.missionSave.randomGameplay;
			mapGameplayId = this.CurrentMission.missionSave.mapGameplayId;
		}
		int turnTimer = 0;
		if (this.CurrentMission != null)
		{
			turnTimer = this.CurrentMission.missionSave.turnTimer;
		}
		int deployTimer = 0;
		if (this.CurrentMission != null)
		{
			deployTimer = this.CurrentMission.missionSave.deployTimer;
		}
		int beaconLimit = 0;
		if (this.CurrentMission != null)
		{
			beaconLimit = this.CurrentMission.missionSave.beaconLimit;
		}
		bool autoDeploy = false;
		if (this.CurrentMission != null)
		{
			autoDeploy = this.CurrentMission.missionSave.autoDeploy;
		}
		bool randomRoaming = false;
		if (this.CurrentMission != null)
		{
			randomRoaming = this.CurrentMission.missionSave.randomRoaming;
		}
		int roamingUnitId = 0;
		if (this.CurrentMission != null)
		{
			roamingUnitId = this.CurrentMission.missionSave.roamingUnitId;
		}
		bool isSkirmish = true;
		if (this.CurrentMission != null)
		{
			isSkirmish = this.CurrentMission.missionSave.isSkirmish;
		}
		float routThreshold = global::Constant.GetFloat(global::ConstantId.ROUT_RATIO_ALIVE);
		if (this.CurrentMission != null)
		{
			routThreshold = this.CurrentMission.missionSave.routThreshold;
		}
		int ratingId = 1;
		if (this.CurrentMission != null)
		{
			ratingId = this.CurrentMission.missionSave.ratingId;
		}
		this.SetMission(global::Mission.GenerateSkirmishMission(mapId, scenarioId));
		if (keepLayout)
		{
			this.CurrentMission.missionSave.randomLayout = randomLayout;
			this.CurrentMission.missionSave.mapLayoutId = mapLayoutId;
			this.CurrentMission.missionSave.randomGameplay = randomGameplay;
			this.CurrentMission.missionSave.mapGameplayId = mapGameplayId;
		}
		this.CurrentMission.missionSave.turnTimer = turnTimer;
		this.CurrentMission.missionSave.deployTimer = deployTimer;
		this.CurrentMission.missionSave.beaconLimit = beaconLimit;
		this.CurrentMission.missionSave.autoDeploy = autoDeploy;
		this.CurrentMission.missionSave.randomRoaming = randomRoaming;
		this.CurrentMission.missionSave.roamingUnitId = roamingUnitId;
		this.CurrentMission.missionSave.isSkirmish = isSkirmish;
		this.CurrentMission.missionSave.routThreshold = routThreshold;
		this.CurrentMission.missionSave.ratingId = ratingId;
		this.SendMission(false);
	}

	public global::System.Collections.IEnumerator SetMissionFull(global::Mission mission, global::WarbandMenuController warbandMenuCtrlr, global::System.Action callback)
	{
		this.SetMission(mission);
		this.FightingWarbands.Clear();
		if (mission.missionSave.isCampaign)
		{
			if (mission.missionSave.campaignId != 0)
			{
				global::System.Collections.Generic.List<global::CampaignMissionJoinCampaignWarbandData> campaignWarbandsData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionJoinCampaignWarbandData>("fk_campaign_mission_id", mission.missionSave.campaignId.ToString());
				for (int i = 0; i < campaignWarbandsData.Count; i++)
				{
					global::CampaignMissionJoinCampaignWarbandData warData = campaignWarbandsData[i];
					if (warData.CampaignWarbandId == global::CampaignWarbandId.NONE)
					{
						this.AddFightingWarband(warbandMenuCtrlr, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_ai"), global::PlayerTypeId.PLAYER);
					}
					else
					{
						global::WarbandMenuController ctrlr = new global::WarbandMenuController(global::Mission.GetCampaignWarband(warData));
						this.AddFightingWarband(ctrlr, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_ai"), warData.PlayerTypeId);
					}
				}
			}
		}
		else
		{
			this.AddFightingWarband(warbandMenuCtrlr, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_ai"), global::PlayerTypeId.PLAYER);
			for (int j = 1; j < mission.missionSave.deployCount; j++)
			{
				bool impressive = false;
				int heroesCount = 0;
				int highestUnitRank = 0;
				for (int k = 0; k < this.FightingWarbands[0].Units.Count; k++)
				{
					highestUnitRank = global::UnityEngine.Mathf.Max(highestUnitRank, global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)this.FightingWarbands[0].Units[j].rankId).Rank);
					int id = this.FightingWarbands[0].Units[k].stats.id;
					global::UnitTypeId unitTypeId = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>(id).UnitTypeId;
					switch (global::Unit.GetUnitTypeId(this.FightingWarbands[0].Units[k], unitTypeId))
					{
					case global::UnitTypeId.IMPRESSIVE:
						impressive = true;
						break;
					case global::UnitTypeId.HERO_1:
					case global::UnitTypeId.HERO_2:
					case global::UnitTypeId.HERO_3:
						heroesCount++;
						break;
					}
				}
				int warbandRating = warbandMenuCtrlr.Warband.GetRating();
				int newWarbandRating = warbandRating;
				if (mission.missionSave.ratingId != 0)
				{
					global::ProcMissionRatingData procMissionRatingData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingData>(mission.missionSave.ratingId);
					int ratingPerc = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(procMissionRatingData.ProcMinValue, procMissionRatingData.ProcMaxValue);
					newWarbandRating += warbandRating * ratingPerc / 100;
				}
				else if (mission.missionSave.rating >= 0)
				{
					if (mission.missionSave.rating < 100)
					{
						newWarbandRating += warbandRating * mission.missionSave.rating / 100;
					}
					else
					{
						newWarbandRating += mission.missionSave.rating;
					}
				}
				else
				{
					global::ProcMissionRatingData procMissionRatingData2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingData>(-mission.missionSave.rating);
					int ratingPerc2 = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(procMissionRatingData2.ProcMinValue, procMissionRatingData2.ProcMaxValue);
					newWarbandRating += warbandRating * ratingPerc2 / 100;
				}
				global::WarbandData nextNotFacedWarband = warbandMenuCtrlr.Warband.GetNextNotFacedWarband(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche);
				yield return base.StartCoroutine(global::Mission.GetProcWarband(newWarbandRating, warbandMenuCtrlr.Warband.Rank, this.FightingWarbands[0].Units.Count, impressive, nextNotFacedWarband, heroesCount, highestUnitRank, delegate(global::WarbandSave save)
				{
					global::WarbandMenuController warbandMenuController = new global::WarbandMenuController(save);
					this.AddFightingWarband(warbandMenuController, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_ai"), global::PlayerTypeId.AI);
					mission.missionSave.rating = warbandMenuController.Warband.GetRating();
					int num = 0;
					for (int l = 0; l < warbandMenuCtrlr.unitCtrlrs.Count; l++)
					{
						if (warbandMenuCtrlr.unitCtrlrs[l].unit.UnitSave.warbandSlotIndex < 12 && warbandMenuCtrlr.unitCtrlrs[l].unit.GetActiveStatus() == global::UnitActiveStatusId.AVAILABLE)
						{
							num = ((warbandMenuCtrlr.unitCtrlrs[l].unit.Rank <= num) ? num : warbandMenuCtrlr.unitCtrlrs[l].unit.Rank);
						}
					}
					if (num <= global::Constant.GetInt(global::ConstantId.ROAMING_MIN_RANK) || mission.missionSave.ratingId < 3)
					{
						mission.missionSave.roamingUnitId = 0;
					}
				}));
			}
		}
		callback();
		yield break;
	}

	public void AddFightingWarband(global::WarbandMenuController ctrlr, string playerName, global::PlayerTypeId playerType)
	{
		this.AddFightingWarband((global::WarbandId)ctrlr.Warband.GetWarbandSave().id, (global::CampaignWarbandId)ctrlr.Warband.GetWarbandSave().campaignId, ctrlr.Warband.GetWarbandSave().name, ctrlr.Warband.GetWarbandSave().overrideName, playerName, ctrlr.Warband.Rank, ctrlr.Warband.GetRating(), global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex, playerType, ctrlr.GetActiveUnitsSerialized());
	}

	public void AddFightingWarband(global::WarbandId type, global::CampaignWarbandId campaignId, string name, string overrideName, string playerName, int rank, int rating, int playerIndex, global::PlayerTypeId playerTypeId, string[] units)
	{
		this.FightingWarbands.Add(new global::MissionWarbandSave(type, campaignId, name, overrideName, playerName, rank, rating, playerIndex, playerTypeId, units));
	}

	public void SetFightingWarband(int warbandMissionIdx, global::WarbandId type, string name, string overrideName, string playerName, int rank, int rating, int playerIndex, global::PlayerTypeId playerTypeId, string[] units)
	{
		this.FightingWarbands[warbandMissionIdx] = new global::MissionWarbandSave(type, global::CampaignWarbandId.NONE, name, overrideName, playerName, rank, rating, playerIndex, playerTypeId, units);
		if (warbandMissionIdx == 0)
		{
			this.FightingWarbands[warbandMissionIdx].IsReady = true;
		}
	}

	public void SetMission(global::Mission mission)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		for (int i = 0; i < mission.missionSave.deployCount; i++)
		{
			string text3 = text;
			text = string.Concat(new object[]
			{
				text3,
				" Deploy ",
				i,
				" : ",
				(global::DeploymentScenarioSlotId)mission.missionSave.deployScenarioSlotIds[i]
			});
			text3 = text2;
			text2 = string.Concat(new object[]
			{
				text3,
				" Objective ",
				i,
				" : ",
				(global::PrimaryObjectiveTypeId)mission.missionSave.objectiveTypeIds[i]
			});
		}
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Set mission  seed = ",
			this.Seed,
			" deployCount = ",
			mission.missionSave.deployScenarioSlotIds.Count,
			" deployScenarioMapLayoutId = ",
			(global::DeploymentScenarioMapLayoutId)mission.missionSave.deployScenarioMapLayoutId,
			" mapLayoutId = ",
			(global::MissionMapLayoutId)mission.missionSave.mapLayoutId,
			" VictoryTypeId = ",
			(global::VictoryTypeId)mission.missionSave.VictoryTypeId,
			" turnTimer = ",
			mission.missionSave.turnTimer,
			" deployTimer = ",
			mission.missionSave.deployTimer,
			text,
			text2,
			" wyrdPlacementId = ",
			(global::WyrdstonePlacementId)mission.missionSave.wyrdPlacementId,
			" wyrdDensityId = ",
			(global::WyrdstoneDensityId)mission.missionSave.wyrdDensityId,
			" searchDensity = ",
			(global::SearchDensityId)mission.missionSave.searchDensityId
		}), "HERMES", null);
		this.CurrentMission = mission;
	}

	public int GetWarbandIndex(int hermesIdx)
	{
		for (int i = 0; i < this.FightingWarbands.Count; i++)
		{
			if (this.FightingWarbands[i].PlayerIndex == hermesIdx)
			{
				return i;
			}
		}
		return -1;
	}

	public void ReloadMission(global::MissionEndDataSave endmission, global::WarbandSave playerWarband)
	{
		this.isReload = true;
		this.Seed = endmission.seed;
		this.usedTraps = endmission.destroyedTraps;
		this.searches = endmission.searches;
		this.aoeZones = endmission.aoeZones;
		this.units = endmission.units;
		this.myrtilusLadder = endmission.myrtilusLadder;
		this.currentLadderIdx = endmission.currentLadderIdx;
		this.currentTurn = endmission.currentTurn;
		this.morals = endmission.warbandMorals;
		this.reinforcementsIdx = endmission.reinforcements;
		this.objectives = endmission.objectives;
		this.converters = endmission.converters;
		this.activaters = endmission.activaters;
		this.dynamicTraps = endmission.dynamicTraps;
		this.destructibles = endmission.destructibles;
		endmission.missionSave.autoDeploy = true;
		this.SetMission(new global::Mission(endmission.missionSave));
		this.FightingWarbands.Clear();
		this.FightingWarbands = endmission.missionWarbands;
	}

	public void SendMissionStartData()
	{
		this.SendMission(true);
		for (int i = 0; i < this.FightingWarbands.Count; i++)
		{
			global::MissionWarbandSave missionWarbandSave = this.FightingWarbands[i];
			global::PandoraDebug.LogInfo(string.Concat(new object[]
			{
				"Send SetWarband type = ",
				missionWarbandSave.WarbandId,
				" name = ",
				missionWarbandSave.Name,
				" player = ",
				missionWarbandSave.PlayerIndex,
				" player type = ",
				missionWarbandSave.PlayerTypeId,
				" units = ",
				missionWarbandSave.Units.Count
			}), "HERMES", null);
			this.Send(true, global::Hermes.SendTarget.OTHERS, this.uid, 2U, new object[]
			{
				i,
				(int)missionWarbandSave.WarbandId,
				missionWarbandSave.Name,
				missionWarbandSave.OverrideName,
				missionWarbandSave.PlayerName,
				missionWarbandSave.Rank,
				missionWarbandSave.Rating,
				missionWarbandSave.PlayerIndex,
				(int)missionWarbandSave.PlayerTypeId,
				missionWarbandSave.SerializedUnits
			});
		}
	}

	public void RefreshMyWarband(global::WarbandMenuController warbandCtrlr, global::System.Collections.Generic.List<int> unitsPosition)
	{
		for (int i = 0; i < this.FightingWarbands.Count; i++)
		{
			global::MissionWarbandSave missionWarbandSave = this.FightingWarbands[i];
			if (missionWarbandSave.PlayerIndex == global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex && missionWarbandSave.PlayerTypeId == global::PlayerTypeId.PLAYER)
			{
				int num;
				string[] array;
				warbandCtrlr.GetSkirmishInfo(unitsPosition, out num, out array);
				this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 2U, new object[]
				{
					i,
					(int)missionWarbandSave.WarbandId,
					missionWarbandSave.Name,
					missionWarbandSave.OverrideName,
					missionWarbandSave.PlayerName,
					missionWarbandSave.Rank,
					num,
					missionWarbandSave.PlayerIndex,
					(int)missionWarbandSave.PlayerTypeId,
					array
				});
				this.ResetWarbandsReady();
				return;
			}
		}
	}

	public void SendMission(bool clearWarbands)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		for (int i = 0; i < this.CurrentMission.missionSave.deployCount; i++)
		{
			string text3 = text;
			text = string.Concat(new object[]
			{
				text3,
				" Deploy ",
				i,
				" : ",
				(global::DeploymentScenarioSlotId)this.CurrentMission.missionSave.deployScenarioSlotIds[i]
			});
			text3 = text2;
			text2 = string.Concat(new object[]
			{
				text3,
				" Objective ",
				i,
				" : ",
				(global::PrimaryObjectiveTypeId)this.CurrentMission.missionSave.objectiveTypeIds[i]
			});
		}
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"SendMission  seed = ",
			this.Seed,
			" deployCount = ",
			this.CurrentMission.missionSave.deployCount,
			" deployScenarioMapLayoutId = ",
			(global::DeploymentScenarioMapLayoutId)this.CurrentMission.missionSave.deployScenarioMapLayoutId,
			" mapLayoutId = ",
			(global::MissionMapLayoutId)this.CurrentMission.missionSave.mapLayoutId,
			" VictoryTypeId = ",
			(global::VictoryTypeId)this.CurrentMission.missionSave.VictoryTypeId,
			" turnTimer = ",
			this.CurrentMission.missionSave.turnTimer,
			" deployTimer = ",
			this.CurrentMission.missionSave.deployTimer,
			text,
			text2,
			" wyrdPlacementId = ",
			(global::WyrdstonePlacementId)this.CurrentMission.missionSave.wyrdPlacementId,
			" wyrdDensityId = ",
			(global::WyrdstoneDensityId)this.CurrentMission.missionSave.wyrdDensityId,
			" searchDensity = ",
			(global::SearchDensityId)this.CurrentMission.missionSave.searchDensityId,
			" routThreshold = ",
			this.CurrentMission.missionSave.routThreshold
		}), "HERMES", null);
		this.Send(true, global::Hermes.SendTarget.OTHERS, this.uid, 3U, new object[]
		{
			this.Seed,
			global::Thoth.WriteToString(this.CurrentMission.missionSave),
			clearWarbands
		});
		this.ResetWarbandsReady();
	}

	private void MissionRPC(int seed, string serializedMission, bool clear)
	{
		this.Seed = seed;
		global::MissionSave missionSave = new global::MissionSave(global::Constant.GetFloat(global::ConstantId.ROUT_RATIO_ALIVE));
		global::Thoth.ReadFromString(serializedMission, missionSave);
		this.SetMission(new global::Mission(missionSave));
		if (clear)
		{
			this.FightingWarbands = new global::System.Collections.Generic.List<global::MissionWarbandSave>();
			for (int i = 0; i < this.CurrentMission.missionSave.deployCount; i++)
			{
				this.FightingWarbands.Add(null);
			}
		}
		else
		{
			this.ResetWarbandsReady();
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SKIRMISH_LOBBY_UPDATED);
	}

	private void SetWarbandInfoRPC(int missionWarbandIdx, int warbandId, string name, string overrideName, string playerName, int rank, int rating, int playerIndex, int playerTypeId, string[] unitSaves)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Client received WarbandInfoRPC : type = ",
			(global::WarbandId)warbandId,
			" name = ",
			name,
			" overridename = ",
			overrideName,
			" playerName = ",
			playerName,
			" player = ",
			playerIndex,
			" playerType = ",
			(global::PlayerTypeId)playerTypeId,
			"HERMES"
		}), "uncategorised", null);
		this.SetFightingWarband(missionWarbandIdx, (global::WarbandId)warbandId, name, overrideName, playerName, rank, rating, playerIndex, (global::PlayerTypeId)playerTypeId, unitSaves);
		if (playerIndex > 0)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SKIRMISH_OPPONENT_JOIN);
		}
		if (playerIndex != global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex)
		{
			this.CurrentMission.missionSave.rating = rating;
			this.CurrentMission.RefreshDifficulty(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetRating(), false);
		}
	}

	public void OnNetworkConnected(global::WarbandMenuController warbandCtrlr, global::System.Collections.Generic.List<int> unitsPosition)
	{
		int num;
		string[] array;
		warbandCtrlr.GetSkirmishInfo(unitsPosition, out num, out array);
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"SendWarbandInfo type = ",
			warbandCtrlr.Warband.Id,
			" name = ",
			warbandCtrlr.Warband.GetWarbandSave().Name,
			" player = ",
			global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex,
			" team = ",
			global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex,
			" player type = ",
			2,
			" units = ",
			warbandCtrlr.GetActiveUnitsSave().Count
		}), "HERMES", null);
		this.Send(true, global::Hermes.SendTarget.OTHERS, this.uid, 1U, new object[]
		{
			(int)warbandCtrlr.Warband.Id,
			warbandCtrlr.Warband.GetWarbandSave().name,
			warbandCtrlr.Warband.GetWarbandSave().overrideName,
			global::PandoraSingleton<global::Hephaestus>.Instance.GetUserName(),
			warbandCtrlr.Warband.Rank,
			num,
			global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex,
			2,
			array
		});
	}

	private void AddWarbandInfoRPC(int warbandId, string name, string overrideName, string playerName, int rank, int rating, int playerIndex, int playerTypeId, string[] unitSaves)
	{
		int count = this.FightingWarbands.Count;
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Server received WarbandInfoRPC : WarbandIdx = ",
			count,
			"type = ",
			(global::WarbandId)warbandId,
			" name = ",
			name,
			" overridename = ",
			overrideName,
			" playerName = ",
			playerName,
			" rank = ",
			rank,
			" rating = ",
			rating,
			" player = ",
			playerIndex,
			" playerType = ",
			(global::PlayerTypeId)playerTypeId,
			" units = ",
			unitSaves
		}), "HERMES", null);
		if (count == 2)
		{
			global::PandoraDebug.LogInfo("Server already have 2 player, this player should be kick soon", "HERMES", null);
		}
		else
		{
			this.AddFightingWarband((global::WarbandId)warbandId, global::CampaignWarbandId.NONE, name, overrideName, playerName, rank, rating, playerIndex, (global::PlayerTypeId)playerTypeId, unitSaves);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SKIRMISH_OPPONENT_JOIN);
			this.SendMissionStartData();
		}
	}

	public void SendReady(bool ready)
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 5U, new object[]
		{
			global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex,
			ready
		});
	}

	private void ReadyRPC(int hermesPlayerIdx, bool ready)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Ready received player : ",
			hermesPlayerIdx,
			" is ",
			(!ready) ? "NOT" : string.Empty,
			" ready"
		}), "HERMES", null);
		foreach (global::MissionWarbandSave missionWarbandSave in this.FightingWarbands)
		{
			if (missionWarbandSave == null)
			{
				global::PandoraDebug.LogDebug("a warbandsave is null in FightingWarbands", "uncategorised", null);
			}
			else if (missionWarbandSave.PlayerIndex == hermesPlayerIdx)
			{
				missionWarbandSave.IsReady = ready;
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SKIRMISH_OPPONENT_READY);
				return;
			}
		}
		global::PandoraDebug.LogWarning("Cannot set Ready flag : Player not found among the fighting warbands", "HERMES", null);
	}

	public void KickPlayerFromLobby(int kickedPlayerIdx)
	{
		this.Send(true, global::Hermes.SendTarget.OTHERS, this.uid, 4U, new object[]
		{
			kickedPlayerIdx
		});
	}

	public void SendKickPlayer(int kickedPlayerIdx)
	{
		base.StartCoroutine(this.SendKickPlayerAsync(kickedPlayerIdx));
	}

	private global::System.Collections.IEnumerator SendKickPlayerAsync(int kickedPlayerIdx)
	{
		global::PandoraDebug.LogInfo("SendKickPlayerAsync " + (global::Hephaestus.LobbyConnexionResult)kickedPlayerIdx, "HERMES", null);
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"fc ",
			global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count,
			" playertype ",
			(global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count <= 1) ? global::PlayerTypeId.NONE : global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerTypeId,
			" isconnected ",
			global::PandoraSingleton<global::Hermes>.Instance.IsConnected()
		}), "uncategorised", null);
		while (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1 && global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerTypeId == global::PlayerTypeId.AI && global::PandoraSingleton<global::Hermes>.Instance.IsConnected())
		{
			global::PandoraDebug.LogInfo("SendKickPlayer " + (global::Hephaestus.LobbyConnexionResult)kickedPlayerIdx, "HERMES", null);
			this.Send(true, global::Hermes.SendTarget.OTHERS, this.uid, 4U, new object[]
			{
				kickedPlayerIdx
			});
			yield return new global::UnityEngine.WaitForSeconds(0.5f);
		}
		yield break;
	}

	private void KickPlayerRPC(int kickedPlayerIdx)
	{
		global::PandoraSingleton<global::SkirmishManager>.Instance.OnKick((global::Hephaestus.LobbyConnexionResult)kickedPlayerIdx);
	}

	public uint uid { get; set; }

	public uint owner { get; set; }

	public bool IsLocked
	{
		get
		{
			return this.locked;
		}
	}

	public void RegisterToHermes()
	{
		global::PandoraSingleton<global::Hermes>.Instance.RegisterMyrtilus(this, true);
		this.uid = 4294967293U;
	}

	public void RemoveFromHermes()
	{
		global::PandoraSingleton<global::Hermes>.Instance.RemoveMyrtilus(this);
	}

	public void Send(bool reliable, global::Hermes.SendTarget target, uint id, uint command, params object[] parms)
	{
		global::PandoraSingleton<global::Hermes>.Instance.Send(reliable, target, id, command, parms);
	}

	public void Receive(ulong from, uint command, object[] parms)
	{
		if (this.locked)
		{
			return;
		}
		switch (command)
		{
		case 1U:
		{
			int warbandId = (int)parms[0];
			string name = (string)parms[1];
			string overrideName = (string)parms[2];
			string playerName = (string)parms[3];
			int rank = (int)parms[4];
			int rating = (int)parms[5];
			int playerIndex = (int)parms[6];
			int playerTypeId = (int)parms[7];
			string[] unitSaves = (string[])parms[8];
			this.AddWarbandInfoRPC(warbandId, name, overrideName, playerName, rank, rating, playerIndex, playerTypeId, unitSaves);
			break;
		}
		case 2U:
		{
			int missionWarbandIdx = (int)parms[0];
			int warbandId2 = (int)parms[1];
			string name2 = (string)parms[2];
			string overrideName2 = (string)parms[3];
			string playerName2 = (string)parms[4];
			int rank2 = (int)parms[5];
			int rating2 = (int)parms[6];
			int playerIndex2 = (int)parms[7];
			int playerTypeId2 = (int)parms[8];
			string[] unitSaves2 = (string[])parms[9];
			this.SetWarbandInfoRPC(missionWarbandIdx, warbandId2, name2, overrideName2, playerName2, rank2, rating2, playerIndex2, playerTypeId2, unitSaves2);
			break;
		}
		case 3U:
		{
			int seed = (int)parms[0];
			string serializedMission = (string)parms[1];
			bool clear = (bool)parms[2];
			this.MissionRPC(seed, serializedMission, clear);
			break;
		}
		case 4U:
		{
			int kickedPlayerIdx = (int)parms[0];
			this.KickPlayerRPC(kickedPlayerIdx);
			break;
		}
		case 5U:
		{
			int hermesPlayerIdx = (int)parms[0];
			bool ready = (bool)parms[1];
			this.ReadyRPC(hermesPlayerIdx, ready);
			break;
		}
		}
	}

	public bool isReload;

	public global::System.Collections.Generic.List<uint> usedTraps = new global::System.Collections.Generic.List<uint>();

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, global::SearchSave>> searches = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, global::SearchSave>>();

	public global::System.Collections.Generic.List<global::EndZoneAoe> aoeZones = new global::System.Collections.Generic.List<global::EndZoneAoe>();

	public global::System.Collections.Generic.List<global::MissionEndUnitSave> units = new global::System.Collections.Generic.List<global::MissionEndUnitSave>();

	public global::System.Collections.Generic.List<global::Tuple<int, int, bool>> morals = new global::System.Collections.Generic.List<global::Tuple<int, int, bool>>();

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, int>> reinforcementsIdx = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, int>>();

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, uint>> objectives = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, uint>>();

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, int>> converters = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, int>>();

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, bool>> activaters = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, bool>>();

	public global::System.Collections.Generic.List<global::EndDynamicTrap> dynamicTraps = new global::System.Collections.Generic.List<global::EndDynamicTrap>();

	public global::System.Collections.Generic.List<global::EndDestructible> destructibles = new global::System.Collections.Generic.List<global::EndDestructible>();

	public global::System.Collections.Generic.List<uint> myrtilusLadder;

	public int currentLadderIdx;

	public int currentTurn;

	public global::System.Collections.Generic.List<global::SpawnNode>[] spawnNodes;

	public global::System.Collections.Generic.List<global::SpawnZone> spawnZones;

	private bool locked;

	private enum CommandList
	{
		NONE,
		ADD_WARBAND,
		SET_WARBAND,
		SET_MISSION,
		KICK_PLAYER,
		SET_READY,
		COUNT
	}
}
