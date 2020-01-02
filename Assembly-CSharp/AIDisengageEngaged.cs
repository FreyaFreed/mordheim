using System;
using RAIN.Action;
using RAIN.Core;

public class AIDisengageEngaged : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "DisengageEngaged";
		this.unitCtrlr.AICtrlr.AddEngagedToExcluded();
		this.success = (this.unitCtrlr.AICtrlr.disengageCount == 0 && this.unitCtrlr.GetAction(global::SkillId.BASE_DISENGAGE).Available);
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.success)
		{
			this.unitCtrlr.AICtrlr.disengageCount++;
			this.unitCtrlr.SendSkill(global::SkillId.BASE_DISENGAGE);
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
	}
}
