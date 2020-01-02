using System;
using UnityEngine;

namespace mset
{
	public class Sky : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.Texture SpecularCube
		{
			get
			{
				return this.specularCube;
			}
			set
			{
				this.specularCube = value;
			}
		}

		public global::UnityEngine.Texture SkyboxCube
		{
			get
			{
				return this.skyboxCube;
			}
			set
			{
				this.skyboxCube = value;
			}
		}

		public global::UnityEngine.Bounds Dimensions
		{
			get
			{
				return this.dimensions;
			}
			set
			{
				this._dirty = true;
				this.dimensions = value;
			}
		}

		public bool Dirty
		{
			get
			{
				return this._dirty;
			}
			set
			{
				this._dirty = value;
			}
		}

		public float MasterIntensity
		{
			get
			{
				return this.masterIntensity;
			}
			set
			{
				this._dirty = true;
				this.masterIntensity = value;
			}
		}

		public float SkyIntensity
		{
			get
			{
				return this.skyIntensity;
			}
			set
			{
				this._dirty = true;
				this.skyIntensity = value;
			}
		}

		public float SpecIntensity
		{
			get
			{
				return this.specIntensity;
			}
			set
			{
				this._dirty = true;
				this.specIntensity = value;
			}
		}

		public float DiffIntensity
		{
			get
			{
				return this.diffIntensity;
			}
			set
			{
				this._dirty = true;
				this.diffIntensity = value;
			}
		}

		public float CamExposure
		{
			get
			{
				return this.camExposure;
			}
			set
			{
				this._dirty = true;
				this.camExposure = value;
			}
		}

		public float SpecIntensityLM
		{
			get
			{
				return this.specIntensityLM;
			}
			set
			{
				this._dirty = true;
				this.specIntensityLM = value;
			}
		}

		public float DiffIntensityLM
		{
			get
			{
				return this.diffIntensityLM;
			}
			set
			{
				this._dirty = true;
				this.diffIntensityLM = value;
			}
		}

		public bool HDRSky
		{
			get
			{
				return this.hdrSky;
			}
			set
			{
				this._dirty = true;
				this.hdrSky = value;
			}
		}

		public bool HDRSpec
		{
			get
			{
				return this.hdrSpec;
			}
			set
			{
				this._dirty = true;
				this.hdrSpec = value;
			}
		}

		public bool LinearSpace
		{
			get
			{
				return this.linearSpace;
			}
			set
			{
				this._dirty = true;
				this.linearSpace = value;
			}
		}

		public bool AutoDetectColorSpace
		{
			get
			{
				return this.autoDetectColorSpace;
			}
			set
			{
				this._dirty = true;
				this.autoDetectColorSpace = value;
			}
		}

		public bool HasDimensions
		{
			get
			{
				return this.hasDimensions;
			}
			set
			{
				this._dirty = true;
				this.hasDimensions = value;
			}
		}

		private global::UnityEngine.Cubemap blackCube
		{
			get
			{
				if (this._blackCube == null)
				{
					this._blackCube = global::UnityEngine.Resources.Load<global::UnityEngine.Cubemap>("blackCube");
				}
				return this._blackCube;
			}
		}

		private global::UnityEngine.Material SkyboxMaterial
		{
			get
			{
				if (this._SkyboxMaterial == null)
				{
					this._SkyboxMaterial = global::UnityEngine.Resources.Load<global::UnityEngine.Material>("skyboxMat");
				}
				return this._SkyboxMaterial;
			}
		}

		private static global::UnityEngine.Material[] getTargetMaterials(global::UnityEngine.Renderer target)
		{
			global::mset.SkyAnchor component = target.gameObject.GetComponent<global::mset.SkyAnchor>();
			if (component != null)
			{
				return component.materials;
			}
			return target.sharedMaterials;
		}

		public void Apply()
		{
			this.Apply(0);
		}

		public void Apply(int blendIndex)
		{
			global::mset.ShaderIDs bids = this.blendIDs[blendIndex];
			this.ApplyGlobally(bids);
		}

		public void Apply(global::UnityEngine.Renderer target)
		{
			this.Apply(target, 0);
		}

		public void Apply(global::UnityEngine.Renderer target, int blendIndex)
		{
			if (target && base.enabled && base.gameObject.activeInHierarchy)
			{
				this.ApplyFast(target, blendIndex);
			}
		}

