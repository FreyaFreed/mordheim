using System;
using RAIN.Core;

public class AIHasAltRangeWpn : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasAltRangeWpn";
		this.success = this.unitCtrlr.IsAltRange();
	}
}
