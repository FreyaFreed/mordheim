using System;
using RAIN.Action;
using RAIN.Core;

public class AIReloadWeapon : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "ReloadWeapon";
		this.success = this.unitCtrlr.GetAction(global::SkillId.BASE_RELOAD).Available;
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.success)
		{
			this.unitCtrlr.SendSkill(global::SkillId.BASE_RELOAD);
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
	}
}
