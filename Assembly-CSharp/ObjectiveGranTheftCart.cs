using System;

public class ObjectiveGranTheftCart : global::Objective
{
	public ObjectiveGranTheftCart(global::PrimaryObjectiveId id, global::WarbandController warCtrlr, global::WarbandId enemyWarbandId, global::WarbandController enemyWarCtrlr) : base(id)
	{
		this.enemyIdol = enemyWarCtrlr.ItemIdol;
		this.itemsToSteal.Add(this.enemyIdol);
		this.searchToCheck.Add(warCtrlr.wagon.chest);
		this.counter.y = 1f;
	}

	public ObjectiveGranTheftCart(global::PrimaryObjectiveId id, global::WarbandId enemyWarbandId) : base(id)
	{
		global::WarbandData warbandData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>((int)enemyWarbandId);
		this.enemyIdol = new global::Item(warbandData.ItemIdIdol, global::ItemQualityId.NORMAL);
	}

	protected override void Track(ref bool objectivesChanged)
	{
		base.CheckItemsToSteal(ref objectivesChanged);
	}

	public override void Reload(uint trackedUid)
	{
		throw new global::System.NotImplementedException();
	}

	private global::Item enemyIdol;
}
