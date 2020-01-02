using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Fade Screen")]
	[global::WellFired.USequencerEvent("Fullscreen/Fade Screen")]
	public class USFadeScreenEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
		}

		public override void ProcessEvent(float deltaTime)
		{
			this.currentCurveSampleTime = deltaTime;
			if (!global::WellFired.USFadeScreenEvent.texture)
			{
				global::WellFired.USFadeScreenEvent.texture = new global::UnityEngine.Texture2D(1, 1, global::UnityEngine.TextureFormat.ARGB32, false);
			}
			float num = this.fadeCurve.Evaluate(this.currentCurveSampleTime);
			num = global::UnityEngine.Mathf.Min(global::UnityEngine.Mathf.Max(0f, num), 1f);
			global::WellFired.USFadeScreenEvent.texture.SetPixel(0, 0, new global::UnityEngine.Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, num));
			global::WellFired.USFadeScreenEvent.texture.Apply();
		}

		public override void EndEvent()
		{
			if (!global::WellFired.USFadeScreenEvent.texture)
			{
				global::WellFired.USFadeScreenEvent.texture = new global::UnityEngine.Texture2D(1, 1, global::UnityEngine.TextureFormat.ARGB32, false);
			}
			float num = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
			num = global::UnityEngine.Mathf.Min(global::UnityEngine.Mathf.Max(0f, num), 1f);
			global::WellFired.USFadeScreenEvent.texture.SetPixel(0, 0, new global::UnityEngine.Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, num));
			global::WellFired.USFadeScreenEvent.texture.Apply();
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			this.currentCurveSampleTime = 0f;
			if (!global::WellFired.USFadeScreenEvent.texture)
			{
				global::WellFired.USFadeScreenEvent.texture = new global::UnityEngine.Texture2D(1, 1, global::UnityEngine.TextureFormat.ARGB32, false);
			}
			global::WellFired.USFadeScreenEvent.texture.SetPixel(0, 0, new global::UnityEngine.Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, 0f));
			global::WellFired.USFadeScreenEvent.texture.Apply();
		}

		private void OnGUI()
		{
			if (!base.Sequence.IsPlaying)
			{
				return;
			}
			float num = 0f;
			foreach (global::UnityEngine.Keyframe keyframe in this.fadeCurve.keys)
			{
				if (keyframe.time > num)
				{
					num = keyframe.time;
				}
			}
			base.Duration = num;
			if (!global::WellFired.USFadeScreenEvent.texture)
			{
				return;
			}
			int depth = global::UnityEngine.GUI.depth;
			global::UnityEngine.GUI.depth = (int)this.uiLayer;
			global::UnityEngine.GUI.DrawTexture(new global::UnityEngine.Rect(0f, 0f, (float)global::UnityEngine.Screen.width, (float)global::UnityEngine.Screen.height), global::WellFired.USFadeScreenEvent.texture);
			global::UnityEngine.GUI.depth = depth;
		}

		private void OnEnable()
		{
			if (global::WellFired.USFadeScreenEvent.texture == null)
			{
				global::WellFired.USFadeScreenEvent.texture = new global::UnityEngine.Texture2D(1, 1, global::UnityEngine.TextureFormat.ARGB32, false);
			}
			global::WellFired.USFadeScreenEvent.texture.SetPixel(0, 0, new global::UnityEngine.Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, 0f));
			global::WellFired.USFadeScreenEvent.texture.Apply();
		}

		public global::WellFired.UILayer uiLayer;

		public global::UnityEngine.AnimationCurve fadeCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe[]
		{
			new global::UnityEngine.Keyframe(0f, 0f),
			new global::UnityEngine.Keyframe(1f, 1f),
			new global::UnityEngine.Keyframe(3f, 1f),
			new global::UnityEngine.Keyframe(4f, 0f)
		});

		public global::UnityEngine.Color fadeColour = global::UnityEngine.Color.black;

		private float currentCurveSampleTime;

		public static global::UnityEngine.Texture2D texture;
	}
}
