using System;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

public class AIShootIdol : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "ShootIdol";
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		global::ActionStatus action = this.unitCtrlr.GetAction(global::SkillId.BASE_SHOOT);
		int teamIdx = this.unitCtrlr.GetWarband().teamIdx;
		if (action.Destructibles.Count > 0)
		{
			global::System.Collections.Generic.List<global::Destructible> list = new global::System.Collections.Generic.List<global::Destructible>(action.Destructibles);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i] == null || list[i].Owner == null || list[i].Owner.GetWarband().teamIdx == teamIdx)
				{
					list.RemoveAt(i);
				}
			}
			if (list.Count > 0)
			{
				int index = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, action.Destructibles.Count);
				this.unitCtrlr.SendSkillSingleDestructible(global::SkillId.BASE_SHOOT, action.Destructibles[index]);
				return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
			}
		}
		return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
	}
}
