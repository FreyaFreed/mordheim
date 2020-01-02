using System;
using RAIN.Action;
using RAIN.Core;
using UnityEngine.Events;

public abstract class AIPathDecisionBase : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIPathClosestOverwatch";
		this.currentResult = global::RAIN.Action.RAINAction.ActionResult.RUNNING;
		float dist = (float)(this.unitCtrlr.unit.Movement * 2);
		this.unitCtrlr.AICtrlr.FindPath(this.GetDecisionId(), dist, new global::UnityEngine.Events.UnityAction<bool>(this.AllPathChecked));
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		return this.currentResult;
	}

	private void AllPathChecked(bool pathSuccess)
	{
		pathSuccess &= (this.unitCtrlr.AICtrlr.currentPath != null);
		this.success = pathSuccess;
		this.currentResult = ((!this.success) ? global::RAIN.Action.RAINAction.ActionResult.FAILURE : global::RAIN.Action.RAINAction.ActionResult.SUCCESS);
	}

	protected abstract global::DecisionPointId GetDecisionId();

	private global::RAIN.Action.RAINAction.ActionResult currentResult;
}
