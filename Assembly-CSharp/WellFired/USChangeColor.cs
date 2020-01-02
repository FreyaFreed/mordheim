using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Change Color")]
	[global::WellFired.USequencerEvent("Render/Change Objects Color")]
	public class USChangeColor : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			if (!global::UnityEngine.Application.isPlaying && global::UnityEngine.Application.isEditor)
			{
				this.previousColor = base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().sharedMaterial.color;
				base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().sharedMaterial.color = this.newColor;
			}
			else
			{
				this.previousColor = base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().material.color;
				base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().material.color = this.newColor;
			}
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
			if (!global::UnityEngine.Application.isPlaying && global::UnityEngine.Application.isEditor)
			{
				base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().sharedMaterial.color = this.previousColor;
			}
			else
			{
				base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().material.color = this.previousColor;
			}
		}

		public global::UnityEngine.Color newColor;

		private global::UnityEngine.Color previousColor;
	}
}
