using System;
using RAIN.Core;

public class AINeedReload : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "NeedReload";
		this.success = this.unitCtrlr.GetAction(global::SkillId.BASE_RELOAD).Available;
	}
}
