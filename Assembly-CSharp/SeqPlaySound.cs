using System;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerFriendlyName("PlaySound")]
[global::WellFired.USequencerEvent("Mordheim/PlaySound")]
public class SeqPlaySound : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController target = null;
		global::SequenceTargetId sequenceTargetId = this.targetId;
		if (sequenceTargetId != global::SequenceTargetId.FOCUSED_UNIT)
		{
			if (sequenceTargetId == global::SequenceTargetId.DEFENDER)
			{
				target = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.defenderCtrlr;
			}
		}
		else
		{
			target = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		}
		global::PandoraSingleton<global::Pan>.Instance.GetSound(this.soundName, true, delegate(global::UnityEngine.AudioClip clip)
		{
			target.audioSource.clip = clip;
			target.audioSource.Play();
			target.audioSource.loop = false;
		});
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public global::SequenceTargetId targetId;

	public string soundName;
}
