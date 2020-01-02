using System;
using Flowmap;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class FlowSimulationField : global::UnityEngine.MonoBehaviour
{
	public virtual global::Flowmap.FieldPass Pass
	{
		get
		{
			return global::Flowmap.FieldPass.Force;
		}
	}

	protected virtual global::UnityEngine.Shader RenderShader
	{
		get
		{
			return null;
		}
	}

	public global::UnityEngine.Material FalloffMaterial
	{
		get
		{
			if (!this.falloffMaterial)
			{
				this.falloffMaterial = new global::UnityEngine.Material(this.RenderShader);
				this.falloffMaterial.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
				this.falloffMaterial.name = "FlowFieldFalloff";
			}
			if (this.falloffMaterial.shader != this.RenderShader)
			{
				this.falloffMaterial.shader = this.RenderShader;
			}
			return this.falloffMaterial;
		}
	}

	public global::GpuRenderPlane RenderPlane
	{
		get
		{
			return this.renderPlane;
		}
	}

	protected void CreateMesh()
	{
		if (this.renderPlane && this.renderPlane.gameObject)
		{
			if (global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Object.Destroy(this.renderPlane.gameObject);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(this.renderPlane.gameObject);
			}
		}
		if (this == null)
		{
			return;
		}
		if (this.renderPlane == null)
		{
			this.renderPlane = new global::UnityEngine.GameObject(base.name + " render plane")
			{
				hideFlags = global::UnityEngine.HideFlags.HideInHierarchy,
				layer = global::FlowmapGenerator.GpuRenderLayer
			}.AddComponent<global::GpuRenderPlane>();
			this.renderPlane.field = this;
		}
		global::UnityEngine.MeshFilter meshFilter = this.renderPlane.GetComponent<global::UnityEngine.MeshFilter>();
		if (!meshFilter)
		{
			meshFilter = this.renderPlane.gameObject.AddComponent<global::UnityEngine.MeshFilter>();
		}
		meshFilter.sharedMesh = global::Flowmap.Primitives.PlaneMesh;
		global::UnityEngine.MeshRenderer meshRenderer = this.renderPlane.GetComponent<global::UnityEngine.MeshRenderer>();
		if (!meshRenderer)
		{
			meshRenderer = this.renderPlane.gameObject.AddComponent<global::UnityEngine.MeshRenderer>();
		}
		meshRenderer.material = this.FalloffMaterial;
		meshRenderer.enabled = false;
	}

	private void Awake()
	{
		this.Init();
	}

	protected virtual void Update()
	{
		if (!this.initialized)
		{
			this.Init();
		}
		if (global::UnityEngine.Application.isPlaying)
		{
			this.UpdateRenderPlane();
		}
	}

	public void DisableRenderPlane()
	{
		if (this.renderPlane)
		{
			this.renderPlane.GetComponent<global::UnityEngine.Renderer>().enabled = false;
		}
	}

	public void DrawFalloffTextureEnabled(bool state)
	{
		this.wantsToDrawPreviewTexture = state;
	}

	public virtual void UpdateRenderPlane()
	{
		if (this.renderPlane == null || this.renderPlane.field != this)
		{
			this.CreateMesh();
		}
		this.renderPlane.transform.position = base.transform.position;
		this.renderPlane.transform.localScale = base.transform.lossyScale;
		this.renderPlane.transform.rotation = base.transform.rotation;
		this.FalloffMaterial.SetTexture("_MainTex", this.falloffTexture);
		this.FalloffMaterial.SetFloat("_Strength", this.strength);
		this.renderPlane.GetComponent<global::UnityEngine.Renderer>().enabled = (global::FlowSimulationField.DrawFalloffTextures && (this.wantsToDrawPreviewTexture || global::FlowSimulationField.DrawFalloffUnselected) && base.enabled);
	}

	public virtual void Init()
	{
		if (this.initialized)
		{
			return;
		}
		this.cachedTransform = base.transform;
		this.CreateMesh();
		this.renderPlane.GetComponent<global::UnityEngine.Renderer>().enabled = this.wantsToDrawPreviewTexture;
		this.cachedTransform = base.transform;
		this.cachedPosition = this.cachedTransform.position;
		this.cachedRotation = this.cachedTransform.rotation;
		this.cachedScale = this.cachedTransform.lossyScale;
		this.hasFalloffTexture = (this.falloffTexture != null);
		if (this.falloffTexture)
		{
			this.falloffTextureDimensions = new global::UnityEngine.Vector2((float)this.falloffTexture.width, (float)this.falloffTexture.height);
			this.falloffTexturePixels = this.falloffTexture.GetPixels();
		}
		else
		{
			this.falloffTextureDimensions = global::UnityEngine.Vector2.zero;
		}
		this.initialized = true;
	}

	public virtual void TickStart()
	{
		if (!base.enabled)
		{
			return;
		}
		global::Flowmap.SimulationPath simulationPath = global::FlowmapGenerator.SimulationPath;
		if (simulationPath != global::Flowmap.SimulationPath.GPU)
		{
			if (simulationPath == global::Flowmap.SimulationPath.CPU)
			{
				this.cachedTransform = base.transform;
				this.cachedPosition = this.cachedTransform.position;
				this.cachedRotation = this.cachedTransform.rotation;
				this.cachedScale = this.cachedTransform.lossyScale;
				this.hasFalloffTexture = (this.falloffTexture != null);
				if (this.falloffTexture)
				{
					this.falloffTextureDimensions = new global::UnityEngine.Vector2((float)this.falloffTexture.width, (float)this.falloffTexture.height);
					this.falloffTexturePixels = this.falloffTexture.GetPixels();
				}
				else
				{
					this.falloffTextureDimensions = global::UnityEngine.Vector2.zero;
				}
			}
		}
		else
		{
			this.UpdateRenderPlane();
			this.FalloffMaterial.SetFloat("_Renderable", 1f);
			this.renderPlane.GetComponent<global::UnityEngine.Renderer>().enabled = true;
		}
	}

	public virtual void TickEnd()
	{
		global::Flowmap.SimulationPath simulationPath = global::FlowmapGenerator.SimulationPath;
		if (simulationPath == global::Flowmap.SimulationPath.GPU)
		{
			this.UpdateRenderPlane();
			this.FalloffMaterial.SetFloat("_Renderable", 0f);
		}
	}

	public global::UnityEngine.Vector2 GetUvScale(global::FlowmapGenerator generator)
	{
		return new global::UnityEngine.Vector2(this.cachedScale.x / generator.Dimensions.x, this.cachedScale.z / generator.Dimensions.y);
	}

	public global::UnityEngine.Vector2 GetUvTransform(global::FlowmapGenerator generator)
	{
		return new global::UnityEngine.Vector2((generator.Position.x - this.cachedPosition.x) / generator.Dimensions.x, (generator.Position.z - this.cachedPosition.z) / generator.Dimensions.y);
	}

	public float GetUvRotation(global::FlowmapGenerator generator)
	{
		return this.cachedRotation.eulerAngles.y * 0.0174532924f;
	}

	public float GetStrengthCpu(global::FlowmapGenerator generator, global::UnityEngine.Vector2 uv)
	{
		global::UnityEngine.Vector2 vector = this.TransformSampleUv(generator, uv, false);
		float num = this.strength;
		if (vector.x < 0f || vector.x > 1f || vector.y < 0f || vector.y > 1f)
		{
			num = 0f;
		}
		if (global::FlowmapGenerator.ThreadCount > 1)
		{
			return num * ((!this.hasFalloffTexture) ? 1f : global::Flowmap.TextureUtilities.SampleColorBilinear(this.falloffTexturePixels, (int)this.falloffTextureDimensions.x, (int)this.falloffTextureDimensions.y, vector.x, vector.y).r);
		}
		return num * ((!this.hasFalloffTexture) ? 1f : this.falloffTexture.GetPixelBilinear(vector.x, vector.y).r);
	}

	protected global::UnityEngine.Vector2 TransformSampleUv(global::FlowmapGenerator generator, global::UnityEngine.Vector2 uv, bool invertY)
	{
		global::UnityEngine.Vector2 vector = uv;
		vector = new global::UnityEngine.Vector2(vector.x + this.GetUvTransform(generator).x, vector.y + this.GetUvTransform(generator).y);
		vector -= global::UnityEngine.Vector2.one * 0.5f;
		vector = new global::UnityEngine.Vector2(vector.x * global::UnityEngine.Mathf.Cos(this.GetUvRotation(generator)) - vector.y * global::UnityEngine.Mathf.Sin(this.GetUvRotation(generator)), vector.x * global::UnityEngine.Mathf.Sin(this.GetUvRotation(generator)) + vector.y * global::UnityEngine.Mathf.Cos(this.GetUvRotation(generator)));
		vector = new global::UnityEngine.Vector2(vector.x / this.GetUvScale(generator).x * (float)((!invertY) ? 1 : -1), vector.y / this.GetUvScale(generator).y * (float)((!invertY) ? 1 : -1));
		vector += global::UnityEngine.Vector2.one * 0.5f;
		return vector;
	}

	protected virtual void OnDrawGizmosSelected()
	{
		global::UnityEngine.Vector3 from = this.cachedTransform.position + this.cachedTransform.right * (-this.cachedTransform.lossyScale.x / 2f) + this.cachedTransform.forward * (-this.cachedTransform.lossyScale.z / 2f);
		global::UnityEngine.Vector3 vector = this.cachedTransform.position + this.cachedTransform.right * (this.cachedTransform.lossyScale.x / 2f) + this.cachedTransform.forward * (-this.cachedTransform.lossyScale.z / 2f);
		global::UnityEngine.Vector3 vector2 = this.cachedTransform.position + this.cachedTransform.right * (-this.cachedTransform.lossyScale.x / 2f) + this.cachedTransform.forward * (this.cachedTransform.lossyScale.z / 2f);
		global::UnityEngine.Vector3 to = this.cachedTransform.position + this.cachedTransform.right * (this.cachedTransform.lossyScale.x / 2f) + this.cachedTransform.forward * (this.cachedTransform.lossyScale.z / 2f);
		global::UnityEngine.Gizmos.DrawLine(from, vector);
		global::UnityEngine.Gizmos.DrawLine(vector2, to);
		global::UnityEngine.Gizmos.DrawLine(from, vector2);
		global::UnityEngine.Gizmos.DrawLine(vector, to);
		this.wantsToDrawPreviewTexture = true;
		this.UpdateRenderPlane();
	}

	protected virtual void OnDrawGizmos()
	{
		this.wantsToDrawPreviewTexture = false;
		this.UpdateRenderPlane();
	}

	private void OnDisable()
	{
		this.wantsToDrawPreviewTexture = false;
		if (this.renderPlane)
		{
			this.renderPlane.GetComponent<global::UnityEngine.Renderer>().enabled = (global::FlowSimulationField.DrawFalloffTextures && this.wantsToDrawPreviewTexture);
		}
	}

	private void OnDestroy()
	{
		this.Cleaup();
	}

	protected virtual void Cleaup()
	{
		if (this.renderPlane && this.renderPlane.gameObject)
		{
			if (global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Object.Destroy(this.renderPlane.gameObject);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(this.renderPlane.gameObject);
			}
		}
		if (this.falloffMaterial)
		{
			if (global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Object.Destroy(this.falloffMaterial);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(this.falloffMaterial);
			}
		}
	}

	public static bool DrawFalloffTextures = true;

	public static bool DrawFalloffUnselected;

	public float strength = 1f;

	public global::UnityEngine.Texture2D falloffTexture;

	protected global::UnityEngine.Transform cachedTransform;

	protected global::UnityEngine.Vector3 cachedPosition;

	protected global::UnityEngine.Quaternion cachedRotation;

	protected global::UnityEngine.Vector3 cachedScale;

	protected global::UnityEngine.Vector2 falloffTextureDimensions;

	protected global::UnityEngine.Color[] falloffTexturePixels;

	private bool initialized;

	protected bool wantsToDrawPreviewTexture;

	protected bool hasFalloffTexture;

	private global::UnityEngine.Material falloffMaterial;

	[global::UnityEngine.SerializeField]
	[global::UnityEngine.HideInInspector]
	protected global::GpuRenderPlane renderPlane;
}
