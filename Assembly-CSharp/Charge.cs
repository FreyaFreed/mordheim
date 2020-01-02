using System;
using UnityEngine;

public class Charge : global::ICheapState
{
	public Charge(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraDebug.LogInfo("Charge Enter ", "UNIT_FLOW", this.unitCtrlr);
		this.unitCtrlr.SetFixed(false);
		this.lastPosition = this.unitCtrlr.transform.position;
		this.blockedTimer = 0f;
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		this.unitCtrlr.FaceTarget(this.unitCtrlr.defenderCtrlr.transform, true);
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("charge", this.unitCtrlr, null);
		this.chargeTime = 0f;
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::SequenceManager>.Instance.EndSequence();
		if (this.unitCtrlr.chargeFx != null)
		{
			this.unitCtrlr.chargeFx.DestroyFx(true);
			this.unitCtrlr.chargeFx = null;
		}
	}

	void global::ICheapState.Update()
	{
		this.chargeTime += global::UnityEngine.Time.deltaTime;
	}

	void global::ICheapState.FixedUpdate()
	{
		if (this.chargeTime > 15f)
		{
			global::PandoraDebug.LogWarning("Charge failed due to too long charge", "CHARGE", this.unitCtrlr);
			global::PandoraSingleton<global::SequenceManager>.Instance.EndSequence();
			this.unitCtrlr.SendStartMove(this.unitCtrlr.transform.position, this.unitCtrlr.transform.rotation);
			return;
		}
		this.unitCtrlr.UpdateTargetsData();
		if (!this.unitCtrlr.Engaged)
		{
			this.unitCtrlr.FaceTarget(this.unitCtrlr.defenderCtrlr.transform, true);
			if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer() && this.unitCtrlr.unit.IsAvailable())
			{
				this.unitCtrlr.CheckEngaged(true);
				if (this.unitCtrlr.Engaged)
				{
					global::PandoraSingleton<global::SequenceManager>.Instance.EndSequence();
					this.unitCtrlr.SetFixed(true);
					this.unitCtrlr.SendEngaged(this.unitCtrlr.transform.position, this.unitCtrlr.transform.rotation, true);
					return;
				}
				if (global::UnityEngine.Vector3.SqrMagnitude(this.unitCtrlr.transform.position - this.lastPosition) < 0.005625f)
				{
					this.blockedTimer += global::UnityEngine.Time.deltaTime;
					if (this.blockedTimer >= 1f)
					{
						global::PandoraDebug.LogWarning("Charge failed due to movement fail", "CHARGE", this.unitCtrlr);
						global::PandoraSingleton<global::SequenceManager>.Instance.EndSequence();
						this.unitCtrlr.SendStartMove(this.unitCtrlr.transform.position, this.unitCtrlr.transform.rotation);
						return;
					}
				}
				else
				{
					this.blockedTimer = 0f;
				}
				this.lastPosition = this.unitCtrlr.transform.position;
			}
		}
	}

	private const float BLOCK_TIME = 1f;

	private const float MAX_CHARGE_TIME = 15f;

	private global::UnitController unitCtrlr;

	private global::UnityEngine.Vector3 lastPosition;

	private float blockedTimer;

	private float chargeTime;
}
