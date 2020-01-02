using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Time/Time Scale")]
	[global::WellFired.USequencerFriendlyName("Time Scale")]
	public class USTimeScaleEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			this.prevTimeScale = global::UnityEngine.Time.timeScale;
		}

		public override void ProcessEvent(float deltaTime)
		{
			this.currentCurveSampleTime = deltaTime;
			global::UnityEngine.Time.timeScale = global::UnityEngine.Mathf.Max(0f, this.scaleCurve.Evaluate(this.currentCurveSampleTime));
		}

		public override void EndEvent()
		{
			float time = this.scaleCurve.keys[this.scaleCurve.length - 1].time;
			global::UnityEngine.Time.timeScale = global::UnityEngine.Mathf.Max(0f, this.scaleCurve.Evaluate(time));
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			this.currentCurveSampleTime = 0f;
			global::UnityEngine.Time.timeScale = this.prevTimeScale;
		}

		public global::UnityEngine.AnimationCurve scaleCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe[]
		{
			new global::UnityEngine.Keyframe(0f, 1f),
			new global::UnityEngine.Keyframe(0.1f, 0.2f),
			new global::UnityEngine.Keyframe(2f, 1f)
		});

		private float currentCurveSampleTime;

		private float prevTimeScale = 1f;
	}
}
