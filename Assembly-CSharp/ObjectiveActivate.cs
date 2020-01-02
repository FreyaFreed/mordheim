using System;
using UnityEngine;

public class ObjectiveActivate : global::Objective
{
	public ObjectiveActivate(global::PrimaryObjectiveId id) : base(id)
	{
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_primary_objective_id";
		int num = (int)id;
		this.activateData = instance.InitData<global::PrimaryObjectiveActivateData>(field, num.ToString())[0];
		int count = global::PandoraSingleton<global::MissionManager>.Instance.GetActivatePoints(this.activateData.PropsName).Count;
		this.counter = new global::UnityEngine.Vector2(0f, (float)this.activateData.PropsCount);
		this.desc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(base.DescKey, new string[]
		{
			global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.activateData.PropsName)
		});
	}

	protected override void Track(ref bool objectivesChanged)
	{
	}

	public void UpdateActivatedItem(string name)
	{
		if (name == this.activateData.PropsName)
		{
			this.counter.x = this.counter.x + 1f;
			global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateObjective(this.guid, 0U);
		}
	}

	public override void Reload(uint trackedUid)
	{
		this.counter.x = this.counter.x + 1f;
	}

	private global::PrimaryObjectiveActivateData activateData;
}
