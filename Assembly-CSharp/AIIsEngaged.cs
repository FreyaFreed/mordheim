using System;
using RAIN.Core;

public class AIIsEngaged : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "IsEngaged";
		if (this.unitCtrlr.Engaged)
		{
			this.unitCtrlr.AICtrlr.atDestination = false;
		}
		this.success = this.unitCtrlr.Engaged;
	}
}
