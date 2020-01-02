using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("stop uSequence")]
	[global::WellFired.USequencerEvent("Sequence/Stop uSequence")]
	public class USStopSequenceEvent : global::WellFired.USEventBase
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
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public global::WellFired.USSequencer sequence;
	}
}
