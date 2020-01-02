using System;
using Flowmap;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::FlowmapGenerator))]
[global::UnityEngine.AddComponentMenu("Flowmaps/Heightmap/Render From Scene")]
[global::UnityEngine.ExecuteInEditMode]
public class FlowRenderHeightmap : global::FlowHeightmap
{
	public static bool Supported
	{
		get
		{
			return global::UnityEngine.SystemInfo.supportsRenderTextures;
		}
	}

	public static string UnsupportedReason
	{
		get
		{
			string result = string.Empty;
			if (!global::UnityEngine.SystemInfo.supportsRenderTextures)
			{
				result = "System doesn't support RenderTextures.";
			}
			return result;
		}
	}

	public override global::UnityEngine.Texture HeightmapTexture
	{
		get
		{
			return this.heightmap;
		}
		set
		{
			global::UnityEngine.Debug.LogWarning("Can't set HeightmapTexture.");
		}
	}

	public override global::UnityEngine.Texture PreviewHeightmapTexture
	{
		get
		{
			return this.HeightmapTexture;
		}
	}

	private global::UnityEngine.Shader ClippedHeightShader
	{
		get
		{
			return global::UnityEngine.Shader.Find("Hidden/DepthToHeightClipped");
		}
	}

	private global::UnityEngine.Shader HeightShader
	{
		get
		{
			return global::UnityEngine.Shader.Find("Hidden/DepthToHeight");
		}
	}

