using System;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using UnityEngine.Events;

public class AIPathIdol : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathIdol";
		this.SetTargets();
		if (this.targets.Count > 0)
		{
			this.currentResult = global::RAIN.Action.RAINAction.ActionResult.RUNNING;
			this.unitCtrlr.AICtrlr.FindPath(this.targets, new global::UnityEngine.Events.UnityAction<bool>(this.OnDestructiblesChecked));
		}
		else
		{
			this.success = false;
			this.currentResult = global::RAIN.Action.RAINAction.ActionResult.FAILURE;
		}
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		return this.currentResult;
	}

	public override void Stop(global::RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}

	private void SetTargets()
	{
		this.targets.Clear();
		int teamIdx = this.unitCtrlr.GetWarband().teamIdx;
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.MapImprints.Count; i++)
		{
			global::Destructible destructible = global::PandoraSingleton<global::MissionManager>.Instance.MapImprints[i].Destructible;
			if (destructible != null && destructible.Owner != null && destructible.Owner.GetWarband().teamIdx != teamIdx)
			{
				this.targets.Add(destructible);
			}
		}
	}

	private void OnDestructiblesChecked(bool foundPath)
	{
		foundPath &= (this.unitCtrlr.AICtrlr.currentPath != null);
		this.success = foundPath;
		this.currentResult = ((!this.success) ? global::RAIN.Action.RAINAction.ActionResult.FAILURE : global::RAIN.Action.RAINAction.ActionResult.SUCCESS);
	}

	private global::RAIN.Action.RAINAction.ActionResult currentResult;

	private global::System.Collections.Generic.List<global::Destructible> targets = new global::System.Collections.Generic.List<global::Destructible>();
}
