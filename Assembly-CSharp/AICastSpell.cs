using System;
using System.Collections.Generic;
using RAIN.Core;

public class AICastSpell : global::AIConsSkillSpell
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AICastSpell";
	}

	protected override global::System.Collections.Generic.List<global::UnitActionId> GetRelatedActions()
	{
		return global::AIController.spellActions;
	}
}
