using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Sequence/Stop And Skip")]
	[global::WellFired.USequencerFriendlyName("Stop and Skip sequencer")]
	public class USStopAndSkipToTimeSequenceEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!this.sequence)
			{
				global::UnityEngine.Debug.LogWarning("No sequence for USstopSequenceEvent : " + base.name, this);
			}
			if (this.sequence)
			{
				this.sequence.Stop();
				this.sequence.SkipTimelineTo(this.timeToSkipTo);
				this.sequence.UpdateSequencer(0f);
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		[global::UnityEngine.SerializeField]
		private global::WellFired.USSequencer sequence;

		[global::UnityEngine.SerializeField]
		private float timeToSkipTo;
	}
}
