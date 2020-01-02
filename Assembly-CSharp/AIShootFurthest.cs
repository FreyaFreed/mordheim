using System;
using RAIN.Core;

public class AIShootFurthest : global::AIShootClosest
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "ShootFurthest";
	}

	protected override bool IsBetter(int currentDist, int dist)
	{
		return currentDist > dist;
	}
}
