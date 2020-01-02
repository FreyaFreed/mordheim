using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Audio/Play Audio")]
	[global::WellFired.USequencerFriendlyName("Play Audio")]
	[global::WellFired.USequencerEventHideDuration]
	public class USPlayAudioEvent : global::WellFired.USEventBase
	{
		public void Update()
		{
			if (!this.loop && this.audioClip)
			{
				base.Duration = this.audioClip.length;
			}
			else
			{
				base.Duration = -1f;
			}
		}

		public override void FireEvent()
		{
			global::UnityEngine.AudioSource audioSource = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			if (!audioSource)
			{
				audioSource = base.AffectedObject.AddComponent<global::UnityEngine.AudioSource>();
				audioSource.playOnAwake = false;
			}
			if (audioSource.clip != this.audioClip)
			{
				audioSource.clip = this.audioClip;
			}
			audioSource.time = 0f;
			audioSource.loop = this.loop;
			if (!base.Sequence.IsPlaying)
			{
				return;
			}
			audioSource.Play();
		}

		public override void ProcessEvent(float deltaTime)
		{
			global::UnityEngine.AudioSource audioSource = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			if (!audioSource)
			{
				audioSource = base.AffectedObject.AddComponent<global::UnityEngine.AudioSource>();
				audioSource.playOnAwake = false;
			}
			if (audioSource.clip != this.audioClip)
			{
				audioSource.clip = this.audioClip;
			}
			if (audioSource.isPlaying)
			{
				return;
			}
			audioSource.time = deltaTime;
			if (base.Sequence.IsPlaying && !audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}

		public override void ManuallySetTime(float deltaTime)
		{
			global::UnityEngine.AudioSource component = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			if (!component)
			{
				return;
			}
			component.time = deltaTime;
		}

		public override void ResumeEvent()
		{
			global::UnityEngine.AudioSource component = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			if (!component)
			{
				return;
			}
			component.time = base.Sequence.RunningTime - base.FireTime;
			if (this.wasPlaying)
			{
				component.Play();
			}
		}

		public override void PauseEvent()
		{
			global::UnityEngine.AudioSource component = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			this.wasPlaying = false;
			if (component && component.isPlaying)
			{
				this.wasPlaying = true;
			}
			if (component)
			{
				component.Pause();
			}
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void EndEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			global::UnityEngine.AudioSource component = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			if (component)
			{
				component.Stop();
			}
		}

		public global::UnityEngine.AudioClip audioClip;

		public bool loop;

		private bool wasPlaying;
	}
}
