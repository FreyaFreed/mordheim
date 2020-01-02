using System;
using UnityEngine;

public class FxTest : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<global::UnityEngine.Camera>().depthTextureMode = global::UnityEngine.DepthTextureMode.Depth;
	}

	private void Update()
	{
		if (global::UnityEngine.Application.targetFrameRate != 60)
		{
			global::UnityEngine.Application.targetFrameRate = 60;
		}
	}
}
