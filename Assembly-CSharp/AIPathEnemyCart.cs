using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathEnemyCart : global::AIPathSearchBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathEnemyCart";
	}

	public override global::System.Collections.Generic.List<global::SearchPoint> GetTargets()
	{
		global::System.Collections.Generic.List<global::SearchPoint> list = new global::System.Collections.Generic.List<global::SearchPoint>();
		int teamIdx = this.unitCtrlr.GetWarband().teamIdx;
		foreach (global::WarbandController warbandController in global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs)
		{
			if (warbandController.teamIdx != teamIdx)
			{
				list.Add(warbandController.wagon.chest);
			}
		}
		return list;
	}
}
