using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerFriendlyName("Set Culling Mask")]
	[global::UnityEngine.ExecuteInEditMode]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Camera/Set Culling Mask")]
	public class USCameraSetCullingMask : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (base.AffectedObject != null)
			{
				this.cameraToAffect = base.AffectedObject.GetComponent<global::UnityEngine.Camera>();
			}
			if (this.cameraToAffect)
			{
				this.prevLayerMask = this.cameraToAffect.cullingMask;
				this.cameraToAffect.cullingMask = this.newLayerMask;
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public override void EndEvent()
		{
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			if (this.cameraToAffect)
			{
				this.cameraToAffect.cullingMask = this.prevLayerMask;
			}
		}

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.LayerMask newLayerMask;

		private int prevLayerMask;

		private global::UnityEngine.Camera cameraToAffect;
	}
}
