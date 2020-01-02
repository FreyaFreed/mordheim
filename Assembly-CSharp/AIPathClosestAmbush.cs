using System;

public class AIPathClosestAmbush : global::AIPathDecisionBase
{
	protected override global::DecisionPointId GetDecisionId()
	{
		return global::DecisionPointId.AMBUSH;
	}
}
