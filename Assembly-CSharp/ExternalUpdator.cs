using System;
using UnityEngine;

public abstract class ExternalUpdator : global::UnityEngine.MonoBehaviour
{
	public void Awake()
	{
		if (global::PandoraSingleton<global::MissionManager>.Exists())
		{
			global::PandoraSingleton<global::MissionManager>.Instance.RegisterExternalUpdator(this);
		}
		base.enabled = false;
		this.cachedTransform = base.transform;
	}

	private void OnDestroy()
	{
		if (global::PandoraSingleton<global::MissionManager>.Exists())
		{
			global::PandoraSingleton<global::MissionManager>.Instance.ReleaseExternalUpdator(this);
		}
	}

	public abstract void ExternalUpdate();

	protected global::UnityEngine.Transform cachedTransform;
}
