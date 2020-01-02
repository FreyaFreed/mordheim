using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathAllyLowHP : global::AIPathUnitBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathAllyLowHP";
	}

	protected override bool CheckAllies()
	{
		return true;
	}

	protected override void SetTargets(global::System.Collections.Generic.List<global::UnitController> allies)
	{
		for (int i = 0; i < allies.Count; i++)
		{
			if (allies[i] != this.unitCtrlr && (float)allies[i].unit.CurrentWound < (float)allies[i].unit.Wound * 0.35f && allies[i].Engaged)
			{
				this.targets.Add(allies[i]);
			}
		}
	}
}
