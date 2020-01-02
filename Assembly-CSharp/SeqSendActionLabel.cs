using System;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/SendActionLabel")]
[global::WellFired.USequencerFriendlyName("SendActionLabel")]
public class SeqSendActionLabel : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		if (string.IsNullOrEmpty(focusedUnit.currentActionData.name))
		{
			return;
		}
		if (focusedUnit.Imprint.State == global::MapImprintStateId.VISIBLE || (focusedUnit.defenderCtrlr != null && focusedUnit.defenderCtrlr.Imprint.State == global::MapImprintStateId.VISIBLE))
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.RETROACTION_ACTION, focusedUnit);
		}
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}
}
