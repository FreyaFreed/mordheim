using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathCorpse : global::AIPathSearchBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathCorpse";
	}

	public override global::System.Collections.Generic.List<global::SearchPoint> GetTargets()
	{
		int num = this.unitCtrlr.unit.CurrentStrategyPoints;
		num -= this.unitCtrlr.GetAction(global::SkillId.BASE_SEARCH).StrategyPoints;
		if (num > 0 && !this.unitCtrlr.unit.IsInventoryFull())
		{
			float dist = (float)this.unitCtrlr.unit.Movement;
			global::System.Collections.Generic.List<global::SearchPoint> searchPointInRadius = global::PandoraSingleton<global::MissionManager>.Instance.GetSearchPointInRadius(this.unitCtrlr.transform.position, dist, global::UnitActionId.SEARCH);
			for (int i = searchPointInRadius.Count - 1; i >= 0; i--)
			{
				if (searchPointInRadius[i].unitController == null)
				{
					searchPointInRadius.RemoveAt(i);
				}
			}
			return searchPointInRadius;
		}
		return new global::System.Collections.Generic.List<global::SearchPoint>();
	}
}
