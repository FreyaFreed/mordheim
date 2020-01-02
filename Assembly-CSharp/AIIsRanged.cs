using System;
using RAIN.Core;

public class AIIsRanged : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "IsRanged";
		this.success = false;
	}
}
