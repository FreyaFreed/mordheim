using System;
using System.Collections.Generic;
using RAIN.Core;
using UnityEngine;

public class AIConsSkillSpell : global::AIBaseAction
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIConsSkillSpell";
	}

	protected override bool IsValid(global::ActionStatus action, global::UnitController target)
	{
		if (this.allies == null)
		{
			this.unitCtrlr.GetAlliesEnemies(out this.allies, out this.enemies);
		}
		bool flag = this.allies.IndexOf(target) != -1 || target == this.unitCtrlr;
		bool flag2 = this.enemies.IndexOf(target) != -1;
		if (this.unitCtrlr.unit.Id == global::UnitId.MANTICORE)
		{
			float num = global::UnityEngine.Vector3.Dot(this.unitCtrlr.transform.forward, target.transform.position - this.unitCtrlr.transform.position);
			if (num < 0.25f)
			{
				return false;
			}
		}
		return (action.skillData.EffectTypeId == global::EffectTypeId.BUFF && flag) || (action.skillData.EffectTypeId == global::EffectTypeId.DEBUFF && flag2) || (action.skillData.EffectTypeId == global::EffectTypeId.INSTANT && action.skillData.WoundMax == 0 && flag) || (action.skillData.EffectTypeId == global::EffectTypeId.INSTANT && action.skillData.WoundMax != 0 && flag2);
	}

	protected override bool ByPassLimit(global::UnitController target)
	{
		return true;
	}

	protected override bool IsBetter(int currentVal, int val)
	{
		return false;
	}

	protected override int GetCriteriaValue(global::UnitController target)
	{
		return 0;
	}

	protected override global::System.Collections.Generic.List<global::UnitActionId> GetRelatedActions()
	{
		return global::AIController.consSkillSpellActions;
	}

	protected global::System.Collections.Generic.List<global::UnitController> allies;

	protected global::System.Collections.Generic.List<global::UnitController> enemies;
}
