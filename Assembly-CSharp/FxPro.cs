using System;
using FxProNS;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Image Effects/FxPro™")]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Camera))]
[global::UnityEngine.ExecuteInEditMode]
public class FxPro : global::UnityEngine.MonoBehaviour
{
	public static global::UnityEngine.Material Mat
	{
		get
		{
			if (null == global::FxPro._mat)
			{
				global::FxPro._mat = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/FxPro"))
				{
					hideFlags = global::UnityEngine.HideFlags.HideAndDontSave
				};
			}
			return global::FxPro._mat;
		}
	}

	private static global::UnityEngine.Material TapMat
	{
		get
		{
			if (null == global::FxPro._tapMat)
			{
				global::FxPro._tapMat = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/FxProTap"))
				{
					hideFlags = global::UnityEngine.HideFlags.HideAndDontSave
				};
			}
			return global::FxPro._tapMat;
		}
	}

	public void Start()
	{
		if (!global::UnityEngine.SystemInfo.supportsImageEffects || !global::UnityEngine.SystemInfo.supportsRenderTextures)
		{
			global::UnityEngine.Debug.LogError("Image effects are not supported on this platform.");
			base.enabled = false;
			return;
		}
	}

	public void Init(bool searchForNonDepthmapAlphaObjects)
	{
		global::FxPro.Mat.SetFloat("_DirtIntensity", global::UnityEngine.Mathf.Exp(this.LensDirtIntensity) - 1f);
		if (null == this.LensDirtTexture || this.LensDirtIntensity <= 0f)
		{
			global::FxPro.Mat.DisableKeyword("LENS_DIRT_ON");
			global::FxPro.Mat.EnableKeyword("LENS_DIRT_OFF");
		}
		else
		{
			global::FxPro.Mat.SetTexture("_LensDirtTex", this.LensDirtTexture);
			global::FxPro.Mat.EnableKeyword("LENS_DIRT_ON");
			global::FxPro.Mat.DisableKeyword("LENS_DIRT_OFF");
		}
		if (this.ChromaticAberration)
		{
			global::FxPro.Mat.EnableKeyword("CHROMATIC_ABERRATION_ON");
			global::FxPro.Mat.DisableKeyword("CHROMATIC_ABERRATION_OFF");
		}
		else
		{
			global::FxPro.Mat.EnableKeyword("CHROMATIC_ABERRATION_OFF");
			global::FxPro.Mat.DisableKeyword("CHROMATIC_ABERRATION_ON");
		}
		if (base.GetComponent<global::UnityEngine.Camera>().hdr)
		{
			global::UnityEngine.Shader.EnableKeyword("FXPRO_HDR_ON");
			global::UnityEngine.Shader.DisableKeyword("FXPRO_HDR_OFF");
		}
		else
		{
			global::UnityEngine.Shader.EnableKeyword("FXPRO_HDR_OFF");
			global::UnityEngine.Shader.DisableKeyword("FXPRO_HDR_ON");
		}
		global::FxPro.Mat.SetFloat("_SCurveIntensity", this.SCurveIntensity);
		if (this.DOFEnabled)
		{
			if (null == this.DOFParams.EffectCamera)
			{
				this.DOFParams.EffectCamera = base.GetComponent<global::UnityEngine.Camera>();
			}
			this.DOFParams.DepthCompression = global::UnityEngine.Mathf.Clamp(this.DOFParams.DepthCompression, 2f, 8f);
			global::FxProNS.Singleton<global::FxProNS.DOFHelper>.Instance.SetParams(this.DOFParams);
			global::FxProNS.Singleton<global::FxProNS.DOFHelper>.Instance.Init(searchForNonDepthmapAlphaObjects);
			global::FxPro.Mat.DisableKeyword("DOF_DISABLED");
			global::FxPro.Mat.EnableKeyword("DOF_ENABLED");
			if (!this.DOFParams.DoubleIntensityBlur)
			{
				global::FxProNS.Singleton<global::FxProNS.DOFHelper>.Instance.SetBlurRadius((this.Quality != global::FxProNS.EffectsQuality.Fastest && this.Quality != global::FxProNS.EffectsQuality.Fast) ? 5 : 3);
			}
			else
			{
				global::FxProNS.Singleton<global::FxProNS.DOFHelper>.Instance.SetBlurRadius((this.Quality != global::FxProNS.EffectsQuality.Fastest && this.Quality != global::FxProNS.EffectsQuality.Fast) ? 10 : 5);
			}
		}
		else
		{
			global::FxPro.Mat.EnableKeyword("DOF_DISABLED");
			global::FxPro.Mat.DisableKeyword("DOF_ENABLED");
		}
		if (this.BloomEnabled)
		{
			this.BloomParams.Quality = this.Quality;
			global::FxProNS.Singleton<global::FxProNS.BloomHelper>.Instance.SetParams(this.BloomParams);
			global::FxProNS.Singleton<global::FxProNS.BloomHelper>.Instance.Init();
			global::FxPro.Mat.DisableKeyword("BLOOM_DISABLED");
			global::FxPro.Mat.EnableKeyword("BLOOM_ENABLED");
		}
		else
		{
			global::FxPro.Mat.EnableKeyword("BLOOM_DISABLED");
			global::FxPro.Mat.DisableKeyword("BLOOM_ENABLED");
		}
		if (this.LensCurvatureEnabled)
		{
			this.UpdateLensCurvatureZoom();
			global::FxPro.Mat.SetFloat("_LensCurvatureBarrelPower", this.LensCurvaturePower);
		}
		if (this.FilmGrainIntensity >= 0.001f)
		{
			global::FxPro.Mat.SetFloat("_FilmGrainIntensity", this.FilmGrainIntensity);
			global::FxPro.Mat.SetFloat("_FilmGrainTiling", this.FilmGrainTiling);
			global::FxPro.Mat.EnableKeyword("FILM_GRAIN_ON");
			global::FxPro.Mat.DisableKeyword("FILM_GRAIN_OFF");
		}
		else
		{
			global::FxPro.Mat.EnableKeyword("FILM_GRAIN_OFF");
			global::FxPro.Mat.DisableKeyword("FILM_GRAIN_ON");
		}
		if (this.VignettingIntensity <= 1f)
		{
			global::FxPro.Mat.SetFloat("_VignettingIntensity", this.VignettingIntensity);
			global::FxPro.Mat.EnableKeyword("VIGNETTING_ON");
			global::FxPro.Mat.DisableKeyword("VIGNETTING_OFF");
		}
		else
		{
			global::FxPro.Mat.EnableKeyword("VIGNETTING_OFF");
			global::FxPro.Mat.DisableKeyword("VIGNETTING_ON");
		}
		global::FxPro.Mat.SetFloat("_ChromaticAberrationOffset", this.ChromaticAberrationOffset);
		if (this.ColorEffectsEnabled)
		{
			global::FxPro.Mat.EnableKeyword("COLOR_FX_ON");
			global::FxPro.Mat.DisableKeyword("COLOR_FX_OFF");
			global::FxPro.Mat.SetColor("_CloseTint", this.CloseTint);
			global::FxPro.Mat.SetColor("_FarTint", this.FarTint);
			global::FxPro.Mat.SetFloat("_CloseTintStrength", this.CloseTintStrength);
			global::FxPro.Mat.SetFloat("_FarTintStrength", this.FarTintStrength);
			global::FxPro.Mat.SetFloat("_DesaturateDarksStrength", this.DesaturateDarksStrength);
			global::FxPro.Mat.SetFloat("_DesaturateFarObjsStrength", this.DesaturateFarObjsStrength);
			global::FxPro.Mat.SetColor("_FogTint", this.FogTint);
			global::FxPro.Mat.SetFloat("_FogStrength", this.FogStrength);
		}
		else
		{
			global::FxPro.Mat.EnableKeyword("COLOR_FX_OFF");
			global::FxPro.Mat.DisableKeyword("COLOR_FX_ON");
		}
	}

