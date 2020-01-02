using System;
using RAIN.Core;

public class AIAttackEnemyDown : global::AIAttackBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AttackEnemyDown";
	}

	protected override bool IsValid(global::ActionStatus action, global::UnitController target)
	{
		return target.unit.Status == global::UnitStateId.STUNNED && base.IsValid(action, target);
	}

	protected override bool ByPassLimit(global::UnitController target)
	{
		return true;
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
