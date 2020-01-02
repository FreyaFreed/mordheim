using System;
using System.Collections.Generic;
using UnityEngine;

namespace mset
{
	[global::UnityEngine.ExecuteInEditMode]
	public class SkyManager : global::UnityEngine.MonoBehaviour
	{
		public static global::mset.SkyManager Get()
		{
			if (global::mset.SkyManager._Instance == null)
			{
				global::mset.SkyManager._Instance = global::UnityEngine.Object.FindObjectOfType<global::mset.SkyManager>();
			}
			return global::mset.SkyManager._Instance;
		}

		public bool BlendingSupport
		{
			get
			{
				return this._BlendingSupport;
			}
			set
			{
				this._BlendingSupport = value;
				global::mset.Sky.EnableBlendingSupport(value);
				if (!value)
				{
					global::mset.Sky.EnableTerrainBlending(false);
				}
			}
		}

		public bool ProjectionSupport
		{
			get
			{
				return this._ProjectionSupport;
			}
			set
			{
				this._ProjectionSupport = value;
				global::mset.Sky.EnableProjectionSupport(value);
			}
		}

		public global::mset.Sky GlobalSky
		{
			get
			{
				return this._GlobalSky;
			}
			set
			{
				this.BlendToGlobalSky(value, 0f);
			}
		}

		public void BlendToGlobalSky(global::mset.Sky next)
		{
			this.BlendToGlobalSky(next, this.GlobalBlendTime, 0f);
		}

		public void BlendToGlobalSky(global::mset.Sky next, float blendTime)
		{
			this.BlendToGlobalSky(next, blendTime, 0f);
		}

		public void BlendToGlobalSky(global::mset.Sky next, float blendTime, float skipTime)
		{
			if (next != null)
			{
				this.nextSky = next;
				this.nextBlendTime = blendTime;
				this.nextSkipTime = skipTime;
			}
			this._GlobalSky = this.nextSky;
		}

		private void ResetLightBlend()
		{
			if (this.nextLights != null)
			{
				for (int i = 0; i < this.nextLights.Length; i++)
				{
					this.nextLights[i].intensity = this.nextIntensities[i];
					this.nextLights[i].enabled = true;
				}
				this.nextLights = null;
				this.nextIntensities = null;
			}
			if (this.prevLights != null)
			{
				for (int j = 0; j < this.prevLights.Length; j++)
				{
					this.prevLights[j].intensity = this.prevIntensities[j];
					this.prevLights[j].enabled = false;
				}
				this.prevLights = null;
				this.prevIntensities = null;
			}
		}

		private void StartLightBlend(global::mset.Sky prev, global::mset.Sky next)
		{
			this.prevLights = null;
			this.prevIntensities = null;
			if (prev)
			{
				this.prevLights = prev.GetComponentsInChildren<global::UnityEngine.Light>();
				if (this.prevLights != null && this.prevLights.Length > 0)
				{
					this.prevIntensities = new float[this.prevLights.Length];
					for (int i = 0; i < this.prevLights.Length; i++)
					{
						this.prevLights[i].enabled = true;
						this.prevIntensities[i] = this.prevLights[i].intensity;
					}
				}
			}
			this.nextLights = null;
			this.nextIntensities = null;
			if (next)
			{
				this.nextLights = next.GetComponentsInChildren<global::UnityEngine.Light>();
				if (this.nextLights != null && this.nextLights.Length > 0)
				{
					this.nextIntensities = new float[this.nextLights.Length];
					for (int j = 0; j < this.nextLights.Length; j++)
					{
						this.nextIntensities[j] = this.nextLights[j].intensity;
						this.nextLights[j].enabled = true;
						this.nextLights[j].intensity = 0f;
					}
				}
			}
		}

		private void UpdateLightBlend()
		{
			if (this.GlobalBlender.IsBlending)
			{
				float blendWeight = this.GlobalBlender.BlendWeight;
				float num = 1f - blendWeight;
				for (int i = 0; i < this.prevLights.Length; i++)
				{
					this.prevLights[i].intensity = num * this.prevIntensities[i];
				}
				for (int j = 0; j < this.nextLights.Length; j++)
				{
					this.nextLights[j].intensity = blendWeight * this.nextIntensities[j];
				}
			}
			else
			{
				this.ResetLightBlend();
			}
		}

