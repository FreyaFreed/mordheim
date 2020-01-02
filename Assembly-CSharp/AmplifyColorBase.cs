using System;
using System.Collections.Generic;
using AmplifyColor;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("")]
public class AmplifyColorBase : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Texture2D DefaultLut
	{
		get
		{
			return (!(this.defaultLut == null)) ? this.defaultLut : this.CreateDefaultLut();
		}
	}

	public bool IsBlending
	{
		get
		{
			return this.blending;
		}
	}

	private float effectVolumesBlendAdjusted
	{
		get
		{
			return global::UnityEngine.Mathf.Clamp01((this.effectVolumesBlendAdjust >= 0.99f) ? 1f : ((this.volumesBlendAmount - this.effectVolumesBlendAdjust) / (1f - this.effectVolumesBlendAdjust)));
		}
	}

	public string SharedInstanceID
	{
		get
		{
			return this.sharedInstanceID;
		}
	}

	public bool WillItBlend
	{
		get
		{
			return this.LutTexture != null && this.LutBlendTexture != null && !this.blending;
		}
	}

	public void NewSharedInstanceID()
	{
		this.sharedInstanceID = global::System.Guid.NewGuid().ToString();
	}

	private void ReportMissingShaders()
	{
		global::UnityEngine.Debug.LogError("[AmplifyColor] Failed to initialize shaders. Please attempt to re-enable the Amplify Color Effect component. If that fails, please reinstall Amplify Color.");
		base.enabled = false;
	}

	private void ReportNotSupported()
	{
		global::UnityEngine.Debug.LogWarning("[AmplifyColor] This image effect is not supported on this platform.");
		base.enabled = false;
	}

	private bool CheckShader(global::UnityEngine.Shader s)
	{
		if (s == null)
		{
			this.ReportMissingShaders();
			return false;
		}
		if (!s.isSupported)
		{
			this.ReportNotSupported();
			return false;
		}
		return true;
	}

	private bool CheckShaders()
	{
		return this.CheckShader(this.shaderBase) && this.CheckShader(this.shaderBlend) && this.CheckShader(this.shaderBlendCache) && this.CheckShader(this.shaderMask) && this.CheckShader(this.shaderMaskBlend) && this.CheckShader(this.shaderProcessOnly);
	}

	private bool CheckSupport()
	{
		if (!global::UnityEngine.SystemInfo.supportsImageEffects || !global::UnityEngine.SystemInfo.supportsRenderTextures)
		{
			this.ReportNotSupported();
			return false;
		}
		return true;
	}

	private void OnEnable()
	{
		if (!this.CheckSupport())
		{
			return;
		}
		if (!this.CreateMaterials())
		{
			return;
		}
		global::UnityEngine.Texture2D texture2D = this.LutTexture as global::UnityEngine.Texture2D;
		global::UnityEngine.Texture2D texture2D2 = this.LutBlendTexture as global::UnityEngine.Texture2D;
		if ((texture2D != null && texture2D.mipmapCount > 1) || (texture2D2 != null && texture2D2.mipmapCount > 1))
		{
			global::UnityEngine.Debug.LogError("[AmplifyColor] Please disable \"Generate Mip Maps\" import settings on all LUT textures to avoid visual glitches. Change Texture Type to \"Advanced\" to access Mip settings.");
		}
	}

	private void OnDisable()
	{
		if (this.actualTriggerProxy != null)
		{
			global::UnityEngine.Object.DestroyImmediate(this.actualTriggerProxy.gameObject);
			this.actualTriggerProxy = null;
		}
		this.ReleaseMaterials();
		this.ReleaseTextures();
	}

	private void VolumesBlendTo(global::UnityEngine.Texture blendTargetLUT, float blendTimeInSec)
	{
		this.volumesLutBlendTexture = blendTargetLUT;
		this.volumesBlendAmount = 0f;
		this.volumesBlendingTime = blendTimeInSec;
		this.volumesBlendingTimeCountdown = blendTimeInSec;
		this.volumesBlending = true;
	}

	public void BlendTo(global::UnityEngine.Texture blendTargetLUT, float blendTimeInSec, global::System.Action onFinishBlend)
	{
		this.LutBlendTexture = blendTargetLUT;
		this.BlendAmount = 0f;
		this.onFinishBlend = onFinishBlend;
		this.blendingTime = blendTimeInSec;
		this.blendingTimeCountdown = blendTimeInSec;
		this.blending = true;
	}

	private void CheckCamera()
	{
		if (this.ownerCamera == null)
		{
			this.ownerCamera = base.GetComponent<global::UnityEngine.Camera>();
		}
		if (this.UseDepthMask && (this.ownerCamera.depthTextureMode & global::UnityEngine.DepthTextureMode.Depth) == global::UnityEngine.DepthTextureMode.None)
		{
			this.ownerCamera.depthTextureMode |= global::UnityEngine.DepthTextureMode.Depth;
		}
	}

	private void Start()
	{
		this.CheckCamera();
		this.worldLUT = this.LutTexture;
		this.worldVolumeEffects = this.EffectFlags.GenerateEffectData(this);
		this.blendVolumeEffects = (this.currentVolumeEffects = this.worldVolumeEffects);
		this.worldExposure = this.Exposure;
		this.blendExposure = (this.currentExposure = this.worldExposure);
	}

	private void Update()
	{
		this.CheckCamera();
		bool flag = false;
		if (this.volumesBlending)
		{
			this.volumesBlendAmount = (this.volumesBlendingTime - this.volumesBlendingTimeCountdown) / this.volumesBlendingTime;
			this.volumesBlendingTimeCountdown -= global::UnityEngine.Time.smoothDeltaTime;
			if (this.volumesBlendAmount >= 1f)
			{
				this.volumesBlendAmount = 1f;
				flag = true;
			}
		}
		else
		{
			this.volumesBlendAmount = global::UnityEngine.Mathf.Clamp01(this.volumesBlendAmount);
		}
		if (this.blending)
		{
			this.BlendAmount = (this.blendingTime - this.blendingTimeCountdown) / this.blendingTime;
			this.blendingTimeCountdown -= global::UnityEngine.Time.smoothDeltaTime;
			if (this.BlendAmount >= 1f)
			{
				this.LutTexture = this.LutBlendTexture;
				this.BlendAmount = 0f;
				this.blending = false;
				this.LutBlendTexture = null;
				if (this.onFinishBlend != null)
				{
					this.onFinishBlend();
				}
			}
		}
		else
		{
			this.BlendAmount = global::UnityEngine.Mathf.Clamp01(this.BlendAmount);
		}
		if (this.UseVolumes)
		{
			if (this.actualTriggerProxy == null)
			{
				global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject(base.name + "+ACVolumeProxy")
				{
					hideFlags = global::UnityEngine.HideFlags.HideAndDontSave
				};
				if (this.TriggerVolumeProxy != null && this.TriggerVolumeProxy.GetComponent<global::UnityEngine.Collider2D>() != null)
				{
					this.actualTriggerProxy = gameObject.AddComponent<global::AmplifyColorTriggerProxy2D>();
				}
				else
				{
					this.actualTriggerProxy = gameObject.AddComponent<global::AmplifyColorTriggerProxy>();
				}
				this.actualTriggerProxy.OwnerEffect = this;
			}
			this.UpdateVolumes();
		}
		else if (this.actualTriggerProxy != null)
		{
			global::UnityEngine.Object.DestroyImmediate(this.actualTriggerProxy.gameObject);
			this.actualTriggerProxy = null;
		}
		if (flag)
		{
			this.LutTexture = this.volumesLutBlendTexture;
			this.volumesBlendAmount = 0f;
			this.volumesBlending = false;
			this.volumesLutBlendTexture = null;
			this.effectVolumesBlendAdjust = 0f;
			this.currentVolumeEffects = this.blendVolumeEffects;
			this.currentVolumeEffects.SetValues(this);
			this.currentExposure = this.blendExposure;
			if (this.blendingFromMidBlend && this.midBlendLUT != null)
			{
				this.midBlendLUT.DiscardContents();
			}
			this.blendingFromMidBlend = false;
		}
	}

	public void EnterVolume(global::AmplifyColorVolumeBase volume)
	{
		if (!this.enteredVolumes.Contains(volume))
		{
			this.enteredVolumes.Insert(0, volume);
		}
	}

	public void ExitVolume(global::AmplifyColorVolumeBase volume)
	{
		if (this.enteredVolumes.Contains(volume))
		{
			this.enteredVolumes.Remove(volume);
		}
	}

	private void UpdateVolumes()
	{
		if (this.volumesBlending)
		{
			this.currentVolumeEffects.BlendValues(this, this.blendVolumeEffects, this.effectVolumesBlendAdjusted);
		}
		if (this.volumesBlending)
		{
			this.Exposure = global::UnityEngine.Mathf.Lerp(this.currentExposure, this.blendExposure, this.effectVolumesBlendAdjusted);
		}
		global::UnityEngine.Transform transform = (!(this.TriggerVolumeProxy == null)) ? this.TriggerVolumeProxy : base.transform;
		if (this.actualTriggerProxy.transform.parent != transform)
		{
			this.actualTriggerProxy.Reference = transform;
			this.actualTriggerProxy.gameObject.layer = global::UnityEngine.LayerMask.NameToLayer("trigger_collision");
		}
		global::AmplifyColorVolumeBase amplifyColorVolumeBase = null;
		int num = int.MinValue;
		for (int i = 0; i < this.enteredVolumes.Count; i++)
		{
			global::AmplifyColorVolumeBase amplifyColorVolumeBase2 = this.enteredVolumes[i];
			if (amplifyColorVolumeBase2.Priority > num)
			{
				amplifyColorVolumeBase = amplifyColorVolumeBase2;
				num = amplifyColorVolumeBase2.Priority;
			}
		}
		if (amplifyColorVolumeBase != this.currentVolumeLut)
		{
			this.currentVolumeLut = amplifyColorVolumeBase;
			global::UnityEngine.Texture texture = (!(amplifyColorVolumeBase == null)) ? amplifyColorVolumeBase.LutTexture : this.worldLUT;
			float num2 = (!(amplifyColorVolumeBase == null)) ? amplifyColorVolumeBase.EnterBlendTime : this.ExitVolumeBlendTime;
			if (this.volumesBlending && !this.blendingFromMidBlend && texture == this.LutTexture)
			{
				this.LutTexture = this.volumesLutBlendTexture;
				this.volumesLutBlendTexture = texture;
				this.volumesBlendingTimeCountdown = num2 * ((this.volumesBlendingTime - this.volumesBlendingTimeCountdown) / this.volumesBlendingTime);
				this.volumesBlendingTime = num2;
				this.currentVolumeEffects = global::AmplifyColor.VolumeEffect.BlendValuesToVolumeEffect(this.EffectFlags, this.currentVolumeEffects, this.blendVolumeEffects, this.effectVolumesBlendAdjusted);
				this.currentExposure = global::UnityEngine.Mathf.Lerp(this.currentExposure, this.blendExposure, this.effectVolumesBlendAdjusted);
				this.effectVolumesBlendAdjust = 1f - this.volumesBlendAmount;
				this.volumesBlendAmount = 1f - this.volumesBlendAmount;
			}
			else
			{
				if (this.volumesBlending)
				{
					this.materialBlendCache.SetFloat("_LerpAmount", this.volumesBlendAmount);
					if (this.blendingFromMidBlend)
					{
						global::UnityEngine.Graphics.Blit(this.midBlendLUT, this.blendCacheLut);
						this.materialBlendCache.SetTexture("_RgbTex", this.blendCacheLut);
					}
					else
					{
						this.materialBlendCache.SetTexture("_RgbTex", this.LutTexture);
					}
					this.materialBlendCache.SetTexture("_LerpRgbTex", (!(this.volumesLutBlendTexture != null)) ? this.defaultLut : this.volumesLutBlendTexture);
					global::UnityEngine.Graphics.Blit(this.midBlendLUT, this.midBlendLUT, this.materialBlendCache);
					this.blendCacheLut.DiscardContents();
					this.currentVolumeEffects = global::AmplifyColor.VolumeEffect.BlendValuesToVolumeEffect(this.EffectFlags, this.currentVolumeEffects, this.blendVolumeEffects, this.effectVolumesBlendAdjusted);
					this.currentExposure = global::UnityEngine.Mathf.Lerp(this.currentExposure, this.blendExposure, this.effectVolumesBlendAdjusted);
					this.effectVolumesBlendAdjust = 0f;
					this.blendingFromMidBlend = true;
				}
				this.VolumesBlendTo(texture, num2);
			}
			this.blendVolumeEffects = ((!(amplifyColorVolumeBase == null)) ? amplifyColorVolumeBase.EffectContainer.FindVolumeEffect(this) : this.worldVolumeEffects);
			this.blendExposure = ((!(amplifyColorVolumeBase == null)) ? amplifyColorVolumeBase.Exposure : this.worldExposure);
			if (this.blendVolumeEffects == null)
			{
				this.blendVolumeEffects = this.worldVolumeEffects;
			}
		}
	}

	private void SetupShader()
	{
		this.colorSpace = global::UnityEngine.ColorSpace.Linear;
		this.qualityLevel = this.QualityLevel;
		this.shaderBase = global::UnityEngine.Shader.Find("Hidden/Amplify Color/Base");
		this.shaderBlend = global::UnityEngine.Shader.Find("Hidden/Amplify Color/Blend");
		this.shaderBlendCache = global::UnityEngine.Shader.Find("Hidden/Amplify Color/BlendCache");
		this.shaderMask = global::UnityEngine.Shader.Find("Hidden/Amplify Color/Mask");
		this.shaderMaskBlend = global::UnityEngine.Shader.Find("Hidden/Amplify Color/MaskBlend");
		this.shaderDepthMask = global::UnityEngine.Shader.Find("Hidden/Amplify Color/DepthMask");
		this.shaderDepthMaskBlend = global::UnityEngine.Shader.Find("Hidden/Amplify Color/DepthMaskBlend");
		this.shaderProcessOnly = global::UnityEngine.Shader.Find("Hidden/Amplify Color/ProcessOnly");
	}

	private void ReleaseMaterials()
	{
		this.SafeRelease<global::UnityEngine.Material>(ref this.materialBase);
		this.SafeRelease<global::UnityEngine.Material>(ref this.materialBlend);
		this.SafeRelease<global::UnityEngine.Material>(ref this.materialBlendCache);
		this.SafeRelease<global::UnityEngine.Material>(ref this.materialMask);
		this.SafeRelease<global::UnityEngine.Material>(ref this.materialMaskBlend);
		this.SafeRelease<global::UnityEngine.Material>(ref this.materialDepthMask);
		this.SafeRelease<global::UnityEngine.Material>(ref this.materialDepthMaskBlend);
		this.SafeRelease<global::UnityEngine.Material>(ref this.materialProcessOnly);
	}

	private global::UnityEngine.Texture2D CreateDefaultLut()
	{
		this.defaultLut = new global::UnityEngine.Texture2D(1024, 32, global::UnityEngine.TextureFormat.RGB24, false, true)
		{
			hideFlags = global::UnityEngine.HideFlags.HideAndDontSave
		};
		this.defaultLut.name = "DefaultLut";
		this.defaultLut.hideFlags = global::UnityEngine.HideFlags.DontSave;
		this.defaultLut.anisoLevel = 1;
		this.defaultLut.filterMode = global::UnityEngine.FilterMode.Bilinear;
		global::UnityEngine.Color32[] array = new global::UnityEngine.Color32[32768];
		for (int i = 0; i < 32; i++)
		{
			int num = i * 32;
			for (int j = 0; j < 32; j++)
			{
				int num2 = num + j * 1024;
				for (int k = 0; k < 32; k++)
				{
					float num3 = (float)k / 31f;
					float num4 = (float)j / 31f;
					float num5 = (float)i / 31f;
					byte r = (byte)(num3 * 255f);
					byte g = (byte)(num4 * 255f);
					byte b = (byte)(num5 * 255f);
					array[num2 + k] = new global::UnityEngine.Color32(r, g, b, byte.MaxValue);
				}
			}
		}
		this.defaultLut.SetPixels32(array);
		this.defaultLut.Apply();
		return this.defaultLut;
	}

	private global::UnityEngine.Texture2D CreateDepthCurveLut()
	{
		this.SafeRelease<global::UnityEngine.Texture2D>(ref this.depthCurveLut);
		this.depthCurveLut = new global::UnityEngine.Texture2D(1024, 1, global::UnityEngine.TextureFormat.Alpha8, false, true)
		{
			hideFlags = global::UnityEngine.HideFlags.HideAndDontSave
		};
		this.depthCurveLut.name = "DepthCurveLut";
		this.depthCurveLut.hideFlags = global::UnityEngine.HideFlags.DontSave;
		this.depthCurveLut.anisoLevel = 1;
		this.depthCurveLut.wrapMode = global::UnityEngine.TextureWrapMode.Clamp;
		this.depthCurveLut.filterMode = global::UnityEngine.FilterMode.Bilinear;
		this.depthCurveColors = new global::UnityEngine.Color32[1024];
		return this.depthCurveLut;
	}

	private void UpdateDepthCurveLut()
	{
		if (this.depthCurveLut == null)
		{
			this.CreateDepthCurveLut();
		}
		float num = 0f;
		int i = 0;
		while (i < 1024)
		{
			this.depthCurveColors[i].a = (byte)global::UnityEngine.Mathf.FloorToInt(global::UnityEngine.Mathf.Clamp01(this.DepthMaskCurve.Evaluate(num)) * 255f);
			i++;
			num += 0.0009775171f;
		}
		this.depthCurveLut.SetPixels32(this.depthCurveColors);
		this.depthCurveLut.Apply();
	}

	private void CheckUpdateDepthCurveLut()
	{
		bool flag = false;
		if (this.DepthMaskCurve.length != this.prevDepthMaskCurve.length)
		{
			flag = true;
		}
		else
		{
			float num = 0f;
			int i = 0;
			while (i < this.DepthMaskCurve.length)
			{
				if (global::UnityEngine.Mathf.Abs(this.DepthMaskCurve.Evaluate(num) - this.prevDepthMaskCurve.Evaluate(num)) > 1.401298E-45f)
				{
					flag = true;
					break;
				}
				i++;
				num += 0.0009775171f;
			}
		}
		if (this.depthCurveLut == null || flag)
		{
			this.UpdateDepthCurveLut();
			this.prevDepthMaskCurve = new global::UnityEngine.AnimationCurve(this.DepthMaskCurve.keys);
		}
	}

	private void CreateHelperTextures()
	{
		this.ReleaseTextures();
		this.blendCacheLut = new global::UnityEngine.RenderTexture(1024, 32, 0, global::UnityEngine.RenderTextureFormat.ARGB32, global::UnityEngine.RenderTextureReadWrite.Linear)
		{
			hideFlags = global::UnityEngine.HideFlags.HideAndDontSave
		};
		this.blendCacheLut.name = "BlendCacheLut";
		this.blendCacheLut.wrapMode = global::UnityEngine.TextureWrapMode.Clamp;
		this.blendCacheLut.useMipMap = false;
		this.blendCacheLut.anisoLevel = 0;
		this.blendCacheLut.Create();
		this.midBlendLUT = new global::UnityEngine.RenderTexture(1024, 32, 0, global::UnityEngine.RenderTextureFormat.ARGB32, global::UnityEngine.RenderTextureReadWrite.Linear)
		{
			hideFlags = global::UnityEngine.HideFlags.HideAndDontSave
		};
		this.midBlendLUT.name = "MidBlendLut";
		this.midBlendLUT.wrapMode = global::UnityEngine.TextureWrapMode.Clamp;
		this.midBlendLUT.useMipMap = false;
		this.midBlendLUT.anisoLevel = 0;
		this.midBlendLUT.Create();
		this.CreateDefaultLut();
		if (this.UseDepthMask)
		{
			this.CreateDepthCurveLut();
		}
	}

	private bool CheckMaterialAndShader(global::UnityEngine.Material material, string name)
	{
		if (material == null || material.shader == null)
		{
			global::UnityEngine.Debug.LogWarning("[AmplifyColor] Error creating " + name + " material. Effect disabled.");
			base.enabled = false;
		}
		else if (!material.shader.isSupported)
		{
			global::UnityEngine.Debug.LogWarning("[AmplifyColor] " + name + " shader not supported on this platform. Effect disabled.");
			base.enabled = false;
		}
		else
		{
			material.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
		}
		return base.enabled;
	}

	private bool CreateMaterials()
	{
		this.SetupShader();
		if (!this.CheckShaders())
		{
			return false;
		}
		this.ReleaseMaterials();
		this.materialBase = new global::UnityEngine.Material(this.shaderBase);
		this.materialBlend = new global::UnityEngine.Material(this.shaderBlend);
		this.materialBlendCache = new global::UnityEngine.Material(this.shaderBlendCache);
		this.materialMask = new global::UnityEngine.Material(this.shaderMask);
		this.materialMaskBlend = new global::UnityEngine.Material(this.shaderMaskBlend);
		this.materialDepthMask = new global::UnityEngine.Material(this.shaderDepthMask);
		this.materialDepthMaskBlend = new global::UnityEngine.Material(this.shaderDepthMaskBlend);
		this.materialProcessOnly = new global::UnityEngine.Material(this.shaderProcessOnly);
		bool flag = true;
		flag = (flag && this.CheckMaterialAndShader(this.materialBase, "BaseMaterial"));
		flag = (flag && this.CheckMaterialAndShader(this.materialBlend, "BlendMaterial"));
		flag = (flag && this.CheckMaterialAndShader(this.materialBlendCache, "BlendCacheMaterial"));
		flag = (flag && this.CheckMaterialAndShader(this.materialMask, "MaskMaterial"));
		flag = (flag && this.CheckMaterialAndShader(this.materialMaskBlend, "MaskBlendMaterial"));
		flag = (flag && this.CheckMaterialAndShader(this.materialDepthMask, "DepthMaskMaterial"));
		flag = (flag && this.CheckMaterialAndShader(this.materialDepthMaskBlend, "DepthMaskBlendMaterial"));
		if (!flag || !this.CheckMaterialAndShader(this.materialProcessOnly, "ProcessOnlyMaterial"))
		{
			return false;
		}
		this.CreateHelperTextures();
		return true;
	}

	private void SetMaterialKeyword(string keyword, bool state)
	{
		bool flag = this.materialBase.IsKeywordEnabled(keyword);
		if (state && !flag)
		{
			this.materialBase.EnableKeyword(keyword);
			this.materialBlend.EnableKeyword(keyword);
			this.materialBlendCache.EnableKeyword(keyword);
			this.materialMask.EnableKeyword(keyword);
			this.materialMaskBlend.EnableKeyword(keyword);
			this.materialDepthMask.EnableKeyword(keyword);
			this.materialDepthMaskBlend.EnableKeyword(keyword);
			this.materialProcessOnly.EnableKeyword(keyword);
		}
		else if (!state && this.materialBase.IsKeywordEnabled(keyword))
		{
			this.materialBase.DisableKeyword(keyword);
			this.materialBlend.DisableKeyword(keyword);
			this.materialBlendCache.DisableKeyword(keyword);
			this.materialMask.DisableKeyword(keyword);
			this.materialMaskBlend.DisableKeyword(keyword);
			this.materialDepthMask.DisableKeyword(keyword);
			this.materialDepthMaskBlend.DisableKeyword(keyword);
			this.materialProcessOnly.DisableKeyword(keyword);
		}
	}

	private void UpdateShaderKeywords()
	{
		this.SetMaterialKeyword("AC_QUALITY_MOBILE", this.QualityLevel == global::AmplifyColor.Quality.Mobile);
		this.SetMaterialKeyword("AC_DITHERING", this.UseDithering);
		this.SetMaterialKeyword("AC_TONEMAPPING", this.UseToneMapping);
	}

	private void SafeRelease<T>(ref T obj) where T : global::UnityEngine.Object
	{
		if (obj != null)
		{
			global::UnityEngine.Object.DestroyImmediate(obj);
			obj = (T)((object)null);
		}
	}

	private void ReleaseTextures()
	{
		this.SafeRelease<global::UnityEngine.RenderTexture>(ref this.blendCacheLut);
		this.SafeRelease<global::UnityEngine.RenderTexture>(ref this.midBlendLUT);
		this.SafeRelease<global::UnityEngine.Texture2D>(ref this.defaultLut);
		this.SafeRelease<global::UnityEngine.Texture2D>(ref this.depthCurveLut);
	}

	public static bool ValidateLutDimensions(global::UnityEngine.Texture lut)
	{
		bool result = true;
		if (lut != null)
		{
			if (lut.width / lut.height != lut.height)
			{
				global::UnityEngine.Debug.LogWarning("[AmplifyColor] Lut " + lut.name + " has invalid dimensions.");
				result = false;
			}
			else if (lut.anisoLevel != 0)
			{
				lut.anisoLevel = 0;
			}
		}
		return result;
	}

	private void UpdatePostEffectParams()
	{
		if (this.UseDepthMask)
		{
			this.CheckUpdateDepthCurveLut();
		}
		this.Exposure = global::UnityEngine.Mathf.Max(this.Exposure, 0f);
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture source, global::UnityEngine.RenderTexture destination)
	{
		this.BlendAmount = global::UnityEngine.Mathf.Clamp01(this.BlendAmount);
		if (this.colorSpace != global::UnityEngine.QualitySettings.activeColorSpace || this.qualityLevel != this.QualityLevel)
		{
			this.CreateMaterials();
		}
		this.UpdatePostEffectParams();
		this.UpdateShaderKeywords();
		bool flag = global::AmplifyColorBase.ValidateLutDimensions(this.LutTexture);
		bool flag2 = global::AmplifyColorBase.ValidateLutDimensions(this.LutBlendTexture);
		bool flag3 = this.LutTexture == null && this.LutBlendTexture == null && this.volumesLutBlendTexture == null;
		global::UnityEngine.Texture texture = (!(this.LutTexture == null)) ? this.LutTexture : this.defaultLut;
		global::UnityEngine.Texture lutBlendTexture = this.LutBlendTexture;
		int pass = ((this.colorSpace != global::UnityEngine.ColorSpace.Linear) ? 0 : 2) + ((!this.ownerCamera.hdr) ? 0 : 1);
		bool flag4 = this.BlendAmount != 0f || this.blending;
		bool flag5 = flag4 || (flag4 && lutBlendTexture != null);
		bool flag6 = flag5;
		bool flag7 = !flag || !flag2 || flag3;
		global::UnityEngine.Material material;
		if (flag7)
		{
			material = this.materialProcessOnly;
		}
		else if (flag5 || this.volumesBlending)
		{
			if (this.UseDepthMask)
			{
				material = this.materialDepthMaskBlend;
			}
			else
			{
				material = ((!(this.MaskTexture != null)) ? this.materialBlend : this.materialMaskBlend);
			}
		}
		else if (this.UseDepthMask)
		{
			material = this.materialDepthMask;
		}
		else
		{
			material = ((!(this.MaskTexture != null)) ? this.materialBase : this.materialMask);
		}
		material.SetFloat("_Exposure", this.Exposure);
		material.SetFloat("_LerpAmount", this.BlendAmount);
		if (this.MaskTexture != null)
		{
			material.SetTexture("_MaskTex", this.MaskTexture);
		}
		if (this.UseDepthMask)
		{
			material.SetTexture("_DepthCurveLut", this.depthCurveLut);
		}
		if (!flag7)
		{
			if (this.volumesBlending)
			{
				this.volumesBlendAmount = global::UnityEngine.Mathf.Clamp01(this.volumesBlendAmount);
				this.materialBlendCache.SetFloat("_LerpAmount", this.volumesBlendAmount);
				if (this.blendingFromMidBlend)
				{
					this.materialBlendCache.SetTexture("_RgbTex", this.midBlendLUT);
				}
				else
				{
					this.materialBlendCache.SetTexture("_RgbTex", texture);
				}
				this.materialBlendCache.SetTexture("_LerpRgbTex", (!(this.volumesLutBlendTexture != null)) ? this.defaultLut : this.volumesLutBlendTexture);
				global::UnityEngine.Graphics.Blit(texture, this.blendCacheLut, this.materialBlendCache);
			}
			if (flag6)
			{
				this.materialBlendCache.SetFloat("_LerpAmount", this.BlendAmount);
				global::UnityEngine.RenderTexture renderTexture = null;
				if (this.volumesBlending)
				{
					renderTexture = global::UnityEngine.RenderTexture.GetTemporary(this.blendCacheLut.width, this.blendCacheLut.height, this.blendCacheLut.depth, this.blendCacheLut.format, global::UnityEngine.RenderTextureReadWrite.Linear);
					global::UnityEngine.Graphics.Blit(this.blendCacheLut, renderTexture);
					this.materialBlendCache.SetTexture("_RgbTex", renderTexture);
				}
				else
				{
					this.materialBlendCache.SetTexture("_RgbTex", texture);
				}
				this.materialBlendCache.SetTexture("_LerpRgbTex", (!(lutBlendTexture != null)) ? this.defaultLut : lutBlendTexture);
				global::UnityEngine.Graphics.Blit(texture, this.blendCacheLut, this.materialBlendCache);
				if (renderTexture != null)
				{
					global::UnityEngine.RenderTexture.ReleaseTemporary(renderTexture);
				}
				material.SetTexture("_RgbBlendCacheTex", this.blendCacheLut);
			}
			else if (this.volumesBlending)
			{
				material.SetTexture("_RgbBlendCacheTex", this.blendCacheLut);
			}
			else
			{
				if (texture != null)
				{
					material.SetTexture("_RgbTex", texture);
				}
				if (lutBlendTexture != null)
				{
					material.SetTexture("_LerpRgbTex", lutBlendTexture);
				}
			}
		}
		global::UnityEngine.Graphics.Blit(source, destination, material, pass);
		if (flag6 || this.volumesBlending)
		{
			this.blendCacheLut.DiscardContents();
		}
	}

	public const int LutSize = 32;

	public const int LutWidth = 1024;

	public const int LutHeight = 32;

	private const int DepthCurveLutRange = 1024;

	public float Exposure = 1f;

	public bool UseToneMapping;

	public bool UseDithering;

	public global::AmplifyColor.Quality QualityLevel = global::AmplifyColor.Quality.Standard;

	public float BlendAmount;

	public global::UnityEngine.Texture LutTexture;

	public global::UnityEngine.Texture LutBlendTexture;

	public global::UnityEngine.Texture MaskTexture;

	public bool UseDepthMask;

	public global::UnityEngine.AnimationCurve DepthMaskCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe[]
	{
		new global::UnityEngine.Keyframe(0f, 1f),
		new global::UnityEngine.Keyframe(1f, 1f)
	});

	public bool UseVolumes;

	public float ExitVolumeBlendTime = 1f;

	public global::UnityEngine.Transform TriggerVolumeProxy;

	public global::UnityEngine.LayerMask VolumeCollisionMask = -1;

	private global::UnityEngine.Camera ownerCamera;

	private global::UnityEngine.Shader shaderBase;

	private global::UnityEngine.Shader shaderBlend;

	private global::UnityEngine.Shader shaderBlendCache;

	private global::UnityEngine.Shader shaderMask;

	private global::UnityEngine.Shader shaderMaskBlend;

	private global::UnityEngine.Shader shaderDepthMask;

	private global::UnityEngine.Shader shaderDepthMaskBlend;

	private global::UnityEngine.Shader shaderProcessOnly;

	private global::UnityEngine.RenderTexture blendCacheLut;

	private global::UnityEngine.Texture2D defaultLut;

	private global::UnityEngine.Texture2D depthCurveLut;

	private global::UnityEngine.Color32[] depthCurveColors;

	private global::UnityEngine.ColorSpace colorSpace = global::UnityEngine.ColorSpace.Uninitialized;

	private global::AmplifyColor.Quality qualityLevel = global::AmplifyColor.Quality.Standard;

	private global::UnityEngine.Material materialBase;

	private global::UnityEngine.Material materialBlend;

	private global::UnityEngine.Material materialBlendCache;

	private global::UnityEngine.Material materialMask;

	private global::UnityEngine.Material materialMaskBlend;

	private global::UnityEngine.Material materialDepthMask;

	private global::UnityEngine.Material materialDepthMaskBlend;

	private global::UnityEngine.Material materialProcessOnly;

	private bool blending;

	private float blendingTime;

	private float blendingTimeCountdown;

	private global::System.Action onFinishBlend;

	private global::UnityEngine.AnimationCurve prevDepthMaskCurve = new global::UnityEngine.AnimationCurve();

	private bool volumesBlending;

	private float volumesBlendingTime;

	private float volumesBlendingTimeCountdown;

	private global::UnityEngine.Texture volumesLutBlendTexture;

	private float volumesBlendAmount;

	private global::UnityEngine.Texture worldLUT;

	private global::AmplifyColorVolumeBase currentVolumeLut;

	private global::UnityEngine.RenderTexture midBlendLUT;

	private bool blendingFromMidBlend;

	private global::AmplifyColor.VolumeEffect worldVolumeEffects;

	private global::AmplifyColor.VolumeEffect currentVolumeEffects;

	private global::AmplifyColor.VolumeEffect blendVolumeEffects;

	private float worldExposure = 1f;

	private float currentExposure = 1f;

	private float blendExposure = 1f;

	private float effectVolumesBlendAdjust;

	private global::System.Collections.Generic.List<global::AmplifyColorVolumeBase> enteredVolumes = new global::System.Collections.Generic.List<global::AmplifyColorVolumeBase>();

	private global::AmplifyColorTriggerProxyBase actualTriggerProxy;

	[global::UnityEngine.HideInInspector]
	public global::AmplifyColor.VolumeEffectFlags EffectFlags = new global::AmplifyColor.VolumeEffectFlags();

	[global::UnityEngine.SerializeField]
	[global::UnityEngine.HideInInspector]
	private string sharedInstanceID = string.Empty;
}
