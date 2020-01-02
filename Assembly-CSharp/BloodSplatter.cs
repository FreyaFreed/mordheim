using System;
using System.Collections.Generic;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Camera))]
[global::UnityEngine.ExecuteInEditMode]
public class BloodSplatter : global::UnityEngine.MonoBehaviour
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
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			global::UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		this.maxCount = this.frameBySplatter * this.frameByImage;
		global::UnityEngine.Vector4 vector = new global::UnityEngine.Vector4(1f, 0f, 0f, 1f);
		this.overlayMaterial.SetVector("_UV_Transform", vector);
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture source, global::UnityEngine.RenderTexture destination)
	{
		if (this.currentCount < this.maxCount)
		{
			this.overlayMaterial.SetFloat("_Intensity", this.intensity);
			int index = (int)((float)this.currentCount / (float)this.frameByImage) + this.offset * this.frameBySplatter;
			this.overlayMaterial.SetTexture("_Overlay", this.splatters[index]);
			global::UnityEngine.Graphics.Blit(source, destination, this.overlayMaterial, (int)this.blendMode);
			this.currentCount++;
			this.currentCount = ((this.currentCount >= this.maxCount) ? (this.maxCount - 1) : this.currentCount);
		}
	}

	public void Activate()
	{
		this.offset = 0;
		this.currentCount = 0;
		base.enabled = true;
	}

	public void Deactivate()
	{
		base.enabled = false;
	}

	public float intensity = 1f;

	public global::BloodSplatter.OverlayBlendMode blendMode = global::BloodSplatter.OverlayBlendMode.Overlay;

	public global::UnityEngine.Shader overlayShader;

	public int frameByImage;

	public int frameBySplatter;

	public global::System.Collections.Generic.List<global::UnityEngine.Texture2D> splatters;

	private global::UnityEngine.Material mat;

	private int currentCount;

	private int maxCount;

	private int offset;

	public enum OverlayBlendMode
	{
		Additive,
		ScreenBlend,
		Multiply,
		Overlay,
		AlphaBlend
	}
}