		private void HandleGlobalSkyChange()
		{
			if (this.nextSky != null)
			{
				this.ResetLightBlend();
				if (this.BlendingSupport && this.nextBlendTime > 0f)
				{
					global::mset.Sky currentSky = this.GlobalBlender.CurrentSky;
					this.GlobalBlender.BlendTime = this.nextBlendTime;
					this.GlobalBlender.BlendToSky(this.nextSky);
					global::mset.Sky[] array = global::UnityEngine.Object.FindObjectsOfType<global::mset.Sky>();
					foreach (global::mset.Sky sky in array)
					{
						sky.ToggleChildLights(false);
					}
					this.GlobalBlender.SkipTime(this.nextSkipTime);
					this.StartLightBlend(currentSky, this.nextSky);
				}
				else
				{
					this.GlobalBlender.SnapToSky(this.nextSky);
					this.nextSky.Apply(0);
					this.nextSky.Apply(1);
					global::mset.Sky[] array3 = global::UnityEngine.Object.FindObjectsOfType<global::mset.Sky>();
					foreach (global::mset.Sky sky2 in array3)
					{
						sky2.ToggleChildLights(false);
					}
					this.nextSky.ToggleChildLights(true);
				}
				this._GlobalSky = this.nextSky;
				this.nextSky = null;
				if (!global::UnityEngine.Application.isPlaying)
				{
					this.EditorApplySkies(true);
				}
			}
			this.UpdateLightBlend();
		}

		private global::UnityEngine.Material SkyboxMaterial
		{
			get
			{
				if (this._SkyboxMaterial == null)
				{
					this._SkyboxMaterial = global::UnityEngine.Resources.Load<global::UnityEngine.Material>("skyboxMat");
					if (!this._SkyboxMaterial)
					{
						global::UnityEngine.Debug.LogError("Failed to find skyboxMat material in Resources folder!");
					}
				}
				return this._SkyboxMaterial;
			}
		}

		public bool ShowSkybox
		{
			get
			{
				return this._ShowSkybox;
			}
			set
			{
				if (value)
				{
					if (this.SkyboxMaterial && global::UnityEngine.RenderSettings.skybox != this.SkyboxMaterial)
					{
						global::UnityEngine.RenderSettings.skybox = this.SkyboxMaterial;
					}
				}
				else if (global::UnityEngine.RenderSettings.skybox != null && (global::UnityEngine.RenderSettings.skybox == this._SkyboxMaterial || global::UnityEngine.RenderSettings.skybox.name == "Internal IBL Skybox"))
				{
					global::UnityEngine.RenderSettings.skybox = null;
				}
				this._ShowSkybox = value;
			}
		}

		private void Start()
		{
			global::mset.Sky.ScrubGlobalKeywords();
			this._SkyboxMaterial = this.SkyboxMaterial;
			this.ShowSkybox = this._ShowSkybox;
			this.BlendingSupport = this._BlendingSupport;
			this.ProjectionSupport = this._ProjectionSupport;
			if (this._GlobalSky == null)
			{
				this._GlobalSky = base.gameObject.GetComponent<global::mset.Sky>();
			}
			if (this._GlobalSky == null)
			{
				this._GlobalSky = global::UnityEngine.Object.FindObjectOfType<global::mset.Sky>();
			}
			this.GlobalBlender.SnapToSky(this._GlobalSky);
		}

		public void RegisterApplicator(global::mset.SkyApplicator app)
		{
			this.skyApplicators.Add(app);
			foreach (global::UnityEngine.Renderer rend in this.dynamicRenderers)
			{
				app.RendererInside(rend);
			}
			foreach (global::UnityEngine.Renderer rend2 in this.staticRenderers)
			{
				app.RendererInside(rend2);
			}
		}

		public void UnregisterApplicator(global::mset.SkyApplicator app, global::System.Collections.Generic.HashSet<global::UnityEngine.Renderer> renderersToClear)
		{
			this.skyApplicators.Remove(app);
			foreach (global::UnityEngine.Renderer target in renderersToClear)
			{
				if (this._GlobalSky != null)
				{
					this._GlobalSky.Apply(target, 0);
				}
			}
		}

		public void UnregisterRenderer(global::UnityEngine.Renderer rend)
		{
			if (!this.dynamicRenderers.Remove(rend))
			{
				this.staticRenderers.Remove(rend);
			}
		}

		public void RegisterNewRenderer(global::UnityEngine.Renderer rend)
		{
			if (!rend.gameObject.activeInHierarchy)
			{
				return;
			}
			int num = 1 << rend.gameObject.layer;
			if ((this.IgnoredLayerMask & num) != 0)
			{
				return;
			}
			if (rend.gameObject.isStatic)
			{
				if (!this.staticRenderers.Contains(rend))
				{
					this.staticRenderers.Add(rend);
					this.ApplyCorrectSky(rend);
				}
			}
			else if (!this.dynamicRenderers.Contains(rend))
			{
				this.dynamicRenderers.Add(rend);
				if (rend.GetComponent<global::mset.SkyAnchor>() == null)
				{
					rend.gameObject.AddComponent(typeof(global::mset.SkyAnchor));
				}
			}
		}

