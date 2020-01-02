using System;
using System.Collections.Generic;

public class AIAttackBase : global::AIBaseAction
{
	protected override bool IsValid(global::ActionStatus action, global::UnitController target)
	{
		return true;
	}

	protected override bool ByPassLimit(global::UnitController target)
	{
		throw new global::System.NotImplementedException();
	}

	protected override bool IsBetter(int currentVal, int val)
	{
		throw new global::System.NotImplementedException();
	}

	protected override int GetCriteriaValue(global::UnitController target)
	{
		throw new global::System.NotImplementedException();
	}

	protected override global::System.Collections.Generic.List<global::UnitActionId> GetRelatedActions()
	{
		return global::AIController.attackActions;
	}
}
