using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraSetter : global::UnityEngine.MonoBehaviour
{
	public void SetCameraInfo(global::UnityEngine.Camera mainCamera)
	{
		if (mainCamera)
		{
			global::UnityEngine.RenderSettings.fog = this.needFog;
			if (this.needFog)
			{
				global::UnityEngine.RenderSettings.fogColor = this.fogColor;
				global::UnityEngine.RenderSettings.fogMode = global::UnityEngine.FogMode.Exponential;
				global::UnityEngine.RenderSettings.fogDensity = global::UnityEngine.Mathf.Max(0.001f, this.fogDensity);
			}
			global::UnityStandardAssets.ImageEffects.GlobalFog component = mainCamera.GetComponent<global::UnityStandardAssets.ImageEffects.GlobalFog>();
			if (component != null)
			{
				component.enabled = this.needFog;
				component.startDistance = this.startDistance;
				component.heightFog = true;
				float num = 0f;
				if (global::PandoraSingleton<global::MissionManager>.Exists() && global::PandoraSingleton<global::MissionManager>.Instance.mapOrigin != null)
				{
					num = global::PandoraSingleton<global::MissionManager>.Instance.mapOrigin.transform.position.y;
				}
				component.height = this.fogHeight + num;
				component.heightDensity = this.fogHeightDensity;
			}
			if (this.amplifyLutTexture)
			{
				global::AmplifyColorEffect component2 = mainCamera.GetComponent<global::AmplifyColorEffect>();
				if (component2)
				{
					component2.LutTexture = this.amplifyLutTexture;
					if (this.useLUTVolumes)
					{
						component2.UseVolumes = this.useLUTVolumes;
					}
				}
			}
		}
	}

	public bool needFog;

	public global::UnityEngine.Color fogColor;

	public float fogDensity = 0.001f;

	public float startDistance;

	public float fogHeight;

	public float fogHeightDensity;

	public global::UnityEngine.Texture2D amplifyLutTexture;

	public bool useLUTVolumes;
}
