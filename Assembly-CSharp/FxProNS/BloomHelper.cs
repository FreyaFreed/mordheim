using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FxProNS
{
	public class BloomHelper : global::FxProNS.Singleton<global::FxProNS.BloomHelper>, global::System.IDisposable
	{
		public static global::UnityEngine.Material Mat
		{
			get
			{
				if (null == global::FxProNS.BloomHelper._mat)
				{
					global::FxProNS.BloomHelper._mat = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/BloomPro"))
					{
						hideFlags = global::UnityEngine.HideFlags.HideAndDontSave
					};
				}
				return global::FxProNS.BloomHelper._mat;
			}
		}

		public void SetParams(global::FxProNS.BloomHelperParams _p)
		{
			this.p = _p;
		}

		public void Init()
		{
			float value = global::UnityEngine.Mathf.Exp(this.p.BloomIntensity) - 1f;
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.IPhonePlayer)
			{
				this.p.BloomThreshold *= 0.75f;
			}
			global::FxProNS.BloomHelper.Mat.SetFloat("_BloomThreshold", this.p.BloomThreshold);
			global::FxProNS.BloomHelper.Mat.SetFloat("_BloomIntensity", value);
			global::FxProNS.BloomHelper.Mat.SetColor("_BloomTint", this.p.BloomTint);
			if (this.p.Quality == global::FxProNS.EffectsQuality.High || this.p.Quality == global::FxProNS.EffectsQuality.Normal)
			{
				this.bloomSamples = 5;
				global::FxProNS.BloomHelper.Mat.EnableKeyword("BLOOM_SAMPLES_5");
				global::FxProNS.BloomHelper.Mat.DisableKeyword("BLOOM_SAMPLES_3");
			}
			if (this.p.Quality == global::FxProNS.EffectsQuality.Fast || this.p.Quality == global::FxProNS.EffectsQuality.Fastest)
			{
				this.bloomSamples = 3;
				global::FxProNS.BloomHelper.Mat.EnableKeyword("BLOOM_SAMPLES_3");
				global::FxProNS.BloomHelper.Mat.DisableKeyword("BLOOM_SAMPLES_5");
			}
			if (this.p.Quality == global::FxProNS.EffectsQuality.High)
			{
				this.bloomBlurRadius = 10f;
				global::FxProNS.BloomHelper.Mat.EnableKeyword("BLUR_RADIUS_10");
				global::FxProNS.BloomHelper.Mat.DisableKeyword("BLUR_RADIUS_5");
			}
			else
			{
				this.bloomBlurRadius = 5f;
				global::FxProNS.BloomHelper.Mat.EnableKeyword("BLUR_RADIUS_5");
				global::FxProNS.BloomHelper.Mat.DisableKeyword("BLUR_RADIUS_10");
			}
			float[] array = this.CalculateBloomTexFactors(global::UnityEngine.Mathf.Exp(this.p.BloomSoftness) - 1f);
			if (array.Length == 5)
			{
				global::FxProNS.BloomHelper.Mat.SetVector("_BloomTexFactors1", new global::UnityEngine.Vector4(array[0], array[1], array[2], array[3]));
				global::FxProNS.BloomHelper.Mat.SetVector("_BloomTexFactors2", new global::UnityEngine.Vector4(array[4], 0f, 0f, 0f));
			}
			else if (array.Length == 3)
			{
				global::FxProNS.BloomHelper.Mat.SetVector("_BloomTexFactors1", new global::UnityEngine.Vector4(array[0], array[1], array[2], 0f));
			}
			else
			{
				global::UnityEngine.Debug.LogError("Unsupported bloomTexFactors.Length: " + array.Length);
			}
			global::FxProNS.RenderTextureManager.Instance.Dispose();
		}

		public void RenderBloomTexture(global::UnityEngine.RenderTexture source, global::UnityEngine.RenderTexture dest)
		{
			global::UnityEngine.RenderTexture renderTexture = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(source.width, source.height, source.depth, source.format);
			global::UnityEngine.Graphics.Blit(source, renderTexture, global::FxProNS.BloomHelper.Mat, 0);
			for (int i = 1; i <= this.bloomSamples; i++)
			{
				float spread = global::UnityEngine.Mathf.Lerp(1f, 2f, (float)(i - 1) / (float)this.bloomSamples);
				global::FxProNS.RenderTextureManager.Instance.SafeAssign(ref renderTexture, global::FxPro.DownsampleTex(renderTexture, 2f));
				global::FxProNS.RenderTextureManager.Instance.SafeAssign(ref renderTexture, this.BlurTex(renderTexture, spread));
				global::FxProNS.BloomHelper.Mat.SetTexture("_DsTex" + i, renderTexture);
			}
			global::UnityEngine.Graphics.Blit(null, dest, global::FxProNS.BloomHelper.Mat, 1);
			global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
		}

		public global::UnityEngine.RenderTexture BlurTex(global::UnityEngine.RenderTexture _input, float _spread)
		{
			float d = _spread * 10f / this.bloomBlurRadius;
			global::UnityEngine.RenderTexture renderTexture = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(_input.width, _input.height, _input.depth, _input.format);
			global::UnityEngine.RenderTexture renderTexture2 = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(_input.width, _input.height, _input.depth, _input.format);
			global::FxProNS.BloomHelper.Mat.SetVector("_SeparableBlurOffsets", new global::UnityEngine.Vector4(1f, 0f, 0f, 0f) * d);
			global::UnityEngine.Graphics.Blit(_input, renderTexture, global::FxProNS.BloomHelper.Mat, 2);
			global::FxProNS.BloomHelper.Mat.SetVector("_SeparableBlurOffsets", new global::UnityEngine.Vector4(0f, 1f, 0f, 0f) * d);
			global::UnityEngine.Graphics.Blit(renderTexture, renderTexture2, global::FxProNS.BloomHelper.Mat, 2);
			renderTexture = global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
			return renderTexture2;
		}

		private float[] CalculateBloomTexFactors(float softness)
		{
			float[] array = new float[this.bloomSamples];
			for (int i = 0; i < array.Length; i++)
			{
				float t = (float)i / (float)(array.Length - 1);
				array[i] = global::UnityEngine.Mathf.Lerp(1f, softness, t);
			}
			return this.MakeSumOne(array);
		}

		private float[] MakeSumOne(global::System.Collections.Generic.IList<float> _in)
		{
			float num = _in.Sum();
			float[] array = new float[_in.Count];
			for (int i = 0; i < _in.Count; i++)
			{
				array[i] = _in[i] / num;
			}
			return array;
		}

		public void Dispose()
		{
			if (null != global::FxProNS.BloomHelper.Mat)
			{
				global::UnityEngine.Object.DestroyImmediate(global::FxProNS.BloomHelper.Mat);
			}
			global::FxProNS.RenderTextureManager.Instance.Dispose();
		}

		private static global::UnityEngine.Material _mat;

		private global::FxProNS.BloomHelperParams p;

		private int bloomSamples = 5;

		private float bloomBlurRadius = 5f;
	}
}
