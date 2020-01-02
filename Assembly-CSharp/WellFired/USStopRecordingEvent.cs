using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Recording/Stop Recording")]
	[global::WellFired.USequencerFriendlyName("Stop Recording")]
	public class USStopRecordingEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Debug.Log("Recording events only work when in play mode");
				return;
			}
			global::WellFired.USRuntimeUtility.StopRecordingSequence();
		}

		public override void ProcessEvent(float deltaTime)
		{
		}
	}
}
