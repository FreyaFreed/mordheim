using System;
using RAIN.Core;

public class AIAttackHealthiest : global::AIAttackBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AttackHealthiest";
	}

	protected override bool ByPassLimit(global::UnitController target)
	{
		return false;
	}

	protected override bool IsBetter(int currentVal, int val)
	{
		return currentVal > val;
	}

	protected override int GetCriteriaValue(global::UnitController target)
	{
		return target.unit.CurrentWound;
	}
}
