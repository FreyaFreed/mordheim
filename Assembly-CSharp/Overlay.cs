using System;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Image Effects/Other/Overlay")]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Camera))]
[global::UnityEngine.ExecuteInEditMode]
public class Overlay : global::UnityEngine.MonoBehaviour
{
	private global::UnityEngine.Material overlayMaterial
	{
		get
		{
			if (this.mat == null)
			{
				this.mat = new global::UnityEngine.Material(this.overlayShader);
				this.mat.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			return this.mat;
		}
	}

	private void Start()
	{
		this.currentIntensity = 0f;
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			global::UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		global::UnityEngine.Vector4 vector = new global::UnityEngine.Vector4(1f, 0f, 0f, 1f);
		this.overlayMaterial.SetVector("_UV_Transform", vector);
		this.overlayMaterial.SetTexture("_Overlay", this.texture);
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture source, global::UnityEngine.RenderTexture destination)
	{
		this.overlayMaterial.SetFloat("_Intensity", this.currentIntensity);
		global::UnityEngine.Graphics.Blit(source, destination, this.overlayMaterial, (int)this.blendMode);
	}

	public global::Overlay.OverlayBlendMode blendMode = global::Overlay.OverlayBlendMode.Overlay;

	public float intensity = 1f;

	public float intensitySpeed = 1f;

	public global::UnityEngine.Texture2D texture;

	public global::UnityEngine.Shader overlayShader;

	private float currentIntensity;

	private global::UnityEngine.Material mat;

	public enum OverlayBlendMode
	{
		Additive,
		ScreenBlend,
		Multiply,
		Overlay,
		AlphaBlend
	}
}
