using System;
using RAIN.Core;
using RAIN.Representation;

public class AIIsEngagedParam : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "IsEngaged";
		if (this.unitCtrlr.Engaged)
		{
			this.unitCtrlr.AICtrlr.atDestination = false;
		}
		this.success = (this.unitCtrlr.EngagedUnits.Count >= this.count.Evaluate<int>(0f, null));
	}

	public global::RAIN.Representation.Expression count;
}
