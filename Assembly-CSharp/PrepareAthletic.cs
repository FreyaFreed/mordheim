using System;
using System.Collections.Generic;
using UnityEngine;

public class PrepareAthletic : global::ICheapState
{
	public PrepareAthletic(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		bool flag = this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE;
		this.unitCtrlr.SetFixed(false);
		this.unitCtrlr.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = true;
		global::UnityEngine.Quaternion rotation = default(global::UnityEngine.Quaternion);
		rotation.SetLookRotation(this.unitCtrlr.activeActionDest.destination.transform.position - this.unitCtrlr.interactivePoint.transform.position, global::UnityEngine.Vector3.up);
		rotation.x = 0f;
		rotation.z = 0f;
		this.unitCtrlr.transform.rotation = rotation;
		this.success = true;
		this.height = this.unitCtrlr.GetFallHeight(this.unitCtrlr.activeActionDest.actionId);
		global::AttributeId attributeId = global::AttributeId.NONE;
		switch (this.unitCtrlr.activeActionDest.actionId)
		{
		case global::UnitActionId.CLIMB_3M:
		case global::UnitActionId.CLIMB_6M:
		case global::UnitActionId.CLIMB_9M:
			this.actionId = global::AthleticAction.CLIMB;
			this.unitCtrlr.transform.position = this.unitCtrlr.interactivePoint.transform.position + this.unitCtrlr.interactivePoint.transform.forward * -((!flag) ? 0.05f : 1.5f);
			attributeId = global::AttributeId.CLIMB_ROLL_3;
			break;
		case global::UnitActionId.LEAP:
			this.actionId = global::AthleticAction.LEAP;
			this.unitCtrlr.transform.position = this.unitCtrlr.interactivePoint.transform.position + this.unitCtrlr.interactivePoint.transform.forward * -((!flag) ? 0.5f : 1.5f);
			attributeId = global::AttributeId.LEAP_ROLL;
			break;
		case global::UnitActionId.JUMP_3M:
		case global::UnitActionId.JUMP_6M:
		case global::UnitActionId.JUMP_9M:
			this.actionId = global::AthleticAction.JUMP;
			this.unitCtrlr.transform.position = this.unitCtrlr.interactivePoint.transform.position + this.unitCtrlr.interactivePoint.transform.forward * -((!flag) ? 0f : 0.5f);
			attributeId = global::AttributeId.JUMP_DOWN_ROLL_3;
			break;
		}
		int roll = this.unitCtrlr.CurrentAction.GetRoll(false);
		this.success = this.unitCtrlr.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, roll, attributeId, false, true, 0);
		if (!this.success)
		{
			if (this.actionId == global::AthleticAction.CLIMB)
			{
				this.unitCtrlr.activeActionDest = this.unitCtrlr.activeActionDest.destination.GetJump();
				this.unitCtrlr.unit.AddEnchantment(global::EnchantmentId.CLIMB_FAIL_EFFECT, this.unitCtrlr.unit, false, true, global::AllegianceId.NONE);
			}
			else if (this.actionId == global::AthleticAction.LEAP)
			{
				this.unitCtrlr.activeActionDest = ((global::ActionZone)this.unitCtrlr.interactivePoint).GetJump();
			}
		}
		else if (this.actionId == global::AthleticAction.LEAP)
		{
			this.height = 3;
		}
		else if (this.actionId == global::AthleticAction.CLIMB)
		{
			this.unitCtrlr.unit.RemoveEnchantments(global::EnchantmentId.CLIMB_FAIL_EFFECT);
		}
		global::System.Collections.Generic.List<global::UnitController> list = new global::System.Collections.Generic.List<global::UnitController>(this.unitCtrlr.activeActionDest.destination.PointsChecker.enemiesOnZone);
		if (this.success && list.Count > 0)
		{
			this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_ATHLETIC_SUCCESS_ENGAGED, this.unitCtrlr.CurrentAction.SkillId, this.unitCtrlr.activeActionDest.actionId);
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
		{
			global::PandoraSingleton<global::MissionManager>.Instance.MoveUnitsOnActionZone(this.unitCtrlr, this.unitCtrlr.activeActionDest.destination.PointsChecker, this.unitCtrlr.activeActionDest.destination.PointsChecker.alliesOnZone, false);
			global::PandoraSingleton<global::MissionManager>.Instance.MoveUnitsOnActionZone(this.unitCtrlr, this.unitCtrlr.activeActionDest.destination.PointsChecker, this.unitCtrlr.activeActionDest.destination.PointsChecker.enemiesOnZone, true);
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
		if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
		{
			this.unitCtrlr.SendAthletic();
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::UnitController unitCtrlr;

	public bool success;

	public global::AthleticAction actionId;

	public int height;

	private global::UnityEngine.Vector3 targetPos;
}
