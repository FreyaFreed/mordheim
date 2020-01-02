using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Debug/Log Message")]
	[global::WellFired.USequencerFriendlyName("Debug Message")]
	public class USMessageEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			global::UnityEngine.Debug.Log(this.message);
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public string message = "Default Message";
	}
}
