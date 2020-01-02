using System;
using RAIN.Core;
using RAIN.Representation;

public class AIHasEnemyInSightParam : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasEnemyInSight";
		this.success = (this.unitCtrlr.HasEnemyInSight(this.distance.Evaluate<float>(0f, null)) || this.unitCtrlr.Engaged || this.unitCtrlr.AICtrlr.hasSeenEnemy);
	}

	public global::RAIN.Representation.Expression distance;
}
