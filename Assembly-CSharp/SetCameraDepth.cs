using System;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class SetCameraDepth : global::UnityEngine.MonoBehaviour
{
	private void Update()
	{
		base.GetComponent<global::UnityEngine.Camera>().depthTextureMode = this.depthMode;
	}

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.DepthTextureMode depthMode;
}
