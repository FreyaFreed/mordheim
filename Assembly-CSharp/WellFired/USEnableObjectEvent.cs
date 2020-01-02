using System;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Object/Toggle Object")]
	[global::WellFired.USequencerFriendlyName("Toggle Object")]
	public class USEnableObjectEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			this.prevEnable = base.AffectedObject.activeSelf;
			base.AffectedObject.SetActive(this.enable);
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			base.AffectedObject.SetActive(this.prevEnable);
		}

		public bool enable;

		private bool prevEnable;
	}
}
