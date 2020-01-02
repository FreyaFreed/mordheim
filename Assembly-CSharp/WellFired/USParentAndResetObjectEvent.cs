using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Transform/Parent and reset Transform")]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Parent and reset Transform")]
	public class USParentAndResetObjectEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			this.previousParent = this.child.parent;
			this.previousPosition = this.child.localPosition;
			this.previousRotation = this.child.localRotation;
			this.child.parent = this.parent;
			this.child.localPosition = global::UnityEngine.Vector3.zero;
			this.child.localRotation = global::UnityEngine.Quaternion.identity;
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
			this.child.parent = this.previousParent;
			this.child.localPosition = this.previousPosition;
			this.child.localRotation = this.previousRotation;
		}

		public global::UnityEngine.Transform parent;

		public global::UnityEngine.Transform child;

		private global::UnityEngine.Transform previousParent;

		private global::UnityEngine.Vector3 previousPosition;

		private global::UnityEngine.Quaternion previousRotation;
	}
}
