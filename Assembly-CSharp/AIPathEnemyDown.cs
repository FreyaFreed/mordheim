using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathEnemyDown : global::AIPathUnitBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		this.actionName = "PathEnemyDown";
		base.Start(ai);
	}

	protected override bool CheckAllies()
	{
		return false;
	}

	protected override void SetTargets(global::System.Collections.Generic.List<global::UnitController> enemies)
	{
		for (int i = 0; i < enemies.Count; i++)
		{
			if (enemies[i].unit.Status == global::UnitStateId.KNOCKED_DOWN || enemies[i].unit.Status == global::UnitStateId.STUNNED)
			{
				this.targets.Add(enemies[i]);
			}
		}
	}
}
