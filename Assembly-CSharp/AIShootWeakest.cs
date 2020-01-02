using System;
using RAIN.Core;

public class AIShootWeakest : global::AIShootHealthiest
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "ShootWeakest";
	}

	protected override bool IsBetter(int currentVal, int val)
	{
		return currentVal < val;
	}
}
