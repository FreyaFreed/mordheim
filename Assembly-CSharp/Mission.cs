using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
	public Mission(global::MissionSave missionSave)
	{
		this.missionSave = missionSave;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			" deploymentScenarioMapLayout = ",
			(global::DeploymentScenarioMapLayoutId)this.missionSave.deployScenarioMapLayoutId,
			" mapLayout = ",
			(global::MissionMapLayoutId)this.missionSave.mapLayoutId,
			" deployCount = ",
			this.missionSave.deployCount,
			" wyrdPlacement = ",
			(global::WyrdstonePlacementId)this.missionSave.wyrdPlacementId,
			" wyrdDensity = ",
			(global::WyrdstoneDensityId)this.missionSave.wyrdDensityId
		});
	}

	public void RefreshDifficulty(int rating, bool isProc)
	{
		int num = 0;
		global::System.Collections.Generic.List<global::ProcMissionRatingData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingData>();
		for (int i = 0; i < list.Count; i++)
		{
			num = global::UnityEngine.Mathf.Max(num, list[i].MaxValue);
		}
		int num2;
		if (!isProc)
		{
			num2 = (int)(((float)this.missionSave.rating / (float)rating - 1f) * 100f);
		}
		else
		{
			if (this.missionSave.ratingId != 0)
			{
				return;
			}
			if (this.missionSave.rating < 0)
			{
				this.missionSave.ratingId = -rating;
				return;
			}
			if (rating < 100)
			{
				num2 = rating;
			}
			else
			{
				num2 = (int)(((float)this.missionSave.rating / (float)rating - 1f) * 100f);
			}
		}
		num2 = global::UnityEngine.Mathf.Clamp(num2, 0, num);
		global::ProcMissionRatingId procMissionRatingId = global::PandoraSingleton<global::DataFactory>.Instance.InitDataClosest<global::ProcMissionRatingData>("max_value", num2, false).Id;
		procMissionRatingId = ((procMissionRatingId != global::ProcMissionRatingId.NONE) ? procMissionRatingId : global::ProcMissionRatingId.NORMAL);
		this.missionSave.ratingId = (int)procMissionRatingId;
	}

	public global::DistrictId GetDistrictId()
	{
		global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>(this.missionSave.deployScenarioMapLayoutId);
		global::MissionMapData missionMapData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapData>((int)deploymentScenarioMapLayoutData.MissionMapId);
		return missionMapData.DistrictId;
	}

	public global::MissionMapId GetMapId()
	{
		if (this.missionSave.randomMap)
		{
			return global::MissionMapId.NONE;
		}
		global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>(this.missionSave.deployScenarioMapLayoutId);
		return deploymentScenarioMapLayoutData.MissionMapId;
	}

	public global::MissionMapLayoutId GetMapLayoutId()
	{
		if (this.missionSave.randomLayout)
		{
			return global::MissionMapLayoutId.NONE;
		}
		return (global::MissionMapLayoutId)this.missionSave.mapLayoutId;
	}

	public global::MissionMapGameplayId GetMapGameplayId()
	{
		if (this.missionSave.randomGameplay)
		{
			return global::MissionMapGameplayId.NONE;
		}
		return (global::MissionMapGameplayId)this.missionSave.mapGameplayId;
	}

	public string GetSkyName()
	{
		if (this.missionSave.randomLayout)
		{
			return "random";
		}
		global::MissionMapLayoutData missionMapLayoutData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapLayoutData>((int)this.GetMapLayoutId());
		return missionMapLayoutData.Name;
	}

	public bool IsAmbush()
	{
		global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>(this.missionSave.deployScenarioMapLayoutId);
		return deploymentScenarioMapLayoutData.Ambush;
	}

	public global::DeploymentScenarioId GetDeploymentScenarioId()
	{
		if (this.missionSave.randomDeployment)
		{
			return global::DeploymentScenarioId.NONE;
		}
		global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>(this.missionSave.deployScenarioMapLayoutId);
		return deploymentScenarioMapLayoutData.DeploymentScenarioId;
	}

	public global::DeploymentId GetDeploymentId(int idx)
	{
		if (this.missionSave.randomSlots)
		{
			return global::DeploymentId.NONE;
		}
		global::DeploymentScenarioSlotData deploymentScenarioSlotData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioSlotData>(this.missionSave.deployScenarioSlotIds[idx]);
		return deploymentScenarioSlotData.DeploymentId;
	}

	public global::System.Collections.Generic.List<global::DeploymentScenarioSlotId> GetMissionDeploySlots()
	{
		global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>(this.missionSave.deployScenarioMapLayoutId);
		global::System.Collections.Generic.List<global::DeploymentScenarioSlotData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioSlotData>("fk_deployment_scenario_id", ((int)deploymentScenarioMapLayoutData.DeploymentScenarioId).ToString());
		global::System.Collections.Generic.List<global::DeploymentScenarioSlotId> list2 = new global::System.Collections.Generic.List<global::DeploymentScenarioSlotId>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i].Id);
		}
		return list2;
	}

	public void SetRandomDeploySlots(global::Tyche tyche)
	{
		this.missionSave.randomSlots = true;
		global::System.Collections.Generic.List<global::DeploymentScenarioSlotId> missionDeploySlots = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.GetMissionDeploySlots();
		for (int i = 0; i < this.missionSave.deployCount; i++)
		{
			int index = tyche.Rand(0, missionDeploySlots.Count);
			this.missionSave.deployScenarioSlotIds[i] = (int)missionDeploySlots[index];
			missionDeploySlots.RemoveAt(index);
		}
	}

	public void SetDeploySlots(int pos, int idx)
	{
		this.missionSave.randomSlots = false;
		global::System.Collections.Generic.List<global::DeploymentScenarioSlotId> missionDeploySlots = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.GetMissionDeploySlots();
		this.missionSave.deployScenarioSlotIds[pos] = (int)missionDeploySlots[idx];
		idx = (idx + 1) % missionDeploySlots.Count;
		this.missionSave.deployScenarioSlotIds[(pos + 1) % this.missionSave.deployCount] = (int)missionDeploySlots[idx];
	}

	public bool HasObjectives()
	{
		return this.missionSave.objectiveTypeIds != null && this.missionSave.objectiveTypeIds.Count > 0 && this.missionSave.objectiveTypeIds[0] != 0;
	}

	public void ClearObjectives()
	{
		this.missionSave.VictoryTypeId = 1;
		for (int i = 0; i < this.missionSave.deployCount; i++)
		{
			this.missionSave.objectiveTypeIds[i] = 0;
			this.missionSave.randomObjectives[i] = false;
		}
	}

	public void RandomizeObjectives(global::Tyche tyche)
	{
		for (int i = 0; i < this.missionSave.deployCount; i++)
		{
			this.SetRandomObjective(tyche, i);
		}
	}

	public void SetObjective(int pos, int idx)
	{
		this.missionSave.randomObjectives[pos] = false;
		global::System.Collections.Generic.List<global::PrimaryObjectiveTypeId> availableObjectiveTypes = this.GetAvailableObjectiveTypes(pos);
		this.missionSave.objectiveTypeIds[pos] = (int)availableObjectiveTypes[idx];
	}

	public void SetRandomObjective(global::Tyche tyche, int pos)
	{
		this.missionSave.randomObjectives[pos] = true;
		global::System.Collections.Generic.List<global::PrimaryObjectiveTypeId> availableObjectiveTypes = this.GetAvailableObjectiveTypes(pos);
		int index = tyche.Rand(0, availableObjectiveTypes.Count);
		this.missionSave.objectiveTypeIds[pos] = (int)availableObjectiveTypes[index];
	}

	public global::System.Collections.Generic.List<global::PrimaryObjectiveTypeId> GetAvailableObjectiveTypes(int idx)
	{
		int id = this.missionSave.deployScenarioSlotIds[this.missionSave.objectiveTargets[idx]];
		global::DeploymentScenarioSlotData deploymentScenarioSlotData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioSlotData>(id);
		global::System.Collections.Generic.List<global::DeploymentJoinPrimaryObjectiveTypeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentJoinPrimaryObjectiveTypeData>("fk_deployment_id", ((int)deploymentScenarioSlotData.DeploymentId).ToString());
		return list.ConvertAll<global::PrimaryObjectiveTypeId>((global::DeploymentJoinPrimaryObjectiveTypeData x) => x.PrimaryObjectiveTypeId);
	}

	public bool IsObjectiveRandom(int idx)
	{
		return this.missionSave.randomObjectives[idx];
	}

	public global::PrimaryObjectiveTypeId GetObjectiveTypeId(int idx)
	{
		return (global::PrimaryObjectiveTypeId)this.missionSave.objectiveTypeIds[idx];
	}

	public void SetRandomRoaming(global::Tyche tyche)
	{
		global::System.Collections.Generic.List<global::UnitRoamingData> datas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRoamingData>();
		global::UnitRoamingData randomRatio = global::UnitRoamingData.GetRandomRatio(datas, tyche, null);
		this.missionSave.randomRoaming = true;
		this.missionSave.roamingUnitId = (int)randomRatio.UnitId;
	}

	public global::System.Collections.Generic.List<global::UnitId> GetRoamingUnitIds()
	{
		if (this.roamingUnitIds == null)
		{
			global::System.Collections.Generic.List<global::UnitRoamingData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRoamingData>();
			this.roamingUnitIds = list.ConvertAll<global::UnitId>((global::UnitRoamingData x) => x.UnitId);
		}
		return this.roamingUnitIds;
	}

	public void SetRoamingUnit(int idx)
	{
		this.missionSave.randomRoaming = false;
		this.missionSave.roamingUnitId = (int)this.GetRoamingUnitIds()[idx];
	}

	public static global::Mission GenerateSkirmishMission(global::MissionMapId mapId = global::MissionMapId.NONE, global::DeploymentScenarioId scenarioId = global::DeploymentScenarioId.NONE)
	{
		global::Tyche localTyche = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche;
		global::System.Collections.Generic.List<global::DeploymentScenarioMapLayoutData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>("skirmish", "1").ToDynList<global::DeploymentScenarioMapLayoutData>();
		if (mapId != global::MissionMapId.NONE)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].MissionMapId != mapId)
				{
					list.RemoveAt(i);
				}
				else if (scenarioId != global::DeploymentScenarioId.NONE && list[i].DeploymentScenarioId != scenarioId)
				{
					list.RemoveAt(i);
				}
			}
		}
		int index = localTyche.Rand(0, list.Count);
		global::DeploymentScenarioMapLayoutData scenarioMapData = list[index];
		global::MissionSave procMissionSave = global::Mission.GetProcMissionSave(localTyche, scenarioMapData);
		procMissionSave.isSkirmish = true;
		procMissionSave.randomRoaming = false;
		procMissionSave.roamingUnitId = 0;
		procMissionSave.rating = 9001;
		procMissionSave.randomMap = (mapId == global::MissionMapId.NONE);
		procMissionSave.randomLayout = true;
		procMissionSave.randomDeployment = (scenarioId == global::DeploymentScenarioId.NONE);
		procMissionSave.randomSlots = true;
		return new global::Mission(procMissionSave);
	}

	public static global::Mission GenerateProcMission(ref global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, int>> mapPositions, global::System.Collections.Generic.Dictionary<int, int>[] wyrdstoneDensityModifiers, global::System.Collections.Generic.Dictionary<int, int>[] searchDensityModifiers, global::System.Collections.Generic.Dictionary<global::ProcMissionRatingId, int> missionRatingModifiers)
	{
		global::Tyche localTyche = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche;
		global::System.Collections.Generic.List<global::DeploymentScenarioMapLayoutData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>("procedural", "1");
		int index = localTyche.Rand(0, list.Count);
		global::DeploymentScenarioMapLayoutData scenarioMapData = list[index];
		global::MissionSave procMissionSave = global::Mission.GetProcMissionSave(localTyche, scenarioMapData);
		global::System.Collections.Generic.List<global::ProcMissionRatingData> datas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingData>();
		global::ProcMissionRatingData randomRatio = global::ProcMissionRatingData.GetRandomRatio(datas, localTyche, missionRatingModifiers);
		procMissionSave.ratingId = (int)randomRatio.Id;
		global::System.Collections.Generic.List<global::ProcMissionRatingWyrdstoneDensityData> datas2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingWyrdstoneDensityData>("fk_proc_mission_rating_id", randomRatio.Id.ToIntString<global::ProcMissionRatingId>());
		global::ProcMissionRatingWyrdstoneDensityData randomRatio2 = global::ProcMissionRatingWyrdstoneDensityData.GetRandomRatio(datas2, localTyche, wyrdstoneDensityModifiers[(int)randomRatio.Id]);
		procMissionSave.wyrdDensityId = (int)randomRatio2.WyrdstoneDensityId;
		global::System.Collections.Generic.List<global::ProcMissionRatingSearchDensityData> datas3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingSearchDensityData>("fk_proc_mission_rating_id", randomRatio.Id.ToIntString<global::ProcMissionRatingId>());
		global::ProcMissionRatingSearchDensityData randomRatio3 = global::ProcMissionRatingSearchDensityData.GetRandomRatio(datas3, localTyche, searchDensityModifiers[(int)randomRatio.Id]);
		procMissionSave.searchDensityId = (int)randomRatio3.SearchDensityId;
		global::Mission mission = new global::Mission(procMissionSave);
		global::DistrictData districtData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DistrictData>((int)mission.GetDistrictId());
		bool flag = true;
		int num = 0;
		int num2 = 0;
		while (flag && num < 100)
		{
			flag = false;
			num2 = localTyche.Rand(0, districtData.Slots);
			for (int i = 0; i < mapPositions.Count; i++)
			{
				if (mapPositions[i].Key == (int)mission.GetDistrictId() && mapPositions[i].Value == num2)
				{
					flag = true;
					break;
				}
			}
			num++;
		}
		procMissionSave.mapPosition = num2;
		mapPositions.Add(new global::System.Collections.Generic.KeyValuePair<int, int>((int)mission.GetDistrictId(), num2));
		mission.RandomizeObjectives(localTyche);
		return mission;
	}

	public static global::MissionSave GetProcMissionSave(global::Tyche tyche, global::DeploymentScenarioMapLayoutData scenarioMapData)
	{
		global::DeploymentScenarioData deploymentScenarioData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioData>((int)scenarioMapData.DeploymentScenarioId);
		global::System.Collections.Generic.List<global::DeploymentScenarioSlotData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioSlotData>("fk_deployment_scenario_id", ((int)deploymentScenarioData.Id).ToConstantString()).ToDynList<global::DeploymentScenarioSlotData>();
		global::System.Collections.Generic.List<int> list2 = new global::System.Collections.Generic.List<int>();
		for (int i = list.Count - 1; i >= 0; i--)
		{
			int index = tyche.Rand(0, list.Count);
			list2.Add((int)list[index].Id);
			list.RemoveAt(index);
		}
		global::System.Collections.Generic.List<int> list3 = new global::System.Collections.Generic.List<int>();
		global::System.Collections.Generic.List<int> list4 = new global::System.Collections.Generic.List<int>();
		global::System.Collections.Generic.List<int> list5 = new global::System.Collections.Generic.List<int>();
		global::System.Collections.Generic.List<int> list6 = new global::System.Collections.Generic.List<int>();
		global::System.Collections.Generic.List<bool> list7 = new global::System.Collections.Generic.List<bool>();
		for (int j = 0; j < list2.Count; j++)
		{
			list3.Add(j);
			list4.Add(0);
			list7.Add(false);
			list6.Add(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, int.MaxValue));
			list5.Add((j + 1) % list2.Count);
		}
		global::MissionMapId missionMapId = scenarioMapData.MissionMapId;
		global::MissionMapLayoutId missionMapLayoutId = scenarioMapData.MissionMapLayoutId;
		if (missionMapLayoutId == global::MissionMapLayoutId.NONE)
		{
			global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
			string field = "fk_mission_map_id";
			int num = (int)missionMapId;
			global::System.Collections.Generic.List<global::MissionMapLayoutData> list8 = instance.InitData<global::MissionMapLayoutData>(field, num.ToString());
			int index2 = tyche.Rand(0, list8.Count);
			missionMapLayoutId = list8[index2].Id;
		}
		global::MissionMapGameplayId mapGameplayId = global::MissionMapGameplayId.NONE;
		global::DataFactory instance2 = global::PandoraSingleton<global::DataFactory>.Instance;
		string field2 = "fk_mission_map_id";
		int num2 = (int)missionMapId;
		global::System.Collections.Generic.List<global::MissionMapGameplayData> list9 = instance2.InitData<global::MissionMapGameplayData>(field2, num2.ToString());
		if (list9.Count > 0)
		{
			int index3 = tyche.Rand(0, list9.Count);
			mapGameplayId = list9[index3].Id;
		}
		global::System.Collections.Generic.List<global::WyrdstonePlacementData> datas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WyrdstonePlacementData>();
		global::WyrdstonePlacementData randomRatio = global::WyrdstonePlacementData.GetRandomRatio(datas, tyche, null);
		global::System.Collections.Generic.List<global::WyrdstoneDensityData> datas2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WyrdstoneDensityData>();
		global::WyrdstoneDensityData randomRatio2 = global::WyrdstoneDensityData.GetRandomRatio(datas2, tyche, null);
		global::System.Collections.Generic.List<global::SearchDensityData> datas3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SearchDensityData>();
		global::SearchDensityData randomRatio3 = global::SearchDensityData.GetRandomRatio(datas3, tyche, null);
		global::System.Collections.Generic.List<global::UnitRoamingData> datas4 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRoamingData>();
		global::UnitRoamingData randomRatio4 = global::UnitRoamingData.GetRandomRatio(datas4, tyche, null);
		return new global::MissionSave(global::Constant.GetFloat(global::ConstantId.ROUT_RATIO_ALIVE))
		{
			campaignId = 0,
			isCampaign = false,
			deployScenarioMapLayoutId = (int)scenarioMapData.Id,
			mapLayoutId = (int)missionMapLayoutId,
			mapGameplayId = (int)mapGameplayId,
			randomGameplay = (list9.Count > 0),
			deployCount = list2.Count,
			teams = list3,
			deployScenarioSlotIds = list2,
			VictoryTypeId = 1,
			objectiveTypeIds = list4,
			objectiveTargets = list5,
			objectiveSeeds = list6,
			randomObjectives = list7,
			wyrdPlacementId = (int)randomRatio.Id,
			wyrdDensityId = (int)randomRatio2.Id,
			searchDensityId = (int)randomRatio3.Id,
			randomRoaming = true,
			roamingUnitId = (int)((randomRatio4 == null) ? global::UnitId.NONE : randomRatio4.UnitId)
		};
	}

	public static global::Mission GenerateAmbushMission(global::Mission refMission)
	{
		global::Tyche localTyche = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche;
		global::System.Collections.Generic.List<global::DeploymentScenarioMapLayoutData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>("ambush", "1");
		int index = localTyche.Rand(0, list.Count);
		global::DeploymentScenarioMapLayoutData scenarioMapData = list[index];
		global::MissionSave procMissionSave = global::Mission.GetProcMissionSave(localTyche, scenarioMapData);
		procMissionSave.rating = refMission.missionSave.rating;
		procMissionSave.ratingId = refMission.missionSave.ratingId;
		global::Mission mission = new global::Mission(procMissionSave);
		mission.RandomizeObjectives(localTyche);
		mission.missionSave.VictoryTypeId = refMission.missionSave.VictoryTypeId;
		mission.missionSave.wyrdPlacementId = refMission.missionSave.wyrdPlacementId;
		mission.missionSave.wyrdDensityId = refMission.missionSave.wyrdDensityId;
		mission.missionSave.searchDensityId = refMission.missionSave.searchDensityId;
		mission.missionSave.ratingId = refMission.missionSave.ratingId;
		return mission;
	}

	public static global::System.Collections.IEnumerator GetProcWarband(int rating, int warRank, int warUnitsCount, bool impressive, global::WarbandData warData, int heroesCount, int highestUnitRank, global::System.Action<global::WarbandSave> callback)
	{
		global::Tyche tyche = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche;
		int ratingPool = 0;
		global::PandoraDebug.LogInfo("Generating Procedural Warband using a rating of : " + rating, "MISSION", null);
		global::WarbandSave warSave = new global::WarbandSave(warData.Id);
		global::System.Collections.Generic.List<global::WarbandNameData> warNamesData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandNameData>("fk_warband_id", ((int)warData.Id).ToString());
		warSave.name = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(warNamesData[tyche.Rand(0, warNamesData.Count)].Name);
		warSave.rank = warRank;
		global::System.Collections.Generic.List<global::ProcWarbandRankData> procWarRanksData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcWarbandRankData>("fk_warband_rank_id", warRank.ToString());
		global::ProcWarbandRankData procWarRankData = null;
		procWarRankData = procWarRanksData[0];
		if (procWarRankData.Id == global::ProcWarbandRankId.NONE)
		{
			procWarRankData = procWarRanksData[1];
		}
		string procWarbandRank = ((int)procWarRankData.Id).ToString();
		global::System.Collections.Generic.List<global::ColorPresetData> presetsData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ColorPresetData>("fk_warband_id", ((int)warData.Id).ToString());
		global::ColorPresetData colorPresetData = presetsData[tyche.Rand(0, presetsData.Count)];
		int offsetPreset = (int)((int)colorPresetData.Id << 8);
		global::System.Collections.Generic.List<global::AttributeData> attributeDataList = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AttributeData>();
		global::System.Collections.Generic.List<global::ProcWarbandRankJoinUnitTypeData> unitTypesData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcWarbandRankJoinUnitTypeData>("fk_proc_warband_rank_id", procWarbandRank);
		global::System.Collections.Generic.List<global::HireUnitInjuryData> injuriesData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::HireUnitInjuryData>("unit_rank", highestUnitRank.ToString());
		unitTypesData.Sort(new global::ProcWarbandRankJoinUnitTypeDataSorter());
		global::System.Collections.Generic.List<global::UnitRating> unitRatings = new global::System.Collections.Generic.List<global::UnitRating>();
		global::System.Collections.Generic.List<global::UnitData> warbandUnitsData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>("fk_warband_id", ((int)warData.Id).ToString(), "released", "1");
		global::UnitRating unitRating = new global::UnitRating();
		unitRatings.Add(unitRating);
		yield return global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(global::Mission.AddProcUnit(unitRating, tyche, warbandUnitsData, global::UnitTypeId.LEADER, offsetPreset, injuriesData));
		ratingPool += unitRating.rating;
		global::System.Collections.Generic.List<global::UnitTypeId> heroesTypes = new global::System.Collections.Generic.List<global::UnitTypeId>();
		for (int i = 0; i < unitTypesData.Count; i++)
		{
			switch (unitTypesData[i].UnitTypeId)
			{
			case global::UnitTypeId.HERO_1:
			case global::UnitTypeId.HERO_2:
			case global::UnitTypeId.HERO_3:
				for (int j = 0; j < unitTypesData[i].MaxCount; j++)
				{
					heroesTypes.Add(unitTypesData[i].UnitTypeId);
				}
				break;
			}
		}
		if (impressive)
		{
			unitRating = new global::UnitRating();
			unitRatings.Add(unitRating);
			yield return global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(global::Mission.AddProcUnit(unitRating, tyche, warbandUnitsData, global::UnitTypeId.IMPRESSIVE, offsetPreset, injuriesData));
			ratingPool += unitRating.rating;
		}
		for (int k = 0; k < heroesCount; k++)
		{
			unitRating = new global::UnitRating();
			unitRatings.Add(unitRating);
			yield return global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(global::Mission.AddProcUnit(unitRating, tyche, warbandUnitsData, heroesTypes, offsetPreset, injuriesData));
			ratingPool += unitRating.rating;
		}
		while (unitRatings.Count < warUnitsCount)
		{
			unitRating = new global::UnitRating();
			unitRatings.Add(unitRating);
			yield return global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(global::Mission.AddProcUnit(unitRating, tyche, warbandUnitsData, global::UnitTypeId.HENCHMEN, offsetPreset, injuriesData));
			ratingPool += unitRating.rating;
		}
		int l = 0;
		while (l < unitRatings.Count && ratingPool < rating)
		{
			global::Unit unit = unitRatings[l].unit;
			int garbageRating = 0;
			global::UnitFactory.AddArmorStyleSet(tyche, ref garbageRating, unit, global::ItemQualityId.NORMAL, true, true, false, false, null);
			int newRating = unit.GetRating();
			ratingPool += newRating - unitRatings[l].rating;
			unitRatings[l].rating = newRating;
			l++;
		}
		bool hasChanges = true;
		while (ratingPool < rating && hasChanges)
		{
			hasChanges = false;
			int m = 0;
			while (m < unitRatings.Count && ratingPool < rating)
			{
				hasChanges |= global::Mission.AdvanceUnit(tyche, unitRatings[m], highestUnitRank, attributeDataList, rating, ref ratingPool, false);
				m++;
			}
			yield return null;
		}
		global::ItemQualityId maxQualityId = global::ItemQualityId.NORMAL;
		if (ratingPool < rating)
		{
			global::System.Collections.Generic.List<global::ProcWarbandRankJoinItemQualityData> procItemQualitiesData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcWarbandRankJoinItemQualityData>("fk_proc_warband_rank_id", procWarbandRank);
			for (int p = 0; p < procItemQualitiesData.Count; p++)
			{
				global::ProcWarbandRankJoinItemQualityData warRankItemQualityData = procItemQualitiesData[p];
				maxQualityId = ((warRankItemQualityData.ItemQualityId <= maxQualityId) ? maxQualityId : warRankItemQualityData.ItemQualityId);
				int counter = tyche.Rand(warRankItemQualityData.MinCount, warRankItemQualityData.MaxCount + 1);
				for (int n = 0; n < unitRatings.Count; n++)
				{
					global::UnitFactory.BoostItemsQuality(tyche, unitRatings[n].unit, warRankItemQualityData.ItemQualityId, ref ratingPool, ref counter, rating);
				}
			}
		}
		bool added = true;
		while (added)
		{
			int i2 = 0;
			while (added && i2 < unitRatings.Count)
			{
				added = false;
				if (ratingPool < rating && !unitRatings[i2].unit.IsInventoryFull())
				{
					global::Item cons = global::UnitFactory.GetProcItem(tyche, ref ratingPool, unitRatings[i2].unit, global::UnitSlotId.ITEM_1, (tyche.Rand(0, 2) != 0) ? global::ItemTypeId.CONSUMABLE_POTIONS : global::ItemTypeId.CONSUMABLE_MISC, maxQualityId, false, false);
					ratingPool += cons.GetRating();
					global::UnitSlotId slotId;
					unitRatings[i2].unit.GetEmptyItemSlot(out slotId, cons);
					unitRatings[i2].unit.EquipItem(slotId, cons, true);
					added = true;
				}
				i2++;
			}
		}
		hasChanges = true;
		while (ratingPool < rating && hasChanges)
		{
			hasChanges = false;
			int k2 = 0;
			while (k2 < unitRatings.Count && ratingPool < rating)
			{
				global::ProcWarbandRankJoinUnitTypeData warRankUnitTypeData = global::Mission.GetWarbandRankUnitType(unitTypesData, unitRatings[k2].unit.GetUnitTypeId());
				hasChanges |= global::Mission.AdvanceUnit(tyche, unitRatings[k2], warRankUnitTypeData.MaxRank, attributeDataList, rating, ref ratingPool, false);
				k2++;
			}
			yield return null;
		}
		for (int i3 = 0; i3 < unitRatings.Count; i3++)
		{
			warSave.units.Add(unitRatings[i3].unit.UnitSave);
		}
		if (callback != null)
		{
			callback(warSave);
		}
		yield break;
	}

	public static bool AdvanceUnit(global::Tyche tyche, global::UnitRating unitRating, int maxRank, global::System.Collections.Generic.List<global::AttributeData> attributeDataList, int ratingCheck, ref int ratingPool, bool full = false)
	{
		bool result = false;
		global::System.Collections.Generic.List<global::UnitJoinUnitRankData> list = new global::System.Collections.Generic.List<global::UnitJoinUnitRankData>();
		global::System.Collections.Generic.List<global::Mutation> list2 = new global::System.Collections.Generic.List<global::Mutation>();
		global::System.Collections.Generic.List<global::Item> previousItems = new global::System.Collections.Generic.List<global::Item>();
		global::Unit unit = unitRating.unit;
		if (unit.Rank < maxRank)
		{
			unit.AddXp(99999, list, list2, previousItems, 0, (!full) ? (unit.Rank + 1) : maxRank);
			if (list.Count > 0)
			{
				result = true;
			}
			if (list2.Count > 0)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					if (list2[i].GroupData.UnitSlotId == global::UnitSlotId.SET1_MAINHAND || list2[i].GroupData.UnitSlotId == global::UnitSlotId.SET1_OFFHAND)
					{
						global::Mission.GenerateUnitWeapons(tyche, unit);
					}
				}
			}
			int num = 0;
			global::UnitFactory.RaiseAttributes(tyche, attributeDataList, unit, ref num, ratingCheck, unitRating.baseAttributes, unitRating.maxAttributes);
			unitRating.UpdateBaseAttributes();
			global::UnitFactory.AddSkillSpells(tyche, unit, ref num, ratingCheck, unitRating.skillsData, unitRating.baseAttributes);
			unitRating.UpdateBaseAttributes();
			int rating = unit.GetRating();
			ratingPool += rating - unitRating.rating;
			unitRating.rating = rating;
		}
		return result;
	}

	private static global::ProcWarbandRankJoinUnitTypeData GetWarbandRankUnitType(global::System.Collections.Generic.List<global::ProcWarbandRankJoinUnitTypeData> warbandRankJoinUnitTypeDatas, global::UnitTypeId unitTypeId)
	{
		for (int i = 0; i < warbandRankJoinUnitTypeDatas.Count; i++)
		{
			if (warbandRankJoinUnitTypeDatas[i].UnitTypeId == unitTypeId)
			{
				return warbandRankJoinUnitTypeDatas[i];
			}
		}
		return null;
	}

	private static global::System.Collections.IEnumerator AddProcUnit(global::UnitRating unitRating, global::Tyche tyche, global::System.Collections.Generic.List<global::UnitData> warbandUnitsData, global::System.Collections.Generic.List<global::UnitTypeId> unitTypeId, int offsetPreset, global::System.Collections.Generic.List<global::HireUnitInjuryData> injuriesData)
	{
		int index = tyche.Rand(0, unitTypeId.Count);
		global::UnitTypeId typeId = unitTypeId[index];
		unitTypeId.RemoveAt(index);
		yield return global::Mission.AddProcUnit(unitRating, tyche, warbandUnitsData, typeId, offsetPreset, injuriesData);
		yield break;
	}

	public static global::System.Collections.IEnumerator AddProcUnit(global::UnitRating unitRating, global::Tyche tyche, global::System.Collections.Generic.List<global::UnitData> warbandUnitsData, global::UnitTypeId unitTypeId, int offsetPreset, global::System.Collections.Generic.List<global::HireUnitInjuryData> injuriesData)
	{
		global::Mission.tempList.Clear();
		for (int i = 0; i < warbandUnitsData.Count; i++)
		{
			if (warbandUnitsData[i].UnitTypeId == unitTypeId)
			{
				global::Mission.tempList.Add(warbandUnitsData[i]);
			}
		}
		global::UnitData unitData = (global::Mission.tempList.Count != 1) ? global::Mission.tempList[tyche.Rand(0, global::Mission.tempList.Count)] : global::Mission.tempList[0];
		global::Unit unit = new global::Unit(new global::UnitSave(unitData.Id));
		yield return null;
		global::System.Collections.Generic.List<global::InjuryId> excludesInjuryIds = new global::System.Collections.Generic.List<global::InjuryId>(global::Unit.HIRE_UNIT_INJURY_EXCLUDES);
		global::HireUnitInjuryData injuryData = global::HireUnitInjuryData.GetRandomRatio(injuriesData, tyche, null);
		for (int j = 0; j < injuryData.Count; j++)
		{
			global::InjuryData newInjury = global::PandoraSingleton<global::HideoutManager>.Instance.Progressor.RollInjury(excludesInjuryIds, unit);
			if (newInjury == null)
			{
				break;
			}
			unit.AddInjury(newInjury, global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, global::Mission.tempRemovedItems, true, -1);
			global::Mission.tempRemovedItems.Clear();
			excludesInjuryIds.Add(newInjury.Id);
		}
		foreach (global::BodyPartId bodyPartId in unit.bodyParts.Keys)
		{
			global::System.Collections.Generic.KeyValuePair<int, int> kv = unit.UnitSave.customParts[bodyPartId];
			unit.UnitSave.customParts[bodyPartId] = new global::System.Collections.Generic.KeyValuePair<int, int>(kv.Key, offsetPreset);
		}
		global::Mission.tempList.Clear();
		global::Mission.GenerateUnitWeapons(tyche, unit);
		unitRating.unit = unit;
		unitRating.rating = unit.GetRating();
		unitRating.skillsData = global::UnitFactory.GetLearnableSkills(unit);
		unitRating.UpdateMaxAttributes();
		unitRating.UpdateBaseAttributes();
		yield break;
	}

	private static void GenerateUnitWeapons(global::Tyche tyche, global::Unit unit)
	{
		int num = 0;
		global::CombatStyleId excludedCombatStyleId = global::UnitFactory.AddCombatStyleSet(tyche, ref num, unit, global::UnitSlotId.SET1_MAINHAND, global::CombatStyleId.NONE, global::ItemQualityId.NORMAL, false, null);
		global::UnitFactory.AddCombatStyleSet(tyche, ref num, unit, global::UnitSlotId.SET2_MAINHAND, excludedCombatStyleId, global::ItemQualityId.NORMAL, false, null);
	}

	public static global::Mission GenerateCampaignMission(global::WarbandId warbandId, int index)
	{
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string[] fields = new string[]
		{
			"fk_warband_id",
			"idx"
		};
		string[] array = new string[2];
		int num = 0;
		int num2 = (int)warbandId;
		array[num] = num2.ToString();
		array[1] = index.ToString();
		global::System.Collections.Generic.List<global::CampaignMissionData> list = instance.InitData<global::CampaignMissionData>(fields, array);
		if (list.Count == 0)
		{
			return null;
		}
		global::CampaignMissionData campaignMissionData = list[0];
		global::System.Collections.Generic.List<global::DeploymentScenarioMapLayoutData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>("fk_deployment_scenario_id", ((int)campaignMissionData.DeploymentScenarioId).ToString());
		global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData = list2[0];
		global::System.Collections.Generic.List<global::CampaignMissionJoinCampaignWarbandData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionJoinCampaignWarbandData>("fk_campaign_mission_id", ((int)campaignMissionData.Id).ToString());
		global::System.Collections.Generic.List<int> list4 = new global::System.Collections.Generic.List<int>();
		global::System.Collections.Generic.List<int> list5 = new global::System.Collections.Generic.List<int>();
		global::System.Collections.Generic.List<int> list6 = new global::System.Collections.Generic.List<int>();
		global::System.Collections.Generic.List<int> list7 = new global::System.Collections.Generic.List<int>();
		global::System.Collections.Generic.List<bool> list8 = new global::System.Collections.Generic.List<bool>();
		global::System.Collections.Generic.List<int> list9 = new global::System.Collections.Generic.List<int>();
		for (int i = 0; i < list3.Count; i++)
		{
			list5.Add((list3[i].CampaignWarbandId != global::CampaignWarbandId.NONE) ? 25 : 24);
			list4.Add(list3[i].Team);
			list6.Add(0);
			list7.Add(0);
			list8.Add(false);
			list9.Add(0);
		}
		global::MissionSave missionSave = new global::MissionSave(global::Constant.GetFloat(global::ConstantId.ROUT_RATIO_ALIVE));
		missionSave.mapPosition = campaignMissionData.MapPos;
		missionSave.rating = campaignMissionData.Rating;
		missionSave.campaignId = (int)campaignMissionData.Id;
		missionSave.isCampaign = true;
		missionSave.isTuto = campaignMissionData.IsTuto;
		missionSave.deployScenarioMapLayoutId = (int)deploymentScenarioMapLayoutData.Id;
		missionSave.mapLayoutId = (int)deploymentScenarioMapLayoutData.MissionMapLayoutId;
		missionSave.deployCount = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionJoinCampaignWarbandData>("fk_campaign_mission_id", campaignMissionData.Id.ToIntString<global::CampaignMissionId>()).Count;
		missionSave.teams = list4;
		missionSave.deployScenarioSlotIds = list5;
		missionSave.VictoryTypeId = ((!missionSave.isTuto) ? 2 : 1);
		missionSave.objectiveTypeIds = list6;
		missionSave.objectiveTargets = list7;
		missionSave.objectiveSeeds = list9;
		missionSave.randomObjectives = list8;
		missionSave.wyrdPlacementId = (int)campaignMissionData.WyrdstonePlacementId;
		missionSave.wyrdDensityId = (int)campaignMissionData.WyrdstoneDensityId;
		missionSave.searchDensityId = (int)campaignMissionData.SearchDensityId;
		return new global::Mission(missionSave);
	}

	public static global::WarbandSave GetCampaignWarband(global::CampaignMissionJoinCampaignWarbandData warbandData)
	{
		global::System.Collections.Generic.List<global::AttributeData> attributesData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AttributeData>();
		global::CampaignWarbandData campaignWarbandData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignWarbandData>((int)warbandData.CampaignWarbandId);
		global::WarbandSave warbandSave = new global::WarbandSave(campaignWarbandData.WarbandId);
		warbandSave.campaignId = (int)warbandData.CampaignWarbandId;
		warbandSave.rank = campaignWarbandData.Rank;
		global::System.Collections.Generic.List<global::CampaignWarbandJoinCampaignUnitData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignWarbandJoinCampaignUnitData>("fk_campaign_warband_id", ((int)warbandData.CampaignWarbandId).ToString());
		global::System.Collections.Generic.List<global::UnitSave> list2 = new global::System.Collections.Generic.List<global::UnitSave>();
		for (int i = 0; i < list.Count; i++)
		{
			global::CampaignUnitData campaignUnitData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignUnitData>((int)list[i].CampaignUnitId);
			global::UnitData unitData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>((int)campaignUnitData.UnitId);
			global::UnitSave unitSave = new global::UnitSave(campaignUnitData.UnitId);
			unitSave.campaignId = (int)campaignUnitData.Id;
			if (!string.IsNullOrEmpty(campaignUnitData.FirstName))
			{
				unitSave.stats.name = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(campaignUnitData.FirstName);
			}
			else if (!string.IsNullOrEmpty(unitData.FirstName))
			{
				unitSave.stats.name = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(unitData.FirstName);
			}
			global::System.Collections.Generic.List<global::UnitRankData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>("rank", campaignUnitData.Rank.ToString());
			unitSave.rankId = list3[0].Id;
			global::System.Collections.Generic.List<global::CampaignUnitJoinSkillData> list4 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignUnitJoinSkillData>("fk_campaign_unit_id", campaignUnitData.Id.ToIntString<global::CampaignUnitId>());
			for (int j = 0; j < list4.Count; j++)
			{
				global::SkillData skillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)list4[j].SkillId);
				if (skillData.SkillTypeId == global::SkillTypeId.SKILL_ACTION)
				{
					if (skillData.Passive)
					{
						unitSave.passiveSkills.Add(skillData.Id);
					}
					else
					{
						unitSave.activeSkills.Add(skillData.Id);
					}
				}
				else if (skillData.SkillTypeId == global::SkillTypeId.SPELL_ACTION)
				{
					unitSave.spells.Add(skillData.Id);
				}
			}
			global::System.Collections.Generic.List<global::CampaignUnitJoinMutationData> list5 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignUnitJoinMutationData>("fk_campaign_unit_id", campaignUnitData.Id.ToIntString<global::CampaignUnitId>());
			for (int k = 0; k < list5.Count; k++)
			{
				unitSave.mutations.Add(list5[k].MutationId);
			}
			global::Unit unit = new global::Unit(unitSave);
			int num = 0;
			int ratingMax = 999999;
			global::UnitFactory.RaiseAttributes(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, attributesData, unit, ref num, ratingMax);
			global::UnitFactory.AddSkillSpells(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, unit, ref num, ratingMax);
			list2.Add(unitSave);
		}
		warbandSave.units = list2;
		return warbandSave;
	}

	public global::MissionSave missionSave;

	private global::System.Collections.Generic.List<global::UnitId> roamingUnitIds;

	private static readonly global::System.Collections.Generic.List<global::UnitData> tempList = new global::System.Collections.Generic.List<global::UnitData>();

	private static readonly global::System.Collections.Generic.List<global::Item> tempRemovedItems = new global::System.Collections.Generic.List<global::Item>();
}
