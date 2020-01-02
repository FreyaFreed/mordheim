using System;
using UnityEngine;

public class ActionData
{
	public void SetAction(string actionName, global::UnityEngine.Sprite actionIcon)
	{
		this.Reset();
		this.name = actionName;
		this.icon = actionIcon;
	}

	public void SetAction(global::ActionStatus actionStatus)
	{
		this.SetAction(this.name = actionStatus.LocalizedName, actionStatus.GetIcon());
		this.mastery = actionStatus.IsMastery;
	}

	public void SetActionOutcome(bool success)
	{
		this.actionOutcome = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!success) ? "com_failure" : "com_success");
	}

	public void SetActionOutcome(string outcome)
	{
		this.actionOutcome = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(outcome);
	}

	public void Reset()
	{
		this.name = string.Empty;
		this.icon = null;
		this.actionOutcome = null;
		this.mastery = false;
	}

	public string name;

	public bool mastery;

	public global::UnityEngine.Sprite icon;

	public string actionOutcome;
}
