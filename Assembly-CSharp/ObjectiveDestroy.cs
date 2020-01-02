using System;
using UnityEngine;

public class ObjectiveDestroy : global::Objective
{
	public ObjectiveDestroy(global::PrimaryObjectiveId id) : base(id)
	{
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_primary_objective_id";
		int num = (int)id;
		this.destroyData = instance.InitData<global::PrimaryObjectiveDestroyData>(field, num.ToString())[0];
		int count = global::PandoraSingleton<global::MissionManager>.Instance.GetDestructibles(this.destroyData.PropsName).Count;
		this.counter = new global::UnityEngine.Vector2(0f, (float)this.destroyData.PropsCount);
		this.desc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(base.DescKey, new string[]
		{
			global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.destroyData.PropsName)
		});
	}

	protected override void Track(ref bool objectivesChanged)
	{
	}

	public void UpdateDestroyedItem(string name)
	{
		if (name == this.destroyData.PropsName)
		{
			this.counter.x = this.counter.x + 1f;
			global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateObjective(this.guid, 0U);
		}
	}

	public override void Reload(uint trackedUid)
	{
		this.counter.x = this.counter.x + 1f;
	}

	private global::PrimaryObjectiveDestroyData destroyData;
}
