using System;
using System.Collections.Generic;
using Prometheus;
using UnityEngine;

public class Perception : global::ICheapState
{
	public Perception(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		int perceptionRoll = this.unitCtrlr.unit.PerceptionRoll;
		this.perceptionSuccess = this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, global::UnityEngine.Mathf.Min(perceptionRoll, global::Constant.GetInt(global::ConstantId.MAX_ROLL)), global::AttributeId.PERCEPTION_ROLL, false, true, 0);
		this.unitCtrlr.currentActionData.SetActionOutcome(this.perceptionSuccess);
		this.unitCtrlr.detectedUnits.Clear();
		this.unitCtrlr.detectedTriggers.Clear();
		this.unitCtrlr.detectedInteractivePoints.Clear();
		if (this.perceptionSuccess && this.unitCtrlr.IsPlayed())
		{
			this.unitCtrlr.detectedUnits.AddRange(global::PandoraSingleton<global::MissionManager>.Instance.GetAliveEnemies(this.unitCtrlr.unit.warbandIdx));
			global::System.Collections.Generic.List<global::TriggerPoint> triggerPoints = global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints;
			for (int i = 0; i < triggerPoints.Count; i++)
			{
				if (triggerPoints[i] is global::Trap)
				{
					this.unitCtrlr.detectedTriggers.Add(triggerPoints[i]);
				}
			}
			global::System.Collections.Generic.List<global::InteractivePoint> interactivePoints = global::PandoraSingleton<global::MissionManager>.Instance.interactivePoints;
			for (int j = 0; j < interactivePoints.Count; j++)
			{
				if (interactivePoints[j] != null && interactivePoints[j].unitActionId == global::UnitActionId.SEARCH)
				{
					this.unitCtrlr.detectedInteractivePoints.Add(interactivePoints[j]);
				}
			}
		}
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("perception", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
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

	private void OnSeqDone()
	{
		this.unitCtrlr.StateMachine.ChangeState(10);
	}

	public void LaunchFx()
	{
		global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx((!this.perceptionSuccess) ? this.unitCtrlr.CurrentAction.fxData.FizzleFx : this.unitCtrlr.CurrentAction.fxData.LaunchFx, this.unitCtrlr, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
	}

	private global::UnitController unitCtrlr;

	private global::CameraAnim camAnim;

	private bool perceptionSuccess;
}
