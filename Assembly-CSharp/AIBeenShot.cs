using System;
using RAIN.Core;

public class AIBeenShot : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "BeenShot";
		this.success = this.unitCtrlr.beenShot;
		this.unitCtrlr.beenShot = false;
	}
}
