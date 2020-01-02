using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective
{
	public Objective(global::PrimaryObjectiveId id)
	{
		if (global::PandoraSingleton<global::MissionManager>.Exists())
		{
			this.guid = global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID();
		}
		this.data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PrimaryObjectiveData>((int)id);
		this.typeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PrimaryObjectiveTypeData>((int)this.data.PrimaryObjectiveTypeId);
		this.counter = global::UnityEngine.Vector2.zero;
		this.done = false;
		this.subDesc = new global::System.Collections.Generic.List<string>();
		this.dones = new global::System.Collections.Generic.List<bool>();
		this.itemsToSteal = new global::System.Collections.Generic.List<global::Item>();
		this.searchToCheck = new global::System.Collections.Generic.List<global::SearchPoint>();
		this.unitsToCheck = new global::System.Collections.Generic.List<global::UnitController>();
		this.RequiredObjectives = new global::System.Collections.Generic.List<global::Objective>();
		this.RequiredCompleteds = new global::System.Collections.Generic.List<bool>();
		this.name = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.NameKey);
		this.desc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.DescKey);
	}

	public global::PrimaryObjectiveId Id
	{
		get
		{
			return this.data.Id;
		}
	}

	public global::PrimaryObjectiveTypeId TypeId
	{
		get
		{
			return this.data.PrimaryObjectiveTypeId;
		}
	}

	public string NameKey
	{
		get
		{
			return "mission_obj_" + this.data.Name;
		}
	}

	public string DescKey
	{
		get
		{
			return "mission_obj_" + this.data.Name + "_desc";
		}
	}

	public global::PrimaryObjectiveResultId ResultId
	{
		get
		{
			if (this.done)
			{
				return global::PrimaryObjectiveResultId.SUCCESS;
			}
			return (!this.typeData.Mandatory) ? global::PrimaryObjectiveResultId.PROGRESS : global::PrimaryObjectiveResultId.FAILED;
		}
	}

	public bool Locked
	{
		get
		{
			return this.locked;
		}
	}

	public global::System.Collections.Generic.List<global::Objective> RequiredObjectives { get; private set; }

	public global::System.Collections.Generic.List<bool> RequiredCompleteds { get; private set; }

	public int SortWeight { get; private set; }

	public static global::System.Collections.Generic.List<global::Objective> CreateMissionObjectives(global::CampaignMissionId missionId, global::WarbandController warCtrlr)
	{
		global::System.Collections.Generic.List<global::Objective> list = new global::System.Collections.Generic.List<global::Objective>();
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_campaign_mission_id";
		int num = (int)missionId;
		global::System.Collections.Generic.List<global::CampaignMissionObjectiveData> list2 = instance.InitData<global::CampaignMissionObjectiveData>(field, num.ToString());
		global::System.Collections.Generic.List<global::Objective> list3 = new global::System.Collections.Generic.List<global::Objective>();
		for (int i = 0; i < list2.Count; i++)
		{
			list3.Clear();
			global::Objective.CreateObjective(ref list3, list2[i].PrimaryObjectiveId, warCtrlr, 0, global::WarbandId.NONE, null, null);
			for (int j = 0; j < list3.Count; j++)
			{
				list3[j].SortWeight = list2[i].SortWeight + j;
			}
			list.AddRange(list3);
		}
		for (int k = list.Count - 1; k >= 0; k--)
		{
			global::Objective objective = list[k];
			global::System.Collections.Generic.List<global::PrimaryObjectiveRequirementData> list4 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PrimaryObjectiveRequirementData>("fk_primary_objective_id", ((int)objective.Id).ToString());
			objective.SetLocked(list4.Count > 0);
			for (int l = 0; l < list4.Count; l++)
			{
				for (int m = 0; m < list.Count; m++)
				{
					if (list[m].Id == list4[l].PrimaryObjectiveIdRequired)
					{
						objective.RequiredObjectives.Add(list[m]);
						objective.RequiredCompleteds.Add(list4[l].RequiredCompleted);
					}
				}
			}
		}
		list.Sort(new global::ObjectiveComparer());
		return list;
	}

	private static global::PrimaryObjectiveId GetRandomObjective(global::PrimaryObjectiveTypeId typeId, int seed)
	{
		global::Tyche tyche = new global::Tyche(seed, true);
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_primary_objective_type_id";
		int num = (int)typeId;
		global::System.Collections.Generic.List<global::PrimaryObjectiveData> list = instance.InitData<global::PrimaryObjectiveData>(field, num.ToString());
		int index = tyche.Rand(0, list.Count);
		return list[index].Id;
	}

	public static void CreateObjective(ref global::System.Collections.Generic.List<global::Objective> objectives, global::PrimaryObjectiveTypeId objectiveTypeId, global::WarbandController warCtrlr, int objectiveSeed = 0, global::WarbandId enemyWarbandId = global::WarbandId.NONE, global::System.Collections.Generic.List<global::Unit> enemies = null, global::WarbandController enemyWarCtrlr = null)
	{
		global::Objective.CreateObjective(ref objectives, global::Objective.GetRandomObjective(objectiveTypeId, objectiveSeed), warCtrlr, objectiveSeed, enemyWarbandId, enemies, enemyWarCtrlr);
	}

	public static void CreateObjective(ref global::System.Collections.Generic.List<global::Objective> objectives, global::PrimaryObjectiveId id, global::WarbandController warCtrlr, int objectiveSeed = 0, global::WarbandId enemyWarbandId = global::WarbandId.NONE, global::System.Collections.Generic.List<global::Unit> enemies = null, global::WarbandController enemyWarCtrlr = null)
	{
		global::PrimaryObjectiveData primaryObjectiveData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PrimaryObjectiveData>((int)id);
		switch (primaryObjectiveData.PrimaryObjectiveTypeId)
		{
		case global::PrimaryObjectiveTypeId.BOUNTY:
			objectives.Add(new global::ObjectiveBounty(id, warCtrlr, objectiveSeed, enemies));
			break;
		case global::PrimaryObjectiveTypeId.GRAND_THEFT_CART:
			objectives.Add(new global::ObjectiveGranTheftCart(id, warCtrlr, enemyWarbandId, enemyWarCtrlr));
			objectives.Add(new global::ObjectiveProtectIdol(global::PrimaryObjectiveId.PROTECT_IDOL, warCtrlr));
			break;
		case global::PrimaryObjectiveTypeId.WYRDSTONE_RUSH:
			objectives.Add(new global::ObjectiveWyrdstoneRush(id, warCtrlr, objectiveSeed));
			break;
		case global::PrimaryObjectiveTypeId.GATHER_INSTALL:
			objectives.Add(new global::ObjectiveGatherInstall(id, warCtrlr));
			break;
		case global::PrimaryObjectiveTypeId.LOCATE:
			objectives.Add(new global::ObjectiveLocate(id));
			break;
		case global::PrimaryObjectiveTypeId.CONVERT:
			objectives.Add(new global::ObjectiveConvert(id));
			break;
		case global::PrimaryObjectiveTypeId.ACTIVATE:
			objectives.Add(new global::ObjectiveActivate(id));
			break;
		case global::PrimaryObjectiveTypeId.WANTED:
			objectives.Add(new global::ObjectiveWanted(id, warCtrlr));
			break;
		case global::PrimaryObjectiveTypeId.KEEP_ALIVE:
			objectives.Add(new global::ObjectiveKeepAlive(id));
			break;
		case global::PrimaryObjectiveTypeId.PROTECT_IDOL:
			objectives.Add(new global::ObjectiveProtectIdol(id, warCtrlr));
			break;
		case global::PrimaryObjectiveTypeId.DESTROY:
			objectives.Add(new global::ObjectiveDestroy(id));
			break;
		default:
			global::PandoraDebug.LogWarning("Objective type " + primaryObjectiveData.PrimaryObjectiveTypeId + " not supported", "uncategorised", null);
			break;
		}
	}

	public static void CreateLoadingObjective(ref global::System.Collections.Generic.List<global::Objective> objectives, global::PrimaryObjectiveTypeId typeId, global::MissionWarbandSave enemyWarband, int seed)
	{
		global::PrimaryObjectiveId randomObjective = global::Objective.GetRandomObjective(typeId, seed);
		switch (typeId)
		{
		case global::PrimaryObjectiveTypeId.NONE:
			break;
		case global::PrimaryObjectiveTypeId.BOUNTY:
			objectives.Add(new global::ObjectiveBounty(randomObjective, enemyWarband.Units, seed));
			break;
		case global::PrimaryObjectiveTypeId.GRAND_THEFT_CART:
			objectives.Add(new global::ObjectiveGranTheftCart(randomObjective, enemyWarband.WarbandId));
			objectives.Add(new global::ObjectiveProtectIdol(global::PrimaryObjectiveId.PROTECT_IDOL, enemyWarband.WarbandId));
			break;
		case global::PrimaryObjectiveTypeId.WYRDSTONE_RUSH:
			objectives.Add(new global::ObjectiveWyrdstoneRush(randomObjective, seed));
			break;
		default:
			global::PandoraDebug.LogWarning("Objective type " + typeId + " not supported", "uncategorised", null);
			break;
		}
	}

	public bool CheckObjective()
	{
		bool flag = false;
		bool flag2 = false;
		this.oldCounter = this.counter;
		this.Track(ref flag);
		this.counter.x = global::UnityEngine.Mathf.Min(this.counter.x, this.counter.y);
		flag |= (this.oldCounter.x != this.counter.x);
		flag |= (this.done != flag2);
		flag2 = (this.counter.x >= this.counter.y);
		this.done = flag2;
		return flag;
	}

	protected abstract void Track(ref bool objectivesChanged);

	public abstract void Reload(uint trackedUid);

	public virtual void SetLocked(bool loc)
	{
		this.locked = loc;
	}

	protected void CheckItemsToSteal(ref bool objectivesChanged)
	{
		this.counter.x = 0f;
		for (int i = 0; i < this.itemsToSteal.Count; i++)
		{
			bool flag = false;
			int num = 0;
			while (num < this.searchToCheck.Count && !flag)
			{
				flag = this.searchToCheck[num].Contains(this.itemsToSteal[i]);
				num++;
			}
			if (!flag && this.unitsToCheck != null)
			{
				int num2 = 0;
				while (num2 < this.unitsToCheck.Count && !flag)
				{
					flag = (this.unitsToCheck[num2].unit.Status != global::UnitStateId.OUT_OF_ACTION && this.unitsToCheck[num2].unit.Items.Contains(this.itemsToSteal[i]));
					num2++;
				}
			}
			if (this.dones.Count != 0)
			{
				if (this.dones[i] != flag)
				{
					objectivesChanged = true;
				}
				this.dones[i] = flag;
			}
			if (flag)
			{
				this.counter.x = this.counter.x + 1f;
			}
		}
	}

	public uint guid;

	public string name;

	public string desc;

	public global::System.Collections.Generic.List<string> subDesc;

	public global::System.Collections.Generic.List<bool> dones;

	public global::UnityEngine.Vector2 counter;

	public global::UnityEngine.Vector2 oldCounter;

	public bool done;

	private bool locked;

	private global::PrimaryObjectiveData data;

	private global::PrimaryObjectiveTypeData typeData;

	protected global::System.Collections.Generic.List<global::SearchPoint> searchToCheck;

	protected global::System.Collections.Generic.List<global::UnitController> unitsToCheck;

	protected global::System.Collections.Generic.List<global::Item> itemsToSteal = new global::System.Collections.Generic.List<global::Item>();
}
