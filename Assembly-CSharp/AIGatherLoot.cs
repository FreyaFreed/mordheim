using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIGatherLoot : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "GatherLoot";
		global::WarbandWagon wagon = this.unitCtrlr.GetWarband().wagon;
		this.success = (!this.unitCtrlr.unit.IsInventoryFull() && (wagon == null || this.unitCtrlr.AICtrlr.targetSearchPoint != wagon.chest) && !this.unitCtrlr.AICtrlr.targetSearchPoint.IsEmpty());
		int num = -1;
		if (this.success)
		{
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			for (int i = 6; i < this.unitCtrlr.unit.Items.Count; i++)
			{
				if (this.unitCtrlr.unit.Items[i].Id == global::ItemId.NONE)
				{
					list.Add(i);
				}
			}
			if (list.Count > 0)
			{
				num = list[global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, list.Count)] - 6;
			}
			global::System.Collections.Generic.List<global::Item> items = this.unitCtrlr.AICtrlr.targetSearchPoint.GetItems();
			int searchSlot = this.GetSearchSlot(items);
			if (searchSlot != -1 && num != -1)
			{
				this.unitCtrlr.SendInventoryChange(searchSlot, num);
			}
			else
			{
				this.success = false;
			}
		}
	}

	protected virtual int GetSearchSlot(global::System.Collections.Generic.List<global::Item> searchItems)
	{
		global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
		for (int i = 0; i < searchItems.Count; i++)
		{
			if (searchItems[i].Id != global::ItemId.NONE)
			{
				list.Add(i);
			}
		}
		if (list.Count > 0)
		{
			return list[global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, list.Count)];
		}
		return -1;
	}
}
