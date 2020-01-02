using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathWeakestEnemy : global::AIPathUnitBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathWeakestEnemy";
	}

	protected override bool CheckAllies()
	{
		return false;
	}

	protected override void SetTargets(global::System.Collections.Generic.List<global::UnitController> enemies)
	{
		for (int i = 0; i < enemies.Count; i++)
		{
			if (this.targets.Count == 0 || enemies[i].unit.CurrentWound < this.targets[0].unit.CurrentWound)
			{
				this.targets.Clear();
				this.targets.Add(enemies[i]);
			}
			else if (enemies[i].unit.CurrentWound == this.targets[0].unit.CurrentWound)
			{
				this.targets.Add(enemies[i]);
			}
		}
	}
}
