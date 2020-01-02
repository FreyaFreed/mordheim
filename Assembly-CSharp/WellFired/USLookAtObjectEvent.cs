using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEvent("Transform/Look At Object")]
	[global::WellFired.USequencerFriendlyName("Look At Object")]
	public class USLookAtObjectEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!this.objectToLookAt)
			{
				global::UnityEngine.Debug.LogWarning("The USLookAtObject event does not provice a object to look at", this);
				return;
			}
			this.previousRotation = base.AffectedObject.transform.rotation;
			this.sourceOrientation = base.AffectedObject.transform.rotation;
		}

		public override void ProcessEvent(float deltaTime)
		{
			if (!this.objectToLookAt)
			{
				global::UnityEngine.Debug.LogWarning("The USLookAtObject event does not provice a object to look at", this);
				return;
			}
			float time = this.inCurve[this.inCurve.length - 1].time;
			float num = this.lookAtTime + time;
			float t = 1f;
			if (deltaTime <= time)
			{
				t = global::UnityEngine.Mathf.Clamp(this.inCurve.Evaluate(deltaTime), 0f, 1f);
			}
			else if (deltaTime >= num)
			{
				t = global::UnityEngine.Mathf.Clamp(this.outCurve.Evaluate(deltaTime - num), 0f, 1f);
			}
			global::UnityEngine.Vector3 position = base.AffectedObject.transform.position;
			global::UnityEngine.Vector3 position2 = this.objectToLookAt.transform.position;
			global::UnityEngine.Vector3 forward = position2 - position;
			global::UnityEngine.Quaternion b = global::UnityEngine.Quaternion.LookRotation(forward);
			base.AffectedObject.transform.rotation = global::UnityEngine.Quaternion.Slerp(this.sourceOrientation, b, t);
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
			base.AffectedObject.transform.rotation = this.previousRotation;
		}

		public global::UnityEngine.GameObject objectToLookAt;

		public global::UnityEngine.AnimationCurve inCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe[]
		{
			new global::UnityEngine.Keyframe(0f, 0f),
			new global::UnityEngine.Keyframe(1f, 1f)
		});

		public global::UnityEngine.AnimationCurve outCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe[]
		{
			new global::UnityEngine.Keyframe(0f, 1f),
			new global::UnityEngine.Keyframe(1f, 0f)
		});

		public float lookAtTime = 2f;

		private global::UnityEngine.Quaternion sourceOrientation = global::UnityEngine.Quaternion.identity;

		private global::UnityEngine.Quaternion previousRotation = global::UnityEngine.Quaternion.identity;
	}
}
