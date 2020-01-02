using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Sequence/Set Playback Rate")]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Set uSequence Playback Rate")]
	public class USSetPlaybackRateEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!this.sequence)
			{
				global::UnityEngine.Debug.LogWarning("No sequence for USSetPlaybackRate : " + base.name, this);
			}
			if (this.sequence)
			{
				this.prevPlaybackRate = this.sequence.PlaybackRate;
				this.sequence.PlaybackRate = this.playbackRate;
			}
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
			if (this.sequence)
			{
				this.sequence.PlaybackRate = this.prevPlaybackRate;
			}
		}

		public global::WellFired.USSequencer sequence;

		public float playbackRate = 1f;

		private float prevPlaybackRate = 1f;
	}
}
