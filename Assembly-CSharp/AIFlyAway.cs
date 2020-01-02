using System;
using System.Collections.Generic;
using RAIN.Core;
using UnityEngine;

public class AIFlyAway : global::AIFlyCombat
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIFlyAway";
	}

	protected override void SetBestFlyPoint(global::System.Collections.Generic.List<global::DecisionPoint> flyPoints, global::System.Collections.Generic.List<global::UnitController> allAliveUnits)
	{
		int num = int.MaxValue;
		float num2 = 0f;
		for (int i = 0; i < flyPoints.Count; i++)
		{
			global::FlyPoint flyPoint = (global::FlyPoint)flyPoints[i];
			flyPoint.PointsChecker.UpdateControlPoints(this.unitCtrlr, allAliveUnits);
			if (flyPoint.PointsChecker.enemiesOnZone.Count <= num)
			{
				float num3 = global::UnityEngine.Vector3.SqrMagnitude(this.unitCtrlr.transform.position - flyPoints[i].transform.position);
				if (num3 > num2)
				{
					num = flyPoint.PointsChecker.enemiesOnZone.Count;
					num2 = num3;
					this.unitCtrlr.AICtrlr.targetDecisionPoint = flyPoint;
				}
			}
		}
	}
}
