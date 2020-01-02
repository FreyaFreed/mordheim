using System;
using System.Collections.Generic;

public class Shrine : global::ActivatePoint
{
	public override void Init(uint id)
	{
		base.Init(id);
		this.reverse = true;
		this.shrineData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ShrineData>((int)this.shrineId);
		this.shrineEnchantments = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ShrineEnchantmentData>("fk_shrine_id", ((int)this.shrineData.Id).ToString());
	}

	public override void Activate(global::UnitController unitCtrlr, bool force = false)
	{
		if (unitCtrlr == null)
		{
			return;
		}
		this.activated = false;
		base.Activate(unitCtrlr, false);
		int rank = unitCtrlr.GetWarband().Rank;
		unitCtrlr.unit.AddToAttribute(global::AttributeId.TOTAL_PRAY, 1);
		for (int i = 0; i < this.shrineEnchantments.Count; i++)
		{
			if (this.shrineEnchantments[i].WarbandRankId == (global::WarbandRankId)rank)
			{
				unitCtrlr.unit.AddEnchantment(this.shrineEnchantments[i].EnchantmentId, unitCtrlr.unit, false, true, global::AllegianceId.NONE);
			}
		}
	}

	protected override bool LinkValid(global::UnitController unitCtrlr, bool reverseCondition)
	{
		return !this.CanInteract(unitCtrlr);
	}

	public global::ShrineId shrineId;

	private global::ShrineData shrineData;

	private global::System.Collections.Generic.List<global::ShrineEnchantmentData> shrineEnchantments;
}
