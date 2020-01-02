using System;
using System.Collections.Generic;
using UnityEngine;

namespace FxProNS
{
	public class DOFHelper : global::FxProNS.Singleton<global::FxProNS.DOFHelper>, global::System.IDisposable
	{
		public static global::UnityEngine.Material Mat
		{
			get
			{
				if (null == global::FxProNS.DOFHelper._mat)
				{
					global::FxProNS.DOFHelper._mat = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/DOFPro"))
					{
						hideFlags = global::UnityEngine.HideFlags.HideAndDontSave
					};
				}
				return global::FxProNS.DOFHelper._mat;
			}
		}

		public void SetParams(global::FxProNS.DOFHelperParams p)
		{
			this._p = p;
		}

		public void Init(bool searchForNonDepthmapAlphaObjects)
		{
			if (this._p == null)
			{
				global::UnityEngine.Debug.LogError("Call SetParams first");
				return;
			}
			if (null == this._p.EffectCamera)
			{
				global::UnityEngine.Debug.LogError("null == p.camera");
				return;
			}
			if (null == global::FxProNS.DOFHelper.Mat)
			{
				return;
			}
			if (!this._p.UseUnityDepthBuffer)
			{
				this._p.EffectCamera.depthTextureMode = global::UnityEngine.DepthTextureMode.None;
				global::FxProNS.DOFHelper.Mat.DisableKeyword("USE_CAMERA_DEPTH_TEXTURE");
				global::FxProNS.DOFHelper.Mat.EnableKeyword("DONT_USE_CAMERA_DEPTH_TEXTURE");
			}
			else
			{
				if (this._p.EffectCamera.depthTextureMode != global::UnityEngine.DepthTextureMode.DepthNormals)
				{
					this._p.EffectCamera.depthTextureMode = global::UnityEngine.DepthTextureMode.Depth;
				}
				global::FxProNS.DOFHelper.Mat.EnableKeyword("USE_CAMERA_DEPTH_TEXTURE");
				global::FxProNS.DOFHelper.Mat.DisableKeyword("DONT_USE_CAMERA_DEPTH_TEXTURE");
			}
			this._p.FocalLengthMultiplier = global::UnityEngine.Mathf.Clamp(this._p.FocalLengthMultiplier, 0.01f, 0.99f);
			this._p.DepthCompression = global::UnityEngine.Mathf.Clamp(this._p.DepthCompression, 1f, 10f);
			global::UnityEngine.Shader.SetGlobalFloat("_OneOverDepthScale", this._p.DepthCompression);
			global::UnityEngine.Shader.SetGlobalFloat("_OneOverDepthFar", 1f / this._p.EffectCamera.farClipPlane);
			if (this._p.BokehEnabled)
			{
				global::FxProNS.DOFHelper.Mat.SetFloat("_BokehThreshold", this._p.BokehThreshold);
				global::FxProNS.DOFHelper.Mat.SetFloat("_BokehGain", this._p.BokehGain);
				global::FxProNS.DOFHelper.Mat.SetFloat("_BokehBias", this._p.BokehBias);
			}
		}

		public void SetBlurRadius(int radius)
		{
			global::UnityEngine.Shader.DisableKeyword("BLUR_RADIUS_10");
			global::UnityEngine.Shader.DisableKeyword("BLUR_RADIUS_5");
			global::UnityEngine.Shader.DisableKeyword("BLUR_RADIUS_3");
			global::UnityEngine.Shader.DisableKeyword("BLUR_RADIUS_2");
			global::UnityEngine.Shader.DisableKeyword("BLUR_RADIUS_1");
			if (radius != 10 && radius != 5 && radius != 3 && radius != 2 && radius != 1)
			{
				radius = 5;
			}
			if (radius < 3)
			{
				radius = 3;
			}
			global::UnityEngine.Shader.EnableKeyword("BLUR_RADIUS_" + radius);
		}

