using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBounty : global::Objective
{
	public ObjectiveBounty(global::PrimaryObjectiveId id, global::WarbandController warCtrlr, int seed, global::System.Collections.Generic.List<global::Unit> enemies) : base(id)
	{
		global::Tyche tyche = new global::Tyche(seed, true);
		this.SetBaseData(enemies.Count, tyche);
		global::System.Collections.Generic.List<global::Unit> list = new global::System.Collections.Generic.List<global::Unit>();
		global::System.Collections.Generic.List<global::Unit> list2 = new global::System.Collections.Generic.List<global::Unit>();
		foreach (global::Unit unit in enemies)
		{
			if (unit.GetUnitTypeId() == global::UnitTypeId.HENCHMEN)
			{
				list2.Add(unit);
			}
			else
			{
				list.Add(unit);
			}
		}
		this.targets = new global::System.Collections.Generic.List<global::Unit>();
		for (int i = 0; i < this.targetsCount; i++)
		{
			global::System.Collections.Generic.List<global::Unit> list3 = (i >= this.bountyData.EliteCount || list.Count <= 0) ? list2 : list;
			if (list3.Count == 0)
			{
				this.targetsCount--;
			}
			else
			{
				int index = tyche.Rand(0, list3.Count);
				this.itemsToSteal.Add(list3[index].deathTrophy);
				this.AddSubObj(list3[index].Name, list3[index].deathTrophy.Name);
				this.targets.Add(list3[index]);
				list3.RemoveAt(index);
			}
		}
		this.searchToCheck.Add(warCtrlr.wagon.chest);
		this.unitsToCheck = warCtrlr.unitCtrlrs;
		this.counter.y = (float)this.targetsCount;
	}

	public ObjectiveBounty(global::PrimaryObjectiveId id, global::System.Collections.Generic.List<global::UnitSave> enemies, int seed) : base(id)
	{
		global::Tyche tyche = new global::Tyche(seed, true);
		this.SetBaseData(enemies.Count, tyche);
		global::System.Collections.Generic.List<global::UnitSave> list = new global::System.Collections.Generic.List<global::UnitSave>();
		global::System.Collections.Generic.List<global::UnitSave> list2 = new global::System.Collections.Generic.List<global::UnitSave>();
		foreach (global::UnitSave unitSave in enemies)
		{
			global::UnitData unitData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>(unitSave.stats.id);
			global::UnitTypeId unitTypeId = global::Unit.GetUnitTypeId(unitSave, unitData.UnitTypeId);
			if (unitTypeId == global::UnitTypeId.HENCHMEN)
			{
				list2.Add(unitSave);
			}
			else
			{
				list.Add(unitSave);
			}
		}
		for (int i = 0; i < this.targetsCount; i++)
		{
			global::System.Collections.Generic.List<global::UnitSave> list3 = (i >= this.bountyData.EliteCount || list.Count <= 0) ? list2 : list;
			if (list3.Count == 0)
			{
				this.targetsCount--;
			}
			else
			{
				int index = tyche.Rand(0, list3.Count);
				global::UnitData unitData2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>(list3[index].stats.id);
				this.AddSubObj(list3[index].stats.Name, unitData2.ItemIdTrophy.ToLowerString());
				list3.RemoveAt(index);
			}
		}
	}

	private void SetBaseData(int totalCount, global::Tyche tyche)
	{
		global::System.Collections.Generic.List<global::PrimaryObjectiveBountyData> datas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PrimaryObjectiveBountyData>();
		this.bountyData = global::PrimaryObjectiveBountyData.GetRandomRatio(datas, tyche, null);
		this.targetsCount = global::UnityEngine.Mathf.RoundToInt((float)totalCount * (float)this.bountyData.UnitPerc / 100f);
		this.targetsCount = global::UnityEngine.Mathf.Clamp(this.targetsCount, 1, totalCount);
		this.desc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_obj_bounty_desc");
	}

	protected override void Track(ref bool objectivesChanged)
	{
		base.CheckItemsToSteal(ref objectivesChanged);
	}

	private void AddSubObj(string unitName, string itemName)
	{
		this.subDesc.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("item_name_" + itemName, new string[]
		{
			unitName
		}));
		this.dones.Add(false);
	}

	public bool IsUnitBounty(global::UnitController unit)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] == unit.unit)
			{
				return true;
			}
		}
		return false;
	}

	public override void Reload(uint trackedUid)
	{
		throw new global::System.NotImplementedException();
	}

	private global::System.Collections.Generic.List<global::Unit> targets;

	private global::PrimaryObjectiveBountyData bountyData;

	private int targetsCount;
}
