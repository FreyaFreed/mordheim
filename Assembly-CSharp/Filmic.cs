using System;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class Filmic : global::UnityEngine.MonoBehaviour
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
			global::UnityEngine.Object.DestroyImmediate(this);
			return;
		}
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture sourceTexture, global::UnityEngine.RenderTexture destTexture)
	{
		if (this.curShader != null)
		{
			this.material.SetFloat("_A", this.ShoulderStrength);
			this.material.SetFloat("_B", this.LinearStrength);
			this.material.SetFloat("_C", this.LinearAngle);
			this.material.SetFloat("_D", this.ToeStrength);
			this.material.SetFloat("_E", this.ToeNumerator);
			this.material.SetFloat("_F", this.ToeDenominator);
			this.material.SetFloat("_W", this.Weight);
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		if (this.curMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.curMaterial);
		}
	}

	public global::UnityEngine.Shader curShader;

	public float ShoulderStrength = 0.15f;

	public float LinearStrength = 0.2f;

	public float LinearAngle = 0.1f;

	public float ToeStrength = 0.35f;

	public float ToeNumerator = 0.01f;

	public float ToeDenominator = 0.7f;

	public float Weight = 10.2f;

	private global::UnityEngine.Material curMaterial;
}
