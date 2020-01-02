using System;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

public class AIKillUnit : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "KillUnit";
		this.target = null;
		this.skillId = ((!this.unitCtrlr.Engaged) ? global::SkillId.BASE_SHOOT : global::SkillId.BASE_ATTACK);
		global::ActionStatus action = this.unitCtrlr.GetAction(this.skillId);
		action.UpdateAvailable();
		int num = (action.GetMinDamage(false) + action.GetMaxDamage(false)) / 2;
		global::System.Collections.Generic.List<global::UnitController> targets = action.Targets;
		for (int i = 0; i < targets.Count; i++)
		{
			if (targets[i].unit.CurrentWound <= num)
			{
				this.target = targets[i];
				break;
			}
		}
		this.success = (this.target != null);
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.success)
		{
			this.unitCtrlr.SendSkillSingleTarget(this.skillId, this.target);
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
	}

	private global::SkillId skillId;

	private global::UnitController target;
}
