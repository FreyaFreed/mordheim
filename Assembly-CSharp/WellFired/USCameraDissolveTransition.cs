using System;
using UnityEngine;
using WellFired.Shared;

namespace WellFired
{
	[global::UnityEngine.ExecuteInEditMode]
	[global::WellFired.USequencerEvent("Camera/Transition/Dissolve")]
	[global::WellFired.USequencerFriendlyName("Dissolve Transition")]
	public class USCameraDissolveTransition : global::WellFired.USEventBase
	{
		private void OnGUI()
		{
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				return;
			}
			this.transition.ProcessTransitionFromOnGUI();
		}

		public override void FireEvent()
		{
			if (this.transition == null)
			{
				this.transition = new global::WellFired.Shared.BaseTransition();
			}
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				global::UnityEngine.Debug.LogError("Can't continue this transition with null cameras.");
				return;
			}
			this.transition.InitializeTransition(this.sourceCamera, this.destinationCamera, global::WellFired.Shared.TypeOfTransition.Dissolve);
		}

		public override void ProcessEvent(float deltaTime)
		{
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				return;
			}
			this.transition.ProcessEventFromNoneOnGUI(deltaTime, base.Duration);
		}

		public override void EndEvent()
		{
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				return;
			}
			this.transition.TransitionComplete();
		}

		public override void StopEvent()
		{
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				return;
			}
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				return;
			}
			this.transition.RevertTransition();
		}

		private global::WellFired.Shared.BaseTransition transition;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Camera sourceCamera;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Camera destinationCamera;
	}
}
