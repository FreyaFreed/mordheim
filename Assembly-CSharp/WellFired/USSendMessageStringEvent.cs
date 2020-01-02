using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Signal/Send Message (String)")]
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Send Message (String)")]
	public class USSendMessageStringEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!global::UnityEngine.Application.isPlaying)
			{
				return;
			}
			if (this.receiver)
			{
				this.receiver.SendMessage(this.action, this.valueToSend);
			}
			else
			{
				global::UnityEngine.Debug.LogWarning(string.Format("No receiver of signal \"{0}\" on object {1} ({2})", this.action, this.receiver.name, this.receiver.GetType().Name), this.receiver);
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public global::UnityEngine.GameObject receiver;

		public string action = "OnSignal";

		[global::UnityEngine.SerializeField]
		private string valueToSend;
	}
}
