using System;
using RAIN.Action;
using RAIN.Core;

public class AIBlackListAll : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "BlackListAll";
		this.unitCtrlr.GetWarband().BlackListAll();
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
	}
}
