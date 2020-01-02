using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Animation (Mecanim)/Animator/Toggle Stabalize Feet")]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Toggle Stabalize Feet")]
	public class USToggleAnimatorStabalizeFeet : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			global::UnityEngine.Animator component = base.AffectedObject.GetComponent<global::UnityEngine.Animator>();
			if (!component)
			{
				global::UnityEngine.Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			this.prevStabalizeFeet = component.stabilizeFeet;
			component.stabilizeFeet = this.stabalizeFeet;
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
			component.stabilizeFeet = this.prevStabalizeFeet;
		}

		public bool stabalizeFeet = true;

		private bool prevStabalizeFeet;
	}
}
