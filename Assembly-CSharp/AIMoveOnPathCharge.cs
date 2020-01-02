using System;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using UnityEngine.Events;

public class AIMoveOnPathCharge : global::AIMoveOnPath
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "MoveOnPathCharge";
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		global::ActionStatus action = this.unitCtrlr.GetAction(global::SkillId.BASE_CHARGE);
		action.UpdateAvailable();
		if (action.Available && this.unitCtrlr.chargeTargets.IndexOf(this.unitCtrlr.AICtrlr.targetEnemy) != -1)
		{
			global::ActionStatus bestAction = this.unitCtrlr.AICtrlr.GetBestAction(global::AIController.chargeActions, out this.refTarget, new global::UnityEngine.Events.UnityAction<global::ActionStatus, global::System.Collections.Generic.List<global::UnitController>>(this.RefineTargets));
			global::SkillId skillId = (bestAction != null) ? bestAction.SkillId : global::SkillId.BASE_CHARGE;
			this.unitCtrlr.SendSkillSingleTarget(skillId, this.unitCtrlr.AICtrlr.targetEnemy);
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		return base.Execute(ai);
	}

	public override void Stop(global::RAIN.Core.AI ai)
	{
		this.unitCtrlr.AICtrlr.targetEnemy = null;
		base.Stop(ai);
	}

	private void RefineTargets(global::ActionStatus action, global::System.Collections.Generic.List<global::UnitController> allTargets)
	{
		allTargets.Clear();
		allTargets.Add(this.unitCtrlr.AICtrlr.targetEnemy);
	}

	private global::UnitController refTarget;
}
