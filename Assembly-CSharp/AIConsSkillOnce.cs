using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIConsSkillOnce : global::AIConsSkillSpell
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIConsSkillOnce";
	}

	protected override bool IsValid(global::ActionStatus action, global::UnitController target)
	{
		this.unitCtrlr.AICtrlr.preFight = true;
		return !this.unitCtrlr.AICtrlr.hasCastSkill && base.IsValid(action, target);
	}

	protected override global::System.Collections.Generic.List<global::UnitActionId> GetRelatedActions()
	{
		return global::AIController.consSkillActions;
	}

	public override void Stop(global::RAIN.Core.AI ai)
	{
		base.Stop(ai);
		this.unitCtrlr.AICtrlr.preFight = false;
	}
}
