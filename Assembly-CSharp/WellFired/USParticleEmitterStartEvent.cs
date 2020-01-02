using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Particle System/Start Emitter")]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Start Emitter (Legacy)")]
	public class USParticleEmitterStartEvent : global::WellFired.USEventBase
	{
		public void Update()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			global::UnityEngine.ParticleSystem component = base.AffectedObject.GetComponent<global::UnityEngine.ParticleSystem>();
			if (component)
			{
				base.Duration = component.duration + component.startLifetime;
			}
		}

		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			global::UnityEngine.ParticleSystem component = base.AffectedObject.GetComponent<global::UnityEngine.ParticleSystem>();
			if (!component)
			{
				global::UnityEngine.Debug.Log("Attempting to emit particles, but the object has no particleSystem USParticleEmitterStartEvent::FireEvent");
				return;
			}
			if (!global::UnityEngine.Application.isPlaying)
			{
				return;
			}
			component.Play();
		}

		public override void ProcessEvent(float deltaTime)
		{
			if (global::UnityEngine.Application.isPlaying)
			{
				return;
			}
			global::UnityEngine.ParticleSystem component = base.AffectedObject.GetComponent<global::UnityEngine.ParticleSystem>();
			component.Simulate(deltaTime);
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			global::UnityEngine.ParticleSystem component = base.AffectedObject.GetComponent<global::UnityEngine.ParticleSystem>();
			if (component)
			{
				component.Stop();
			}
		}
	}
}