	public void OnEnable()
	{
		this.Init(true);
	}

	public void OnDisable()
	{
		if (null != global::FxPro.Mat)
		{
			global::UnityEngine.Object.DestroyImmediate(global::FxPro.Mat);
		}
		global::FxProNS.RenderTextureManager.Instance.Dispose();
		global::FxProNS.Singleton<global::FxProNS.DOFHelper>.Instance.Dispose();
		global::FxProNS.Singleton<global::FxProNS.BloomHelper>.Instance.Dispose();
	}

	public void OnValidate()
	{
		this.Init(false);
	}

	public static global::UnityEngine.RenderTexture DownsampleTex(global::UnityEngine.RenderTexture input, float downsampleBy)
	{
		global::UnityEngine.RenderTexture renderTexture = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(global::UnityEngine.Mathf.RoundToInt((float)input.width / downsampleBy), global::UnityEngine.Mathf.RoundToInt((float)input.height / downsampleBy), input.depth, input.format);
		renderTexture.filterMode = global::UnityEngine.FilterMode.Bilinear;
		global::UnityEngine.Graphics.BlitMultiTap(input, renderTexture, global::FxPro.TapMat, new global::UnityEngine.Vector2[]
		{
			new global::UnityEngine.Vector2(-1f, -1f),
			new global::UnityEngine.Vector2(-1f, 1f),
			new global::UnityEngine.Vector2(1f, 1f),
			new global::UnityEngine.Vector2(1f, -1f)
		});
		return renderTexture;
	}

