using System;
using System.Collections.Generic;
using UnityEngine;

public class WarbandController
{
	public WarbandController(global::MissionWarbandSave warband, global::DeploymentId deployId, int index, int teamIndex, global::PrimaryObjectiveTypeId objId, int objTargetIndex, int objSeed)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"PlayerIndex = ",
			warband.PlayerIndex,
			" index = ",
			index
		}), "LOADING", null);
		this.WarData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>((int)warband.WarbandId);
		this.CampWarData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignWarbandData>((int)warband.CampaignWarId);
		this.name = warband.Name;
		this.Rank = warband.Rank;
		this.Rating = warband.Rating;
		this.saveIdx = index;
		this.idx = index;
		this.teamIdx = teamIndex;
		this.playerIdx = warband.PlayerIndex;
		this.playerTypeId = warband.PlayerTypeId;
		this.objectiveTypeId = objId;
		this.objectiveTargetIdx = objTargetIndex;
		this.objectiveSeed = objSeed;
		this.deploymentId = deployId;
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"WarbandController idx = ",
			this.idx,
			" playerIdx = ",
			this.playerIdx,
			" deployId = ",
			deployId,
			" objectiveId = ",
			this.objectiveTypeId
		}), "WARBAND", null);
		this.unitCtrlrs = new global::System.Collections.Generic.List<global::UnitController>();
		global::WarbandRankData warbandRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankData>(warband.Rank);
		this.moralValue = warbandRankData.Moral;
		this.MaxMoralValue = 0;
		this.canRout = true;
		this.defeated = false;
		this.idolMoralRemoved = false;
		if (this.playerTypeId == global::PlayerTypeId.AI)
		{
			this.SquadManager = new global::SquadManager(this);
		}
	}

	public global::CampaignWarbandId CampaignWarbandId
	{
		get
		{
			return this.CampWarData.Id;
		}
	}

	public bool NeedWagon
	{
		get
		{
			return !string.IsNullOrEmpty(this.WarData.Wagon) && !this.CampWarData.NoWagon;
		}
	}

	public global::WarbandData WarData { get; private set; }

	public global::CampaignWarbandData CampWarData { get; private set; }

	public global::Item ItemIdol { get; set; }

	public int Rank { get; private set; }

	public int Rating { get; private set; }

	public int MaxMoralValue { get; private set; }

	public int BlackList { get; private set; }

	public bool AllObjectivesCompleted { get; private set; }

	public float MoralRatio
	{
		get
		{
			return global::UnityEngine.Mathf.Clamp01((float)this.moralValue / (float)this.MaxMoralValue);
		}
	}

	public global::SquadManager SquadManager { get; private set; }

	public global::UnityEngine.GameObject Beacon { get; private set; }

	public int MoralValue
	{
		get
		{
			return this.moralValue;
		}
		set
		{
			if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto || this.playerTypeId == global::PlayerTypeId.PLAYER)
			{
				this.moralValue = global::UnityEngine.Mathf.Max(value, 0);
				global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateMoral(this.idx, this.moralValue, this.OldMoralValue, this.idolMoralRemoved);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.WARBAND_MORALE_CHANGED);
			}
		}
	}

	public void Destroy()
	{
		for (int i = 0; i < this.unitCtrlrs.Count; i++)
		{
			global::UnityEngine.Object.Destroy(this.unitCtrlrs[i].gameObject);
		}
		this.unitCtrlrs.Clear();
	}

	public void GenerateUnit(global::UnitSave unitSave, global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation, bool merge = true)
	{
		global::PandoraDebug.LogInfo("Instantiate UnitControllers: " + (global::UnitId)unitSave.stats.id, "WARBAND", null);
		this.unitCtrlrs.Add(null);
		global::PandoraSingleton<global::UnitFactory>.Instance.GenerateUnit(this, this.unitCtrlrs.Count - 1, unitSave, position, rotation, merge);
		global::UnitData unitData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>(unitSave.stats.id);
		if (unitData.UnitIdDeathSpawn != global::UnitId.NONE)
		{
			for (int i = 0; i < unitData.DeathSpawnCount; i++)
			{
				global::UnitSave unitSave2 = new global::UnitSave();
				global::Thoth.Copy(unitSave, unitSave2);
				unitSave2.stats.id = (int)unitData.UnitIdDeathSpawn;
				unitSave2.stats.name = string.Empty;
				this.GenerateUnit(unitSave2, global::UnityEngine.Vector3.one * 100f, global::UnityEngine.Quaternion.identity, true);
			}
		}
	}

	public global::UnitController GetLeader()
	{
		return this.unitCtrlrs.Find((global::UnitController x) => x.unit.IsLeader);
	}

	public global::UnitController GetAliveLeader()
	{
		global::UnitController unitController = null;
		for (int i = 0; i < this.unitCtrlrs.Count; i++)
		{
			global::UnitController unitController2 = this.unitCtrlrs[i];
			if (unitController2.unit.IsAvailable())
			{
				if (unitController2.unit.IsLeader)
				{
					return unitController2;
				}
				if ((unitController2.unit.GetUnitTypeId() == global::UnitTypeId.HERO_1 || unitController2.unit.GetUnitTypeId() == global::UnitTypeId.HERO_2 || unitController2.unit.GetUnitTypeId() == global::UnitTypeId.HERO_3) && (unitController == null || unitController2.unit.WarbandRoutRoll > unitController.unit.WarbandRoutRoll))
				{
					unitController = unitController2;
				}
			}
		}
		return unitController;
	}

	public global::UnitController GetAliveHighestLeaderShip()
	{
		global::UnitController unitController = null;
		for (int i = 0; i < this.unitCtrlrs.Count; i++)
		{
			if (this.unitCtrlrs[i].unit.IsAvailable() && (unitController == null || unitController.unit.Leadership < this.unitCtrlrs[i].unit.Leadership))
			{
				unitController = this.unitCtrlrs[i];
			}
		}
		return unitController;
	}

	public void SetWagon(global::UnityEngine.GameObject cart)
	{
		this.wagon = cart.GetComponent<global::WarbandWagon>();
		bool flag = this.playerIdx == global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex && this.playerTypeId == global::PlayerTypeId.PLAYER;
		this.wagon.mapImprint = cart.AddComponent<global::MapImprint>();
		this.wagon.mapImprint.Init("wagon/" + this.WarData.Id.ToLowerString(), null, true, (!flag) ? global::MapImprintType.ENEMY_WAGON : global::MapImprintType.PLAYER_WAGON, null, null, null, null, null);
		this.wagon.mapImprint.Wagon = this.wagon;
		this.wagon.idol.imprintIcon = null;
		this.wagon.idol.warbandController = this;
		this.wagon.idol.loc_name = "search_idol";
		this.wagon.idol.Init(global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID());
		if (this.wagon.idol.items.Count > 0)
		{
			this.ItemIdol = this.wagon.idol.items[0];
			this.ItemIdol.Save.ownerMyrtilus = (uint)(200000000 + this.idx);
		}
		this.AddMoralIdol();
		this.wagon.idol.AddIdolImprint(this.wagon.idol.items[0]);
		global::Shrine component = this.wagon.idol.GetComponent<global::Shrine>();
		component.imprintIcon = null;
		component.Init(global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID());
		global::InteractiveRestriction interactiveRestriction = new global::InteractiveRestriction();
		interactiveRestriction.teamIdx = this.teamIdx;
		interactiveRestriction.warbandId = this.WarData.Id;
		component.restrictions.Add(interactiveRestriction);
		this.wagon.chest.imprintIcon = null;
		this.wagon.chest.warbandController = this;
		this.wagon.chest.loc_name = "search_chest";
		int cartSize = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankData>("rank", this.Rank.ToConstantString())[0].CartSize;
		this.wagon.chest.Init(global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID(), cartSize, false);
		string str = (global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr() != this) ? "enemy" : "own";
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/beacon/", global::AssetBundleId.FX, "map_beacon_" + str + ".prefab", delegate(global::UnityEngine.Object beaconPrefab)
		{
			this.Beacon = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)beaconPrefab);
			this.Beacon.transform.position = this.wagon.transform.position;
			this.Beacon.transform.rotation = global::UnityEngine.Quaternion.identity;
			this.Beacon.SetActive(global::PandoraSingleton<global::GameManager>.Instance.WagonBeaconsEnabled);
		});
		global::CampaignMissionId campaignId = (global::CampaignMissionId)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.campaignId;
		if (campaignId != global::CampaignMissionId.NONE)
		{
			global::System.Collections.Generic.List<global::CampaignMissionItemData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionItemData>(new string[]
			{
				"fk_campaign_mission_id",
				"fk_campaign_warband_id"
			}, new string[]
			{
				((int)campaignId).ToConstantString(),
				((int)this.CampaignWarbandId).ToConstantString()
			});
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list[i].Amount; j++)
				{
					this.wagon.chest.AddItem(list[i].ItemId, list[i].ItemQualityId, list[i].RuneMarkId, list[i].RuneMarkQualityId, list[i].AllegianceId, true);
				}
			}
		}
	}

	public bool IsAmbusher()
	{
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.IsAmbush())
		{
			switch (this.deploymentId)
			{
			case global::DeploymentId.SCOUTING:
			case global::DeploymentId.AMBUSHER:
				return true;
			}
		}
		return false;
	}

	public bool IsRoaming()
	{
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto && this.unitCtrlrs.Count == 1 && this.unitCtrlrs[0].unit.Data.UnitTypeId == global::UnitTypeId.MONSTER)
		{
			return true;
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.isCampaign)
		{
			return false;
		}
		for (int i = 0; i < this.unitCtrlrs.Count; i++)
		{
			if (this.unitCtrlrs[i].unit.Id == (global::UnitId)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.roamingUnitId)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsAmbushed()
	{
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.IsAmbush())
		{
			global::DeploymentId deploymentId = this.deploymentId;
			if (deploymentId == global::DeploymentId.WAGON || deploymentId == global::DeploymentId.AMBUSHED)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsPlayed()
	{
		return this.playerIdx == global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex && this.playerTypeId == global::PlayerTypeId.PLAYER;
	}

	public void InitBlackLists()
	{
		if (this.playerTypeId == global::PlayerTypeId.PLAYER || this.playerTypeId == global::PlayerTypeId.AI)
		{
			this.BlackListAll();
		}
		if (this.CampaignWarbandId != global::CampaignWarbandId.NONE)
		{
			global::System.Collections.Generic.List<global::CampaignWarbandWhitelistData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignWarbandWhitelistData>("fk_campaign_warband_id", ((int)this.CampaignWarbandId).ToConstantString());
			for (int i = 0; i < list.Count; i++)
			{
				int campaignWarbandIdx = global::PandoraSingleton<global::MissionManager>.Instance.GetCampaignWarbandIdx(list[i].CampaignWarbandIdWhitelisted);
				this.WhiteListWarband(campaignWarbandIdx);
			}
		}
	}

	public void BlackListAll()
	{
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; i++)
		{
			global::WarbandController warbandController = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i];
			if (warbandController.idx != this.idx && warbandController.teamIdx != this.teamIdx && warbandController.playerTypeId != global::PlayerTypeId.PASSIVE_AI && !warbandController.defeated)
			{
				this.BlackListWarband(i);
			}
		}
	}

	public void WhiteListWarband(int warbandIdx)
	{
		this.BlackList &= ~(1 << warbandIdx);
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Warband ",
			this.idx,
			" White listing ",
			warbandIdx,
			" (",
			this.BlackList,
			")"
		}), "WarbandController", null);
	}

	public void BlackListWarband(int warbandIdx)
	{
		if (warbandIdx != this.idx)
		{
			this.BlackList |= 1 << warbandIdx;
			global::PandoraDebug.LogInfo(string.Concat(new object[]
			{
				"Warband ",
				this.idx,
				" Black listing ",
				warbandIdx,
				" (",
				this.BlackList,
				")"
			}), "WarbandController", null);
		}
	}

	public bool BlackListed(int idx)
	{
		return (this.BlackList & 1 << idx) != 0;
	}

	public bool AllUnitsDead()
	{
		for (int i = 0; i < this.unitCtrlrs.Count; i++)
		{
			if (this.unitCtrlrs[i].unit.Status != global::UnitStateId.OUT_OF_ACTION)
			{
				return false;
			}
		}
		return true;
	}

	public void SetupObjectivesCampaign(global::CampaignMissionId missionId)
	{
		this.objectives = global::Objective.CreateMissionObjectives(missionId, this);
	}

	public void SetupObjectivesProc()
	{
		if (this.objectiveTypeId == global::PrimaryObjectiveTypeId.NONE)
		{
			return;
		}
		global::WarbandController warbandController = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[this.objectiveTargetIdx];
		global::Objective.CreateObjective(ref this.objectives, this.objectiveTypeId, this, this.objectiveSeed, warbandController.WarData.Id, warbandController.unitCtrlrs.ConvertAll<global::Unit>((global::UnitController x) => x.unit), warbandController);
	}

	public bool CheckObjectives(bool dontSendNotice = false, bool forceSendNotice = false)
	{
		bool flag = false;
		this.AllObjectivesCompleted = (this.objectives.Count > 0);
		for (int i = 0; i < this.objectives.Count; i++)
		{
			global::Objective objective = this.objectives[i];
			bool flag2 = objective.CheckObjective();
			flag = (flag || flag2);
			if (objective.RequiredObjectives.Count > 0)
			{
				bool flag3 = false;
				for (int j = 0; j < objective.RequiredObjectives.Count; j++)
				{
					flag3 |= ((objective.RequiredCompleteds[j] && !objective.RequiredObjectives[j].done) || (!objective.RequiredCompleteds[j] && objective.RequiredObjectives[j].counter.x == 0f));
				}
				objective.SetLocked(flag3);
			}
			this.AllObjectivesCompleted &= objective.done;
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr() == this)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.UpdateObjectivesUI(false);
		}
		return this.AllObjectivesCompleted;
	}

	public bool HasFailedMandatoryObjective()
	{
		for (int i = 0; i < this.objectives.Count; i++)
		{
			if (this.objectives[i].ResultId == global::PrimaryObjectiveResultId.FAILED)
			{
				return true;
			}
		}
		return false;
	}

	public void SearchOpened(global::SearchPoint search)
	{
		if (!search.isWyrdstone && this.openedSearch.IndexOf(search) == -1 && global::PandoraSingleton<global::MissionManager>.Instance.GetSearchPoints().IndexOf(search) != -1)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.unit.AddToAttribute(global::AttributeId.TOTAL_SEARCH, 1);
			this.openedSearch.Add(search);
			global::PandoraSingleton<global::MissionManager>.Instance.UpdateObjectivesUI(false);
			global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateOpenedSearches(this.saveIdx, search.guid);
			if (this.idx == global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().idx)
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.OPENED_CHESTS, 1);
			}
		}
	}

	public void ConvertItem(global::ItemId id, int count)
	{
		for (int i = 0; i < this.objectives.Count; i++)
		{
			if (this.objectives[i].TypeId == global::PrimaryObjectiveTypeId.CONVERT)
			{
				(this.objectives[i] as global::ObjectiveConvert).UpdateConvertedItems(id, count);
			}
		}
	}

	public void LocateItem(global::System.Collections.Generic.List<global::Item> items)
	{
		for (int i = 0; i < this.objectives.Count; i++)
		{
			if (this.objectives[i].TypeId == global::PrimaryObjectiveTypeId.LOCATE)
			{
				(this.objectives[i] as global::ObjectiveLocate).UpdateLocatedItems(items);
			}
		}
	}

	public void LocateZone(global::LocateZone zone)
	{
		for (int i = 0; i < this.objectives.Count; i++)
		{
			if (this.objectives[i].TypeId == global::PrimaryObjectiveTypeId.LOCATE)
			{
				(this.objectives[i] as global::ObjectiveLocate).UpdateLocatedZone(zone, true);
			}
		}
	}

	public void Activate(string pointName)
	{
		for (int i = 0; i < this.objectives.Count; i++)
		{
			if (this.objectives[i].TypeId == global::PrimaryObjectiveTypeId.ACTIVATE)
			{
				((global::ObjectiveActivate)this.objectives[i]).UpdateActivatedItem(pointName);
			}
		}
	}

	public void DestroyDestructible(string destrName)
	{
		for (int i = 0; i < this.objectives.Count; i++)
		{
			if (this.objectives[i].TypeId == global::PrimaryObjectiveTypeId.DESTROY)
			{
				((global::ObjectiveDestroy)this.objectives[i]).UpdateDestroyedItem(destrName);
			}
		}
	}

	public void OnUnitCreated(global::UnitController ctrlr)
	{
		this.moralValue += ctrlr.unit.Moral;
		this.MaxMoralValue = this.moralValue;
	}

	public int GetMVU(global::Tyche tyche, bool enemy)
	{
		if (this.unitCtrlrs.Count == 0)
		{
			return 0;
		}
		global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
		int attribute = this.unitCtrlrs[0].unit.GetAttribute(global::AttributeId.CURRENT_MVU);
		for (int i = 0; i < this.unitCtrlrs.Count; i++)
		{
			if (this.unitCtrlrs[i].unit.GetUnitTypeId() != global::UnitTypeId.DRAMATIS && (!this.IsPlayed() || this.unitCtrlrs[i].unit.CampaignData == null))
			{
				if (this.unitCtrlrs[i].unit.GetAttribute(global::AttributeId.CURRENT_MVU) > attribute)
				{
					attribute = this.unitCtrlrs[i].unit.GetAttribute(global::AttributeId.CURRENT_MVU);
					list.Clear();
				}
				if (this.unitCtrlrs[i].unit.GetAttribute(global::AttributeId.CURRENT_MVU) == attribute)
				{
					list.Add(i);
				}
			}
		}
		if (list.Count == 0)
		{
			return 0;
		}
		if (enemy)
		{
			return list[tyche.Rand(0, list.Count)];
		}
		return this.unitCtrlrs[list[tyche.Rand(0, list.Count)]].unit.UnitSave.warbandSlotIndex;
	}

	public int GetCollectedWyrdStone()
	{
		int num = 0;
		if (this.wagon != null && this.wagon.chest != null)
		{
			for (int i = 0; i < this.wagon.chest.items.Count; i++)
			{
				if (this.wagon.chest.items[i].IsWyrdStone)
				{
					num++;
				}
			}
		}
		if (this.unitCtrlrs != null)
		{
			for (int j = 0; j < this.unitCtrlrs.Count; j++)
			{
				for (int k = 0; k < this.unitCtrlrs[j].unit.Items.Count; k++)
				{
					if (this.unitCtrlrs[j].unit.Items[k].IsWyrdStone)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	public void AddMoralIdol()
	{
		if (this.idolMoralRemoved)
		{
			this.idolMoralRemoved = false;
			this.MoralValue += global::Constant.GetInt(global::ConstantId.MORAL_IDOL);
			global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(global::CombatLogger.LogMessage.GAIN_IDOL, new string[]
			{
				this.name,
				global::Constant.GetInt(global::ConstantId.MORAL_IDOL).ToConstantString()
			});
			if (global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr() == this)
			{
				global::PandoraSingleton<global::Pan>.Instance.Narrate("stolen_idol");
			}
		}
	}

	public void RemoveMoralIdol()
	{
		if (!this.idolMoralRemoved)
		{
			this.idolMoralRemoved = true;
			this.MoralValue -= global::Constant.GetInt(global::ConstantId.MORAL_IDOL);
			global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(global::CombatLogger.LogMessage.LOSE_IDOL, new string[]
			{
				this.name,
				global::Constant.GetInt(global::ConstantId.MORAL_IDOL).ToConstantString()
			});
		}
	}

	public const uint WARBAND_MYRTILUS = 200000000U;

	public string name;

	public int idx;

	public int saveIdx;

	public bool canRout;

	public bool defeated;

	public int teamIdx;

	public int playerIdx;

	public global::PlayerTypeId playerTypeId;

	public global::DeploymentId deploymentId;

	public global::PrimaryObjectiveTypeId objectiveTypeId;

	public int objectiveTargetIdx;

	public int objectiveSeed;

	public global::System.Collections.Generic.List<global::UnitController> unitCtrlrs;

	public bool idolMoralRemoved;

	public int OldMoralValue = -1;

	private int moralValue;

	public global::WarbandWagon wagon;

	public global::System.Collections.Generic.List<global::Objective> objectives = new global::System.Collections.Generic.List<global::Objective>();

	public global::System.Collections.Generic.List<global::InteractivePoint> openedSearch = new global::System.Collections.Generic.List<global::InteractivePoint>();

	public float percSearch;

	public float percWyrd;

	public global::System.Collections.Generic.List<global::Item> rewardItems;

	public global::System.Collections.Generic.List<global::Item> wyrdstones = new global::System.Collections.Generic.List<global::Item>();

	public int spoilsFound;
}
