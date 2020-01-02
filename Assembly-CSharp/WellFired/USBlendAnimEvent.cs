using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerFriendlyName("Blend Animation (Legacy)")]
	[global::WellFired.USequencerEvent("Animation (Legacy)/Blend Animation")]
	public class USBlendAnimEvent : global::WellFired.USEventBase
	{
		public void Update()
		{
			if (base.Duration < 0f)
			{
				base.Duration = 2f;
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
			component.wrapMode = global::UnityEngine.WrapMode.Loop;
			component.Play(this.animationClipSource.name);
		}

		public override void ProcessEvent(float deltaTime)
		{
			global::UnityEngine.Animation animation = base.AffectedObject.GetComponent<global::UnityEngine.Animation>();
			if (!animation)
			{
				global::UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Trying to play an animation : ",
					this.animationClipSource.name,
					" but : ",
					base.AffectedObject,
					" doesn't have an animation component, we will add one, this time, though you should add it manually"
				}));
				animation = base.AffectedObject.AddComponent<global::UnityEngine.Animation>();
			}
			if (animation[this.animationClipSource.name] == null)
			{
				global::UnityEngine.Debug.LogError("Trying to play an animation : " + this.animationClipSource.name + " but it isn't in the animation list. I will add it, this time, though you should add it manually.");
				animation.AddClip(this.animationClipSource, this.animationClipSource.name);
			}
			if (animation[this.animationClipDest.name] == null)
			{
				global::UnityEngine.Debug.LogError("Trying to play an animation : " + this.animationClipDest.name + " but it isn't in the animation list. I will add it, this time, though you should add it manually.");
				animation.AddClip(this.animationClipDest, this.animationClipDest.name);
			}
			if (deltaTime < this.blendPoint)
			{
				animation.CrossFade(this.animationClipSource.name);
			}
			else
			{
				animation.CrossFade(this.animationClipDest.name);
			}
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

		public global::UnityEngine.AnimationClip animationClipSource;

		public global::UnityEngine.AnimationClip animationClipDest;

		public float blendPoint = 1f;
	}
}
