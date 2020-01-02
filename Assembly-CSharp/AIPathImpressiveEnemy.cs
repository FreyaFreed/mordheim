using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathImpressiveEnemy : global::AIPathUnitBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathImpressiveEnemy";
	}

	protected override bool CheckAllies()
	{
		return false;
	}

	protected override void SetTargets(global::System.Collections.Generic.List<global::UnitController> enemies)
	{
		for (int i = 0; i < enemies.Count; i++)
		{
			if (enemies[i].unit.GetUnitTypeId() == global::UnitTypeId.IMPRESSIVE)
			{
				this.targets.Add(enemies[i]);
			}
		}
	}
}
