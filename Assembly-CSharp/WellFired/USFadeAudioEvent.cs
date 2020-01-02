using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Audio/Fade Audio")]
	[global::WellFired.USequencerFriendlyName("Fade Audio")]
	[global::WellFired.USequencerEventHideDuration]
	public class USFadeAudioEvent : global::WellFired.USEventBase
	{
		public void Update()
		{
			base.Duration = (float)this.fadeCurve.length;
		}

		public override void FireEvent()
		{
			global::UnityEngine.AudioSource component = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			if (!component)
			{
				global::UnityEngine.Debug.LogWarning("Trying to fade audio on an object without an AudioSource");
				return;
			}
			this.previousVolume = component.volume;
		}

		public override void ProcessEvent(float deltaTime)
		{
			global::UnityEngine.AudioSource component = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			if (!component)
			{
				global::UnityEngine.Debug.LogWarning("Trying to fade audio on an object without an AudioSource");
				return;
			}
			component.volume = this.fadeCurve.Evaluate(deltaTime);
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			global::UnityEngine.AudioSource component = base.AffectedObject.GetComponent<global::UnityEngine.AudioSource>();
			if (!component)
			{
				global::UnityEngine.Debug.LogWarning("Trying to fade audio on an object without an AudioSource");
				return;
			}
			component.volume = this.previousVolume;
		}

		private float previousVolume = 1f;

		public global::UnityEngine.AnimationCurve fadeCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe[]
		{
			new global::UnityEngine.Keyframe(0f, 1f),
			new global::UnityEngine.Keyframe(1f, 0f)
		});
	}
}
