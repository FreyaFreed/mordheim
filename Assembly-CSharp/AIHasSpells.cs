using System;
using RAIN.Core;

public class AIHasSpells : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "HasSpells";
		this.success = false;
		for (int i = 0; i < this.unitCtrlr.actionStatus.Count; i++)
		{
			if (this.unitCtrlr.actionStatus[i].skillData.SkillTypeId == global::SkillTypeId.SPELL_ACTION)
			{
				this.success = true;
				break;
			}
		}
	}
}
