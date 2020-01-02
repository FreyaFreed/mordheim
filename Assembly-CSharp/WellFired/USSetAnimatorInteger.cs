﻿using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerFriendlyName("Set Mecanim Integer")]
	[global::WellFired.USequencerEvent("Animation (Mecanim)/Animator/Set Value/Integer")]
	[global::WellFired.USequencerEventHideDuration]
	public class USSetAnimatorInteger : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			global::UnityEngine.Animator component = base.AffectedObject.GetComponent<global::UnityEngine.Animator>();
			if (!component)
			{
				global::UnityEngine.Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			if (this.valueName.Length == 0)
			{
				global::UnityEngine.Debug.LogWarning("Invalid name passed to the uSequencer Event Set Float", this);
				return;
			}
			this.hash = global::UnityEngine.Animator.StringToHash(this.valueName);
			this.prevValue = component.GetInteger(this.hash);
			component.SetInteger(this.hash, this.Value);
		}

		public override void ProcessEvent(float runningTime)
		{
			global::UnityEngine.Animator component = base.AffectedObject.GetComponent<global::UnityEngine.Animator>();
			if (!component)
			{
				global::UnityEngine.Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			if (this.valueName.Length == 0)
			{
				global::UnityEngine.Debug.LogWarning("Invalid name passed to the uSequencer Event Set Float", this);
				return;
			}
			this.hash = global::UnityEngine.Animator.StringToHash(this.valueName);
			this.prevValue = component.GetInteger(this.hash);
			component.SetInteger(this.hash, this.Value);
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			global::UnityEngine.Animator component = base.AffectedObject.GetComponent<global::UnityEngine.Animator>();
			if (!component)
			{
				return;
			}
			if (this.valueName.Length == 0)
			{
				return;
			}
			component.SetInteger(this.hash, this.prevValue);
		}

		public string valueName = string.Empty;

		public int Value;

		private int prevValue;

		private int hash;
	}
}
