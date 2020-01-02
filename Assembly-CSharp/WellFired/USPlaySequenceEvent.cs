using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Play uSequence")]
	[global::WellFired.USequencerEvent("Sequence/Play uSequence")]
	public class USPlaySequenceEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!this.sequence)
			{
				global::UnityEngine.Debug.LogWarning("No sequence for USPlaySequenceEvent : " + base.name, this);
				return;
			}
			if (!global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Debug.LogWarning("Sequence playback controls are not supported in the editor, but will work in game, just fine.");
				return;
			}
			if (!this.restartSequencer)
			{
				this.sequence.Play();
			}
			else
			{
				this.sequence.RunningTime = 0f;
				this.sequence.Play();
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public global::WellFired.USSequencer sequence;

		public bool restartSequencer;
	}
}
