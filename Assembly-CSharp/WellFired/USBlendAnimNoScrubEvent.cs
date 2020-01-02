using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerFriendlyName("Blend Animation No Scrub (Legacy)")]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Animation (Legacy)/Blend Animation No Scrub")]
	public class USBlendAnimNoScrubEvent : global::WellFired.USEventBase
	{
		public void Update()
		{
			if (base.Duration < 0f)
			{
				base.Duration = this.blendedAnimation.length;
			}
		}

		public override void FireEvent()
		{
			global::UnityEngine.Animation component = base.AffectedObject.GetComponent<global::UnityEngine.Animation>();
			if (!component)
			{
				global::UnityEngine.Debug.Log("Attempting to play an animation on a GameObject without an Animation Component from USPlayAnimEvent.FireEvent");
				return;
			}
			component[this.blendedAnimation.name].wrapMode = global::UnityEngine.WrapMode.Once;
			component[this.blendedAnimation.name].layer = 1;
		}

		public override void ProcessEvent(float deltaTime)
		{
			base.GetComponent<global::UnityEngine.Animation>().CrossFade(this.blendedAnimation.name);
		}

		public override void StopEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			global::UnityEngine.Animation component = base.AffectedObject.GetComponent<global::UnityEngine.Animation>();
			if (component)
			{
				component.Stop();
			}
		}

		public global::UnityEngine.AnimationClip blendedAnimation;
	}
}
