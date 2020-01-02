using System;
using UnityEngine;

public class ObjectiveConvert : global::Objective
{
	public ObjectiveConvert(global::PrimaryObjectiveId id) : base(id)
	{
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_primary_objective_id";
		int num = (int)id;
		this.convertData = instance.InitData<global::PrimaryObjectiveConvertData>(field, num.ToString())[0];
		this.counter = new global::UnityEngine.Vector2(0f, (float)this.convertData.ItemCount);
		this.desc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(base.DescKey, new string[]
		{
			this.convertData.ItemId.ToLowerString()
		});
	}

	protected override void Track(ref bool objectivesChanged)
	{
	}

	public void UpdateConvertedItems(global::ItemId id, int count)
	{
		if (id == this.convertData.ItemId)
		{
			this.counter.x = this.counter.x + (float)count;
			for (int i = 0; i < count; i++)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateObjective(this.guid, 0U);
			}
		}
	}

	public override void Reload(uint trackedUid)
	{
		this.counter.x = this.counter.x + 1f;
	}

	private global::PrimaryObjectiveConvertData convertData;
}
