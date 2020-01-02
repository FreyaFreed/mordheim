using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIEmptyInventoryCart : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "EmptyInventoryCart";
		this.success = (this.unitCtrlr.GetWarband().wagon != null && this.unitCtrlr.AICtrlr.targetSearchPoint == this.unitCtrlr.GetWarband().wagon.chest && !this.unitCtrlr.AICtrlr.targetSearchPoint.IsFull() && !this.unitCtrlr.unit.IsInventoryEmpty());
		if (this.success)
		{
			int num = -1;
			int num2 = -1;
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			for (int i = 6; i < this.unitCtrlr.unit.Items.Count; i++)
			{
				if (this.unitCtrlr.unit.Items[i].Id != global::ItemId.NONE)
				{
					list.Add(i);
				}
			}
			if (list.Count > 0)
			{
				num = list[global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, list.Count)] - 6;
			}
			list = new global::System.Collections.Generic.List<int>();
			global::System.Collections.Generic.List<global::Item> items = this.unitCtrlr.AICtrlr.targetSearchPoint.GetItems();
			for (int j = 0; j < items.Count; j++)
			{
				if (items[j].Id == global::ItemId.NONE)
				{
					list.Add(j);
				}
			}
			if (list.Count > 0)
			{
				num2 = list[global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, list.Count)];
			}
			if (num2 != -1 && num != -1)
			{
				this.unitCtrlr.SendInventoryChange(num2, num);
			}
			else
			{
				this.success = false;
			}
		}
	}
}
