using System;
using RAIN.Core;

public class AIIsPlayObjectiveBounty : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "IsPlayObjectiveBounty";
		this.warCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr();
		this.success = (this.warCtrlr.objectives[0].TypeId == global::PrimaryObjectiveTypeId.BOUNTY);
	}

	private global::WarbandController warCtrlr;
}
