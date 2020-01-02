using System;
using RAIN.Core;

public class AIIsPlayObjectiveGTC : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "IsPlayObjectiveGTC";
		this.warCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr();
		this.success = (this.warCtrlr.objectives[0].Id == global::PrimaryObjectiveId.GRAND_THEFT_CART);
	}

	private global::WarbandController warCtrlr;
}
