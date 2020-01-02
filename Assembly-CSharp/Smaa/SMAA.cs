using System;
using UnityEngine;

namespace Smaa
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Camera))]
	[global::UnityEngine.AddComponentMenu("Image Effects/Subpixel Morphological Antialiasing")]
	[global::UnityEngine.ExecuteInEditMode]
	public class SMAA : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.Material Material
		{
			get
			{
				if (this.m_Material == null)
				{
					this.m_Material = new global::UnityEngine.Material(this.Shader);
					this.m_Material.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
				}
				return this.m_Material;
			}
		}

		private void OnEnable()
		{
			if (this.AreaTex == null)
			{
				this.AreaTex = global::UnityEngine.Resources.Load<global::UnityEngine.Texture2D>("AreaTex");
			}
			if (this.SearchTex == null)
			{
				this.SearchTex = global::UnityEngine.Resources.Load<global::UnityEngine.Texture2D>("SearchTex");
			}
			this.m_Camera = base.GetComponent<global::UnityEngine.Camera>();
		}

		private void Start()
		{
			if (!global::UnityEngine.SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (!this.Shader || !this.Shader.isSupported)
			{
				base.enabled = false;
			}
			this.CreatePresets();
		}

		private void OnDisable()
		{
			if (this.m_Material != null)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_Material);
			}
		}

		private void OnRenderImage(global::UnityEngine.RenderTexture source, global::UnityEngine.RenderTexture destination)
		{
			int pixelWidth = this.m_Camera.pixelWidth;
			int pixelHeight = this.m_Camera.pixelHeight;
			global::Smaa.Preset preset = this.CustomPreset;
			if (this.Quality == global::Smaa.QualityPreset.Low)
			{
				preset = this.m_LowPreset;
			}
			else if (this.Quality == global::Smaa.QualityPreset.Medium)
			{
				preset = this.m_MediumPreset;
			}
			else if (this.Quality == global::Smaa.QualityPreset.High)
			{
				preset = this.m_HighPreset;
			}
			else if (this.Quality == global::Smaa.QualityPreset.Ultra)
			{
				preset = this.m_UltraPreset;
			}
			int detectionMethod = (int)this.DetectionMethod;
			int pass = 4;
			int pass2 = 5;
			this.Material.SetTexture("_AreaTex", this.AreaTex);
			this.Material.SetTexture("_SearchTex", this.SearchTex);
			this.Material.SetTexture("_SourceTex", source);
			this.Material.SetVector("_Metrics", new global::UnityEngine.Vector4(1f / (float)pixelWidth, 1f / (float)pixelHeight, (float)pixelWidth, (float)pixelHeight));
			this.Material.SetVector("_Params1", new global::UnityEngine.Vector4(preset.Threshold, preset.DepthThreshold, (float)preset.MaxSearchSteps, (float)preset.MaxSearchStepsDiag));
			this.Material.SetVector("_Params2", new global::UnityEngine.Vector2((float)preset.CornerRounding, preset.LocalContrastAdaptationFactor));
			global::UnityEngine.Shader.DisableKeyword("USE_PREDICATION");
			if (this.DetectionMethod == global::Smaa.EdgeDetectionMethod.Depth)
			{
				this.m_Camera.depthTextureMode |= global::UnityEngine.DepthTextureMode.Depth;
			}
			else if (this.UsePredication)
			{
				this.m_Camera.depthTextureMode |= global::UnityEngine.DepthTextureMode.Depth;
				global::UnityEngine.Shader.EnableKeyword("USE_PREDICATION");
				this.Material.SetVector("_Params3", new global::UnityEngine.Vector3(this.CustomPredicationPreset.Threshold, this.CustomPredicationPreset.Scale, this.CustomPredicationPreset.Strength));
			}
			global::UnityEngine.Shader.DisableKeyword("USE_DIAG_SEARCH");
			global::UnityEngine.Shader.DisableKeyword("USE_CORNER_DETECTION");
			if (preset.DiagDetection)
			{
				global::UnityEngine.Shader.EnableKeyword("USE_DIAG_SEARCH");
			}
			if (preset.CornerDetection)
			{
				global::UnityEngine.Shader.EnableKeyword("USE_CORNER_DETECTION");
			}
			global::UnityEngine.RenderTexture renderTexture = this.TempRT(pixelWidth, pixelHeight);
			global::UnityEngine.RenderTexture renderTexture2 = this.TempRT(pixelWidth, pixelHeight);
			this.Clear(renderTexture);
			this.Clear(renderTexture2);
			global::UnityEngine.Graphics.Blit(source, renderTexture, this.Material, detectionMethod);
			if (this.DebugPass == global::Smaa.DebugPass.Edges)
			{
				global::UnityEngine.Graphics.Blit(renderTexture, destination);
			}
			else
			{
				global::UnityEngine.Graphics.Blit(renderTexture, renderTexture2, this.Material, pass);
				if (this.DebugPass == global::Smaa.DebugPass.Weights)
				{
					global::UnityEngine.Graphics.Blit(renderTexture2, destination);
				}
				else
				{
					global::UnityEngine.Graphics.Blit(renderTexture2, destination, this.Material, pass2);
				}
			}
			global::UnityEngine.RenderTexture.ReleaseTemporary(renderTexture);
			global::UnityEngine.RenderTexture.ReleaseTemporary(renderTexture2);
		}

		private void Clear(global::UnityEngine.RenderTexture rt)
		{
			global::UnityEngine.Graphics.Blit(rt, rt, this.Material, 0);
		}

		private global::UnityEngine.RenderTexture TempRT(int width, int height)
		{
			int depthBuffer = 0;
			return global::UnityEngine.RenderTexture.GetTemporary(width, height, depthBuffer, global::UnityEngine.RenderTextureFormat.ARGB32, global::UnityEngine.RenderTextureReadWrite.Linear);
		}

		private void CreatePresets()
		{
			this.m_LowPreset = new global::Smaa.Preset
			{
				Threshold = 0.15f,
				MaxSearchSteps = 4
			};
			this.m_LowPreset.DiagDetection = false;
			this.m_LowPreset.CornerDetection = false;
			this.m_MediumPreset = new global::Smaa.Preset
			{
				Threshold = 0.1f,
				MaxSearchSteps = 8
			};
			this.m_MediumPreset.DiagDetection = false;
			this.m_MediumPreset.CornerDetection = false;
			this.m_HighPreset = new global::Smaa.Preset
			{
				Threshold = 0.1f,
				MaxSearchSteps = 16,
				MaxSearchStepsDiag = 8,
				CornerRounding = 25
			};
			this.m_UltraPreset = new global::Smaa.Preset
			{
				Threshold = 0.05f,
				MaxSearchSteps = 32,
				MaxSearchStepsDiag = 16,
				CornerRounding = 25
			};
		}

		public global::Smaa.DebugPass DebugPass;

		public global::Smaa.QualityPreset Quality = global::Smaa.QualityPreset.High;

		public global::Smaa.EdgeDetectionMethod DetectionMethod = global::Smaa.EdgeDetectionMethod.Luma;

		public bool UsePredication;

		public global::Smaa.Preset CustomPreset;

		public global::Smaa.PredicationPreset CustomPredicationPreset;

		public global::UnityEngine.Shader Shader;

		public global::UnityEngine.Texture2D AreaTex;

		public global::UnityEngine.Texture2D SearchTex;

		protected global::UnityEngine.Camera m_Camera;

		protected global::Smaa.Preset m_LowPreset;

		protected global::Smaa.Preset m_MediumPreset;

		protected global::Smaa.Preset m_HighPreset;

		protected global::Smaa.Preset m_UltraPreset;

		protected global::UnityEngine.Material m_Material;
	}
}
