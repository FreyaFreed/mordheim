using System;
using RAIN.Core;

public class AIHasSpottedEnemies : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIHasSpottedEnemies";
		this.success = (this.unitCtrlr.GetWarband().SquadManager.GetSpottedEnemies().Count > 0 || this.unitCtrlr.Engaged || this.unitCtrlr.beenShot);
	}
}
