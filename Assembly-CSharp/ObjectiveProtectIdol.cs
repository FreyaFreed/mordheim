using System;

public class ObjectiveProtectIdol : global::Objective
{
	public ObjectiveProtectIdol(global::PrimaryObjectiveId id, global::WarbandController warCtrlr) : base(id)
	{
		this.myIdol = warCtrlr.ItemIdol;
		this.itemsToSteal.Add(this.myIdol);
		this.searchToCheck.Add(warCtrlr.wagon.idol);
		this.counter.y = 1f;
		this.desc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(base.DescKey, new string[]
		{
			this.myIdol.Name
		});
	}

	public ObjectiveProtectIdol(global::PrimaryObjectiveId id, global::WarbandId enemyWarbandId) : base(id)
	{
		global::WarbandData warbandData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>((int)enemyWarbandId);
		this.myIdol = new global::Item(warbandData.ItemIdIdol, global::ItemQualityId.NORMAL);
		this.desc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(base.DescKey, new string[]
		{
			this.myIdol.Name
		});
	}

	protected override void Track(ref bool objectivesChanged)
	{
		base.CheckItemsToSteal(ref objectivesChanged);
	}

	public override void Reload(uint trackedUid)
	{
		throw new global::System.NotImplementedException();
	}

	private global::Item myIdol;
}
