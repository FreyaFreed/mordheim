using System;
using RAIN.Action;
using RAIN.Core;

public class AIStanceParry : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "StanceParry";
		this.success = this.unitCtrlr.GetAction(global::SkillId.BASE_STANCE_PARRY).Available;
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.success)
		{
			this.unitCtrlr.SendSkill(global::SkillId.BASE_STANCE_PARRY);
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
	}
}
