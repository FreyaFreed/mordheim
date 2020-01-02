using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Change Texture")]
	[global::WellFired.USequencerEvent("Render/Change Objects Texture")]
	public class USChangeTexture : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			if (!this.newTexture)
			{
				global::UnityEngine.Debug.LogWarning("you've not given a texture to the USChangeTexture Event", this);
				return;
			}
			if (!global::UnityEngine.Application.isPlaying && global::UnityEngine.Application.isEditor)
			{
				this.previousTexture = base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().sharedMaterial.mainTexture;
				base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().sharedMaterial.mainTexture = this.newTexture;
			}
			else
			{
				this.previousTexture = base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().material.mainTexture;
				base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().material.mainTexture = this.newTexture;
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
			if (!this.previousTexture)
			{
				return;
			}
			if (!global::UnityEngine.Application.isPlaying && global::UnityEngine.Application.isEditor)
			{
				base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().sharedMaterial.mainTexture = this.previousTexture;
			}
			else
			{
				base.AffectedObject.GetComponent<global::UnityEngine.Renderer>().material.mainTexture = this.previousTexture;
			}
			this.previousTexture = null;
		}

		public global::UnityEngine.Texture newTexture;

		private global::UnityEngine.Texture previousTexture;
	}
}
