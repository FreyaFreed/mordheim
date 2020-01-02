using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveWyrdstoneRush : global::Objective
{
	public ObjectiveWyrdstoneRush(global::PrimaryObjectiveId id, global::WarbandController warCtrlr, int seed) : base(id)
	{
		this.SetBaseData(seed);
		this.wyrdstoneRushTarget = (int)global::UnityEngine.Mathf.Ceil((float)global::PandoraSingleton<global::MissionManager>.Instance.numWyrdstones * (float)this.rushData.WyrdstonePerc / 100f);
		this.searchToCheck.Add(warCtrlr.wagon.chest);
		this.unitsToCheck = warCtrlr.unitCtrlrs;
		this.counter.y = (float)this.wyrdstoneRushTarget;
	}

	public ObjectiveWyrdstoneRush(global::PrimaryObjectiveId id, int seed) : base(id)
	{
		this.SetBaseData(seed);
	}

	protected override void Track(ref bool objectivesChanged)
	{
		int num = 0;
		for (int i = 0; i < this.searchToCheck.Count; i++)
		{
			num += this.CountWyrdstones(this.searchToCheck[i].items);
		}
		for (int j = 0; j < this.unitsToCheck.Count; j++)
		{
			if (this.unitsToCheck[j].unit.Status != global::UnitStateId.OUT_OF_ACTION)
			{
				num += this.CountWyrdstones(this.unitsToCheck[j].unit.Items);
			}
		}
		this.counter.x = (float)num;
	}

	private int CountWyrdstones(global::System.Collections.Generic.List<global::Item> items)
	{
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].Id == global::ItemId.WYRDSTONE_SHARD || items[i].Id == global::ItemId.WYRDSTONE_FRAGMENT || items[i].Id == global::ItemId.WYRDSTONE_CLUSTER)
			{
				num++;
			}
		}
		return num;
	}

	private void SetBaseData(int seed)
	{
		global::System.Collections.Generic.List<global::PrimaryObjectiveWyrdData> datas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PrimaryObjectiveWyrdData>();
		global::Tyche tyche = new global::Tyche(seed, true);
		this.rushData = global::PrimaryObjectiveWyrdData.GetRandomRatio(datas, tyche, null);
		this.desc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_obj_wyrdstone_rush_desc", new string[]
		{
			this.rushData.WyrdstonePerc.ToString()
		});
	}

	public override void Reload(uint trackedUid)
	{
		throw new global::System.NotImplementedException();
	}

	private int wyrdstoneRushTarget;

	private global::PrimaryObjectiveWyrdData rushData;
}
