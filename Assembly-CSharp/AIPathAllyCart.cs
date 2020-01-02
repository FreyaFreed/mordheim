using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathAllyCart : global::AIPathSearchBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathAllyCart";
	}

	public override global::System.Collections.Generic.List<global::SearchPoint> GetTargets()
	{
		return new global::System.Collections.Generic.List<global::SearchPoint>
		{
			this.unitCtrlr.GetWarband().wagon.chest
		};
	}
}
