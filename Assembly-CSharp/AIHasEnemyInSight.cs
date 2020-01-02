using System;
using RAIN.Core;

public class AIHasEnemyInSight : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasEnemyInSight";
		this.success = (this.unitCtrlr.HasEnemyInSight() || this.unitCtrlr.Engaged || this.unitCtrlr.AICtrlr.hasSeenEnemy);
	}
}
