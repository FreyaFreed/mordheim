using System;
using RAIN.Core;

public class AICanMove : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "CanMove";
		this.success = (this.unitCtrlr.unit.CurrentStrategyPoints > 0);
	}
}
