using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerFriendlyName("Set Ambient Light")]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Light/Set Ambient Light")]
	public class USSetAmbientLightEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			this.prevLightColor = global::UnityEngine.RenderSettings.ambientLight;
			global::UnityEngine.RenderSettings.ambientLight = this.lightColor;
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
			global::UnityEngine.RenderSettings.ambientLight = this.prevLightColor;
		}

		public global::UnityEngine.Color lightColor = global::UnityEngine.Color.red;

		private global::UnityEngine.Color prevLightColor;
	}
}
