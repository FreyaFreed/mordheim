using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerFriendlyName("Rotate")]
	[global::WellFired.USequencerEvent("Transform/Rotate Object")]
	public class USRotateObjectEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			this.previousRotation = base.AffectedObject.transform.rotation;
			this.sourceOrientation = base.AffectedObject.transform.rotation;
		}

		public override void ProcessEvent(float deltaTime)
		{
			base.AffectedObject.transform.rotation = this.sourceOrientation;
			base.AffectedObject.transform.Rotate(this.rotationAxis, this.rotateSpeedPerSecond * deltaTime);
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
			base.AffectedObject.transform.rotation = this.previousRotation;
		}

		public void Update()
		{
			if (base.Duration < 0f)
			{
				base.Duration = 4f;
			}
		}

		public float rotateSpeedPerSecond = 90f;

		public global::UnityEngine.Vector3 rotationAxis = global::UnityEngine.Vector3.up;

		private global::UnityEngine.Quaternion sourceOrientation = global::UnityEngine.Quaternion.identity;

		private global::UnityEngine.Quaternion previousRotation = global::UnityEngine.Quaternion.identity;
	}
}
