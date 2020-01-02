using System;
using RAIN.Core;

public class AIHasMeleeWpn : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasMeleeWpn";
		this.success = this.unitCtrlr.HasClose();
	}
}