		private void CalculateAndUpdateFocalDist()
		{
			if (null == this._p.EffectCamera)
			{
				global::UnityEngine.Debug.LogError("null == p.camera");
				return;
			}
			float num;
			if (!this._p.AutoFocus && null != this._p.Target)
			{
				num = this._p.EffectCamera.WorldToViewportPoint(this._p.Target.position).z;
			}
			else
			{
				num = (this._curAutoFocusDist = global::UnityEngine.Mathf.Lerp(this._curAutoFocusDist, this.CalculateAutoFocusDist(), global::UnityEngine.Time.deltaTime * this._p.AutoFocusSpeed));
			}
			num /= this._p.EffectCamera.farClipPlane;
			num *= this._p.FocalDistMultiplier * this._p.DepthCompression;
			global::FxProNS.DOFHelper.Mat.SetFloat("_FocalDist", num);
			global::FxProNS.DOFHelper.Mat.SetFloat("_FocalLength", num * this._p.FocalLengthMultiplier);
		}

		private float CalculateAutoFocusDist()
		{
			if (null == this._p.EffectCamera)
			{
				return 0f;
			}
			global::UnityEngine.RaycastHit raycastHit;
			return (!global::UnityEngine.Physics.Raycast(this._p.EffectCamera.transform.position, this._p.EffectCamera.transform.forward, out raycastHit, float.PositiveInfinity, this._p.AutoFocusLayerMask.value)) ? this._p.EffectCamera.farClipPlane : raycastHit.distance;
		}

		public void RenderCOCTexture(global::UnityEngine.RenderTexture src, global::UnityEngine.RenderTexture dest, float blurScale)
		{
			this.CalculateAndUpdateFocalDist();
			if (null == this._p.EffectCamera)
			{
				global::UnityEngine.Debug.LogError("null == p.camera");
				return;
			}
			if (this._p.EffectCamera.depthTextureMode == global::UnityEngine.DepthTextureMode.None)
			{
				this._p.EffectCamera.depthTextureMode = global::UnityEngine.DepthTextureMode.Depth;
			}
			if (this._p.DOFBlurSize > 0.001f)
			{
				global::UnityEngine.RenderTexture renderTexture = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(src.width, src.height, src.depth, src.format);
				global::UnityEngine.RenderTexture renderTexture2 = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(src.width, src.height, src.depth, src.format);
				global::UnityEngine.Graphics.Blit(src, renderTexture, global::FxProNS.DOFHelper.Mat, 0);
				global::FxProNS.DOFHelper.Mat.SetVector("_SeparableBlurOffsets", new global::UnityEngine.Vector4(blurScale, 0f, 0f, 0f));
				global::UnityEngine.Graphics.Blit(renderTexture, renderTexture2, global::FxProNS.DOFHelper.Mat, 2);
				global::FxProNS.DOFHelper.Mat.SetVector("_SeparableBlurOffsets", new global::UnityEngine.Vector4(0f, blurScale, 0f, 0f));
				global::UnityEngine.Graphics.Blit(renderTexture2, dest, global::FxProNS.DOFHelper.Mat, 2);
				global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
				global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture2);
			}
			else
			{
				global::UnityEngine.Graphics.Blit(src, dest, global::FxProNS.DOFHelper.Mat, 0);
			}
		}

		public void RenderDOFBlur(global::UnityEngine.RenderTexture src, global::UnityEngine.RenderTexture dest, global::UnityEngine.RenderTexture cocTexture)
		{
			if (null == cocTexture)
			{
				global::UnityEngine.Debug.LogError("null == cocTexture");
				return;
			}
			global::FxProNS.DOFHelper.Mat.SetTexture("_COCTex", cocTexture);
			if (this._p.BokehEnabled)
			{
				global::FxProNS.DOFHelper.Mat.SetFloat("_BlurIntensity", this._p.DOFBlurSize);
				global::UnityEngine.Graphics.Blit(src, dest, global::FxProNS.DOFHelper.Mat, 4);
			}
			else
			{
				global::UnityEngine.RenderTexture renderTexture = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(src.width, src.height, src.depth, src.format);
				global::FxProNS.DOFHelper.Mat.SetVector("_SeparableBlurOffsets", new global::UnityEngine.Vector4(this._p.DOFBlurSize, 0f, 0f, 0f));
				global::UnityEngine.Graphics.Blit(src, renderTexture, global::FxProNS.DOFHelper.Mat, 1);
				global::FxProNS.DOFHelper.Mat.SetVector("_SeparableBlurOffsets", new global::UnityEngine.Vector4(0f, this._p.DOFBlurSize, 0f, 0f));
				global::UnityEngine.Graphics.Blit(renderTexture, dest, global::FxProNS.DOFHelper.Mat, 1);
				global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
			}
		}

