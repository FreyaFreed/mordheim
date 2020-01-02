using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerFriendlyName("Warp To Object")]
	[global::WellFired.USequencerEvent("Transform/Warp To Object")]
	[global::WellFired.USequencerEventHideDuration]
	public class USWarpToObject : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (this.objectToWarpTo)
			{
				base.AffectedObject.transform.position = this.objectToWarpTo.transform.position;
				if (this.useObjectRotation)
				{
					base.AffectedObject.transform.rotation = this.objectToWarpTo.transform.rotation;
				}
			}
			else
			{
				global::UnityEngine.Debug.LogError(base.AffectedObject.name + ": No Object attached to WarpToObjectSequencer Script");
			}
			this.previousTransform = base.AffectedObject.transform;
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
			if (this.previousTransform)
			{
				base.AffectedObject.transform.position = this.previousTransform.position;
				base.AffectedObject.transform.rotation = this.previousTransform.rotation;
			}
		}

		public global::UnityEngine.GameObject objectToWarpTo;

		public bool useObjectRotation;

		private global::UnityEngine.Transform previousTransform;
	}
}
