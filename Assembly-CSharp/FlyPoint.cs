using System;

public class FlyPoint : global::DecisionPoint
{
	private void Awake()
	{
		this.PointsChecker = new global::EllipsePointsChecker(base.transform, false, global::Constant.GetFloat(global::ConstantId.MELEE_RANGE_VERY_LARGE), global::Constant.GetFloat(global::ConstantId.MELEE_RANGE_VERY_VERY_LARGE));
	}

	public global::EllipsePointsChecker PointsChecker;
}
