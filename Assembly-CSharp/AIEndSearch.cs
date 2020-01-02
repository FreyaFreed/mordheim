using System;
using RAIN.Action;
using RAIN.Core;

public class AIEndSearch : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "EndSearch";
		this.unitCtrlr.AICtrlr.targetSearchPoint = null;
		this.unitCtrlr.AICtrlr.atDestination = false;
		this.unitCtrlr.StateMachine.ChangeState(9);
		this.unitCtrlr.AICtrlr.GotoPreviousMode();
		this.unitCtrlr.SendInventoryDone();
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
	}

	private bool inSequence;
}
