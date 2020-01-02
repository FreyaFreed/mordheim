using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionStarter : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		if (!global::PandoraSingleton<global::MissionStartData>.Exists())
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("mission_start_data");
			gameObject.AddComponent<global::MissionStartData>();
		}
	}

	private void Update()
	{
		if (!global::PandoraSingleton<global::MissionLoader>.Exists())
		{
			return;
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.ResetSeed();
		global::MissionSave missionSave = new global::MissionSave(global::Constant.GetFloat(global::ConstantId.ROUT_RATIO_ALIVE));
		missionSave.campaignId = 0;
		missionSave.isCampaign = false;
		missionSave.mapPosition = 0;
		missionSave.rating = 0;
		missionSave.deployScenarioMapLayoutId = (int)this.scenarioMapLayoutId;
		missionSave.mapGameplayId = (int)this.gameplayId;
		missionSave.mapLayoutId = (int)this.mapLayoutId;
		missionSave.deployCount = this.warbands.Count;
		missionSave.teams = new global::System.Collections.Generic.List<int>();
		missionSave.deployScenarioSlotIds = new global::System.Collections.Generic.List<int>();
		missionSave.VictoryTypeId = (int)this.victoryTypeId;
		missionSave.objectiveTypeIds = new global::System.Collections.Generic.List<int>();
		missionSave.objectiveTargets = new global::System.Collections.Generic.List<int>();
		missionSave.objectiveSeeds = new global::System.Collections.Generic.List<int>();
		missionSave.turnTimer = this.turnTimer;
		missionSave.deployTimer = this.deployTimer;
		missionSave.beaconLimit = this.beaconLimit;
		missionSave.wyrdPlacementId = (int)this.wyrdPlacementId;
		missionSave.wyrdDensityId = (int)this.wyrdDensityId;
		missionSave.searchDensityId = (int)this.searchDensityId;
		missionSave.autoDeploy = this.autoDeploy;
		missionSave.roamingUnitId = (int)this.roamingUnitId;
		missionSave.ratingId = (int)this.missionRating;
		for (int i = 0; i < this.warbands.Count; i++)
		{
			missionSave.teams.Add(this.warbands[i].team);
			missionSave.deployScenarioSlotIds.Add((int)this.warbands[i].deployId);
			missionSave.objectiveTypeIds.Add((int)this.warbands[i].objectiveTypeId);
			missionSave.objectiveTargets.Add(this.warbands[i].objectiveTargetIdx);
			missionSave.objectiveSeeds.Add(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.RandInt());
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.SetMission(new global::Mission(missionSave));
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_deployment_scenario_id";
		int num = (int)this.deployScenarioId;
		global::System.Collections.Generic.List<global::DeploymentScenarioSlotData> list = instance.InitData<global::DeploymentScenarioSlotData>(field, num.ToString()).ToDynList<global::DeploymentScenarioSlotData>();
		global::System.Collections.Generic.List<int> list2 = new global::System.Collections.Generic.List<int>();
		for (int j = list.Count - 1; j >= 0; j--)
		{
			global::Tyche tyche = new global::Tyche((int)(global::UnityEngine.Random.value * 2.14748365E+09f), true);
			int index = tyche.Rand(0, list.Count);
			list2.Add((int)list[index].Id);
			list.RemoveAt(index);
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.deployScenarioSlotIds = list2;
		global::System.Collections.Generic.List<int> list3 = new global::System.Collections.Generic.List<int>();
		global::System.Collections.Generic.List<int> list4 = new global::System.Collections.Generic.List<int>();
		if (this.warbands != null && this.warbands.Count > 0)
		{
			int num2 = 0;
			foreach (global::WarbandInitData warbandInitData in this.warbands)
			{
				global::WarbandSave warbandSave = new global::WarbandSave(warbandInitData.id);
				warbandSave.name = warbandInitData.name;
				global::WarbandData warbandData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>((int)warbandInitData.id);
				foreach (global::UnitInitData unitInitData in warbandInitData.units)
				{
					global::UnitSave unitSave = new global::UnitSave(unitInitData.id);
					unitSave.stats.name = unitInitData.name;
					unitSave.rankId = unitInitData.rank;
					foreach (global::WeaponInitData weaponInitData in unitInitData.weapons)
					{
						unitSave.items[(int)weaponInitData.slotId] = new global::ItemSave(weaponInitData.itemId, (weaponInitData.qualityId != global::ItemQualityId.NONE) ? weaponInitData.qualityId : global::ItemQualityId.NORMAL, weaponInitData.runeMarkId, weaponInitData.runeMarkQualityId, warbandData.AllegianceId, 1);
					}
					foreach (global::SkillId id in unitInitData.skills)
					{
						global::SkillData skillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)id);
						if (skillData.Passive)
						{
							unitSave.passiveSkills.Add(skillData.Id);
						}
						else
						{
							unitSave.activeSkills.Add(skillData.Id);
						}
					}
					unitSave.spells = unitInitData.spells;
					foreach (global::MutationId item in unitInitData.mutations)
					{
						unitSave.mutations.Add(item);
					}
					unitSave.injuries = unitInitData.injuries;
					warbandSave.units.Add(unitSave);
				}
				global::WarbandMenuController warbandMenuController = new global::WarbandMenuController(warbandSave);
				global::PandoraSingleton<global::MissionStartData>.Instance.AddFightingWarband((global::WarbandId)warbandMenuController.Warband.GetWarbandSave().id, global::CampaignWarbandId.NONE, warbandMenuController.Warband.GetWarbandSave().name, warbandMenuController.Warband.GetWarbandSave().overrideName, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_ai"), warbandInitData.rank, 0, global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex, warbandInitData.playerId, warbandMenuController.GetActiveUnitsSerialized());
				list3.Add((int)warbandInitData.objectiveTypeId);
				list4.Add(warbandInitData.objectiveTargetIdx);
				num2++;
			}
		}
		if (this.ground != null)
		{
			global::PandoraSingleton<global::MissionLoader>.Instance.ground = this.ground;
			global::PandoraSingleton<global::MissionLoader>.Instance.trapCount = this.trapCount;
		}
		global::PandoraSingleton<global::TransitionManager>.Instance.noTransition = true;
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	public global::UnityEngine.GameObject ground;

	public int seed;

	public global::DeploymentScenarioMapLayoutId scenarioMapLayoutId;

	public global::MissionMapLayoutId mapLayoutId;

	public global::DeploymentScenarioId deployScenarioId;

	public global::MissionMapGameplayId gameplayId;

	public int turnTimer;

	public int deployTimer;

	public int beaconLimit;

	public global::WyrdstonePlacementId wyrdPlacementId;

	public global::WyrdstoneDensityId wyrdDensityId;

	public global::SearchDensityId searchDensityId;

	public int trapCount;

	public global::VictoryTypeId victoryTypeId;

	public bool autoDeploy;

	public global::UnitId roamingUnitId;

	public global::ProcMissionRatingId missionRating;

	public global::System.Collections.Generic.List<global::WarbandInitData> warbands;
}
