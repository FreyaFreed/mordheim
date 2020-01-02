using System;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerFriendlyName("InitSkillDebug")]
[global::WellFired.USequencerEvent("Mordheim/InitSkillDebug")]
public class SeqInitSkillDebug : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		if (global::UnityEngine.Application.loadedLevelName != "sequence")
		{
			return;
		}
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		global::SkillId currentAction = global::SkillId.NONE;
		for (int i = 0; i < focusedUnit.actionStatus.Count; i++)
		{
			if (focusedUnit.actionStatus[i].skillData != null && focusedUnit.actionStatus[i].skillData.SkillTypeId == global::SkillTypeId.SKILL_ACTION)
			{
				currentAction = focusedUnit.actionStatus[i].SkillId;
			}
		}
		focusedUnit.SetCurrentAction(currentAction);
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}
}
