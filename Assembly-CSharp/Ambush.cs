using System;
using UnityEngine;

public class Ambush : global::ICheapState
{
	public Ambush(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
		global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
		if (this.unitCtrlr.Engaged || !this.unitCtrlr.HasClose())
		{
			this.unitCtrlr.unit.AddEnchantment(global::EnchantmentId.BASE_AMBUSH_OVERWATCH_REMOVER, this.unitCtrlr.unit, false, true, global::AllegianceId.NONE);
			this.unitCtrlr.StateMachine.ChangeState(9);
		}
		else if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer() && currentUnit != null && this.unitCtrlr.GetWarband().teamIdx != currentUnit.GetWarband().teamIdx && !currentUnit.IsInFriendlyZone && (currentUnit.StateMachine.GetActiveStateId() == 11 || currentUnit.StateMachine.GetActiveStateId() == 42 || currentUnit.StateMachine.GetActiveStateId() == 43) && global::UnityEngine.Vector3.SqrMagnitude(currentUnit.transform.position - currentUnit.startPosition) > global::Constant.GetFloat(global::ConstantId.MOVE_MINIMUM) * global::Constant.GetFloat(global::ConstantId.MOVE_MINIMUM) && this.unitCtrlr.CanChargeUnit(currentUnit, true))
		{
			currentUnit.SendAskInterruption(global::UnitActionId.AMBUSH, this.unitCtrlr.uid, currentUnit.transform.position, currentUnit.transform.rotation);
		}
	}

	private global::UnitController unitCtrlr;
}