		public void ApplyFast(global::UnityEngine.Renderer target, int blendIndex)
		{
			if (global::mset.Sky.propBlock == null)
			{
				global::mset.Sky.propBlock = new global::UnityEngine.MaterialPropertyBlock();
			}
			target.GetPropertyBlock(global::mset.Sky.propBlock);
			this.ApplyToBlock(ref global::mset.Sky.propBlock, this.blendIDs[blendIndex]);
			target.SetPropertyBlock(global::mset.Sky.propBlock);
		}

		public void Apply(global::UnityEngine.Material target)
		{
			this.Apply(target, 0);
		}

		public void Apply(global::UnityEngine.Material target, int blendIndex)
		{
			if (target && base.enabled && base.gameObject.activeInHierarchy)
			{
				this.ApplyToMaterial(target, this.blendIDs[blendIndex]);
			}
		}

		private void ApplyToBlock(ref global::UnityEngine.MaterialPropertyBlock block, global::mset.ShaderIDs bids)
		{
			block.SetVector(bids.exposureIBL, this.exposures);
			block.SetVector(bids.exposureLM, this.exposuresLM);
			block.SetMatrix(bids.skyMatrix, this.skyMatrix);
			block.SetMatrix(bids.invSkyMatrix, this.invMatrix);
			block.SetVector(bids.skyMin, this.skyMin);
			block.SetVector(bids.skyMax, this.skyMax);
			if (this.specularCube)
			{
				block.SetTexture(bids.specCubeIBL, this.specularCube);
			}
			else
			{
				block.SetTexture(bids.specCubeIBL, this.blackCube);
			}
			block.SetVector(bids.SH[0], this.SH.cBuffer[0]);
			block.SetVector(bids.SH[1], this.SH.cBuffer[1]);
			block.SetVector(bids.SH[2], this.SH.cBuffer[2]);
			block.SetVector(bids.SH[3], this.SH.cBuffer[3]);
			block.SetVector(bids.SH[4], this.SH.cBuffer[4]);
			block.SetVector(bids.SH[5], this.SH.cBuffer[5]);
			block.SetVector(bids.SH[6], this.SH.cBuffer[6]);
			block.SetVector(bids.SH[7], this.SH.cBuffer[7]);
			block.SetVector(bids.SH[8], this.SH.cBuffer[8]);
		}

		private void ApplyToMaterial(global::UnityEngine.Material mat, global::mset.ShaderIDs bids)
		{
			mat.SetVector(bids.exposureIBL, this.exposures);
			mat.SetVector(bids.exposureLM, this.exposuresLM);
			mat.SetMatrix(bids.skyMatrix, this.skyMatrix);
			mat.SetMatrix(bids.invSkyMatrix, this.invMatrix);
			mat.SetVector(bids.skyMin, this.skyMin);
			mat.SetVector(bids.skyMax, this.skyMax);
			if (this.specularCube)
			{
				mat.SetTexture(bids.specCubeIBL, this.specularCube);
			}
			else
			{
				mat.SetTexture(bids.specCubeIBL, this.blackCube);
			}
			if (this.skyboxCube)
			{
				mat.SetTexture(bids.skyCubeIBL, this.skyboxCube);
			}
			for (int i = 0; i < 9; i++)
			{
				mat.SetVector(bids.SH[i], this.SH.cBuffer[i]);
			}
		}

		private void ApplySkyTransform(global::mset.ShaderIDs bids)
		{
			global::UnityEngine.Shader.SetGlobalMatrix(bids.skyMatrix, this.skyMatrix);
			global::UnityEngine.Shader.SetGlobalMatrix(bids.invSkyMatrix, this.invMatrix);
			global::UnityEngine.Shader.SetGlobalVector(bids.skyMin, this.skyMin);
			global::UnityEngine.Shader.SetGlobalVector(bids.skyMax, this.skyMax);
		}

