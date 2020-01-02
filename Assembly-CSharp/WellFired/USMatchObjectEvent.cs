using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerFriendlyName("Match Objects Orientation")]
	[global::WellFired.USequencerEvent("Transform/Match Objects Orientation")]
	public class USMatchObjectEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!this.objectToMatch)
			{
				global::UnityEngine.Debug.LogWarning("The USMatchObjectEvent event does not provice a object to match", this);
				return;
			}
			this.sourceRotation = base.AffectedObject.transform.rotation;
			this.sourcePosition = base.AffectedObject.transform.position;
		}

		public override void ProcessEvent(float deltaTime)
		{
			if (!this.objectToMatch)
			{
				global::UnityEngine.Debug.LogWarning("The USMatchObjectEvent event does not provice a object to look at", this);
				return;
			}
			float t = global::UnityEngine.Mathf.Clamp(this.inCurve.Evaluate(deltaTime), 0f, 1f);
			global::UnityEngine.Vector3 position = this.objectToMatch.transform.position;
			global::UnityEngine.Quaternion rotation = this.objectToMatch.transform.rotation;
			base.AffectedObject.transform.rotation = global::UnityEngine.Quaternion.Slerp(this.sourceRotation, rotation, t);
			base.AffectedObject.transform.position = global::UnityEngine.Vector3.Slerp(this.sourcePosition, position, t);
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
			base.AffectedObject.transform.rotation = this.sourceRotation;
			base.AffectedObject.transform.position = this.sourcePosition;
		}

		public global::UnityEngine.GameObject objectToMatch;

		public global::UnityEngine.AnimationCurve inCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe[]
		{
			new global::UnityEngine.Keyframe(0f, 0f),
			new global::UnityEngine.Keyframe(1f, 1f)
		});

		private global::UnityEngine.Quaternion sourceRotation = global::UnityEngine.Quaternion.identity;

		private global::UnityEngine.Vector3 sourcePosition = global::UnityEngine.Vector3.zero;
	}
}
