using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Particle System/Stop Emitter")]
	[global::WellFired.USequencerFriendlyName("Stop Emitter (Legacy)")]
	public class USParticleEmitterStopEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			global::UnityEngine.ParticleSystem component = base.AffectedObject.GetComponent<global::UnityEngine.ParticleSystem>();
			if (!component)
			{
				global::UnityEngine.Debug.Log("Attempting to emit particles, but the object has no particleSystem USParticleEmitterStartEvent::FireEvent");
				return;
			}
			component.Stop();
		}

		public override void ProcessEvent(float deltaTime)
		{
		}
	}
}
