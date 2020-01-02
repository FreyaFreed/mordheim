using System;
using System.Collections.Generic;

public class AIShootBase : global::AIBaseAction
{
	protected override bool IsValid(global::ActionStatus action, global::UnitController target)
	{
		return target.GetWarband().teamIdx != this.unitCtrlr.GetWarband().teamIdx;
	}

	protected override bool ByPassLimit(global::UnitController current)
	{
		return true;
	}

	protected override int GetCriteriaValue(global::UnitController current)
	{
		return 0;
	}

	protected override bool IsBetter(int currentDist, int dist)
	{
		return true;
	}

	protected override global::System.Collections.Generic.List<global::UnitActionId> GetRelatedActions()
	{
		return global::AIController.shootActions;
	}
}
