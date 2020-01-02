using System;
using System.Collections.Generic;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/EndSequence")]
[global::WellFired.USequencerFriendlyName("EndSequence")]
public class SeqEndSequence : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		if (!global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying)
		{
			global::PandoraDebug.LogWarning("EndSequence FireEvent SequenceManager is not playing. This is most probably a second call to this event", "uncategorised", null);
			return;
		}
		if (this.finishers != null && this.finishers.Count > 0)
		{
			global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
			foreach (global::SeqFinisher seqFinisher in this.finishers)
			{
				if (focusedUnit.unit.Data.UnitBaseId == seqFinisher.id && (seqFinisher.style == global::AnimStyleId.NONE || focusedUnit.unit.currentAnimStyleId == seqFinisher.style))
				{
					this.End();
				}
			}
		}
		else
		{
			this.End();
		}
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	private void End()
	{
		global::PandoraDebug.LogInfo("SeqEndSequence Ending Sequence " + ((!(global::PandoraSingleton<global::SequenceManager>.Instance.currentSeq != null)) ? "none" : global::PandoraSingleton<global::SequenceManager>.Instance.currentSeq.name), "SEQUENCE", null);
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.TurnOffDOF();
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SetFOV(45f, 0.2f);
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.ActivateOverlay(false, this.vignetteFadeTime);
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.DeactivateBloodSplatter();
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SEQUENCE_ENDED);
	}

	private float vignetteFadeTime = 0.2f;

	public global::System.Collections.Generic.List<global::SeqFinisher> finishers;
}
