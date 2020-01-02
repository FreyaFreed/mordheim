using System;
using RAIN.Action;
using RAIN.Core;

public class AIEndTurn : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "EndTurn";
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		this.unitCtrlr.SendSkill(global::SkillId.BASE_END_TURN);
		return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
	}
}