		public void RenderEffect(global::UnityEngine.RenderTexture src, global::UnityEngine.RenderTexture dest)
		{
			this.RenderEffect(src, dest, false);
		}

		public void RenderEffect(global::UnityEngine.RenderTexture src, global::UnityEngine.RenderTexture dest, bool visualizeCOC)
		{
			global::UnityEngine.RenderTexture renderTexture = global::FxProNS.RenderTextureManager.Instance.RequestRenderTexture(src.width, src.height, src.depth, src.format);
			this.RenderCOCTexture(src, renderTexture, 0f);
			if (visualizeCOC)
			{
				global::UnityEngine.Graphics.Blit(renderTexture, dest);
				global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
				global::FxProNS.RenderTextureManager.Instance.ReleaseAllRenderTextures();
				return;
			}
			this.RenderDOFBlur(src, dest, renderTexture);
			global::FxProNS.RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
		}

		public static global::UnityEngine.GameObject[] GetNonDepthmapAlphaObjects()
		{
			if (!global::UnityEngine.Application.isPlaying)
			{
				return new global::UnityEngine.GameObject[0];
			}
			global::UnityEngine.Renderer[] array = global::UnityEngine.Object.FindObjectsOfType<global::UnityEngine.Renderer>();
			global::System.Collections.Generic.List<global::UnityEngine.GameObject> list = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
			global::System.Collections.Generic.List<global::UnityEngine.Material> list2 = new global::System.Collections.Generic.List<global::UnityEngine.Material>();
			foreach (global::UnityEngine.Renderer renderer in array)
			{
				if (renderer.sharedMaterials != null)
				{
					if (!(null != renderer.GetComponent<global::UnityEngine.ParticleSystem>()))
					{
						foreach (global::UnityEngine.Material material in renderer.sharedMaterials)
						{
							if (!(null == material.shader))
							{
								bool flag = null == material.GetTag("RenderType", false);
								if (flag || (!(material.GetTag("RenderType", false).ToLower() == "transparent") && !(material.GetTag("Queue", false).ToLower() == "transparent")))
								{
									if (material.GetTag("OUTPUT_DEPTH_TO_ALPHA", false) == null || material.GetTag("OUTPUT_DEPTH_TO_ALPHA", false).ToLower() != "true")
									{
										flag = true;
									}
									if (flag)
									{
										if (!list2.Contains(material))
										{
											list2.Add(material);
											global::UnityEngine.Debug.Log("Non-depthmapped: " + global::FxProNS.DOFHelper.GetFullPath(renderer.gameObject));
											list.Add(renderer.gameObject);
										}
									}
								}
							}
						}
					}
				}
			}
			return list.ToArray();
		}

		public static string GetFullPath(global::UnityEngine.GameObject obj)
		{
			string text = "/" + obj.name;
			while (obj.transform.parent != null)
			{
				obj = obj.transform.parent.gameObject;
				text = "/" + obj.name + text;
			}
			return "'" + text + "'";
		}

		public void Dispose()
		{
			if (null != global::FxProNS.DOFHelper.Mat)
			{
				global::UnityEngine.Object.DestroyImmediate(global::FxProNS.DOFHelper.Mat);
			}
			global::FxProNS.RenderTextureManager.Instance.Dispose();
		}

		private static global::UnityEngine.Material _mat;

		private global::FxProNS.DOFHelperParams _p;

		private float _curAutoFocusDist;
	}
}