		private void ApplyGlobally(global::mset.ShaderIDs bids)
		{
			global::UnityEngine.Shader.SetGlobalMatrix(bids.skyMatrix, this.skyMatrix);
			global::UnityEngine.Shader.SetGlobalMatrix(bids.invSkyMatrix, this.invMatrix);
			global::UnityEngine.Shader.SetGlobalVector(bids.skyMin, this.skyMin);
			global::UnityEngine.Shader.SetGlobalVector(bids.skyMax, this.skyMax);
			global::UnityEngine.Shader.SetGlobalVector(bids.exposureIBL, this.exposures);
			global::UnityEngine.Shader.SetGlobalVector(bids.exposureLM, this.exposuresLM);
			global::UnityEngine.Shader.SetGlobalFloat("_EmissionLM", 1f);
			global::UnityEngine.Shader.SetGlobalVector("_UniformOcclusion", global::UnityEngine.Vector4.one);
			if (this.specularCube)
			{
				global::UnityEngine.Shader.SetGlobalTexture(bids.specCubeIBL, this.specularCube);
			}
			else
			{
				global::UnityEngine.Shader.SetGlobalTexture(bids.specCubeIBL, this.blackCube);
			}
			if (this.skyboxCube)
			{
				global::UnityEngine.Shader.SetGlobalTexture(bids.skyCubeIBL, this.skyboxCube);
			}
			for (int i = 0; i < 9; i++)
			{
				global::UnityEngine.Shader.SetGlobalVector(bids.SH[i], this.SH.cBuffer[i]);
			}
		}

		public static void ScrubGlobalKeywords()
		{
			global::UnityEngine.Shader.DisableKeyword("MARMO_SKY_BLEND_ON");
			global::UnityEngine.Shader.DisableKeyword("MARMO_SKY_BLEND_OFF");
			global::UnityEngine.Shader.DisableKeyword("MARMO_BOX_PROJECTION_ON");
			global::UnityEngine.Shader.DisableKeyword("MARMO_BOX_PROJECTION_OFF");
			global::UnityEngine.Shader.DisableKeyword("MARMO_TERRAIN_BLEND_ON");
			global::UnityEngine.Shader.DisableKeyword("MARMO_TERRAIN_BLEND_OFF");
		}

		public static void ScrubKeywords(global::UnityEngine.Material[] materials)
		{
			foreach (global::UnityEngine.Material material in materials)
			{
				if (material != null)
				{
					material.DisableKeyword("MARMO_SKY_BLEND_ON");
					material.DisableKeyword("MARMO_SKY_BLEND_OFF");
					material.DisableKeyword("MARMO_BOX_PROJECTION_ON");
					material.DisableKeyword("MARMO_BOX_PROJECTION_OFF");
					material.DisableKeyword("MARMO_TERRAIN_BLEND_ON");
					material.DisableKeyword("MARMO_TERRAIN_BLEND_OFF");
				}
			}
		}

		public static void EnableProjectionSupport(bool enable)
		{
			if (enable)
			{
				global::UnityEngine.Shader.DisableKeyword("MARMO_BOX_PROJECTION_OFF");
			}
			else
			{
				global::UnityEngine.Shader.EnableKeyword("MARMO_BOX_PROJECTION_OFF");
			}
			global::mset.Sky.internalProjectionSupport = enable;
		}

		public static void EnableGlobalProjection(bool enable)
		{
			if (!global::mset.Sky.internalProjectionSupport)
			{
				return;
			}
			if (enable)
			{
				global::UnityEngine.Shader.EnableKeyword("MARMO_BOX_PROJECTION_ON");
			}
			else
			{
				global::UnityEngine.Shader.DisableKeyword("MARMO_BOX_PROJECTION_ON");
			}
		}

		public static void EnableProjection(global::UnityEngine.Renderer target, global::UnityEngine.Material[] mats, bool enable)
		{
			if (!global::mset.Sky.internalProjectionSupport)
			{
				return;
			}
			if (mats == null)
			{
				return;
			}
			if (enable)
			{
				foreach (global::UnityEngine.Material material in mats)
				{
					if (material)
					{
						material.EnableKeyword("MARMO_BOX_PROJECTION_ON");
						material.DisableKeyword("MARMO_BOX_PROJECTION_OFF");
					}
				}
			}
			else
			{
				foreach (global::UnityEngine.Material material2 in mats)
				{
					if (material2)
					{
						material2.DisableKeyword("MARMO_BOX_PROJECTION_ON");
						material2.EnableKeyword("MARMO_BOX_PROJECTION_OFF");
					}
				}
			}
		}

		public static void EnableProjection(global::UnityEngine.Material mat, bool enable)
		{
			if (!global::mset.Sky.internalProjectionSupport)
			{
				return;
			}
			if (enable)
			{
				mat.EnableKeyword("MARMO_BOX_PROJECTION_ON");
				mat.DisableKeyword("MARMO_BOX_PROJECTION_OFF");
			}
			else
			{
				mat.DisableKeyword("MARMO_BOX_PROJECTION_ON");
				mat.EnableKeyword("MARMO_BOX_PROJECTION_OFF");
			}
		}

