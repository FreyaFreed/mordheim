using System;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using UnityEngine;

public class AIMoveOnPathRange : global::AIMoveOnPath
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "MoveOnPathRange";
		this.minRange = 0;
		if (this.unitCtrlr.HasRange())
		{
			this.minRange = this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Item.RangeMin;
		}
		this.shooting = false;
		this.target = null;
		this.action = this.unitCtrlr.GetAction(global::SkillId.BASE_SHOOT);
		this.targets = this.action.Targets;
		this.rangeMin = this.action.RangeMin;
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		this.action.UpdateAvailable();
		if (this.action.Available)
		{
			if (this.unitCtrlr.AICtrlr.targetEnemy != null)
			{
				for (int i = 0; i < this.targets.Count; i++)
				{
					if (this.targets[i] == this.unitCtrlr.AICtrlr.targetEnemy)
					{
						this.SetShoot(this.targets[i]);
						return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
					}
				}
			}
			else if (this.targets.Count > 0)
			{
				this.SetShoot(this.targets[global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.targets.Count)]);
				return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
			}
		}
		if (this.unitCtrlr.AICtrlr.targetEnemy != null && global::UnityEngine.Vector3.SqrMagnitude(this.unitCtrlr.transform.position - this.unitCtrlr.AICtrlr.targetEnemy.transform.position) < (float)(this.minRange * this.minRange))
		{
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		return base.Execute(ai);
	}

	public override void Stop(global::RAIN.Core.AI ai)
	{
		this.unitCtrlr.AICtrlr.targetEnemy = null;
		base.Stop(ai);
	}

	public override void GotoNextState()
	{
		if (this.shooting && this.target != null)
		{
			this.unitCtrlr.SendSkillSingleTarget(global::SkillId.BASE_SHOOT, this.target);
			return;
		}
		base.GotoNextState();
	}

	private void SetShoot(global::UnitController unit)
	{
		this.target = unit;
		this.shooting = true;
		base.EndMove();
	}

	private int minRange;

	private global::ActionStatus action;

	private global::System.Collections.Generic.List<global::UnitController> targets;

	private global::UnitController target;

	private bool shooting;

	private int rangeMin;
}
