using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathTreasure : global::AIPathSearchBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathTreasure";
	}

	public override global::System.Collections.Generic.List<global::SearchPoint> GetTargets()
	{
		int num = this.unitCtrlr.unit.CurrentStrategyPoints;
		num -= this.unitCtrlr.GetAction(global::SkillId.BASE_SEARCH).StrategyPoints;
		if (num > 0 && !this.unitCtrlr.unit.IsInventoryFull())
		{
			float dist = (float)(num * this.unitCtrlr.unit.Movement);
			return global::PandoraSingleton<global::MissionManager>.Instance.GetSearchPointInRadius(this.unitCtrlr.transform.position, dist, global::UnitActionId.SEARCH);
		}
		return new global::System.Collections.Generic.List<global::SearchPoint>();
	}
}
