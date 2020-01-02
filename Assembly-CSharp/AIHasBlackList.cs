using System;
using RAIN.Core;

public class AIHasBlackList : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasBlackList";
		this.success = (this.unitCtrlr.GetWarband().BlackList != 0);
	}
}
