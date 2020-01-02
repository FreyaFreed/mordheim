using System;
using RAIN.Core;

public class AIIsPlayObjectiveRush : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "IsPlayObjectiveRush";
		this.warCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr();
		this.success = (this.warCtrlr.objectives[0].TypeId == global::PrimaryObjectiveTypeId.WYRDSTONE_RUSH);
	}

	private global::WarbandController warCtrlr;
}
