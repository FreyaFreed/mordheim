using System;
using UnityEngine;

public class CandleFX : global::ExternalUpdator
{
	public new void Awake()
	{
		base.Awake();
		this.lightSource = base.GetComponent<global::UnityEngine.Light>();
		this.intensity = this.lightSource.intensity;
		this.defaultShadows = this.lightSource.shadows;
	}

	public override void ExternalUpdate()
	{
		if (!global::PandoraSingleton<global::TransitionManager>.Instance.GameLoadingDone || global::UnityEngine.Camera.main == null)
		{
			return;
		}
		float num = global::UnityEngine.Vector3.SqrMagnitude(this.cachedTransform.position - global::UnityEngine.Camera.main.transform.position);
		if (num < 2500f)
		{
			this.lightSource.enabled = true;
			if (num < 625f)
			{
				this.lightSource.shadows = this.defaultShadows;
			}
			else
			{
				this.lightSource.shadows = global::UnityEngine.LightShadows.None;
			}
			if (this.lightSource.intensity > this.intensity)
			{
				this.lightSource.intensity -= global::UnityEngine.Time.deltaTime;
			}
			else
			{
				this.lightSource.intensity += global::UnityEngine.Time.deltaTime;
			}
			if (global::UnityEngine.Random.value < this.intensityOccurence / 60f)
			{
				if (this.lightSource.intensity > this.intensity)
				{
					this.lightSource.intensity = this.intensity - this.intensity * this.intensityChange * global::UnityEngine.Random.value;
				}
				else
				{
					this.lightSource.intensity = this.intensity + this.intensity * this.intensityChange * global::UnityEngine.Random.value;
				}
			}
			return;
		}
		this.lightSource.enabled = false;
	}

	private const float SHADOW_DIST = 625f;

	private const float LIGHT_DIST = 2500f;

	public float intensityOccurence = 3f;

	[global::UnityEngine.Range(0f, 1f)]
	public float intensityChange = 0.2f;

	public float rangeOccurence = 3f;

	[global::UnityEngine.Range(0f, 1f)]
	public float rangeChange = 0.2f;

	private global::UnityEngine.Light lightSource;

	private float intensity;

	private global::UnityEngine.LightShadows defaultShadows;
}
