using System;
using RAIN.Action;
using RAIN.Core;

public class AIFleeEngaged : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "FleeEngaged";
		this.unitCtrlr.AICtrlr.AddEngagedToExcluded();
		this.success = (this.unitCtrlr.AICtrlr.disengageCount == 0 && this.unitCtrlr.GetAction(global::SkillId.BASE_FLEE).Available);
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.success)
		{
			this.unitCtrlr.SendSkill(global::SkillId.BASE_FLEE);
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
	}
}