	private global::UnityEngine.RenderTexture ApplyColorEffects(global::UnityEngine.RenderTexture input)
	{
		if (!this.ColorEffectsEnabled)
		{
			return input;
		}
		global::UnityEngine.RenderTexture renderTexture = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(input.width, input.height, input.depth, input.format);
		global::UnityEngine.Graphics.Blit(input, renderTexture, global::FxPro.Mat, 5);
		return renderTexture;
	}

	private global::UnityEngine.RenderTexture ApplyLensCurvature(global::UnityEngine.RenderTexture input)
	{
		if (!this.LensCurvatureEnabled)
		{
			return input;
		}
		global::UnityEngine.RenderTexture renderTexture = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(input.width, input.height, input.depth, input.format);
		global::UnityEngine.Graphics.Blit(input, renderTexture, global::FxPro.Mat, (!this.LensCurvaturePrecise) ? 4 : 3);
		return renderTexture;
	}

	private global::UnityEngine.RenderTexture ApplyChromaticAberration(global::UnityEngine.RenderTexture input)
	{
		if (!this.ChromaticAberration)
		{
			return null;
		}
		global::UnityEngine.RenderTexture renderTexture = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(input.width, input.height, input.depth, input.format);
		renderTexture.filterMode = global::UnityEngine.FilterMode.Bilinear;
		global::UnityEngine.Graphics.Blit(input, renderTexture, global::FxPro.Mat, 2);
		global::FxPro.Mat.SetTexture("_ChromAberrTex", renderTexture);
		return renderTexture;
	}

	private global::UnityEngine.Vector2 ApplyLensCurvature(global::UnityEngine.Vector2 uv, float barrelPower, bool precise)
	{
		uv = uv * 2f - global::UnityEngine.Vector2.one;
		uv.x *= base.GetComponent<global::UnityEngine.Camera>().aspect * 2f;
		float f = global::UnityEngine.Mathf.Atan2(uv.y, uv.x);
		float num = uv.magnitude;
		if (precise)
		{
			num = global::UnityEngine.Mathf.Pow(num, barrelPower);
		}
		else
		{
			num = global::UnityEngine.Mathf.Lerp(num, num * num, global::UnityEngine.Mathf.Clamp01(barrelPower - 1f));
		}
		uv.x = num * global::UnityEngine.Mathf.Cos(f);
		uv.y = num * global::UnityEngine.Mathf.Sin(f);
		uv.x /= base.GetComponent<global::UnityEngine.Camera>().aspect * 2f;
		return 0.5f * (uv + global::UnityEngine.Vector2.one);
	}

	private void UpdateLensCurvatureZoom()
	{
		float value = 1f / this.ApplyLensCurvature(new global::UnityEngine.Vector2(1f, 1f), this.LensCurvaturePower, this.LensCurvaturePrecise).x;
		global::FxPro.Mat.SetFloat("_LensCurvatureZoom", value);
	}