		public static void EnableBlendingSupport(bool enable)
		{
			if (enable)
			{
				global::UnityEngine.Shader.DisableKeyword("MARMO_SKY_BLEND_OFF");
			}
			else
			{
				global::UnityEngine.Shader.EnableKeyword("MARMO_SKY_BLEND_OFF");
			}
			global::mset.Sky.internalBlendingSupport = enable;
		}

		public static void EnableTerrainBlending(bool enable)
		{
			if (!global::mset.Sky.internalBlendingSupport)
			{
				return;
			}
			if (enable)
			{
				global::UnityEngine.Shader.EnableKeyword("MARMO_TERRAIN_BLEND_ON");
				global::UnityEngine.Shader.DisableKeyword("MARMO_TERRAIN_BLEND_OFF");
			}
			else
			{
				global::UnityEngine.Shader.DisableKeyword("MARMO_TERRAIN_BLEND_ON");
				global::UnityEngine.Shader.EnableKeyword("MARMO_TERRAIN_BLEND_OFF");
			}
		}

		public static void EnableGlobalBlending(bool enable)
		{
			if (!global::mset.Sky.internalBlendingSupport)
			{
				return;
			}
			if (enable)
			{
				global::UnityEngine.Shader.EnableKeyword("MARMO_SKY_BLEND_ON");
			}
			else
			{
				global::UnityEngine.Shader.DisableKeyword("MARMO_SKY_BLEND_ON");
			}
		}

		public static void EnableBlending(global::UnityEngine.Renderer target, global::UnityEngine.Material[] mats, bool enable)
		{
			if (!global::mset.Sky.internalBlendingSupport)
			{
				return;
			}
			if (mats == null)
			{
				return;
			}
			if (enable)
			{
				foreach (global::UnityEngine.Material material in mats)
				{
					if (material)
					{
						material.EnableKeyword("MARMO_SKY_BLEND_ON");
						material.DisableKeyword("MARMO_SKY_BLEND_OFF");
					}
				}
			}
			else
			{
				foreach (global::UnityEngine.Material material2 in mats)
				{
					if (material2)
					{
						material2.DisableKeyword("MARMO_SKY_BLEND_ON");
						material2.EnableKeyword("MARMO_SKY_BLEND_OFF");
					}
				}
			}
		}

		public static void EnableBlending(global::UnityEngine.Material mat, bool enable)
		{
			if (!global::mset.Sky.internalBlendingSupport)
			{
				return;
			}
			if (enable)
			{
				mat.EnableKeyword("MARMO_SKY_BLEND_ON");
				mat.DisableKeyword("MARMO_SKY_BLEND_OFF");
			}
			else
			{
				mat.DisableKeyword("MARMO_SKY_BLEND_ON");
				mat.EnableKeyword("MARMO_SKY_BLEND_OFF");
			}
		}

		public static void SetBlendWeight(float weight)
		{
			global::UnityEngine.Shader.SetGlobalFloat("_BlendWeightIBL", weight);
		}

		public static void SetBlendWeight(global::UnityEngine.Renderer target, float weight)
		{
			if (global::mset.Sky.propBlock == null)
			{
				global::mset.Sky.propBlock = new global::UnityEngine.MaterialPropertyBlock();
			}
			target.GetPropertyBlock(global::mset.Sky.propBlock);
			global::mset.Sky.propBlock.SetFloat("_BlendWeightIBL", weight);
			target.SetPropertyBlock(global::mset.Sky.propBlock);
		}

		public static void SetBlendWeight(global::UnityEngine.Material mat, float weight)
		{
			mat.SetFloat("_BlendWeightIBL", weight);
		}

		public static void SetUniformOcclusion(global::UnityEngine.Renderer target, float diffuse, float specular)
		{
			if (target != null)
			{
				global::UnityEngine.Vector4 one = global::UnityEngine.Vector4.one;
				one.x = diffuse;
				one.y = specular;
				global::UnityEngine.Material[] targetMaterials = global::mset.Sky.getTargetMaterials(target);
				foreach (global::UnityEngine.Material material in targetMaterials)
				{
					material.SetVector("_UniformOcclusion", one);
				}
			}
		}

		public void SetCustomExposure(float diffInt, float specInt, float skyInt, float camExpo)
		{
			this.SetCustomExposure(null, diffInt, specInt, skyInt, camExpo);
		}

