using System;
using RAIN.Core;

public class AIShootImpressive : global::AIShootBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "ShootImpressive";
	}

	protected override bool ByPassLimit(global::UnitController current)
	{
		return current.unit.GetUnitTypeId() == global::UnitTypeId.IMPRESSIVE;
	}

	protected override bool IsBetter(int currentVal, int val)
	{
		return false;
	}

	protected override int GetCriteriaValue(global::UnitController current)
	{
		return 0;
	}
}
