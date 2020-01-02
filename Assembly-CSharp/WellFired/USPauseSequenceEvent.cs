using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Sequence/Pause uSequence")]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Pause uSequence")]
	public class USPauseSequenceEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!this.sequence)
			{
				global::UnityEngine.Debug.LogWarning("No sequence for USPauseSequenceEvent : " + base.name, this);
			}
			if (this.sequence)
			{
				this.sequence.Pause();
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public global::WellFired.USSequencer sequence;
	}
}