		public void SeekNewRenderers()
		{
			global::UnityEngine.Renderer[] array = global::UnityEngine.Object.FindObjectsOfType<global::UnityEngine.MeshRenderer>();
			for (int i = 0; i < array.Length; i++)
			{
				this.RegisterNewRenderer(array[i]);
			}
			array = global::UnityEngine.Object.FindObjectsOfType<global::UnityEngine.SkinnedMeshRenderer>();
			for (int j = 0; j < array.Length; j++)
			{
				this.RegisterNewRenderer(array[j]);
			}
		}

		public void ApplyCorrectSky(global::UnityEngine.Renderer rend)
		{
			bool flag = false;
			global::mset.SkyAnchor component = rend.GetComponent<global::mset.SkyAnchor>();
			if (component && component.BindType == global::mset.SkyAnchor.AnchorBindType.TargetSky)
			{
				component.Apply();
				flag = true;
			}
			foreach (global::mset.SkyApplicator skyApplicator in this.skyApplicators)
			{
				if (flag)
				{
					skyApplicator.RemoveRenderer(rend);
				}
				else if (skyApplicator.RendererInside(rend))
				{
					flag = true;
				}
			}
			if (!flag && this._GlobalSky != null)
			{
				if (component != null)
				{
					if (component.CurrentApplicator != null)
					{
						component.CurrentApplicator.RemoveRenderer(rend);
						component.CurrentApplicator = null;
					}
					component.BlendToGlobalSky(this._GlobalSky);
				}
				if (!this.globalSkyChildren.Contains(rend))
				{
					this.globalSkyChildren.Add(rend);
				}
			}
			if ((flag || this._GlobalSky == null) && this.globalSkyChildren.Contains(rend))
			{
				this.globalSkyChildren.Remove(rend);
			}
		}

		public void EditorUpdate(bool forceApply)
		{
			global::mset.Sky.EnableGlobalProjection(true);
			global::mset.Sky.EnableBlendingSupport(false);
			global::mset.Sky.EnableTerrainBlending(false);
			if (this._GlobalSky)
			{
				this._GlobalSky.Apply(0);
				this._GlobalSky.Apply(1);
				if (this.SkyboxMaterial)
				{
					this._GlobalSky.Apply(this.SkyboxMaterial, 0);
					this._GlobalSky.Apply(this.SkyboxMaterial, 1);
				}
				this._GlobalSky.Dirty = false;
			}
			this.HandleGlobalSkyChange();
			if (this.EditorAutoApply)
			{
				this.EditorApplySkies(forceApply);
			}
		}

		private void EditorApplySkies(bool forceApply)
		{
			global::UnityEngine.Shader.SetGlobalVector("_UniformOcclusion", global::UnityEngine.Vector4.one);
			global::mset.SkyApplicator[] apps = global::UnityEngine.Object.FindObjectsOfType<global::mset.SkyApplicator>();
			object[] renderers = global::UnityEngine.Object.FindObjectsOfType<global::UnityEngine.MeshRenderer>();
			this.EditorApplyToList(renderers, apps, forceApply);
			renderers = global::UnityEngine.Object.FindObjectsOfType<global::UnityEngine.SkinnedMeshRenderer>();
			this.EditorApplyToList(renderers, apps, forceApply);
		}

