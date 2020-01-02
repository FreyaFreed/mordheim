using System;
using UnityEngine;

public class HangingNode : global::UnityEngine.MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (this.isPlank)
		{
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.magenta;
			global::PandoraUtils.DrawFacingGizmoCube(base.transform, 1f, 0.5f, 1f, 0f, 0f, true);
		}
	}

	public bool isPlank = true;

	public string blockingProp;

	public global::UnityEngine.Vector3 positionOffset;
}
