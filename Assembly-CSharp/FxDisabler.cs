using System;
using UnityEngine;

public class FxDisabler : global::ExternalUpdator
{
	public new void Awake()
	{
		base.Awake();
		this.particles = base.GetComponentsInChildren<global::UnityEngine.ParticleSystem>();
		this.particlesRenderer = new global::UnityEngine.ParticleSystemRenderer[this.particles.Length];
		for (int i = 0; i < this.particles.Length; i++)
		{
			this.particlesRenderer[i] = this.particles[i].GetComponent<global::UnityEngine.ParticleSystemRenderer>();
		}
		this.isOn = true;
		this.isActiveOn = true;
		this.sqrDist = this.distance * this.distance;
	}

	public override void ExternalUpdate()
	{
		if (global::UnityEngine.Camera.main != null)
		{
			float num = global::UnityEngine.Vector3.SqrMagnitude(this.cachedTransform.position - global::UnityEngine.Camera.main.transform.position);
			if (num < this.sqrDist)
			{
				if (!this.isOn)
				{
					this.isOn = true;
					base.gameObject.SetActive(true);
					for (int i = 0; i < this.particles.Length; i++)
					{
						this.particles[i].emission.enabled = true;
						this.particles[i].Play();
						if (this.particlesRenderer[i] != null)
						{
							this.particlesRenderer[i].enabled = true;
						}
					}
				}
			}
			else
			{
				if (this.isOn)
				{
					this.isOn = false;
					for (int j = 0; j < this.particles.Length; j++)
					{
						this.particles[j].emission.enabled = false;
						if (this.particlesRenderer[j] != null)
						{
							this.particlesRenderer[j].enabled = false;
						}
					}
				}
				for (int k = 0; k < this.particles.Length; k++)
				{
					if (base.gameObject.activeSelf && this.particles[k].particleCount == 0)
					{
						base.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	public float distance = 25f;

	private global::UnityEngine.ParticleSystem[] particles;

	private global::UnityEngine.ParticleSystemRenderer[] particlesRenderer;

	private bool isOn;

	private bool isActiveOn;

	private float sqrDist;
}
