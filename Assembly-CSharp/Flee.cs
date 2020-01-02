using System;
using System.Collections.Generic;
using UnityEngine;

public class Flee : global::ICheapState
{
	public Flee(global::UnitController ctrler)
	{
		this.unitCtrlr = ctrler;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraDebug.LogDebug("Flee Enter", "FLOW", this.unitCtrlr);
		this.unitCtrlr.SetFixed(false);
		this.unitCtrlr.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = true;
		if (this.freeAttackers == null)
		{
			this.freeAttackers = new global::System.Collections.Generic.Stack<global::UnitController>();
			for (int i = 0; i < this.unitCtrlr.EngagedUnits.Count; i++)
			{
				if (this.unitCtrlr.EngagedUnits[i].unit.IsAvailable() && this.unitCtrlr.EngagedUnits[i].HasClose())
				{
					this.freeAttackers.Push(this.unitCtrlr.EngagedUnits[i]);
				}
			}
			global::PandoraDebug.LogDebug("Flee Enter FreeAttackers size = " + this.freeAttackers.Count, "uncategorised", null);
		}
		this.unitCtrlr.Fleeing = true;
		this.checkNext = true;
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
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
		global::PandoraDebug.LogDebug("Flee NextFreeAttack", "uncategorised", null);
		if (this.freeAttackers.Count > 0 && this.unitCtrlr.unit.Status != global::UnitStateId.OUT_OF_ACTION)
		{
			global::UnitController unitController = this.freeAttackers.Pop();
			unitController.defenderCtrlr = this.unitCtrlr;
			global::PandoraDebug.LogDebug("Flee NextFreeAttack Pop!", "uncategorised", null);
			if (unitController.unit.IsAvailable())
			{
				unitController.FaceTarget(unitController.defenderCtrlr.transform, false);
				this.unitCtrlr.WaitForAction(global::UnitController.State.FLEE);
				global::PandoraDebug.LogDebug("Flee NextFreeAttack Launch Attack", "uncategorised", null);
				unitController.SkillSingleTargetRPC(386, this.unitCtrlr.uid);
			}
			else
			{
				this.checkNext = true;
			}
		}
		else
		{
			global::PandoraDebug.LogDebug("Flee NextFreeAttack No free attacker", "uncategorised", null);
			this.freeAttackers = null;
			this.unitCtrlr.Fleeing = false;
			if (this.unitCtrlr.unit.IsAvailable())
			{
				global::PandoraDebug.LogDebug("Flee NextFreeAttack Switch to Disengage", "uncategorised", null);
				this.unitCtrlr.SetCurrentAction(global::SkillId.BASE_FLEE);
				if (this.unitCtrlr.IsMine())
				{
					this.unitCtrlr.StateMachine.ChangeState(41);
				}
				else
				{
					this.unitCtrlr.StateMachine.ChangeState(43);
				}
			}
			else
			{
				this.unitCtrlr.StateMachine.ChangeState(10);
			}
		}
	}

	private global::UnitController unitCtrlr;

	private global::System.Collections.Generic.Stack<global::UnitController> freeAttackers;

	private bool checkNext;
}
