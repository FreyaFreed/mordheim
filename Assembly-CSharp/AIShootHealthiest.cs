using System;
using RAIN.Core;

public class AIShootHealthiest : global::AIShootBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "ShootHealthiest";
	}

	protected override bool ByPassLimit(global::UnitController current)
	{
		return false;
	}

	protected override bool IsBetter(int currentVal, int val)
	{
		return currentVal > val;
	}

	protected override int GetCriteriaValue(global::UnitController current)
	{
		return current.unit.CurrentWound;
	}
}
