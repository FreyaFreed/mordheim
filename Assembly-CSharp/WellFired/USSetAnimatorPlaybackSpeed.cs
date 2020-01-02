using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Set Playback Speed")]
	[global::WellFired.USequencerEvent("Animation (Mecanim)/Animator/Set Playback Speed")]
	public class USSetAnimatorPlaybackSpeed : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			global::UnityEngine.Animator component = base.AffectedObject.GetComponent<global::UnityEngine.Animator>();
			if (!component)
			{
				global::UnityEngine.Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			this.prevPlaybackSpeed = component.speed;
			component.speed = this.playbackSpeed;
		}

		public override void ProcessEvent(float runningTime)
		{
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			global::UnityEngine.Animator component = base.AffectedObject.GetComponent<global::UnityEngine.Animator>();
			if (!component)
			{
				return;
			}
			component.speed = this.prevPlaybackSpeed;
		}

		public float playbackSpeed = 1f;

		private float prevPlaybackSpeed;
	}
}
