using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Recording/Start Recording")]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Start Recording")]
	public class USStartRecordingEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Debug.Log("Recording events only work when in play mode");
				return;
			}
			global::WellFired.USRuntimeUtility.StartRecordingSequence(base.Sequence, global::WellFired.USRecordRuntimePreferences.CapturePath, global::WellFired.USRecord.GetFramerate(), global::WellFired.USRecord.GetUpscaleAmount());
		}

		public override void ProcessEvent(float deltaTime)
		{
		}
	}
}
