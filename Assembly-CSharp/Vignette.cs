using System;
using System.Collections;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class Vignette : global::UnityEngine.MonoBehaviour
{
	private global::UnityEngine.Material material
	{
		get
		{
			if (this.curMaterial == null)
			{
				this.curMaterial = new global::UnityEngine.Material(this.curShader);
				this.curMaterial.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			return this.curMaterial;
		}
	}

	private void Start()
	{
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture sourceTexture, global::UnityEngine.RenderTexture destTexture)
	{
		if (this.curShader != null)
		{
			this.material.SetFloat("_VignettePower", this.power);
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnDisable()
	{
		if (this.curMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.curMaterial);
		}
	}

	public void Activate(bool activate)
	{
		if (activate)
		{
			base.StartCoroutine(this.TurnOn());
		}
		else
		{
			base.StartCoroutine(this.TurnOff());
		}
	}

	private global::System.Collections.IEnumerator TurnOn()
	{
		this.power = 0f;
		base.enabled = true;
		while (this.power < this.VignettePower)
		{
			this.power += this.VignetteSpeed;
			this.power = ((this.power <= this.VignettePower) ? this.power : this.VignettePower);
			yield return 0;
		}
		yield break;
	}

	private global::System.Collections.IEnumerator TurnOff()
	{
		while (this.power > 0f)
		{
			this.power -= this.VignetteSpeed;
			this.power = ((this.power >= 0f) ? this.power : (this.power = 0f));
			yield return 0;
		}
		base.enabled = false;
		yield break;
	}

	public global::UnityEngine.Shader curShader;

	public float VignettePower = 5f;

	public float VignetteSpeed = 1f;

	private global::UnityEngine.Material curMaterial;

	private float power;
}
