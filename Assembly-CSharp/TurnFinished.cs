using System;
using UnityEngine;

public class TurnFinished : global::ICheapState
{
	public TurnFinished(global::UnitController ctrler)
	{
		this.unitCtrlr = ctrler;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraDebug.LogInfo("TurnFinished Enter ", "UNIT_FLOW", this.unitCtrlr);
		if (this.unitCtrlr.unit.Status != global::UnitStateId.OUT_OF_ACTION)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(global::CombatLogger.LogMessage.TURN_END, new string[]
			{
				this.unitCtrlr.GetLogName()
			});
		}
		if (this.unitCtrlr.friendlyZoneEntryPoint != global::UnityEngine.Vector3.zero)
		{
			this.unitCtrlr.transform.position = this.unitCtrlr.friendlyZoneEntryPoint;
			this.unitCtrlr.friendlyZoneEntryPoint = global::UnityEngine.Vector3.zero;
		}
		if (this.unitCtrlr.AICtrlr != null)
		{
			this.unitCtrlr.AICtrlr.TurnEndCleanUp();
		}
		this.unitCtrlr.friendlyEntered.Clear();
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.SetKinemantic(true);
		global::PandoraSingleton<global::MissionManager>.Instance.ClearBeacons();
		this.unitCtrlr.LastActivatedAction = null;
		this.unitCtrlr.unit.SetAttribute(global::AttributeId.CURRENT_STRATEGY_POINTS, 0);
		if (this.unitCtrlr.IsPlayed())
		{
			this.unitCtrlr.Imprint.SetCurrent(false);
			global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWOwnMoving(this.unitCtrlr);
		}
		else
		{
			global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWTargetMoving(this.unitCtrlr);
		}
		this.unitCtrlr.SetAnimSpeed(0f);
		if (this.unitCtrlr.unit.Status != global::UnitStateId.OUT_OF_ACTION)
		{
			this.unitCtrlr.SetGraphWalkability(false);
			this.unitCtrlr.GetComponent<global::UnityEngine.Collider>().enabled = true;
		}
		else
		{
			this.unitCtrlr.SetGraphWalkability(true);
		}
		this.unitCtrlr.HideDetected();
		this.unitCtrlr.detectedUnits.Clear();
		this.unitCtrlr.detectedTriggers.Clear();
		this.unitCtrlr.detectedInteractivePoints.Clear();
		this.unitCtrlr.TurnStarted = false;
		this.unitCtrlr.beenShot = false;
		if (global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() == this.unitCtrlr)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.TurnTimer.Pause();
		}
		else
		{
			global::PandoraDebug.LogWarning("Ending unit turn when it's not the current unit " + this.unitCtrlr.name + "; current unit is: " + global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit().name, "FLOW", this.unitCtrlr);
		}
		if (global::PandoraSingleton<global::Hermes>.Instance.IsConnected() && this.unitCtrlr.IsMine())
		{
			this.unitCtrlr.StopSync();
		}
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(this.unitCtrlr);
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(this.unitCtrlr);
		if (global::PandoraSingleton<global::GameManager>.Instance.currentSave != null && !global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.missionSave.isTuto)
		{
			global::PandoraSingleton<global::GameManager>.Instance.Save.SaveCampaign(global::PandoraSingleton<global::GameManager>.Instance.currentSave, global::PandoraSingleton<global::GameManager>.Instance.campaign);
		}
	}

	void global::ICheapState.Update()
	{
		if (this.unitCtrlr.unit.IsAvailable())
		{
			if (this.unitCtrlr.unit.OverwatchLeft > 0 && this.unitCtrlr.HasRange())
			{
				global::PandoraDebug.LogInfo("Going to overwatch : (left " + this.unitCtrlr.unit.OverwatchLeft + ")", "ENDTURN", this.unitCtrlr);
				this.unitCtrlr.StateMachine.ChangeState(36);
				return;
			}
			if (this.unitCtrlr.unit.AmbushLeft > 0 && this.unitCtrlr.HasClose())
			{
				global::PandoraDebug.LogInfo("Going to ambush : (left " + this.unitCtrlr.unit.AmbushLeft + ")", "ENDTURN", this.unitCtrlr);
				this.unitCtrlr.StateMachine.ChangeState(37);
				return;
			}
		}
		this.unitCtrlr.StateMachine.ChangeState(9);
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::UnitController unitCtrlr;
}
