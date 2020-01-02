using System;
using RAIN.Core;
using RAIN.Representation;
using UnityEngine;

public class AIHasLowHealth : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasLowHealth";
		int num = this.expr.Evaluate<int>(global::UnityEngine.Time.deltaTime, null);
		this.success = ((float)this.unitCtrlr.unit.CurrentWound <= (float)(this.unitCtrlr.unit.Wound * num) / 100f);
	}

	public global::RAIN.Representation.Expression expr;
}
