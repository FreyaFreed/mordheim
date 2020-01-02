using System;
using UnityEngine;

public class LookAt : global::ExternalUpdator
{
	public new void Awake()
	{
		base.Awake();
	}

	public override void ExternalUpdate()
	{
		global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
		if (currentUnit != null && global::UnityEngine.Vector3.SqrMagnitude(this.cachedTransform.position - currentUnit.transform.position) < 100f)
		{
			this.cachedTransform.LookAt(currentUnit.transform.position + global::UnityEngine.Vector3.up);
		}
	}

	private const float LOOK_DIST = 100f;

	public bool yOnly;
}
