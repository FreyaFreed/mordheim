using System;

public class InteractiveTarget
{
	public InteractiveTarget(global::ActionStatus action, global::InteractivePoint point)
	{
		this.action = action;
		this.point = point;
	}

	public global::ActionStatus action;

	public global::InteractivePoint point;
}
