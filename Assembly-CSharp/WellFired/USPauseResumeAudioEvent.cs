using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Audio/Pause Or Resume Audio")]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Pause Or Resume Audio")]
	public class USPauseResumeAudioEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				global::UnityEngine.Debug.Log("USSequencer is trying to play an audio clip, but you didn't give it Audio To Play from USPauseAudioEvent::FireEvent");
				return;
			}
			global::UnityEngine.AudioSource component = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			if (!component)
			{
				global::UnityEngine.Debug.Log("USSequencer is trying to play an audio source, but the GameObject doesn't contain an AudioClip from USPauseAudioEvent::FireEvent");
				return;
			}
			if (this.pause)
			{
				component.Pause();
			}
			if (!this.pause)
			{
				component.Play();
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
			global::UnityEngine.AudioSource component = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			if (!component)
			{
				global::UnityEngine.Debug.Log("USSequencer is trying to play an audio source, but the GameObject doesn't contain an AudioClip from USPauseAudioEvent::FireEvent");
				return;
			}
			if (component.isPlaying)
			{
				return;
			}
		}

		public bool pause = true;
	}
}
