using System;

public class UIStatCritical : global::UIStat
{
	public override void RefreshAttribute(global::Unit unit)
	{
		this.statId = ((!unit.HasRange()) ? global::AttributeId.CRITICAL_MELEE_ATTEMPT_ROLL : global::AttributeId.CRITICAL_RANGE_ATTEMPT_ROLL);
		base.RefreshAttribute(unit);
	}
}
