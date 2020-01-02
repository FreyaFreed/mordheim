using System;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Heathen/Image Effects/Selective Glow")]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Camera))]
public class SelectiveGlow : global::UnityEngine.MonoBehaviour
{
	private global::UnityEngine.Material GetMaterial()
	{
		if (this.m_Material == null || (global::UnityEngine.Application.isEditor && !global::UnityEngine.Application.isPlaying))
		{
			if (this.UseFastBlur)
			{
				this.m_Material = new global::UnityEngine.Material(this.fastBlurShader);
			}
			else
			{
				this.m_Material = new global::UnityEngine.Material(this.blurShader);
			}
			this.m_Material.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
		}
		return this.m_Material;
	}

	private global::UnityEngine.Material GetCompositeMaterial()
	{
		if (this.m_CompositeMaterial == null)
		{
			this.m_CompositeMaterial = new global::UnityEngine.Material(this.compositeShader);
			this.m_CompositeMaterial.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
		}
		return this.m_CompositeMaterial;
	}

	private void OnDisable()
	{
		if (this.m_Material)
		{
			global::UnityEngine.Object.DestroyImmediate(this.m_Material);
		}
		global::UnityEngine.Object.DestroyImmediate(this.m_CompositeMaterial);
		global::UnityEngine.Object.DestroyImmediate(this.shaderCamera);
		if (this.renderTexture != null)
		{
			global::UnityEngine.RenderTexture.ReleaseTemporary(this.renderTexture);
			this.renderTexture = null;
		}
	}

