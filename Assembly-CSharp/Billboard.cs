using System;
using UnityEngine;

public class Billboard : global::ExternalUpdator
{
	public override void ExternalUpdate()
	{
		if (global::UnityEngine.Camera.main == null)
		{
			return;
		}
		global::UnityEngine.Vector3 position = global::UnityEngine.Camera.main.transform.position;
		position.y = this.cachedTransform.position.y;
		this.cachedTransform.LookAt(position, global::UnityEngine.Vector3.up);
	}
}
