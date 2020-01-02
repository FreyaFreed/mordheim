using System;
using RAIN.Core;
using RAIN.Representation;

public class AIIsTargetHealthBelow : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AICanTargetCounter";
		float num = this.expr.Evaluate<float>(0f, null);
		this.success = false;
		if (this.unitCtrlr.defenderCtrlr != null)
		{
			float num2 = (float)(this.unitCtrlr.defenderCtrlr.unit.CurrentWound / this.unitCtrlr.defenderCtrlr.unit.Wound);
			this.success = (num2 < num);
		}
	}

	public global::RAIN.Representation.Expression expr;
}
