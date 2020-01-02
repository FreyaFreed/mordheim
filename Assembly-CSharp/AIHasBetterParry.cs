using System;
using RAIN.Core;

public class AIHasBetterParry : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasBetterParry";
		this.success = (this.unitCtrlr.unit.ParryingRoll >= this.unitCtrlr.unit.DodgeRoll);
	}
}