		public void SetCustomExposure(global::UnityEngine.Renderer target, float diffInt, float specInt, float skyInt, float camExpo)
		{
			global::UnityEngine.Vector4 one = global::UnityEngine.Vector4.one;
			this.ComputeExposureVector(ref one, diffInt, specInt, skyInt, camExpo);
			if (target == null)
			{
				global::UnityEngine.Shader.SetGlobalVector(this.blendIDs[0].exposureIBL, one);
			}
			else
			{
				global::UnityEngine.Material[] targetMaterials = global::mset.Sky.getTargetMaterials(target);
				foreach (global::UnityEngine.Material material in targetMaterials)
				{
					material.SetVector(this.blendIDs[0].exposureIBL, one);
				}
			}
		}

		public void ToggleChildLights(bool enable)
		{
			global::UnityEngine.Light[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.Light>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = enable;
			}
		}

		private void UpdateSkySize()
		{
			if (this.HasDimensions)
			{
				this.skyMin = this.Dimensions.center - this.Dimensions.extents;
				this.skyMax = this.Dimensions.center + this.Dimensions.extents;
				global::UnityEngine.Vector3 localScale = base.transform.localScale;
				this.skyMin.x = this.skyMin.x * localScale.x;
				this.skyMin.y = this.skyMin.y * localScale.y;
				this.skyMin.z = this.skyMin.z * localScale.z;
				this.skyMax.x = this.skyMax.x * localScale.x;
				this.skyMax.y = this.skyMax.y * localScale.y;
				this.skyMax.z = this.skyMax.z * localScale.z;
			}
			else
			{
				this.skyMax = global::UnityEngine.Vector4.one * 100000f;
				this.skyMin = global::UnityEngine.Vector4.one * -100000f;
			}
		}

		private void UpdateSkyTransform()
		{
			this.skyMatrix.SetTRS(base.transform.position, base.transform.rotation, global::UnityEngine.Vector3.one);
			this.invMatrix = this.skyMatrix.inverse;
		}

		private void ComputeExposureVector(ref global::UnityEngine.Vector4 result, float diffInt, float specInt, float skyInt, float camExpo)
		{
			result.x = this.masterIntensity * diffInt;
			result.y = this.masterIntensity * specInt;
			result.z = this.masterIntensity * skyInt * camExpo;
			result.w = camExpo;
			float num = 6f;
			if (this.linearSpace)
			{
				num = global::UnityEngine.Mathf.Pow(num, 2.2f);
			}
			if (!this.hdrSpec)
			{
				result.y /= num;
			}
			if (!this.hdrSky)
			{
				result.z /= num;
			}
		}

		private void UpdateExposures()
		{
			this.ComputeExposureVector(ref this.exposures, this.diffIntensity, this.specIntensity, this.skyIntensity, this.camExposure);
			this.exposuresLM.x = this.diffIntensityLM;
			this.exposuresLM.y = this.specIntensityLM;
		}

		private void UpdatePropertyIDs()
		{
			this.blendIDs[0].Link();
			this.blendIDs[1].Link("1");
		}

		public void Awake()
		{
			this.UpdatePropertyIDs();
			global::mset.Sky.propBlock = new global::UnityEngine.MaterialPropertyBlock();
		}

		private void Reset()
		{
			this.skyMatrix = (this.invMatrix = global::UnityEngine.Matrix4x4.identity);
			this.exposures = global::UnityEngine.Vector4.one;
			this.exposuresLM = global::UnityEngine.Vector4.one;
			this.specularCube = (this.skyboxCube = null);
			this.masterIntensity = (this.skyIntensity = (this.specIntensity = (this.diffIntensity = 1f)));
			this.hdrSky = (this.hdrSpec = false);
		}

		private void OnEnable()
		{
			if (this.SH == null)
			{
				this.SH = new global::mset.SHEncoding();
			}
			if (this.CustomSH != null)
			{
				this.SH.copyFrom(this.CustomSH.SH);
			}
			this.SH.copyToBuffer();
		}

		private void OnLevelWasLoaded(int level)
		{
			this.UpdateExposures();
			this.UpdateSkyTransform();
			this.UpdateSkySize();
		}

		private void Start()
		{
			this.UpdateExposures();
			this.UpdateSkyTransform();
			this.UpdateSkySize();
		}

		private void Update()
		{
			if (base.transform.hasChanged)
			{
				this.Dirty = true;
				this.UpdateSkyTransform();
				this.UpdateSkySize();
				base.transform.hasChanged = false;
			}
			this.UpdateExposures();
		}

		private void OnDestroy()
		{
			this.SH = null;
			this._blackCube = null;
			this.specularCube = null;
			this.skyboxCube = null;
		}

