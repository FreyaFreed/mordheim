using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Attach/Attach To Parent")]
	[global::WellFired.USequencerFriendlyName("Attach Object To Parent")]
	[global::WellFired.USequencerEventHideDuration]
	public class USAttachToParentEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!this.parentObject)
			{
				global::UnityEngine.Debug.Log("USAttachEvent has been asked to attach an object, but it hasn't been given a parent from USAttachEvent::FireEvent");
				return;
			}
			this.originalParent = base.AffectedObject.transform.parent;
			base.AffectedObject.transform.parent = this.parentObject;
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
			base.AffectedObject.transform.SetParent(this.originalParent);
		}

		public global::UnityEngine.Transform parentObject;

		private global::UnityEngine.Transform originalParent;
	}
}
