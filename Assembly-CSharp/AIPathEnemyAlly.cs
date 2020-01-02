using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathEnemyAlly : global::AIPathUnitBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathEnemyAlly";
	}

	protected override bool CheckAllies()
	{
		return true;
	}

	protected override void SetTargets(global::System.Collections.Generic.List<global::UnitController> allies)
	{
		for (int i = 0; i < allies.Count; i++)
		{
			if (allies[i] != this.unitCtrlr && allies[i].Engaged)
			{
				this.targets.AddRange(allies[i].EngagedUnits);
			}
		}
	}
}
