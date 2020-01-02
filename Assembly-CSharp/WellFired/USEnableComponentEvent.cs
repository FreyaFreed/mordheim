using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerFriendlyName("Toggle Component")]
	[global::WellFired.USequencerEvent("Object/Toggle Component")]
	[global::WellFired.USequencerEventHideDuration]
	public class USEnableComponentEvent : global::WellFired.USEventBase
	{
		public string ComponentName
		{
			get
			{
				return this.componentName;
			}
			set
			{
				this.componentName = value;
			}
		}

		public override void FireEvent()
		{
			global::UnityEngine.Behaviour behaviour = base.AffectedObject.GetComponent(this.ComponentName) as global::UnityEngine.Behaviour;
			if (!behaviour)
			{
				return;
			}
			this.prevEnable = behaviour.enabled;
			behaviour.enabled = this.enableComponent;
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
			global::UnityEngine.Behaviour behaviour = base.AffectedObject.GetComponent(this.ComponentName) as global::UnityEngine.Behaviour;
			if (!behaviour)
			{
				return;
			}
			behaviour.enabled = this.prevEnable;
		}

		public bool enableComponent;

		private bool prevEnable;

		[global::UnityEngine.HideInInspector]
		[global::UnityEngine.SerializeField]
		private string componentName;
	}
}
