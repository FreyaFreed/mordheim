using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Animation (Legacy)/Play Animation Advanced")]
	[global::WellFired.USequencerFriendlyName("Play Advanced Animation (Legacy)")]
	public class USPlayAdvancedAnimEvent : global::WellFired.USEventBase
	{
		public void Update()
		{
			if (this.wrapMode != global::UnityEngine.WrapMode.Loop && this.animationClip)
			{
				base.Duration = this.animationClip.length / this.playbackSpeed;
			}
		}

		public override void FireEvent()
		{
			if (!this.animationClip)
			{
				global::UnityEngine.Debug.Log("Attempting to play an animation on a GameObject but you haven't given the event an animation clip from USPlayAnimEvent::FireEvent");
				return;
			}
			global::UnityEngine.Animation component = base.AffectedObject.GetComponent<global::UnityEngine.Animation>();
			if (!component)
			{
				global::UnityEngine.Debug.Log("Attempting to play an animation on a GameObject without an Animation Component from USPlayAnimEvent.FireEvent");
				return;
			}
			component.wrapMode = this.wrapMode;
			if (this.crossFadeAnimation)
			{
				component.CrossFade(this.animationClip.name);
			}
			else
			{
				component.Play(this.animationClip.name);
			}
			global::UnityEngine.AnimationState animationState = component[this.animationClip.name];
			if (!animationState)
			{
				return;
			}
			animationState.enabled = true;
			animationState.weight = this.animationWeight;
			animationState.blendMode = this.blendMode;
			animationState.layer = this.animationLayer;
			animationState.speed = this.playbackSpeed;
		}

		public override void ProcessEvent(float deltaTime)
		{
			global::UnityEngine.Animation animation = base.AffectedObject.GetComponent<global::UnityEngine.Animation>();
			if (!animation)
			{
				global::UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Trying to play an animation : ",
					this.animationClip.name,
					" but : ",
					base.AffectedObject,
					" doesn't have an animation component, we will add one, this time, though you should add it manually"
				}));
				animation = base.AffectedObject.AddComponent<global::UnityEngine.Animation>();
			}
			if (animation[this.animationClip.name] == null)
			{
				global::UnityEngine.Debug.LogError("Trying to play an animation : " + this.animationClip.name + " but it isn't in the animation list. I will add it, this time, though you should add it manually.");
				animation.AddClip(this.animationClip, this.animationClip.name);
			}
			global::UnityEngine.AnimationState animationState = animation[this.animationClip.name];
			if (!animation.IsPlaying(this.animationClip.name))
			{
				animation.wrapMode = this.wrapMode;
				if (this.crossFadeAnimation)
				{
					animation.CrossFade(this.animationClip.name);
				}
				else
				{
					animation.Play(this.animationClip.name);
				}
			}
			animationState.weight = this.animationWeight;
			animationState.blendMode = this.blendMode;
			animationState.layer = this.animationLayer;
			animationState.speed = this.playbackSpeed;
			animationState.time = deltaTime * this.playbackSpeed;
			animationState.enabled = true;
			animation.Sample();
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

		public override void EndEvent()
		{
			this.StopEvent();
		}

		public global::UnityEngine.AnimationClip animationClip;

		public global::UnityEngine.WrapMode wrapMode;

		public global::UnityEngine.AnimationBlendMode blendMode;

		public float playbackSpeed = 1f;

		public float animationWeight = 1f;

		public int animationLayer = 1;

		public bool crossFadeAnimation;
	}
}
