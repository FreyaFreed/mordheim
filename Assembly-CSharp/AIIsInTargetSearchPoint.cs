using System;
using RAIN.Core;

public class AIIsInTargetSearchPoint : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "IsInTargetSearchPoint";
		this.success = false;
		foreach (global::InteractivePoint interactivePoint in this.unitCtrlr.interactivePoints)
		{
			global::SearchPoint x = (global::SearchPoint)interactivePoint;
			if (x == this.unitCtrlr.AICtrlr.targetSearchPoint)
			{
				this.success = true;
				break;
			}
		}
	}
}
