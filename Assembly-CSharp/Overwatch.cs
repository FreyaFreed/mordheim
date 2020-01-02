using System;
using UnityEngine;

public class Overwatch : global::ICheapState
{
	public Overwatch(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.requiredPerc = global::Constant.GetFloat(global::ConstantId.RANGE_SHOOT_REQUIRED_PERC);
		this.minDistance = (float)this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Item.RangeMin;
		this.distance = (float)(this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Item.RangeMax - 2);
		if (this.unitCtrlr.GetCurrentShots() == 0)
		{
			this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Reload();
			if (this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + 1)] != null && this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + 1)].Item.TypeData.IsRange)
			{
				this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + 1)].Reload();
			}
		}
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
		if (currentUnit == null)
		{
			return;
		}
		if (this.targetData == null || this.targetData.unitCtrlr != currentUnit)
		{
			this.targetData = new global::TargetData(currentUnit);
		}
		if (this.unitCtrlr.Engaged || !this.unitCtrlr.HasRange())
		{
			this.unitCtrlr.unit.AddEnchantment(global::EnchantmentId.BASE_AMBUSH_OVERWATCH_REMOVER, this.unitCtrlr.unit, false, true, global::AllegianceId.NONE);
			this.unitCtrlr.StateMachine.ChangeState(9);
		}
		else if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer() && this.unitCtrlr.GetWarband().teamIdx != currentUnit.GetWarband().teamIdx && !this.unitCtrlr.Engaged && !currentUnit.IsInFriendlyZone && (currentUnit.StateMachine.GetActiveStateId() == 11 || currentUnit.StateMachine.GetActiveStateId() == 42 || currentUnit.StateMachine.GetActiveStateId() == 43) && global::UnityEngine.Vector3.SqrMagnitude(currentUnit.transform.position - currentUnit.startPosition) > global::Constant.GetFloat(global::ConstantId.MOVE_MINIMUM) * global::Constant.GetFloat(global::ConstantId.MOVE_MINIMUM) && this.unitCtrlr.CanTargetFromPoint(this.targetData, this.minDistance, this.distance, this.requiredPerc, true, true, global::BoneId.NONE))
		{
			currentUnit.SendAskInterruption(global::UnitActionId.OVERWATCH, this.unitCtrlr.uid, currentUnit.transform.position, currentUnit.transform.rotation);
		}
	}

	private global::UnitController unitCtrlr;

	private float minDistance;

	private float distance;

	private global::TargetData targetData;

	private float requiredPerc;
}
