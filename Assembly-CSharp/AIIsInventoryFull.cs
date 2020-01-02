using System;
using RAIN.Core;

public class AIIsInventoryFull : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "IsInventoryFull";
		this.success = this.unitCtrlr.unit.IsInventoryFull();
	}
}
