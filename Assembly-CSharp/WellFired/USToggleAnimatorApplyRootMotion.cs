using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Animation (Mecanim)/Animator/Toggle Apply Root Motion")]
	[global::WellFired.USequencerFriendlyName("Toggle Apply Root Motion")]
	public class USToggleAnimatorApplyRootMotion : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			global::UnityEngine.Animator component = base.AffectedObject.GetComponent<global::UnityEngine.Animator>();
			if (!component)
			{
				global::UnityEngine.Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			this.prevApplyRootMotion = component.applyRootMotion;
			component.applyRootMotion = this.applyRootMotion;
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
			component.applyRootMotion = this.prevApplyRootMotion;
		}

		public bool applyRootMotion = true;

		private bool prevApplyRootMotion;
	}
}
