using System;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using UnityEngine.Events;

public abstract class AIPathSearchBase : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathUnit";
		if (this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE || this.unitCtrlr.unit.BothArmsMutated())
		{
			this.success = false;
			this.currentResult = global::RAIN.Action.RAINAction.ActionResult.FAILURE;
			return;
		}
		global::System.Collections.Generic.List<global::SearchPoint> targets = this.GetTargets();
		for (int i = targets.Count - 1; i >= 0; i--)
		{
			if (this.unitCtrlr.AICtrlr.AlreadyLootSearchPoint(targets[i]))
			{
				targets.RemoveAt(i);
			}
		}
		if (targets.Count > 0)
		{
			this.currentResult = global::RAIN.Action.RAINAction.ActionResult.RUNNING;
			this.unitCtrlr.AICtrlr.FindPath(targets, new global::UnityEngine.Events.UnityAction<bool>(this.OnSearchChecked));
		}
		else
		{
			this.success = false;
			this.currentResult = global::RAIN.Action.RAINAction.ActionResult.FAILURE;
		}
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		return this.currentResult;
	}

	public override void Stop(global::RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}

	public abstract global::System.Collections.Generic.List<global::SearchPoint> GetTargets();

	private void OnSearchChecked(bool success)
	{
		success &= (this.unitCtrlr.AICtrlr.currentPath != null);
		this.success = success;
		this.currentResult = ((!success) ? global::RAIN.Action.RAINAction.ActionResult.FAILURE : global::RAIN.Action.RAINAction.ActionResult.SUCCESS);
	}

	private global::RAIN.Action.RAINAction.ActionResult currentResult;
}
