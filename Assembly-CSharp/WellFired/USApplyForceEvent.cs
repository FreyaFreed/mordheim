using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Physics/Apply Force")]
	[global::WellFired.USequencerFriendlyName("Apply Force")]
	public class USApplyForceEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			global::UnityEngine.Rigidbody component = base.AffectedObject.GetComponent<global::UnityEngine.Rigidbody>();
			if (!component)
			{
				global::UnityEngine.Debug.Log("Attempting to apply an impulse to an object, but it has no rigid body from USequencerApplyImpulseEvent::FireEvent");
				return;
			}
			component.AddForceAtPosition(this.direction * this.strength, base.transform.position, this.type);
			this.previousTransform = component.transform;
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
			component.Sleep();
			if (this.previousTransform)
			{
				base.AffectedObject.transform.position = this.previousTransform.position;
				base.AffectedObject.transform.rotation = this.previousTransform.rotation;
			}
		}

		public global::UnityEngine.Vector3 direction = global::UnityEngine.Vector3.up;

		public float strength = 1f;

		public global::UnityEngine.ForceMode type = global::UnityEngine.ForceMode.Impulse;

		private global::UnityEngine.Transform previousTransform;
	}
}
