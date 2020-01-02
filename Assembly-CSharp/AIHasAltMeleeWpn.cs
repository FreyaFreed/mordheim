using System;
using RAIN.Core;

public class AIHasAltMeleeWpn : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasAltMeleeWpn";
		this.success = this.unitCtrlr.IsAltClose();
	}
}
