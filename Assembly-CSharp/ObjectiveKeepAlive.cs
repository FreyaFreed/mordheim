using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveKeepAlive : global::Objective
{
	public ObjectiveKeepAlive(global::PrimaryObjectiveId id) : base(id)
	{
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_primary_objective_id";
		int num = (int)id;
		global::PrimaryObjectiveKeepAliveData primaryObjectiveKeepAliveData = instance.InitData<global::PrimaryObjectiveKeepAliveData>(field, num.ToString())[0];
		global::System.Collections.Generic.List<global::UnitController> allUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
		for (int i = 0; i < allUnits.Count; i++)
		{
			if (allUnits[i].unit.CampaignData != null && allUnits[i].unit.CampaignData.Id == primaryObjectiveKeepAliveData.CampaignUnitId)
			{
				this.keepAliveUnit = allUnits[i].unit;
				break;
			}
		}
		this.counter = new global::UnityEngine.Vector2(1f, 1f);
	}

	public override void Reload(uint trackedUid)
	{
		throw new global::System.NotImplementedException();
	}

	protected override void Track(ref bool objectivesChanged)
	{
		this.counter.x = (float)((this.keepAliveUnit.Status != global::UnitStateId.OUT_OF_ACTION) ? 1 : 0);
	}

	private global::Unit keepAliveUnit;
}
