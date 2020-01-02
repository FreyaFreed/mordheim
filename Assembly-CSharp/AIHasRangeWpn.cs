using System;
using RAIN.Core;

public class AIHasRangeWpn : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasRangeWpn";
		this.success = this.unitCtrlr.HasRange();
	}
}