	private global::UnityEngine.Material CompareMaterial
	{
		get
		{
			if (!this.compareMaterial)
			{
				this.compareMaterial = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/DepthCompare"));
				this.compareMaterial.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			return this.compareMaterial;
		}
	}

	private global::UnityEngine.Material ResizeMaterial
	{
		get
		{
			if (!this.resizeMaterial)
			{
				this.resizeMaterial = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/RenderHeightmapResize"));
				this.resizeMaterial.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			return this.resizeMaterial;
		}
	}

	private void Awake()
	{
		this.UpdateHeightmap();
	}

	public void UpdateHeightmap()
	{
		if (this.heightmap == null || this.heightmap.width != this.resolutionX || this.heightmap.height != this.resolutionY)
		{
			this.heightmap = new global::UnityEngine.RenderTexture(this.resolutionX, this.resolutionY, 24, global::FlowmapGenerator.GetSingleChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.Linear);
			this.heightmap.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
		}
		if (this.renderingCamera == null)
		{
			this.renderingCamera = new global::UnityEngine.GameObject("Render Heightmap", new global::System.Type[]
			{
				typeof(global::UnityEngine.Camera)
			}).GetComponent<global::UnityEngine.Camera>();
			this.renderingCamera.gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.renderingCamera.enabled = false;
			this.renderingCamera.renderingPath = global::UnityEngine.RenderingPath.Forward;
			this.renderingCamera.clearFlags = global::UnityEngine.CameraClearFlags.Color;
			this.renderingCamera.backgroundColor = global::UnityEngine.Color.black;
			this.renderingCamera.orthographic = true;
		}
		this.renderingCamera.cullingMask = this.cullingMask;
		this.renderingCamera.transform.rotation = global::UnityEngine.Quaternion.identity;
		this.renderingCamera.orthographicSize = global::UnityEngine.Mathf.Max(base.Generator.Dimensions.x, base.Generator.Dimensions.y) * 0.5f;
		this.renderingCamera.transform.position = base.Generator.transform.position + global::UnityEngine.Vector3.up * this.heightMax;
		this.renderingCamera.transform.rotation = global::UnityEngine.Quaternion.LookRotation(global::UnityEngine.Vector3.down, global::UnityEngine.Vector3.forward);
		global::Flowmap.FluidDepth fluidDepth = this.fluidDepth;
		if (fluidDepth != global::Flowmap.FluidDepth.DeepWater)
		{
			if (fluidDepth == global::Flowmap.FluidDepth.Surface)
			{
				global::UnityEngine.Shader.SetGlobalFloat("_HeightmapRenderDepthMax", base.Generator.transform.position.y - this.heightMin);
				global::UnityEngine.Shader.SetGlobalFloat("_HeightmapRenderDepthMin", base.Generator.transform.position.y + this.heightMax);
				this.renderingCamera.nearClipPlane = 0.001f;
				this.renderingCamera.farClipPlane = this.heightMin + this.heightMax;
				this.renderingCamera.targetTexture = this.heightmap;
				this.renderingCamera.RenderWithShader(this.HeightShader, "RenderType");
			}
		}
		else
		{
			global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(this.resolutionX, this.resolutionY, 24, global::FlowmapGenerator.GetSingleChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.sRGB);
			global::UnityEngine.RenderTexture temporary2 = global::UnityEngine.RenderTexture.GetTemporary(this.resolutionX, this.resolutionY, 24, global::FlowmapGenerator.GetSingleChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.sRGB);
			global::UnityEngine.RenderTexture temporary3 = global::UnityEngine.RenderTexture.GetTemporary(this.resolutionX, this.resolutionY, 24, global::FlowmapGenerator.GetSingleChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.sRGB);
			global::UnityEngine.Shader.SetGlobalFloat("_HeightmapRenderDepthMin", base.Generator.transform.position.y);
			global::UnityEngine.Shader.SetGlobalFloat("_HeightmapRenderDepthMax", base.Generator.transform.position.y - this.heightMin);
			this.renderingCamera.targetTexture = temporary;
			this.renderingCamera.nearClipPlane = 0.01f;
			this.renderingCamera.farClipPlane = 100f;
			this.renderingCamera.RenderWithShader(this.ClippedHeightShader, "RenderType");
			global::UnityEngine.Shader.SetGlobalFloat("_HeightmapRenderDepthMin", base.Generator.transform.position.y);
			global::UnityEngine.Shader.SetGlobalFloat("_HeightmapRenderDepthMax", base.Generator.transform.position.y - this.heightMin);
			this.renderingCamera.nearClipPlane = this.heightMax;
			this.renderingCamera.farClipPlane = this.heightMin + this.heightMax;
			this.renderingCamera.targetTexture = temporary2;
			this.renderingCamera.RenderWithShader(this.HeightShader, "RenderType");
			global::UnityEngine.Shader.SetGlobalFloat("_HeightmapRenderDepthMin", base.Generator.transform.position.y);
			global::UnityEngine.Shader.SetGlobalFloat("_HeightmapRenderDepthMax", base.Generator.transform.position.y - this.heightMin);
			this.renderingCamera.nearClipPlane = 0.01f;
			this.renderingCamera.farClipPlane = this.heightMin + this.heightMax;
			this.renderingCamera.targetTexture = temporary3;
			this.renderingCamera.RenderWithShader(this.HeightShader, "RenderType");
			this.CompareMaterial.SetTexture("_OverhangMaskTex", temporary);
			this.CompareMaterial.SetTexture("_HeightBelowSurfaceTex", temporary2);
			this.CompareMaterial.SetTexture("_HeightIntersectingTex", temporary3);
			global::UnityEngine.Graphics.Blit(null, this.heightmap, this.CompareMaterial);
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary);
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary2);
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary3);
		}
		if (base.Generator.Dimensions.x != base.Generator.Dimensions.y)
		{
			global::UnityEngine.RenderTexture temporary4 = global::UnityEngine.RenderTexture.GetTemporary(this.resolutionX, this.resolutionY, 24, global::FlowmapGenerator.GetSingleChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.Linear);
			this.ResizeMaterial.SetTexture("_Heightmap", this.heightmap);
			if (base.Generator.Dimensions.y > base.Generator.Dimensions.x)
			{
				this.ResizeMaterial.SetVector("_AspectRatio", new global::UnityEngine.Vector4(base.Generator.Dimensions.x / base.Generator.Dimensions.y, 1f, 0f, 0f));
			}
			else
			{
				this.ResizeMaterial.SetVector("_AspectRatio", new global::UnityEngine.Vector4(1f, 1f / (base.Generator.Dimensions.x / base.Generator.Dimensions.y), 0f, 0f));
			}
			global::UnityEngine.Graphics.Blit(null, temporary4, this.ResizeMaterial, 0);
			global::UnityEngine.Graphics.Blit(temporary4, this.heightmap);
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary4);
		}
	}

	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		global::UnityEngine.Gizmos.DrawWireCube(base.transform.position + global::UnityEngine.Vector3.up * (this.heightMax - this.heightMin) / 2f, new global::UnityEngine.Vector3(base.Generator.Dimensions.x, this.heightMax + this.heightMin, base.Generator.Dimensions.y));
	}

	public int resolutionX = 256;

	public int resolutionY = 256;

	public global::Flowmap.FluidDepth fluidDepth;

	public float heightMax = 1f;

	public float heightMin = 1f;

	public global::UnityEngine.LayerMask cullingMask = 1;

	public bool dynamicUpdating;

	private global::UnityEngine.Camera renderingCamera;

	private global::UnityEngine.RenderTexture heightmap;

	private global::UnityEngine.Material compareMaterial;

	private global::UnityEngine.Material resizeMaterial;
}
