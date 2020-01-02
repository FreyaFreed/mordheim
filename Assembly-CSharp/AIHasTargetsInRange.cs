using System;
using RAIN.Core;

public class AIHasTargetsInRange : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIHasTargetsInRange";
		global::ActionStatus action = this.unitCtrlr.GetAction(global::SkillId.BASE_SHOOT);
		action.SetTargets();
		this.success = (action.Targets.Count > 0);
	}
}
