using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Animation (Mecanim)/Animator/Set Layer Weight")]
	[global::WellFired.USequencerFriendlyName("Set Layer Weight")]
	public class USSetAnimatorLayerWeight : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			global::UnityEngine.Animator component = base.AffectedObject.GetComponent<global::UnityEngine.Animator>();
			if (!component)
			{
				global::UnityEngine.Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			if (this.layerIndex < 0)
			{
				global::UnityEngine.Debug.LogWarning("Set Animator Layer weight, incorrect index : " + this.layerIndex);
				return;
			}
			this.prevLayerWeight = component.GetLayerWeight(this.layerIndex);
			component.SetLayerWeight(this.layerIndex, this.layerWeight);
		}

		public override void ProcessEvent(float runningTime)
		{
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			global::UnityEngine.Animator component = base.AffectedObject.GetComponent<global::UnityEngine.Animator>();
			if (!component)
			{
				return;
			}
			if (this.layerIndex < 0)
			{
				return;
			}
			component.SetLayerWeight(this.layerIndex, this.prevLayerWeight);
		}

		public float layerWeight = 1f;

		public int layerIndex = -1;

		private float prevLayerWeight;
	}
}
