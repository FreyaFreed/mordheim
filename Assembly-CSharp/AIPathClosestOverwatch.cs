using System;

public class AIPathClosestOverwatch : global::AIPathDecisionBase
{
	protected override global::DecisionPointId GetDecisionId()
	{
		return global::DecisionPointId.OVERWATCH;
	}
}
