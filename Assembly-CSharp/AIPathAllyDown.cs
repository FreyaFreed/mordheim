using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathAllyDown : global::AIPathUnitBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathAllyDown";
	}

	protected override bool CheckAllies()
	{
		return true;
	}

	protected override void SetTargets(global::System.Collections.Generic.List<global::UnitController> allies)
	{
		for (int i = 0; i < allies.Count; i++)
		{
			if (allies[i] != this.unitCtrlr && (allies[i].unit.Status == global::UnitStateId.KNOCKED_DOWN || allies[i].unit.Status == global::UnitStateId.STUNNED) && allies[i].Engaged)
			{
				this.targets.Add(allies[i]);
			}
		}
	}
}
