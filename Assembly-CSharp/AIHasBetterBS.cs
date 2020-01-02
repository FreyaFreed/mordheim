using System;
using RAIN.Core;

public class AIHasBetterBS : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIHasBetterBS";
		this.success = ((float)this.unitCtrlr.unit.BallisticSkill >= (float)this.unitCtrlr.unit.WeaponSkill * 1.25f);
	}

	private const float WS_RATIO = 1.25f;
}
