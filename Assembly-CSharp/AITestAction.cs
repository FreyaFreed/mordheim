using System;
using RAIN.Core;
using RAIN.Representation;
using UnityEngine;

public class AITestAction : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AITestAction";
		global::UnityEngine.Debug.Log("valid = " + this.expr.IsValid);
		int num = this.expr.Evaluate<int>(global::UnityEngine.Time.deltaTime, null);
		global::UnityEngine.Debug.Log("evalutat = " + num);
		global::UnityEngine.Debug.Log("to string = " + this.expr.ToString());
	}

	public global::RAIN.Representation.Expression expr;
}
