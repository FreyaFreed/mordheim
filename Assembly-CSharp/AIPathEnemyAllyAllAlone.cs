using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIPathEnemyAllyAllAlone : global::AIPathUnitBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathEnemyAllyAllAlone";
	}

	protected override bool CheckAllies()
	{
		return false;
	}

	protected override void SetTargets(global::System.Collections.Generic.List<global::UnitController> enemies)
	{
		int teamIdx = this.unitCtrlr.GetWarband().teamIdx;
		for (int i = 0; i < enemies.Count; i++)
		{
			for (int j = 0; j < enemies[i].EngagedUnits.Count; j++)
			{
				if (enemies[i].EngagedUnits[j].GetWarband().teamIdx == teamIdx && enemies[i].EngagedUnits[j].IsAllAlone())
				{
					this.targets.Add(enemies[i]);
				}
			}
		}
	}
}
