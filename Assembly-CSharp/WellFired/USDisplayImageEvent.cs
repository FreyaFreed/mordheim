using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Fullscreen/Display Image")]
	[global::WellFired.USequencerFriendlyName("Display Image")]
	[global::WellFired.USequencerEventHideDuration]
	public class USDisplayImageEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!this.displayImage)
			{
				global::UnityEngine.Debug.LogWarning("Trying to use a DisplayImage Event, but you didn't give it an image to display", this);
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
			this.currentCurveSampleTime = deltaTime;
		}

		public override void EndEvent()
		{
			float b = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
			b = global::UnityEngine.Mathf.Min(global::UnityEngine.Mathf.Max(0f, b), 1f);
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			this.currentCurveSampleTime = 0f;
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
			float num2 = this.fadeCurve.Evaluate(this.currentCurveSampleTime);
			num2 = global::UnityEngine.Mathf.Min(global::UnityEngine.Mathf.Max(0f, num2), 1f);
			if (!this.displayImage)
			{
				return;
			}
			global::UnityEngine.Rect position = new global::UnityEngine.Rect((float)global::UnityEngine.Screen.width * 0.5f, (float)global::UnityEngine.Screen.height * 0.5f, (float)this.displayImage.width, (float)this.displayImage.height);
			switch (this.displayPosition)
			{
			case global::WellFired.UIPosition.TopLeft:
				position.x = 0f;
				position.y = 0f;
				break;
			case global::WellFired.UIPosition.TopRight:
				position.x = (float)global::UnityEngine.Screen.width;
				position.y = 0f;
				break;
			case global::WellFired.UIPosition.BottomLeft:
				position.x = 0f;
				position.y = (float)global::UnityEngine.Screen.height;
				break;
			case global::WellFired.UIPosition.BottomRight:
				position.x = (float)global::UnityEngine.Screen.width;
				position.y = (float)global::UnityEngine.Screen.height;
				break;
			}
			switch (this.anchorPosition)
			{
			case global::WellFired.UIPosition.Center:
				position.x -= (float)this.displayImage.width * 0.5f;
				position.y -= (float)this.displayImage.height * 0.5f;
				break;
			case global::WellFired.UIPosition.TopRight:
				position.x -= (float)this.displayImage.width;
				break;
			case global::WellFired.UIPosition.BottomLeft:
				position.y -= (float)this.displayImage.height;
				break;
			case global::WellFired.UIPosition.BottomRight:
				position.x -= (float)this.displayImage.width;
				position.y -= (float)this.displayImage.height;
				break;
			}
			global::UnityEngine.GUI.depth = (int)this.uiLayer;
			global::UnityEngine.Color color = global::UnityEngine.GUI.color;
			global::UnityEngine.GUI.color = new global::UnityEngine.Color(1f, 1f, 1f, num2);
			global::UnityEngine.GUI.DrawTexture(position, this.displayImage);
			global::UnityEngine.GUI.color = color;
		}

		public global::WellFired.UILayer uiLayer;

		public global::UnityEngine.AnimationCurve fadeCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe[]
		{
			new global::UnityEngine.Keyframe(0f, 0f),
			new global::UnityEngine.Keyframe(1f, 1f),
			new global::UnityEngine.Keyframe(3f, 1f),
			new global::UnityEngine.Keyframe(4f, 0f)
		});

		public global::UnityEngine.Texture2D displayImage;

		public global::WellFired.UIPosition displayPosition;

		public global::WellFired.UIPosition anchorPosition;

		private float currentCurveSampleTime;
	}
}
