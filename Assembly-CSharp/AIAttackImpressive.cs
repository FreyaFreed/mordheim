using System;
using RAIN.Core;

public class AIAttackImpressive : global::AIAttackBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AttackImpressive";
	}

	protected override bool ByPassLimit(global::UnitController target)
	{
		return target.unit.Data.UnitTypeId == global::UnitTypeId.IMPRESSIVE;
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
