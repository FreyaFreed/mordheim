using System;
using System.Collections.Generic;
using RAIN.Core;
using UnityEngine;

public class AIConsSkillReserve : global::AIConsSkillSpell
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIConsSkillReserve";
	}

	protected override bool IsValid(global::ActionStatus action, global::UnitController target)
	{
		return action.OffensePoints <= global::UnityEngine.Mathf.Max(this.unitCtrlr.unit.CurrentOffensePoints - 2, 0) && action.StrategyPoints <= global::UnityEngine.Mathf.Max(this.unitCtrlr.unit.CurrentStrategyPoints - 2, 0) && base.IsValid(action, target);
	}

	protected override global::System.Collections.Generic.List<global::UnitActionId> GetRelatedActions()
	{
		return global::AIController.consSkillActions;
	}

	public override void Stop(global::RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}
}
