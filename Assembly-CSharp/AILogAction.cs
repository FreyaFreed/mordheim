using System;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using UnityEngine;

public class AILogAction : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "LogAction";
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		global::UnityEngine.Debug.Log("[BT] Debug action : " + this.message);
		return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
	}

	public global::RAIN.Representation.Expression message = new global::RAIN.Representation.Expression();
}
