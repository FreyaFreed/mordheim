using System;
using RAIN.Core;

public class AIHasIdolInSight : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasIdolInSight";
		this.success = this.unitCtrlr.HasIdolInSight();
	}
}
