using System;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/SendNotice")]
[global::WellFired.USequencerFriendlyName("SendNotice")]
public class SeqSendNotice : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::Notices notices = (global::Notices)((int)global::System.Enum.Parse(typeof(global::Notices), this.noticeName));
		global::UnitController unitController = null;
		global::SequenceTargetId sequenceTargetId = this.targetId;
		if (sequenceTargetId != global::SequenceTargetId.FOCUSED_UNIT)
		{
			if (sequenceTargetId == global::SequenceTargetId.DEFENDER)
			{
				global::PandoraDebug.LogDebug("SequenceTargetId... Defender", "SEQUENCE", null);
				unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.defenderCtrlr;
			}
		}
		else
		{
			global::PandoraDebug.LogDebug("SequenceTargetId... Attacker / focusedUnit", "SEQUENCE", null);
			unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		}
		if (unitController.Imprint.State == global::MapImprintStateId.VISIBLE)
		{
			switch (notices)
			{
			case global::Notices.PHASE_RECOVERY_CHECK:
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.RETROACTION_ACTION, unitController);
				return;
			case global::Notices.REACTION_ACTION_OUTCOME:
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.RETROACTION_ACTION_OUTCOME, unitController);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.RETROACTION_SHOW_OUTCOME, unitController);
				return;
			}
			if (!string.IsNullOrEmpty(this.action))
			{
			}
		}
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public string noticeName;

	public string action;

	public global::SequenceTargetId targetId;
}
