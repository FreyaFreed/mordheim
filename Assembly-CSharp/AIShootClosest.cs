using System;
using RAIN.Core;
using UnityEngine;

public class AIShootClosest : global::AIShootBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "ShootClosest";
	}

	protected override bool ByPassLimit(global::UnitController current)
	{
		return false;
	}

	protected override int GetCriteriaValue(global::UnitController current)
	{
		return (int)global::UnityEngine.Vector3.SqrMagnitude(current.transform.position - this.unitCtrlr.transform.position);
	}

	protected override bool IsBetter(int currentDist, int dist)
	{
		return currentDist < dist;
	}
}
