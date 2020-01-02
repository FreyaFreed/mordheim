using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Physics/Sleep Rigid Body")]
	[global::WellFired.USequencerFriendlyName("Sleep Rigid Body")]
	[global::WellFired.USequencerEventHideDuration]
	public class USSleepRigidBody : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			global::UnityEngine.Rigidbody component = base.AffectedObject.GetComponent<global::UnityEngine.Rigidbody>();
			if (!component)
			{
				global::UnityEngine.Debug.Log("Attempting to Nullify a force on an object, but it has no rigid body from USSleepRigidBody::FireEvent");
				return;
			}
			component.Sleep();
		}

		public override void ProcessEvent(float deltaTime)
		{
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
			global::UnityEngine.Rigidbody component = base.AffectedObject.GetComponent<global::UnityEngine.Rigidbody>();
			if (!component)
			{
				return;
			}
			component.WakeUp();
		}
	}
}
