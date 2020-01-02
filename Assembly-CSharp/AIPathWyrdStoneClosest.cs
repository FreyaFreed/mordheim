using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathWyrdStoneClosest : global::AIPathSearchBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathWyrdStoneClosest";
	}

	public override global::System.Collections.Generic.List<global::SearchPoint> GetTargets()
	{
		return global::PandoraSingleton<global::MissionManager>.Instance.GetWyrdstonePoints();
	}
}
