using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveWanted : global::Objective
{
	public ObjectiveWanted(global::PrimaryObjectiveId id, global::WarbandController warCtrlr) : base(id)
	{
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_primary_objective_id";
		int num = (int)id;
		global::System.Collections.Generic.List<global::PrimaryObjectiveWantedData> list = instance.InitData<global::PrimaryObjectiveWantedData>(field, num.ToString());
		global::System.Collections.Generic.List<global::UnitController> allEnemies = global::PandoraSingleton<global::MissionManager>.Instance.GetAllEnemies(warCtrlr.idx);
		this.wantedUnits = new global::System.Collections.Generic.List<global::Unit>();
		for (int i = 0; i < list.Count; i++)
		{
			for (int j = 0; j < allEnemies.Count; j++)
			{
				if (allEnemies[j].unit.UnitSave.campaignId == (int)list[i].CampaignUnitId)
				{
					global::Unit unit = allEnemies[j].unit;
					this.wantedUnits.Add(unit);
					if (list.Count > 1)
					{
						this.subDesc.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_sub_obj_wanted", new string[]
						{
							allEnemies[j].unit.Name
						}));
					}
					this.dones.Add(false);
				}
			}
		}
		this.counter = new global::UnityEngine.Vector2(0f, (float)this.wantedUnits.Count);
	}

	public override void Reload(uint trackedUid)
	{
		throw new global::System.NotImplementedException();
	}

	protected override void Track(ref bool objectivesChanged)
	{
		this.counter.x = 0f;
		for (int i = 0; i < this.wantedUnits.Count; i++)
		{
			this.dones[i] = (this.wantedUnits[i].Status == global::UnitStateId.OUT_OF_ACTION);
			this.counter.x = this.counter.x + (float)((!this.dones[i]) ? 0 : 1);
		}
	}

	private global::System.Collections.Generic.List<global::Unit> wantedUnits;
}
