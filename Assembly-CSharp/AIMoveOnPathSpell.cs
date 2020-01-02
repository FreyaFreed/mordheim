using System;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using UnityEngine;

public class AIMoveOnPathSpell : global::AIMoveOnPath
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "MoveOnPathSpell";
		global::System.Collections.Generic.List<global::ActionStatus> actions = this.unitCtrlr.GetActions(global::UnitActionId.SPELL);
		this.success &= (actions.Count > 0);
		if (this.success)
		{
			this.spell = actions[0];
		}
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.spell == null)
		{
			return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
		}
		this.targets = this.spell.Targets;
		if (this.targets.Count > 0)
		{
			this.spell.UpdateAvailable();
			if (this.spell.Available)
			{
				switch (this.spell.skillData.TargetingId)
				{
				case global::TargetingId.SINGLE_TARGET:
					this.unitCtrlr.SendSkillSingleTarget(this.spell.SkillId, this.targets[0]);
					return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
				case global::TargetingId.LINE:
				case global::TargetingId.CONE:
					this.unitCtrlr.SendSkillTargets(this.spell.SkillId, this.unitCtrlr.transform.position + global::UnityEngine.Vector3.up, this.targets[0].transform.position - this.unitCtrlr.transform.position);
					return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
				case global::TargetingId.AREA:
					this.unitCtrlr.SendSkillTargets(this.spell.SkillId, this.targets[0].transform.position, this.targets[0].transform.position - this.unitCtrlr.transform.position);
					return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
				}
			}
		}
		return base.Execute(ai);
	}

	private global::ActionStatus spell;

	private global::System.Collections.Generic.List<global::UnitController> targets;
}
