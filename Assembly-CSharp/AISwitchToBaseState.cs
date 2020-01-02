using System;
using RAIN.Action;
using RAIN.Core;

public class AISwitchToBaseState : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "SwitchToBaseState";
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		this.unitCtrlr.AICtrlr.GotoBaseMode();
		return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
	}
}
