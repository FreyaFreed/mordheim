using System;
using System.Collections.Generic;

public class PrepareFly : global::ICheapState
{
	public PrepareFly(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		if (this.unitCtrlr.AICtrlr == null)
		{
			global::System.Collections.Generic.List<global::DecisionPoint> decisionPoints = global::PandoraSingleton<global::MissionManager>.Instance.GetDecisionPoints(this.unitCtrlr, global::DecisionPointId.FLY, float.MaxValue, false);
			this.bestPoint = (global::FlyPoint)decisionPoints[0];
		}
		else
		{
			this.bestPoint = (global::FlyPoint)this.unitCtrlr.AICtrlr.targetDecisionPoint;
		}
		this.bestPoint.PointsChecker.UpdateControlPoints(this.unitCtrlr, global::PandoraSingleton<global::MissionManager>.Instance.GetAllAliveUnits());
		if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
		{
			global::PandoraSingleton<global::MissionManager>.Instance.MoveUnitsOnActionZone(this.unitCtrlr, this.bestPoint.PointsChecker, this.bestPoint.PointsChecker.alliesOnZone, false);
			global::PandoraSingleton<global::MissionManager>.Instance.MoveUnitsOnActionZone(this.unitCtrlr, this.bestPoint.PointsChecker, this.bestPoint.PointsChecker.enemiesOnZone, true);
		}
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.SetKinemantic(true);
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
		if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
		{
			this.unitCtrlr.SendFly();
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::UnitController unitCtrlr;

	private global::FlyPoint bestPoint;
}
