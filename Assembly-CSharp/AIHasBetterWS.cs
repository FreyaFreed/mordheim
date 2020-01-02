using System;
using RAIN.Core;

public class AIHasBetterWS : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIHasBetterWS";
		this.success = ((float)this.unitCtrlr.unit.WeaponSkill >= (float)this.unitCtrlr.unit.BallisticSkill * 0.8f);
	}

	private const float BS_RATIO = 0.8f;
}
