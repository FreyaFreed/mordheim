using System;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class GpuRenderPlane : global::UnityEngine.MonoBehaviour
{
	private void Update()
	{
		if (this.field == null)
		{
			if (global::UnityEngine.Application.isPlaying && base.gameObject)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(base.gameObject);
			}
		}
		else if (this.field.RenderPlane != this)
		{
			if (global::UnityEngine.Application.isPlaying && base.gameObject)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(base.gameObject);
			}
		}
	}

	public global::FlowSimulationField field;
}
