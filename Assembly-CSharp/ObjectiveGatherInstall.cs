using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveGatherInstall : global::Objective
{
	public ObjectiveGatherInstall(global::PrimaryObjectiveId id, global::WarbandController warCtrlr) : base(id)
	{
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_primary_objective_id";
		int num = (int)id;
		this.gathersData = instance.InitData<global::PrimaryObjectiveGatherInstallData>(field, num.ToString());
		global::System.Collections.Generic.List<global::Item> collection = new global::System.Collections.Generic.List<global::Item>();
		this.searchToCheck = new global::System.Collections.Generic.List<global::SearchPoint>();
		for (int i = 0; i < this.gathersData.Count; i++)
		{
			if (this.gathersData[i].ItemId != global::ItemId.NONE)
			{
				collection = global::PandoraSingleton<global::MissionManager>.Instance.FindObjectivesInSearch(this.gathersData[i].ItemId);
				global::PandoraSingleton<global::MissionManager>.Instance.FindObjectiveInUnits(this.gathersData[i].ItemId, ref collection);
				this.itemsToSteal.AddRange(collection);
			}
			if (!string.IsNullOrEmpty(this.gathersData[i].CheckSearch))
			{
				this.searchToCheck.AddRange(global::PandoraSingleton<global::MissionManager>.Instance.GetSearchPoints(this.gathersData[i].CheckSearch));
			}
			if (this.gathersData[i].CheckWagon)
			{
				this.searchToCheck.Add(warCtrlr.wagon.chest);
			}
			if (this.gathersData[i].CheckUnits)
			{
				this.unitsToCheck = warCtrlr.unitCtrlrs;
			}
		}
		this.counter = new global::UnityEngine.Vector2(0f, (float)this.gathersData[0].ItemCount);
		this.desc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(base.DescKey, new string[]
		{
			this.gathersData[0].ItemId.ToLowerString()
		});
	}

	public override void Reload(uint trackedUid)
	{
		throw new global::System.NotImplementedException();
	}

	protected override void Track(ref bool objectivesChanged)
	{
		base.CheckItemsToSteal(ref objectivesChanged);
	}

	private global::System.Collections.Generic.List<global::PrimaryObjectiveGatherInstallData> gathersData;
}
