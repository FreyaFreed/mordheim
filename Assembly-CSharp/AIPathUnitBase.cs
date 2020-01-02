using System;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using UnityEngine.Events;

public abstract class AIPathUnitBase : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathUnit";
		if (this.unitCtrlr.unit.CurrentStrategyPoints == 0)
		{
			this.success = false;
			this.currentResult = global::RAIN.Action.RAINAction.ActionResult.FAILURE;
			return;
		}
		this.useSpotted = this.useOnlySpottedEnemies.IsValid;
		this.keepOnlyReachable = this.getOnlyReachables.IsValid;
		global::System.Collections.Generic.List<global::UnitController> list = (!this.useSpotted) ? this.GetUnitsPool() : this.unitCtrlr.GetWarband().SquadManager.GetSpottedEnemies();
		if (!this.keepOnlyReachable)
		{
			this.targets.Clear();
			this.SetTargets(list);
		}
		else
		{
			this.targets = list;
		}
		this.unitCtrlr.AICtrlr.RemoveInactive(this.targets);
		if (this.targets.Count > 0)
		{
			this.currentResult = global::RAIN.Action.RAINAction.ActionResult.RUNNING;
			this.unitCtrlr.AICtrlr.FindPath(this.targets, new global::UnityEngine.Events.UnityAction<bool>(this.OnUnitsChecked), this.keepOnlyReachable);
		}
		else
		{
			this.success = false;
			this.currentResult = global::RAIN.Action.RAINAction.ActionResult.FAILURE;
		}
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		return this.currentResult;
	}

	public override void Stop(global::RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}

	protected abstract bool CheckAllies();

	private global::System.Collections.Generic.List<global::UnitController> GetUnitsPool()
	{
		if (this.CheckAllies())
		{
			return global::PandoraSingleton<global::MissionManager>.Instance.GetAliveAllies(this.unitCtrlr.GetWarband().idx);
		}
		return global::PandoraSingleton<global::MissionManager>.Instance.GetAliveEnemies(this.unitCtrlr.GetWarband().idx);
	}

	protected abstract void SetTargets(global::System.Collections.Generic.List<global::UnitController> unitsToCheck);

	private void OnUnitsChecked(bool pathSuccess)
	{
		pathSuccess &= (this.unitCtrlr.AICtrlr.currentPath != null);
		this.success = pathSuccess;
		if (this.success)
		{
			if (this.keepOnlyReachable)
			{
				this.unitCtrlr.AICtrlr.targetEnemy = null;
				if (this.unitCtrlr.AICtrlr.reachableUnits.Count > 0)
				{
					this.unitCtrlr.AICtrlr.targetEnemy = null;
					this.targets.Clear();
					this.SetTargets(this.unitCtrlr.AICtrlr.reachableUnits);
					if (this.targets.Count > 0)
					{
						this.unitCtrlr.AICtrlr.targetEnemy = this.targets[global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.targets.Count)];
						this.unitCtrlr.AICtrlr.currentPath = this.unitCtrlr.AICtrlr.reachableUnitsPaths[this.unitCtrlr.AICtrlr.targetEnemy];
					}
				}
				this.success = (this.unitCtrlr.AICtrlr.targetEnemy != null);
			}
			this.unitCtrlr.AICtrlr.Squad.SetTarget(this.unitCtrlr.AICtrlr.targetEnemy, this.useSpotted);
		}
		this.currentResult = ((!this.success) ? global::RAIN.Action.RAINAction.ActionResult.FAILURE : global::RAIN.Action.RAINAction.ActionResult.SUCCESS);
	}

	private global::RAIN.Action.RAINAction.ActionResult currentResult;

	public global::RAIN.Representation.Expression useOnlySpottedEnemies = new global::RAIN.Representation.Expression();

	public global::RAIN.Representation.Expression getOnlyReachables = new global::RAIN.Representation.Expression();

	private bool useSpotted;

	private bool keepOnlyReachable;

	protected global::System.Collections.Generic.List<global::UnitController> targets = new global::System.Collections.Generic.List<global::UnitController>();
}
