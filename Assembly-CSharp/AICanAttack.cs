using System;
using RAIN.Core;

public class AICanAttack : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "CanAttack";
		this.success = ((this.unitCtrlr.HasClose() || this.unitCtrlr.IsAltClose()) && this.unitCtrlr.HasEnemyInSight());
	}
}