		private void EditorApplyToList(object[] renderers, global::mset.SkyApplicator[] apps, bool forceApply)
		{
			foreach (object obj in renderers)
			{
				global::UnityEngine.Renderer renderer = (global::UnityEngine.Renderer)obj;
				int num = 1 << renderer.gameObject.layer;
				if ((this.IgnoredLayerMask & num) == 0)
				{
					if (renderer.gameObject.activeInHierarchy)
					{
						if (forceApply)
						{
							global::UnityEngine.MaterialPropertyBlock materialPropertyBlock = new global::UnityEngine.MaterialPropertyBlock();
							materialPropertyBlock.Clear();
							renderer.SetPropertyBlock(materialPropertyBlock);
						}
						global::mset.SkyAnchor skyAnchor = renderer.gameObject.GetComponent<global::mset.SkyAnchor>();
						if (skyAnchor && !skyAnchor.enabled)
						{
							skyAnchor = null;
						}
						bool flag = renderer.transform.hasChanged || (skyAnchor && skyAnchor.HasChanged);
						bool flag2 = false;
						if (skyAnchor && skyAnchor.BindType == global::mset.SkyAnchor.AnchorBindType.TargetSky)
						{
							skyAnchor.Apply();
							flag2 = true;
						}
						if (this.GameAutoApply && !flag2)
						{
							foreach (global::mset.SkyApplicator skyApplicator in apps)
							{
								if (skyApplicator.gameObject.activeInHierarchy)
								{
									if (skyApplicator.TargetSky && (forceApply || skyApplicator.HasChanged || skyApplicator.TargetSky.Dirty || flag))
									{
										flag2 |= skyApplicator.ApplyInside(renderer);
										skyApplicator.TargetSky.Dirty = false;
									}
									skyApplicator.HasChanged = false;
								}
							}
						}
						if (!flag2 && this._GlobalSky && (forceApply || this._GlobalSky.Dirty || flag))
						{
							this._GlobalSky.Apply(renderer, 0);
						}
						renderer.transform.hasChanged = false;
						if (skyAnchor)
						{
							skyAnchor.HasChanged = false;
						}
					}
				}
			}
			if (forceApply && this._GlobalSky)
			{
				this._GlobalSky.Apply(0);
				if (this._SkyboxMaterial)
				{
					this._GlobalSky.Apply(this._SkyboxMaterial, 0);
				}
				this._GlobalSky.Dirty = false;
			}
		}

		public void LateUpdate()
		{
			if (this.firstFrame && this._GlobalSky)
			{
				this.firstFrame = false;
				this._GlobalSky.Apply(0);
				this._GlobalSky.Apply(1);
				if (this._SkyboxMaterial)
				{
					this._GlobalSky.Apply(this._SkyboxMaterial, 0);
					this._GlobalSky.Apply(this._SkyboxMaterial, 1);
				}
			}
			float num = 0f;
			if (this.lastTimestamp > 0f)
			{
				num = global::UnityEngine.Time.realtimeSinceStartup - this.lastTimestamp;
			}
			this.lastTimestamp = global::UnityEngine.Time.realtimeSinceStartup;
			this.seekTimer -= num;
			this.HandleGlobalSkyChange();
			this.GameApplySkies(false);
		}

		public void GameApplySkies(bool forceApply)
		{
			this.GlobalBlender.ApplyToTerrain();
			this.GlobalBlender.Apply();
			if (this._SkyboxMaterial)
			{
				this.GlobalBlender.Apply(this._SkyboxMaterial);
			}
			if (this.GameAutoApply || forceApply)
			{
				if (this.seekTimer <= 0f || forceApply)
				{
					this.SeekNewRenderers();
					this.seekTimer = 0.5f;
				}
				global::System.Collections.Generic.List<global::mset.SkyApplicator> list = new global::System.Collections.Generic.List<global::mset.SkyApplicator>();
				foreach (global::mset.SkyApplicator skyApplicator in this.skyApplicators)
				{
					if (skyApplicator == null || skyApplicator.gameObject == null)
					{
						list.Add(skyApplicator);
					}
				}
				foreach (global::mset.SkyApplicator item in list)
				{
					this.skyApplicators.Remove(item);
				}
				if (this.GlobalBlender.IsBlending || this.GlobalBlender.CurrentSky.Dirty || this.GlobalBlender.WasBlending(global::UnityEngine.Time.deltaTime))
				{
					foreach (global::UnityEngine.Renderer renderer in this.globalSkyChildren)
					{
						if (renderer)
						{
							global::mset.SkyAnchor component = renderer.GetComponent<global::mset.SkyAnchor>();
							if (component != null)
							{
								this.GlobalBlender.Apply(renderer, component.materials);
							}
						}
					}
				}
				int num = 0;
				int num2 = 0;
				global::System.Collections.Generic.List<global::UnityEngine.Renderer> list2 = new global::System.Collections.Generic.List<global::UnityEngine.Renderer>();
				foreach (global::UnityEngine.Renderer renderer2 in this.dynamicRenderers)
				{
					num2++;
					if (forceApply || num2 >= this.renderCheckIterator)
					{
						if (renderer2 == null || renderer2.gameObject == null)
						{
							list2.Add(renderer2);
						}
						else if (renderer2.gameObject.activeInHierarchy)
						{
							this.renderCheckIterator++;
							if (!forceApply && num > 50)
							{
								this.renderCheckIterator--;
								break;
							}
							global::mset.SkyAnchor component2 = renderer2.GetComponent<global::mset.SkyAnchor>();
							if (component2.HasChanged)
							{
								num++;
								component2.HasChanged = false;
								if (this.AutoMaterial)
								{
									component2.UpdateMaterials();
								}
								this.ApplyCorrectSky(renderer2);
							}
						}
					}
				}
				foreach (global::UnityEngine.Renderer item2 in list2)
				{
					this.dynamicRenderers.Remove(item2);
				}
				if (this.renderCheckIterator >= this.dynamicRenderers.Count)
				{
					this.renderCheckIterator = 0;
				}
			}
			this._GlobalSky.Dirty = false;
		}

