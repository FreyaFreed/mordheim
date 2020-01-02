using System;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Image Effects/SSAO Pro")]
[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Camera))]
[global::UnityEngine.HelpURL("http://www.thomashourdel.com/ssaopro/doc/")]
public class SSAOPro : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Material Material
	{
		get
		{
			if (this.m_Material_v2 == null)
			{
				this.m_Material_v2 = new global::UnityEngine.Material(this.ShaderSSAO);
				this.m_Material_v2.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			return this.m_Material_v2;
		}
	}

	public global::UnityEngine.Shader ShaderSSAO
	{
		get
		{
			if (this.m_ShaderSSAO_v2 == null)
			{
				this.m_ShaderSSAO_v2 = global::UnityEngine.Shader.Find("Hidden/SSAO Pro V2");
			}
			return this.m_ShaderSSAO_v2;
		}
	}

	private void Start()
	{
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			global::UnityEngine.Debug.LogWarning("Image Effects are not supported on this platform.");
			base.enabled = false;
			return;
		}
		if (!global::UnityEngine.SystemInfo.supportsRenderTextures)
		{
			global::UnityEngine.Debug.LogWarning("RenderTextures are not supported on this platform.");
			base.enabled = false;
			return;
		}
		if (this.ShaderSSAO != null && !this.ShaderSSAO.isSupported)
		{
			global::UnityEngine.Debug.LogWarning("Unsupported shader (SSAO).");
			base.enabled = false;
			return;
		}
	}

	private void OnEnable()
	{
		this.m_Camera = base.GetComponent<global::UnityEngine.Camera>();
	}

	private void OnDestroy()
	{
		if (this.m_Material_v2 != null)
		{
			global::UnityEngine.Object.DestroyImmediate(this.m_Material_v2);
		}
		if (this.m_RWSCamera != null)
		{
			global::UnityEngine.Object.DestroyImmediate(this.m_RWSCamera.gameObject);
		}
	}

	[global::UnityEngine.ImageEffectOpaque]
	private void OnRenderImage(global::UnityEngine.RenderTexture source, global::UnityEngine.RenderTexture destination)
	{
		if (this.ShaderSSAO == null)
		{
			global::UnityEngine.Graphics.Blit(source, destination);
			return;
		}
		int pass = this.SetShaderStates();
		this.Material.SetMatrix("_InverseViewProject", (this.m_Camera.projectionMatrix * this.m_Camera.worldToCameraMatrix).inverse);
		this.Material.SetMatrix("_CameraModelView", this.m_Camera.cameraToWorldMatrix);
		this.Material.SetTexture("_NoiseTex", this.NoiseTexture);
		this.Material.SetVector("_Params1", new global::UnityEngine.Vector4((!(this.NoiseTexture == null)) ? ((float)this.NoiseTexture.width) : 0f, this.Radius, this.Intensity, this.Distance));
		this.Material.SetVector("_Params2", new global::UnityEngine.Vector4(this.Bias, this.LumContribution, this.CutoffDistance, this.CutoffFalloff));
		this.Material.SetColor("_OcclusionColor", this.OcclusionColor);
		if (this.Blur == global::SSAOPro.BlurMode.None)
		{
			global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(source.width / this.Downsampling, source.height / this.Downsampling, 0, global::UnityEngine.RenderTextureFormat.ARGB32);
			global::UnityEngine.Graphics.Blit(temporary, temporary, this.Material, 0);
			if (this.DebugAO)
			{
				global::UnityEngine.Graphics.Blit(source, temporary, this.Material, pass);
				global::UnityEngine.Graphics.Blit(temporary, destination);
				global::UnityEngine.RenderTexture.ReleaseTemporary(temporary);
				return;
			}
			global::UnityEngine.Graphics.Blit(source, temporary, this.Material, pass);
			this.Material.SetTexture("_SSAOTex", temporary);
			global::UnityEngine.Graphics.Blit(source, destination, this.Material, 8);
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary);
		}
		else
		{
			int pass2 = 5;
			if (this.Blur == global::SSAOPro.BlurMode.Bilateral)
			{
				pass2 = 6;
			}
			else if (this.Blur == global::SSAOPro.BlurMode.HighQualityBilateral)
			{
				pass2 = 7;
			}
			int num = (!this.BlurDownsampling) ? 1 : this.Downsampling;
			global::UnityEngine.RenderTexture temporary2 = global::UnityEngine.RenderTexture.GetTemporary(source.width / num, source.height / num, 0, global::UnityEngine.RenderTextureFormat.ARGB32);
			global::UnityEngine.RenderTexture temporary3 = global::UnityEngine.RenderTexture.GetTemporary(source.width / this.Downsampling, source.height / this.Downsampling, 0, global::UnityEngine.RenderTextureFormat.ARGB32);
			global::UnityEngine.Graphics.Blit(temporary2, temporary2, this.Material, 0);
			global::UnityEngine.Graphics.Blit(source, temporary2, this.Material, pass);
			if (this.Blur == global::SSAOPro.BlurMode.HighQualityBilateral)
			{
				this.Material.SetFloat("_BilateralThreshold", this.BlurBilateralThreshold / 10000f);
			}
			for (int i = 0; i < this.BlurPasses; i++)
			{
				this.Material.SetVector("_Direction", new global::UnityEngine.Vector2(1f / (float)source.width, 0f));
				global::UnityEngine.Graphics.Blit(temporary2, temporary3, this.Material, pass2);
				this.Material.SetVector("_Direction", new global::UnityEngine.Vector2(0f, 1f / (float)source.height));
				global::UnityEngine.Graphics.Blit(temporary3, temporary2, this.Material, pass2);
			}
			if (!this.DebugAO)
			{
				this.Material.SetTexture("_SSAOTex", temporary2);
				global::UnityEngine.Graphics.Blit(source, destination, this.Material, 8);
			}
			else
			{
				global::UnityEngine.Graphics.Blit(temporary2, destination);
			}
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary2);
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary3);
		}
	}

	private int SetShaderStates()
	{
		this.m_Camera.depthTextureMode |= global::UnityEngine.DepthTextureMode.Depth;
		this.m_Camera.depthTextureMode |= global::UnityEngine.DepthTextureMode.DepthNormals;
		this.keywords[0] = ((this.Samples != global::SSAOPro.SampleCount.Low) ? ((this.Samples != global::SSAOPro.SampleCount.Medium) ? ((this.Samples != global::SSAOPro.SampleCount.High) ? ((this.Samples != global::SSAOPro.SampleCount.Ultra) ? "SAMPLES_VERY_LOW" : "SAMPLES_ULTRA") : "SAMPLES_HIGH") : "SAMPLES_MEDIUM") : "SAMPLES_LOW");
		this.keywords[1] = "HIGH_PRECISION_DEPTHMAP_OFF";
		this.Material.shaderKeywords = this.keywords;
		int num = 0;
		if (this.NoiseTexture != null)
		{
			num = 1;
		}
		if (this.LumContribution >= 0.001f)
		{
			num += 2;
		}
		return 1 + num;
	}

	public global::UnityEngine.Texture2D NoiseTexture;

	public bool UseHighPrecisionDepthMap;

	public global::SSAOPro.SampleCount Samples = global::SSAOPro.SampleCount.Medium;

	[global::UnityEngine.Range(1f, 4f)]
	public int Downsampling = 1;

	[global::UnityEngine.Range(0.01f, 1.25f)]
	public float Radius = 0.125f;

	[global::UnityEngine.Range(0f, 16f)]
	public float Intensity = 2f;

	[global::UnityEngine.Range(0f, 10f)]
	public float Distance = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bias = 0.1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float LumContribution = 0.5f;

	[global::UnityEngine.ColorUsage(false)]
	public global::UnityEngine.Color OcclusionColor = global::UnityEngine.Color.black;

	public float CutoffDistance = 150f;

	public float CutoffFalloff = 50f;

	public global::SSAOPro.BlurMode Blur;

	public bool BlurDownsampling;

	[global::UnityEngine.Range(1f, 4f)]
	public int BlurPasses = 1;

	[global::UnityEngine.Range(0.05f, 1f)]
	public float BlurBilateralThreshold = 0.1f;

	public bool DebugAO;

	protected global::UnityEngine.Shader m_ShaderSSAO_v2;

	protected global::UnityEngine.Shader m_ShaderHighPrecisionDepth;

	protected global::UnityEngine.Material m_Material_v2;

	protected global::UnityEngine.Camera m_Camera;

	protected global::UnityEngine.Camera m_RWSCamera;

	protected global::UnityEngine.RenderTextureFormat m_RTFormat = global::UnityEngine.RenderTextureFormat.RFloat;

	private string[] keywords = new string[2];

	public enum BlurMode
	{
		None,
		Gaussian,
		Bilateral,
		HighQualityBilateral
	}

	public enum SampleCount
	{
		VeryLow,
		Low,
		Medium,
		High,
		Ultra
	}
}
