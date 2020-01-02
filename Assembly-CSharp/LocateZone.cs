using System;
using UnityEngine;

public class LocateZone : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Bounds ColliderBounds { get; private set; }

	private void Start()
	{
		global::UnityEngine.Renderer[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.Renderer>();
		foreach (global::UnityEngine.Renderer renderer in componentsInChildren)
		{
			renderer.enabled = false;
		}
		global::UnityEngine.Collider componentInChildren = base.GetComponentInChildren<global::UnityEngine.Collider>();
		if (componentInChildren != null)
		{
			this.ColliderBounds = componentInChildren.bounds;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.RegisterLocateZone(this);
	}

	public uint guid;
}
