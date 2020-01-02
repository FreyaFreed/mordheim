using System;
using RAIN.Action;
using RAIN.Core;

public class AISearch : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "Search";
		this.success = (this.unitCtrlr.AICtrlr.targetSearchPoint != null && !this.unitCtrlr.AICtrlr.AlreadyLootSearchPoint(this.unitCtrlr.AICtrlr.targetSearchPoint) && this.unitCtrlr.interactivePoints.IndexOf(this.unitCtrlr.AICtrlr.targetSearchPoint) != -1 && this.unitCtrlr.GetAction(global::SkillId.BASE_SEARCH).Available);
		if (this.success)
		{
			this.success = false;
			for (int i = 0; i < this.unitCtrlr.interactivePoints.Count; i++)
			{
				if (this.unitCtrlr.interactivePoints[i] == this.unitCtrlr.AICtrlr.targetSearchPoint)
				{
					this.success = true;
					break;
				}
			}
		}
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.success)
		{
			this.unitCtrlr.AICtrlr.lootedSearchPoints.Add(this.unitCtrlr.AICtrlr.targetSearchPoint);
			this.unitCtrlr.SendInteractiveAction(global::SkillId.BASE_SEARCH, this.unitCtrlr.AICtrlr.targetSearchPoint);
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
	}
}
