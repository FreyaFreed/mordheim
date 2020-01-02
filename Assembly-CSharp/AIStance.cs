using System;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

public class AIStance : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "Stance";
		this.bestAction = this.unitCtrlr.AICtrlr.GetBestAction(global::AIController.stanceActions, out this.refTarget, delegate(global::ActionStatus action, global::System.Collections.Generic.List<global::UnitController> list)
		{
			list.Clear();
			list.Add(this.unitCtrlr);
		});
		this.success = (this.bestAction != null);
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.success)
		{
			this.unitCtrlr.SendSkill(this.bestAction.SkillId);
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
	}

	private global::ActionStatus bestAction;

	private global::UnitController refTarget;
}
