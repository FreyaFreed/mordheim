using System;
using RAIN.Action;
using RAIN.Core;

[global::RAIN.Action.RAINAction]
public class AIBase : global::RAIN.Action.RAINAction
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		this.actionName = "Base";
		if (this.unitCtrlr == null)
		{
			this.unitCtrlr = ai.Body.GetComponent<global::UnitController>();
		}
		this.success = true;
		global::PandoraDebug.LogInfo("START " + this.unitCtrlr.name + " : " + base.GetType().Name, "AI", this.unitCtrlr);
		base.Start(ai);
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		return (!this.success) ? global::RAIN.Action.RAINAction.ActionResult.FAILURE : global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
	}

	public override void Stop(global::RAIN.Core.AI ai)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			this.unitCtrlr.name,
			" : ",
			this.actionName,
			" ",
			(!this.success) ? global::RAIN.Action.RAINAction.ActionResult.FAILURE : global::RAIN.Action.RAINAction.ActionResult.SUCCESS
		}), "AI", this.unitCtrlr);
		base.Stop(ai);
	}

	protected global::UnitController unitCtrlr;

	protected bool success;
}