		public void DrawProjectionCube(global::UnityEngine.Vector3 center, global::UnityEngine.Vector3 radius)
		{
			if (this.projMaterial == null)
			{
				this.projMaterial = global::UnityEngine.Resources.Load<global::UnityEngine.Material>("projectionMat");
				if (!this.projMaterial)
				{
					global::UnityEngine.Debug.LogError("Failed to find projectionMat material in Resources folder!");
				}
			}
			global::UnityEngine.Vector4 vector = global::UnityEngine.Vector4.one;
			vector.z = this.CamExposure;
			vector *= this.masterIntensity;
			global::mset.ShaderIDs shaderIDs = this.blendIDs[0];
			this.projMaterial.color = new global::UnityEngine.Color(0.7f, 0.7f, 0.7f, 1f);
			this.projMaterial.SetVector(shaderIDs.skyMin, -this.Dimensions.extents);
			this.projMaterial.SetVector(shaderIDs.skyMax, this.Dimensions.extents);
			this.projMaterial.SetVector(shaderIDs.exposureIBL, vector);
			this.projMaterial.SetTexture(shaderIDs.skyCubeIBL, this.specularCube);
			this.projMaterial.SetMatrix(shaderIDs.skyMatrix, this.skyMatrix);
			this.projMaterial.SetMatrix(shaderIDs.invSkyMatrix, this.invMatrix);
			this.projMaterial.SetPass(0);
			global::UnityEngine.GL.PushMatrix();
			global::UnityEngine.GL.MultMatrix(base.transform.localToWorldMatrix);
			global::mset.GLUtil.DrawCube(center, -radius);
			global::UnityEngine.GL.End();
			global::UnityEngine.GL.PopMatrix();
		}

		private void OnTriggerEnter(global::UnityEngine.Collider other)
		{
			if (other.GetComponent<global::UnityEngine.Renderer>())
			{
				this.Apply(other.GetComponent<global::UnityEngine.Renderer>(), 0);
			}
		}

		private void OnPostRender()
		{
		}

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Texture specularCube;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Texture skyboxCube;

		public bool IsProbe;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Bounds dimensions = new global::UnityEngine.Bounds(global::UnityEngine.Vector3.zero, global::UnityEngine.Vector3.one);

		private bool _dirty;

		[global::UnityEngine.SerializeField]
		private float masterIntensity = 1f;

		[global::UnityEngine.SerializeField]
		private float skyIntensity = 1f;

		[global::UnityEngine.SerializeField]
		private float specIntensity = 1f;

		[global::UnityEngine.SerializeField]
		private float diffIntensity = 1f;

		[global::UnityEngine.SerializeField]
		private float camExposure = 1f;

		[global::UnityEngine.SerializeField]
		private float specIntensityLM = 1f;

		[global::UnityEngine.SerializeField]
		private float diffIntensityLM = 1f;

		[global::UnityEngine.SerializeField]
		private bool hdrSky = true;

		[global::UnityEngine.SerializeField]
		private bool hdrSpec = true;

		[global::UnityEngine.SerializeField]
		private bool linearSpace = true;

		[global::UnityEngine.SerializeField]
		private bool autoDetectColorSpace = true;

		[global::UnityEngine.SerializeField]
		private bool hasDimensions;

		public global::mset.SHEncoding SH = new global::mset.SHEncoding();

		public global::mset.SHEncodingFile CustomSH;

		private global::UnityEngine.Matrix4x4 skyMatrix = global::UnityEngine.Matrix4x4.identity;

		private global::UnityEngine.Matrix4x4 invMatrix = global::UnityEngine.Matrix4x4.identity;

		private global::UnityEngine.Vector4 exposures = global::UnityEngine.Vector4.one;

		private global::UnityEngine.Vector4 exposuresLM = global::UnityEngine.Vector4.one;

		private global::UnityEngine.Vector4 skyMin = -global::UnityEngine.Vector4.one;

		private global::UnityEngine.Vector4 skyMax = global::UnityEngine.Vector4.one;

		private global::mset.ShaderIDs[] blendIDs = new global::mset.ShaderIDs[]
		{
			new global::mset.ShaderIDs(),
			new global::mset.ShaderIDs()
		};

		private static global::UnityEngine.MaterialPropertyBlock propBlock;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Cubemap _blackCube;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _SkyboxMaterial;

		private static bool internalProjectionSupport;

		private static bool internalBlendingSupport;

		private global::UnityEngine.Material projMaterial;
	}
}
