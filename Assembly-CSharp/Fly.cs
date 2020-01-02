using System;
using System.Collections.Generic;
using UnityEngine;

public class Fly : global::ICheapState
{
	public Fly(global::UnitController ctrlr)
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
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.SetKinemantic(true);
		this.unitCtrlr.currentActionData.SetAction(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_name_fly"), global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/fly", true));
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("fly", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	public void FlyToPoint()
	{
		this.unitCtrlr.SetFixed(this.bestPoint.transform.position, true);
		this.unitCtrlr.transform.rotation = this.bestPoint.transform.rotation;
		this.unitCtrlr.Imprint.alwaysHide = false;
		this.unitCtrlr.Imprint.needsRefresh = true;
	}

	public void OnSeqDone()
	{
		this.unitCtrlr.StateMachine.ChangeState(10);
	}

	private global::UnitController unitCtrlr;

	private global::FlyPoint bestPoint;
}