	private void Start()
	{
		if (this.compositeShader == null)
		{
			base.enabled = false;
			global::UnityEngine.Debug.LogWarning("Composite Shader is not assigned");
			return;
		}
		if (this.renderGlowShader == null)
		{
			base.enabled = false;
			global::UnityEngine.Debug.LogWarning("Render Glow Shader is not assigned");
			return;
		}
		if (this.blurShader == null)
		{
			base.enabled = false;
			global::UnityEngine.Debug.LogWarning("Blur Shader is not assigned");
			return;
		}
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			global::UnityEngine.Debug.LogWarning("Image Effects are not supported");
			return;
		}
		if (!this.GetMaterial().shader.isSupported)
		{
			base.enabled = false;
			global::UnityEngine.Debug.LogWarning("Blur shader can't run on the users graphics card");
			return;
		}
	}

	private void OnPreRender()
	{
		if (!base.enabled || !base.gameObject.activeSelf)
		{
			return;
		}
		if (this.renderTexture != null)
		{
			global::UnityEngine.RenderTexture.ReleaseTemporary(this.renderTexture);
			this.renderTexture = null;
		}
		global::UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Camera rect = ",
			base.GetComponent<global::UnityEngine.Camera>().pixelWidth.ToString(),
			",",
			base.GetComponent<global::UnityEngine.Camera>().pixelHeight,
			" screen = ",
			global::UnityEngine.Screen.width.ToString(),
			",",
			global::UnityEngine.Screen.height.ToString()
		}));
		this.renderTexture = global::UnityEngine.RenderTexture.GetTemporary((int)((float)global::UnityEngine.Screen.width * (1f / base.GetComponent<global::UnityEngine.Camera>().rect.height)), (int)((float)global::UnityEngine.Screen.height * (1f / base.GetComponent<global::UnityEngine.Camera>().rect.width)), 16);
		if (!this.shaderCamera)
		{
			this.shaderCamera = new global::UnityEngine.GameObject("ShaderCamera", new global::System.Type[]
			{
				typeof(global::UnityEngine.Camera)
			});
			this.shaderCamera.GetComponent<global::UnityEngine.Camera>().enabled = false;
			this.shaderCamera.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.shaderCamera.GetComponent<global::UnityEngine.Camera>().rect = base.GetComponent<global::UnityEngine.Camera>().rect;
		}
		global::UnityEngine.Camera component = this.shaderCamera.GetComponent<global::UnityEngine.Camera>();
		component.CopyFrom(base.GetComponent<global::UnityEngine.Camera>());
		component.rect = new global::UnityEngine.Rect(0f, 0f, 1f, 1f);
		component.backgroundColor = new global::UnityEngine.Color(0f, 0f, 0f, 0f);
		component.clearFlags = global::UnityEngine.CameraClearFlags.Color;
		component.targetTexture = this.renderTexture;
		component.RenderWithShader(this.renderGlowShader, "RenderType");
	}

	private void FourTapCone(global::UnityEngine.RenderTexture source, global::UnityEngine.RenderTexture dest, int iteration)
	{
		global::UnityEngine.RenderTexture.active = dest;
		source.SetGlobalShaderProperty("__RenderTex");
		global::UnityEngine.Material material = this.GetMaterial();
		if (this.UseFastBlur)
		{
			float num = 1f / (1f * (float)(1 << this.SampleDivision));
			float num2 = (float)iteration * 1f;
			material.SetVector("_Parameter", new global::UnityEngine.Vector4(this.blurSpread * num + num2, -this.blurSpread * num - num2, 0f, 0f));
			global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(dest.width, dest.height, 0, dest.format);
			temporary.filterMode = global::UnityEngine.FilterMode.Bilinear;
			dest.filterMode = global::UnityEngine.FilterMode.Bilinear;
			global::UnityEngine.Graphics.Blit(source, temporary, material, 1);
			global::UnityEngine.Graphics.Blit(temporary, dest, material, 2);
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary);
		}
		else
		{
			float num3 = 0.5f + (float)iteration * this.blurSpread;
			global::UnityEngine.Graphics.BlitMultiTap(source, dest, material, new global::UnityEngine.Vector2[]
			{
				new global::UnityEngine.Vector2(num3, num3),
				new global::UnityEngine.Vector2(-num3, num3),
				new global::UnityEngine.Vector2(num3, -num3),
				new global::UnityEngine.Vector2(-num3, -num3)
			});
		}
	}

	private void DownSample4x(global::UnityEngine.RenderTexture source, global::UnityEngine.RenderTexture dest)
	{
		global::UnityEngine.RenderTexture.active = dest;
		source.SetGlobalShaderProperty("__RenderTex");
		global::UnityEngine.Material material = this.GetMaterial();
		float num = 1f;
		if (this.UseFastBlur)
		{
			float num2 = 1f / (1f * (float)(1 << this.SampleDivision));
			material.SetVector("_Parameter", new global::UnityEngine.Vector4(this.blurSpread * num2, -this.blurSpread * num2, 0f, 0f));
			global::UnityEngine.Graphics.Blit(source, dest, material, 0);
		}
		else
		{
			global::UnityEngine.Graphics.BlitMultiTap(source, dest, material, new global::UnityEngine.Vector2[]
			{
				new global::UnityEngine.Vector2(num, num),
				new global::UnityEngine.Vector2(-num, num),
				new global::UnityEngine.Vector2(num, -num),
				new global::UnityEngine.Vector2(-num, -num)
			});
		}
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture source, global::UnityEngine.RenderTexture destination)
	{
		if (!base.enabled || !base.gameObject.activeSelf)
		{
			return;
		}
		int width = source.width / this.SampleDivision;
		int height = source.height / this.SampleDivision;
		global::UnityEngine.RenderTexture renderTexture = global::UnityEngine.RenderTexture.GetTemporary(width, height, 0);
		this.DownSample4x(this.renderTexture, renderTexture);
		for (int i = 0; i < this.iterations; i++)
		{
			global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(width, height, 0);
			this.FourTapCone(renderTexture, temporary, i);
			global::UnityEngine.RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		global::UnityEngine.Material compositeMaterial = this.GetCompositeMaterial();
		compositeMaterial.SetTexture("_BlurTex", renderTexture);
		compositeMaterial.SetTexture("_BlurRamp", this.renderTexture);
		compositeMaterial.SetFloat("_Outter", this.Intensity);
		global::UnityEngine.Graphics.Blit(source, destination, compositeMaterial);
		global::UnityEngine.RenderTexture.ReleaseTemporary(renderTexture);
		if (this.renderTexture != null)
		{
			global::UnityEngine.RenderTexture.ReleaseTemporary(this.renderTexture);
			this.renderTexture = null;
		}
	}

	public bool UseFastBlur;

	[global::UnityEngine.Range(1f, 10f)]
	public int SampleDivision = 4;

	[global::UnityEngine.Range(1f, 20f)]
	public int iterations = 5;

	[global::UnityEngine.Range(0f, 1f)]
	public float blurSpread = 0.6f;

	[global::UnityEngine.Range(0f, 25f)]
	public float Intensity = 4f;

	public global::UnityEngine.Shader compositeShader;

	public global::UnityEngine.Shader renderGlowShader;

	public global::UnityEngine.Shader blurShader;

	public global::UnityEngine.Shader fastBlurShader;

	private global::UnityEngine.Material m_Material;

	private global::UnityEngine.Material m_CompositeMaterial;

	private global::UnityEngine.RenderTexture renderTexture;

	private global::UnityEngine.GameObject shaderCamera;
}
