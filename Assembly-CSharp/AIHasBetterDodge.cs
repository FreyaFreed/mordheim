using System;
using RAIN.Core;

public class AIHasBetterDodge : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasBetterDodge";
		this.success = (this.unitCtrlr.unit.DodgeRoll >= this.unitCtrlr.unit.ParryingRoll);
	}
}
