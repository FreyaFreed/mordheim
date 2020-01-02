using System;
using RAIN.Core;

public class AIAttackWeakest : global::AIAttackHealthiest
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AttackWeakest";
	}

	protected override bool IsBetter(int currentVal, int val)
	{
		return currentVal < val;
	}
}
