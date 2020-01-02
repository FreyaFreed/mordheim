using System;
using UnityEngine;

public class AIControlled : global::ICheapState
{
	public AIControlled(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.AICtrlr.RestartBT();
		this.unitCtrlr.AICtrlr.failedMove = 0;
		this.unitCtrlr.AICtrlr.UpdateVisibility();
		this.unitCtrlr.UpdateActionStatus(false, global::UnitActionRefreshId.NONE);
		global::PandoraSingleton<global::MissionManager>.Instance.TurnOffActionZones();
		this.processTimer = 0f;
		if (global::PandoraSingleton<global::GameManager>.Instance.IsFastForwarded)
		{
			global::UnityEngine.Time.timeScale = 1.5f;
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::GameManager>.Instance.ResetTimeScale();
		if (!this.unitCtrlr.GetComponent<global::UnityEngine.Rigidbody>().isKinematic)
		{
			this.unitCtrlr.GetComponent<global::UnityEngine.Rigidbody>().velocity = global::UnityEngine.Vector3.zero;
		}
		this.unitCtrlr.SetAnimSpeed(0f);
	}

	void global::ICheapState.Update()
	{
		this.processTimer += global::UnityEngine.Time.deltaTime;
		if (this.unitCtrlr.AICtrlr.failedMove == 3 || this.processTimer > 30f)
		{
			global::PandoraDebug.LogInfo("AI turn finished because it failed to much movements or was thinking too much", "AI", this.unitCtrlr);
			this.unitCtrlr.SendSkill(global::SkillId.BASE_END_TURN);
			return;
		}
	}

	void global::ICheapState.FixedUpdate()
	{
		if (!this.unitCtrlr.IsAnimating())
		{
			this.unitCtrlr.AICtrlr.FixedUpdate();
		}
	}

	private const int FAILED_MOVEMENT_MAX = 3;

	private const float MAX_PROCESS_TIME = 30f;

	private global::UnitController unitCtrlr;

	private float processTimer;
}
