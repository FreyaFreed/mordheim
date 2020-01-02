using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Fullscreen/Print Text")]
	[global::WellFired.USequencerFriendlyName("Print Text")]
	public class USPrintTextEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			this.priorText = this.currentText;
			this.currentText = this.textToPrint;
			if (base.Duration > 0f)
			{
				this.currentText = string.Empty;
			}
			this.display = true;
		}

		public override void ProcessEvent(float deltaTime)
		{
			if (this.printRatePerCharacter <= 0f)
			{
				this.currentText = this.textToPrint;
			}
			else
			{
				int num = (int)(deltaTime / this.printRatePerCharacter);
				if (num < this.textToPrint.Length)
				{
					this.currentText = this.textToPrint.Substring(0, num);
				}
				else
				{
					this.currentText = this.textToPrint;
				}
			}
			this.display = true;
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			this.currentText = this.priorText;
			this.display = false;
		}

		private void OnGUI()
		{
			if (!base.Sequence.IsPlaying)
			{
				return;
			}
			if (!this.display)
			{
				return;
			}
			int depth = global::UnityEngine.GUI.depth;
			global::UnityEngine.GUI.depth = (int)this.uiLayer;
			global::UnityEngine.GUI.Label(this.position, this.currentText);
			global::UnityEngine.GUI.depth = depth;
		}

		public global::WellFired.UILayer uiLayer;

		public string textToPrint = string.Empty;

		public global::UnityEngine.Rect position = new global::UnityEngine.Rect(0f, 0f, (float)global::UnityEngine.Screen.width, (float)global::UnityEngine.Screen.height);

		public float printRatePerCharacter;

		private string priorText = string.Empty;

		private string currentText = string.Empty;

		private bool display;
	}
}
