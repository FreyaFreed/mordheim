using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Skip uSequence")]
	[global::WellFired.USequencerEvent("Sequence/Skip uSequence")]
	public class USSkipSequenceEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!this.sequence)
			{
				global::UnityEngine.Debug.LogWarning("No sequence for USSkipSequenceEvent : " + base.name, this);
				return;
			}
			if (!this.skipToEnd && this.skipToTime < 0f && this.skipToTime > this.sequence.Duration)
			{
				global::UnityEngine.Debug.LogWarning("You haven't set the properties correctly on the Sequence for this USSkipSequenceEvent, either the skipToTime is invalid, or you haven't flagged it to skip to the end", this);
				return;
			}
			if (this.skipToEnd)
			{
				this.sequence.SkipTimelineTo(this.sequence.Duration);
			}
			else
			{
				this.sequence.SkipTimelineTo(this.skipToTime);
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public global::WellFired.USSequencer sequence;

		public bool skipToEnd = true;

		public float skipToTime = -1f;
	}
}
