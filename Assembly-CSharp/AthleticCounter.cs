using System;
using System.Collections.Generic;
using UnityEngine;

public class AthleticCounter : global::ICheapState
{
	public AthleticCounter(global::UnitController ctrler)
	{
		this.unitCtrlr = ctrler;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraDebug.LogDebug("AthleticCounter Enter", "FLOW", this.unitCtrlr);
		this.unitCtrlr.SetFixed(false);
		this.unitCtrlr.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = true;
		if (this.freeAttackers == null)
		{
			this.freeAttackers = new global::System.Collections.Generic.Stack<global::UnitController>();
			for (int i = 0; i < this.unitCtrlr.EngagedUnits.Count; i++)
			{
				if (this.unitCtrlr.EngagedUnits[i].CanCounterAttack())
				{
					this.freeAttackers.Push(this.unitCtrlr.EngagedUnits[i]);
				}
			}
			global::PandoraDebug.LogDebug("AthleticCounter Enter Attackers size = " + this.freeAttackers.Count, "uncategorised", null);
		}
		this.unitCtrlr.Fleeing = true;
		this.checkNext = true;
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
		this.unitCtrlr.UpdateTargetsData();
		if (this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWOwnMoving(this.unitCtrlr);
		}
		else
		{
			global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWTargetMoving(this.unitCtrlr);
		}
		if (this.checkNext)
		{
			this.checkNext = false;
			this.NextFreeAttack();
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	public void NextFreeAttack()
	{
		global::PandoraDebug.LogDebug("AthleticCounter NextFreeAttack", "uncategorised", null);
		if (this.freeAttackers.Count > 0 && this.unitCtrlr.unit.Status != global::UnitStateId.OUT_OF_ACTION)
		{
			global::PandoraDebug.LogDebug("AthleticCounter NextAttack Pop!", "uncategorised", null);
			global::UnitController unitController = this.freeAttackers.Pop();
			unitController.attackerCtrlr = this.unitCtrlr;
			unitController.UpdateActionStatus(false, global::UnitActionRefreshId.NONE);
			if (unitController.unit.IsAvailable())
			{
				unitController.FaceTarget(this.unitCtrlr.transform, false);
				this.unitCtrlr.WaitForAction(global::UnitController.State.ATHLETIC_COUNTER);
				global::PandoraDebug.LogDebug("AthleticCounter NextAttack Launch Attack", "uncategorised", null);
				unitController.StateMachine.ChangeState(34);
			}
			else
			{
				this.checkNext = true;
			}
		}
		else
		{
			global::PandoraDebug.LogDebug("AthleticCounter NextAttack No more attacker", "uncategorised", null);
			this.freeAttackers = null;
			this.unitCtrlr.Fleeing = false;
			this.unitCtrlr.StateMachine.ChangeState(10);
		}
	}

	private global::UnitController unitCtrlr;

	private global::System.Collections.Generic.Stack<global::UnitController> freeAttackers;

	private bool checkNext;
}
