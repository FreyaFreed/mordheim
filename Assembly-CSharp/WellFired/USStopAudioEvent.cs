using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerFriendlyName("Stop Audio")]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Audio/Stop Audio")]
	public class USStopAudioEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				global::UnityEngine.Debug.Log("USSequencer is trying to play an audio clip, but you didn't give it Audio To Play from USPlayAudioEvent::FireEvent");
				return;
			}
			global::UnityEngine.AudioSource component = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			if (!component)
			{
				global::UnityEngine.Debug.Log("USSequencer is trying to play an audio source, but the GameObject doesn't contain an AudioClip from USPlayAudioEvent::FireEvent");
				return;
			}
			component.Stop();
		}

		public override void ProcessEvent(float deltaTime)
		{
		}
	}
}
