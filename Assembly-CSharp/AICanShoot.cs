using System;
using RAIN.Core;

public class AICanShoot : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "CanShoot";
		this.success = false;
		for (int i = 0; i < this.unitCtrlr.actionStatus.Count; i++)
		{
			if (this.unitCtrlr.actionStatus[i].IsShootAction() && this.unitCtrlr.actionStatus[i].Available)
			{
				this.success = true;
				return;
			}
		}
	}
}