	private void RenderEffects(global::UnityEngine.RenderTexture source, global::UnityEngine.RenderTexture destination)
	{
		source.filterMode = global::UnityEngine.FilterMode.Bilinear;
		global::UnityEngine.RenderTexture tex = source;
		global::UnityEngine.RenderTexture renderTexture = source;
		global::UnityEngine.RenderTexture renderTexture2 = this.ApplyColorEffects(source);
		global::FxProNS.RenderTextureManager.Instance.SafeAssign(ref renderTexture2, this.ApplyLensCurvature(renderTexture2));
		if (this.ChromaticAberrationPrecise)
		{
			tex = this.ApplyChromaticAberration(renderTexture2);
		}
		global::FxProNS.RenderTextureManager.Instance.SafeAssign(ref renderTexture, global::FxPro.DownsampleTex(renderTexture2, 2f));
		if (this.Quality == global::FxProNS.EffectsQuality.Fastest)
		{
			global::FxProNS.RenderTextureManager.Instance.SafeAssign(ref renderTexture, global::FxPro.DownsampleTex(renderTexture, 2f));
		}
		global::UnityEngine.RenderTexture renderTexture3 = null;
		global::UnityEngine.RenderTexture renderTexture4 = null;
		if (this.DOFEnabled)
		{
			if (null == this.DOFParams.EffectCamera)
			{
				global::UnityEngine.Debug.LogError("null == DOFParams.camera");
				return;
			}
			renderTexture3 = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth, renderTexture.format);
			global::FxProNS.Singleton<global::FxProNS.DOFHelper>.Instance.RenderCOCTexture(renderTexture, renderTexture3, (!this.BlurCOCTexture) ? 0f : 1.5f);
			if (this.VisualizeCOC)
			{
				global::UnityEngine.Graphics.Blit(renderTexture3, destination, global::FxProNS.DOFHelper.Mat, 3);
				global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture3);
				global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
				return;
			}
			renderTexture4 = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth, renderTexture.format);
			global::FxProNS.Singleton<global::FxProNS.DOFHelper>.Instance.RenderDOFBlur(renderTexture, renderTexture4, renderTexture3);
			global::FxPro.Mat.SetTexture("_DOFTex", renderTexture4);
			global::FxPro.Mat.SetTexture("_COCTex", renderTexture3);
			global::UnityEngine.Graphics.Blit(renderTexture4, destination);
		}
		if (!this.ChromaticAberrationPrecise)
		{
			tex = this.ApplyChromaticAberration(renderTexture);
		}
		if (this.BloomEnabled)
		{
			global::UnityEngine.RenderTexture renderTexture5 = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth, renderTexture.format);
			global::FxProNS.Singleton<global::FxProNS.BloomHelper>.Instance.RenderBloomTexture(renderTexture, renderTexture5);
			global::FxPro.Mat.SetTexture("_BloomTex", renderTexture5);
			if (this.VisualizeBloom)
			{
				global::UnityEngine.Graphics.Blit(renderTexture5, destination);
				return;
			}
		}
		global::UnityEngine.Graphics.Blit(renderTexture2, destination, global::FxPro.Mat, 0);
		global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture3);
		global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture4);
		global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
		global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(tex);
	}

	[global::UnityEngine.ImageEffectTransformsToLDR]
	public void OnRenderImage(global::UnityEngine.RenderTexture source, global::UnityEngine.RenderTexture destination)
	{
		this.RenderEffects(source, destination);
		global::FxProNS.RenderTextureManager.Instance.ReleaseAllRenderTextures();
	}

	private const bool VisualizeLensCurvature = false;

	public global::FxProNS.EffectsQuality Quality = global::FxProNS.EffectsQuality.Normal;

	private static global::UnityEngine.Material _mat;

	private static global::UnityEngine.Material _tapMat;

	public bool BloomEnabled = true;

	public global::FxProNS.BloomHelperParams BloomParams = new global::FxProNS.BloomHelperParams();

	public bool VisualizeBloom;

	public global::UnityEngine.Texture2D LensDirtTexture;

	[global::UnityEngine.Range(0f, 2f)]
	public float LensDirtIntensity = 1f;

	public bool ChromaticAberration = true;

	public bool ChromaticAberrationPrecise;

	[global::UnityEngine.Range(1f, 2.5f)]
	public float ChromaticAberrationOffset = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float SCurveIntensity = 0.5f;

	public bool LensCurvatureEnabled = true;

	[global::UnityEngine.Range(1f, 2f)]
	public float LensCurvaturePower = 1.1f;

	public bool LensCurvaturePrecise;

	[global::UnityEngine.Range(0f, 1f)]
	public float FilmGrainIntensity = 0.5f;

	[global::UnityEngine.Range(1f, 10f)]
	public float FilmGrainTiling = 4f;

	[global::UnityEngine.Range(0f, 1f)]
	public float VignettingIntensity = 0.5f;

	public bool DOFEnabled = true;

	public bool BlurCOCTexture = true;

	public global::FxProNS.DOFHelperParams DOFParams = new global::FxProNS.DOFHelperParams();

	public bool VisualizeCOC;

	private global::UnityEngine.Texture2D _gridTexture;

	public bool ColorEffectsEnabled = true;

	public global::UnityEngine.Color CloseTint = new global::UnityEngine.Color(1f, 0.5f, 0f, 1f);

	public global::UnityEngine.Color FarTint = new global::UnityEngine.Color(0f, 0f, 1f, 1f);

	[global::UnityEngine.Range(0f, 1f)]
	public float CloseTintStrength = 0.5f;

	[global::UnityEngine.Range(0f, 1f)]
	public float FarTintStrength = 0.5f;

	[global::UnityEngine.Range(0f, 2f)]
	public float DesaturateDarksStrength = 0.5f;

	[global::UnityEngine.Range(0f, 1f)]
	public float DesaturateFarObjsStrength = 0.5f;

	public global::UnityEngine.Color FogTint = global::UnityEngine.Color.white;

	[global::UnityEngine.Range(0f, 1f)]
	public float FogStrength = 0.5f;
}
