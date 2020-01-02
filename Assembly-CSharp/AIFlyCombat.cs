using System;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using UnityEngine;

public class AIFlyCombat : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIFlyCombat";
		global::ActionStatus action = this.unitCtrlr.GetAction(global::SkillId.BASE_FLY);
		action.UpdateAvailable();
		this.success = action.Available;
		if (this.success)
		{
			this.unitCtrlr.AICtrlr.targetDecisionPoint = null;
			global::System.Collections.Generic.List<global::DecisionPoint> decisionPoints = global::PandoraSingleton<global::MissionManager>.Instance.GetDecisionPoints(this.unitCtrlr, global::DecisionPointId.FLY, float.MaxValue, false);
			global::System.Collections.Generic.List<global::UnitController> allAliveUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllAliveUnits();
			for (int i = decisionPoints.Count - 1; i >= 0; i--)
			{
				if (global::UnityEngine.Vector3.SqrMagnitude(this.unitCtrlr.transform.position - decisionPoints[i].transform.position) < 25f)
				{
					decisionPoints.RemoveAt(i);
				}
			}
			this.SetBestFlyPoint(decisionPoints, allAliveUnits);
			this.success = (this.unitCtrlr.AICtrlr.targetDecisionPoint != null);
		}
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.success)
		{
			this.unitCtrlr.SendSkill(global::SkillId.BASE_FLY);
		}
		return base.Execute(ai);
	}

	protected virtual void SetBestFlyPoint(global::System.Collections.Generic.List<global::DecisionPoint> flyPoints, global::System.Collections.Generic.List<global::UnitController> allAliveUnits)
	{
		int count = this.unitCtrlr.EngagedUnits.Count;
		for (int i = 0; i < flyPoints.Count; i++)
		{
			global::FlyPoint flyPoint = (global::FlyPoint)flyPoints[i];
			flyPoint.PointsChecker.UpdateControlPoints(this.unitCtrlr, allAliveUnits);
			if (flyPoint.PointsChecker.enemiesOnZone.Count > count)
			{
				count = flyPoint.PointsChecker.enemiesOnZone.Count;
				this.unitCtrlr.AICtrlr.targetDecisionPoint = flyPoint;
			}
		}
		if (this.unitCtrlr.AICtrlr.targetDecisionPoint == null && !this.unitCtrlr.Engaged)
		{
			float num = float.MaxValue;
			global::System.Collections.Generic.List<global::UnitController> aliveEnemies = global::PandoraSingleton<global::MissionManager>.Instance.GetAliveEnemies(this.unitCtrlr.GetWarband().idx);
			for (int j = 0; j < flyPoints.Count; j++)
			{
				for (int k = 0; k < aliveEnemies.Count; k++)
				{
					float num2 = global::UnityEngine.Vector3.SqrMagnitude(flyPoints[j].transform.position - aliveEnemies[k].transform.position);
					if (num2 < num)
					{
						num = num2;
						this.unitCtrlr.AICtrlr.targetDecisionPoint = flyPoints[j];
					}
				}
			}
		}
	}

	private const float MIN_VALID_SQR_DIST = 25f;
}