		public void ProbeSkies(global::UnityEngine.GameObject[] objects, global::mset.Sky[] skies, bool probeAll, bool probeIBL)
		{
			int num = 0;
			global::System.Collections.Generic.List<global::mset.Sky> list = new global::System.Collections.Generic.List<global::mset.Sky>();
			string str = string.Empty;
			if (skies != null)
			{
				foreach (global::mset.Sky sky in skies)
				{
					if (sky)
					{
						if (probeAll || sky.IsProbe)
						{
							list.Add(sky);
						}
						else
						{
							num++;
							str = str + sky.name + "\n";
						}
					}
				}
			}
			if (objects != null)
			{
				foreach (global::UnityEngine.GameObject gameObject in objects)
				{
					global::mset.Sky component = gameObject.GetComponent<global::mset.Sky>();
					if (component)
					{
						if (probeAll || component.IsProbe)
						{
							list.Add(component);
						}
						else
						{
							num++;
							str = str + component.name + "\n";
						}
					}
				}
			}
			if (num > 0)
			{
			}
			if (list.Count > 0)
			{
				this.ProbeExposures = ((!probeIBL) ? global::UnityEngine.Vector4.zero : global::UnityEngine.Vector4.one);
				this.SkiesToProbe = new global::mset.Sky[list.Count];
				for (int k = 0; k < list.Count; k++)
				{
					this.SkiesToProbe[k] = list[k];
				}
			}
		}

		private static global::mset.SkyManager _Instance;

		public bool LinearSpace = true;

		[global::UnityEngine.SerializeField]
		private bool _BlendingSupport = true;

		[global::UnityEngine.SerializeField]
		private bool _ProjectionSupport = true;

		public bool GameAutoApply = true;

		public bool EditorAutoApply = true;

		public bool AutoMaterial;

		public int IgnoredLayerMask;

		public int[] _IgnoredLayers;

		public int _IgnoredLayerCount;

		[global::UnityEngine.SerializeField]
		private global::mset.Sky _GlobalSky;

		[global::UnityEngine.SerializeField]
		private global::mset.SkyBlender GlobalBlender = new global::mset.SkyBlender();

		private global::mset.Sky nextSky;

		private float nextBlendTime;

		private float nextSkipTime;

		public float LocalBlendTime = 0.25f;

		public float GlobalBlendTime = 0.25f;

		private global::UnityEngine.Light[] prevLights;

		private global::UnityEngine.Light[] nextLights;

		private float[] prevIntensities;

		private float[] nextIntensities;

		private global::UnityEngine.Material _SkyboxMaterial;

		[global::UnityEngine.SerializeField]
		private bool _ShowSkybox = true;

		public global::UnityEngine.Camera ProbeCamera;

		private global::System.Collections.Generic.HashSet<global::UnityEngine.Renderer> staticRenderers = new global::System.Collections.Generic.HashSet<global::UnityEngine.Renderer>();

		private global::System.Collections.Generic.HashSet<global::UnityEngine.Renderer> dynamicRenderers = new global::System.Collections.Generic.HashSet<global::UnityEngine.Renderer>();

		private global::System.Collections.Generic.HashSet<global::UnityEngine.Renderer> globalSkyChildren = new global::System.Collections.Generic.HashSet<global::UnityEngine.Renderer>();

		private global::System.Collections.Generic.HashSet<global::mset.SkyApplicator> skyApplicators = new global::System.Collections.Generic.HashSet<global::mset.SkyApplicator>();

		private float seekTimer;

		private float lastTimestamp = -1f;

		private int renderCheckIterator;

		private bool firstFrame = true;

		public global::mset.Sky[] SkiesToProbe;

		public int ProbeExponent = 512;

		public global::UnityEngine.Vector4 ProbeExposures = global::UnityEngine.Vector4.one;

		public bool ProbeWithCubeRT = true;

		public bool ProbeOnlyStatic;
	}
}
