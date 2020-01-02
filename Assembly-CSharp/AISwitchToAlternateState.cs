using System;
using RAIN.Core;

public class AISwitchToAlternateState : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "SwitchToActiveState";
		this.success = this.unitCtrlr.AICtrlr.GotoAlternateMode();
	}
}
