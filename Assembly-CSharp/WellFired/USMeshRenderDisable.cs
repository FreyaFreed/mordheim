using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerFriendlyName("Toggle Mesh Renderer")]
	[global::WellFired.USequencerEvent("Render/Toggle Mesh Renderer")]
	public class USMeshRenderDisable : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			global::UnityEngine.MeshRenderer component = base.AffectedObject.GetComponent<global::UnityEngine.MeshRenderer>();
			if (!component)
			{
				global::UnityEngine.Debug.LogWarning("You didn't add a Mesh Renderer to the Affected Object", base.AffectedObject);
				return;
			}
			this.previousEnable = component.enabled;
			component.enabled = this.enable;
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public override void EndEvent()
		{
			this.UndoEvent();
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
			global::UnityEngine.MeshRenderer component = base.AffectedObject.GetComponent<global::UnityEngine.MeshRenderer>();
			if (!component)
			{
				global::UnityEngine.Debug.LogWarning("You didn't add a Mesh Renderer to the Affected Object", base.AffectedObject);
				return;
			}
			component.enabled = this.previousEnable;
		}

		public bool enable;

		private bool previousEnable;
	}
}
