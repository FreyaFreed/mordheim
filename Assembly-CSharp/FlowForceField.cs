using System;
using Flowmap;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Flowmaps/Fields/Force")]
public class FlowForceField : global::FlowSimulationField
{
	public override global::Flowmap.FieldPass Pass
	{
		get
		{
			return global::Flowmap.FieldPass.Force;
		}
	}

	protected override global::UnityEngine.Shader RenderShader
	{
		get
		{
			return global::UnityEngine.Shader.Find("Hidden/ForceFieldPreview");
		}
	}

	public override void Init()
	{
		base.Init();
		this.UpdateVectorTexture();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void UpdateVectorTexture()
	{
		int num = 64;
		int num2 = 64;
		this.vectorTexture = new global::UnityEngine.Texture2D(num, num2, global::UnityEngine.TextureFormat.ARGB32, false, true);
		this.vectorTexture.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
		this.vectorTexture.name = "VectorTexture";
		global::UnityEngine.Color[] array = new global::UnityEngine.Color[num * num2];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				global::UnityEngine.Vector2 vector = global::UnityEngine.Vector2.zero;
				float a = 1f - global::UnityEngine.Mathf.Clamp01(vector.magnitude);
				global::UnityEngine.Color white = global::UnityEngine.Color.white;
				switch (this.force)
				{
				case global::Flowmap.FluidForce.Attract:
				case global::Flowmap.FluidForce.Repulse:
					vector = new global::UnityEngine.Vector2(((float)j / (float)num - 0.5f) * 2f, ((float)i / (float)num2 - 0.5f) * 2f);
					vector = vector.normalized;
					vector = new global::UnityEngine.Vector2(vector.x * 0.5f + 0.5f, vector.y * 0.5f + 0.5f);
					white = new global::UnityEngine.Color(vector.x, vector.y, 0f, a);
					break;
				case global::Flowmap.FluidForce.VortexCounterClockwise:
				case global::Flowmap.FluidForce.VortexClockwise:
				{
					vector = new global::UnityEngine.Vector2(((float)j / (float)num - 0.5f) * 2f, ((float)i / (float)num2 - 0.5f) * 2f);
					vector = vector.normalized;
					global::UnityEngine.Vector3 vector2 = global::UnityEngine.Vector3.Cross(new global::UnityEngine.Vector3(vector.x, 0f, vector.y), global::UnityEngine.Vector3.down);
					vector = new global::UnityEngine.Vector2(vector2.x * 0.5f + 0.5f, vector2.z * 0.5f + 0.5f);
					white = new global::UnityEngine.Color(vector.x, vector.y, 0f, a);
					break;
				}
				case global::Flowmap.FluidForce.Directional:
					vector = global::UnityEngine.Vector2.one;
					white = new global::UnityEngine.Color(vector.x, vector.y, 0f, a);
					break;
				case global::Flowmap.FluidForce.Calm:
					white = new global::UnityEngine.Color(0.5f, 0.5f, 1f, a);
					break;
				}
				array[j + i * num] = white;
			}
		}
		this.vectorTexture.SetPixels(array);
		this.vectorTexture.Apply(false);
		this.vectorTexturePixels = this.vectorTexture.GetPixels();
		this.vectorTextureDimensions = new global::UnityEngine.Vector2((float)this.vectorTexture.width, (float)this.vectorTexture.height);
	}

	public override void UpdateRenderPlane()
	{
		base.UpdateRenderPlane();
		global::Flowmap.SimulationPath simulationPath = global::FlowmapGenerator.SimulationPath;
		if (simulationPath == global::Flowmap.SimulationPath.GPU)
		{
			base.FalloffMaterial.SetTexture("_VectorTex", this.vectorTexture);
			switch (this.force)
			{
			case global::Flowmap.FluidForce.Attract:
				base.FalloffMaterial.SetVector("_VectorScale", global::UnityEngine.Vector2.one);
				base.FalloffMaterial.SetFloat("_VectorInvert", 1f);
				break;
			case global::Flowmap.FluidForce.Repulse:
				base.FalloffMaterial.SetVector("_VectorScale", global::UnityEngine.Vector2.one);
				base.FalloffMaterial.SetFloat("_VectorInvert", 0f);
				break;
			case global::Flowmap.FluidForce.VortexCounterClockwise:
				base.FalloffMaterial.SetVector("_VectorScale", global::UnityEngine.Vector2.one);
				base.FalloffMaterial.SetFloat("_VectorInvert", 1f);
				break;
			case global::Flowmap.FluidForce.VortexClockwise:
				base.FalloffMaterial.SetVector("_VectorScale", global::UnityEngine.Vector2.one);
				base.FalloffMaterial.SetFloat("_VectorInvert", 0f);
				break;
			case global::Flowmap.FluidForce.Directional:
			{
				global::UnityEngine.Vector2 v = new global::UnityEngine.Vector2(base.transform.forward.x * 0.5f + 0.5f, base.transform.forward.z * 0.5f + 0.5f);
				base.FalloffMaterial.SetVector("_VectorScale", v);
				base.FalloffMaterial.SetFloat("_VectorInvert", 0f);
				break;
			}
			case global::Flowmap.FluidForce.Calm:
				base.FalloffMaterial.SetVector("_VectorScale", global::UnityEngine.Vector2.one);
				base.FalloffMaterial.SetFloat("_VectorInvert", 0f);
				break;
			}
		}
		if ((this.wantsToDrawPreviewTexture || global::FlowSimulationField.DrawFalloffUnselected) && base.enabled)
		{
			switch (this.force)
			{
			case global::Flowmap.FluidForce.Attract:
				base.FalloffMaterial.SetTexture("_VectorPreviewTex", this.attractVectorPreview);
				break;
			case global::Flowmap.FluidForce.Repulse:
				base.FalloffMaterial.SetTexture("_VectorPreviewTex", this.repulseVectorPreview);
				break;
			case global::Flowmap.FluidForce.VortexCounterClockwise:
				base.FalloffMaterial.SetTexture("_VectorPreviewTex", this.vortexCounterClockwiseVectorPreview);
				break;
			case global::Flowmap.FluidForce.VortexClockwise:
				base.FalloffMaterial.SetTexture("_VectorPreviewTex", this.vortexClockwiseVectorPreview);
				break;
			case global::Flowmap.FluidForce.Directional:
				base.FalloffMaterial.SetTexture("_VectorPreviewTex", this.directionalVectorPreview);
				break;
			}
		}
		else
		{
			base.FalloffMaterial.SetTexture("_VectorPreviewTex", null);
		}
	}

	public override void TickStart()
	{
		base.TickStart();
		global::Flowmap.SimulationPath simulationPath = global::FlowmapGenerator.SimulationPath;
		if (simulationPath == global::Flowmap.SimulationPath.CPU)
		{
			if (this.vectorTexturePixels == null)
			{
				this.Init();
			}
			this.cachedForwardVector = base.transform.forward;
		}
	}

	public global::UnityEngine.Vector3 GetForceCpu(global::FlowmapGenerator generator, global::UnityEngine.Vector2 uv)
	{
		global::UnityEngine.Vector2 vector = base.TransformSampleUv(generator, uv, this.force == global::Flowmap.FluidForce.Attract || this.force == global::Flowmap.FluidForce.VortexCounterClockwise);
		global::UnityEngine.Color color = (global::FlowmapGenerator.ThreadCount <= 1) ? this.vectorTexture.GetPixelBilinear(vector.x, vector.y) : global::Flowmap.TextureUtilities.SampleColorBilinear(this.vectorTexturePixels, (int)this.vectorTextureDimensions.x, (int)this.vectorTextureDimensions.y, vector.x, vector.y);
		if (this.force == global::Flowmap.FluidForce.Directional)
		{
			color = new global::UnityEngine.Color(this.cachedForwardVector.x * 0.5f + 0.5f, this.cachedForwardVector.z * 0.5f + 0.5f, 0f, 0f);
		}
		global::UnityEngine.Vector3 a = new global::UnityEngine.Vector3(color.r * 2f - 1f, color.g * 2f - 1f, color.b);
		return this.strength * a * (float)((vector.x < 0f || vector.x > 1f || vector.y < 0f || vector.y > 1f) ? 0 : 1) * ((!this.hasFalloffTexture) ? 1f : ((global::FlowmapGenerator.ThreadCount <= 1) ? this.falloffTexture.GetPixelBilinear(vector.x, vector.y).r : global::Flowmap.TextureUtilities.SampleColorBilinear(this.falloffTexturePixels, (int)this.falloffTextureDimensions.x, (int)this.falloffTextureDimensions.y, vector.x, vector.y).r));
	}

	protected override void Cleaup()
	{
		base.Cleaup();
		if (this.vectorTexture)
		{
			if (global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Object.Destroy(this.vectorTexture);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(this.vectorTexture);
			}
		}
	}

	public global::Flowmap.FluidForce force;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Texture2D vectorTexture;

	private global::UnityEngine.Vector2 vectorTextureDimensions;

	private global::UnityEngine.Color[] vectorTexturePixels;

	private global::UnityEngine.Vector3 cachedForwardVector;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Texture2D attractVectorPreview;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Texture2D repulseVectorPreview;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Texture2D vortexClockwiseVectorPreview;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Texture2D vortexCounterClockwiseVectorPreview;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Texture2D directionalVectorPreview;
}
