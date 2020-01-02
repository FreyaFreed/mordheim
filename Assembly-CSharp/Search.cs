using System;
using UnityEngine;

public class Search : global::ICheapState
{
	public Search(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		if (this.unitCtrlr.interactivePoint.Highlight != null)
		{
			this.unitCtrlr.FaceTarget(this.unitCtrlr.interactivePoint.Highlight.transform, true);
		}
		else
		{
			this.unitCtrlr.FaceTarget(this.unitCtrlr.interactivePoint.transform, true);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.ForceFocusedUnit(this.unitCtrlr);
		if (this.unitCtrlr.interactivePoint is global::SearchPoint)
		{
			((global::SearchPoint)this.unitCtrlr.interactivePoint).wasSearched |= this.unitCtrlr.IsPlayed();
			this.unitCtrlr.GetWarband().SearchOpened((global::SearchPoint)this.unitCtrlr.interactivePoint);
			((global::SearchPoint)this.unitCtrlr.interactivePoint).InitInteraction();
		}
		global::PandoraSingleton<global::MissionManager>.Instance.TurnTimer.Pause();
		this.unitCtrlr.searchVariation = (int)this.unitCtrlr.interactivePoint.anim;
		this.unitCtrlr.LaunchAction(global::UnitActionId.SEARCH, true, global::UnitStateId.NONE, this.unitCtrlr.searchVariation);
		if (global::PandoraSingleton<global::GameManager>.Instance.IsFastForwarded && this.unitCtrlr.unit.isAI)
		{
			global::UnityEngine.Time.timeScale = 1.5f;
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::GameManager>.Instance.ResetTimeScale();
	}

	void global::ICheapState.Update()
	{
		this.unitCtrlr.RegisterItems();
		if (this.unitCtrlr.AICtrlr == null)
		{
			this.unitCtrlr.StateMachine.ChangeState(15);
		}
		else
		{
			global::Inventory inventory = (global::Inventory)this.unitCtrlr.StateMachine.GetState(15);
			inventory.UpdateInventory();
			this.unitCtrlr.AICtrlr.GotoSearchMode();
			this.unitCtrlr.StateMachine.ChangeState(42);
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::UnitController unitCtrlr;
}
