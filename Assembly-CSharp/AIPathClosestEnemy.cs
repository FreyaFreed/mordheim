using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathClosestEnemy : global::AIPathUnitBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathClosestEnemy";
	}

	protected override bool CheckAllies()
	{
		return false;
	}

	protected override void SetTargets(global::System.Collections.Generic.List<global::UnitController> all)
	{
		this.targets.AddRange(all);
	}
}
