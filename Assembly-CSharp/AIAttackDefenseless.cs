using System;
using RAIN.Core;

public class AIAttackDefenseless : global::AIAttackBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIAttackDefenseless";
	}

	protected override bool ByPassLimit(global::UnitController target)
	{
		return !target.CanCounterAttack();
	}

	protected override bool IsBetter(int currentVal, int val)
	{
		return false;
	}

	protected override int GetCriteriaValue(global::UnitController target)
	{
		return 0;
	}
}
