using System;
using System.Collections.Generic;
using Flowmap;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Flowmaps/Generator")]
public class FlowmapGenerator : global::UnityEngine.MonoBehaviour
{
	public static global::UnityEngine.LayerMask GpuRenderLayer
	{
		get
		{
			return global::UnityEngine.LayerMask.NameToLayer("Default");
		}
	}

	public static bool SupportsGPUPath
	{
		get
		{
			return global::UnityEngine.SystemInfo.SupportsRenderTextureFormat(global::UnityEngine.RenderTextureFormat.ARGBHalf) && global::UnityEngine.SystemInfo.supportsRenderTextures;
		}
	}

	public static int ThreadCount
	{
		get
		{
			return global::FlowmapGenerator._threadCount;
		}
		set
		{
			global::FlowmapGenerator._threadCount = value;
		}
	}

	public static global::UnityEngine.RenderTextureFormat GetSingleChannelRTFormat
	{
		get
		{
			return (!global::UnityEngine.SystemInfo.SupportsRenderTextureFormat(global::UnityEngine.RenderTextureFormat.RFloat)) ? global::UnityEngine.RenderTextureFormat.ARGBHalf : global::UnityEngine.RenderTextureFormat.RFloat;
		}
	}

	public static global::UnityEngine.RenderTextureFormat GetTwoChannelRTFormat
	{
		get
		{
			return (!global::UnityEngine.SystemInfo.SupportsRenderTextureFormat(global::UnityEngine.RenderTextureFormat.RGFloat)) ? global::UnityEngine.RenderTextureFormat.ARGBHalf : global::UnityEngine.RenderTextureFormat.RGFloat;
		}
	}

	public static global::UnityEngine.RenderTextureFormat GetFourChannelRTFormat
	{
		get
		{
			return (!global::UnityEngine.SystemInfo.SupportsRenderTextureFormat(global::UnityEngine.RenderTextureFormat.ARGBFloat)) ? global::UnityEngine.RenderTextureFormat.ARGBHalf : global::UnityEngine.RenderTextureFormat.ARGBFloat;
		}
	}

	public global::FlowSimulationField[] Fields
	{
		get
		{
			this.CleanNullFields();
			return this.fields.ToArray();
		}
	}

	public global::Flowmap.SimulationPath GetSimulationPath()
	{
		return (!this.gpuAcceleration || !global::FlowmapGenerator.SupportsGPUPath) ? global::Flowmap.SimulationPath.CPU : global::Flowmap.SimulationPath.GPU;
	}

	public global::UnityEngine.Vector2 Dimensions
	{
		get
		{
			return this.dimensions;
		}
		set
		{
			this.dimensions = value;
		}
	}

	public global::UnityEngine.Vector3 Position
	{
		get
		{
			return this.cachedPosition;
		}
	}

	public global::FlowSimulator FlowSimulator
	{
		get
		{
			if (!this.flowSimulator)
			{
				this.flowSimulator = base.GetComponent<global::FlowSimulator>();
			}
			return this.flowSimulator;
		}
	}

	public global::FlowHeightmap Heightmap
	{
		get
		{
			if (!this.heightmap)
			{
				this.heightmap = base.GetComponent<global::FlowHeightmap>();
			}
			return this.heightmap;
		}
	}

	private void Awake()
	{
		base.transform.rotation = global::UnityEngine.Quaternion.identity;
		this.cachedPosition = base.transform.position;
		this.UpdateThreadCount();
	}

	private void Start()
	{
		this.UpdateSimulationPath();
		if (this.FlowSimulator)
		{
			this.FlowSimulator.Init();
			if (this.FlowSimulator.simulateOnPlay && global::UnityEngine.Application.isPlaying)
			{
				this.FlowSimulator.StartSimulating();
			}
		}
	}

	public void UpdateSimulationPath()
	{
		global::FlowmapGenerator.SimulationPath = this.GetSimulationPath();
	}

	public void UpdateThreadCount()
	{
		global::FlowmapGenerator._threadCount = this.maxThreadCount;
	}

	private void Update()
	{
		base.transform.rotation = global::UnityEngine.Quaternion.identity;
		this.cachedPosition = base.transform.position;
		if (this.autoAddChildFields)
		{
			foreach (global::FlowSimulationField field in base.GetComponentsInChildren<global::FlowSimulationField>())
			{
				this.AddSimulationField(field);
			}
		}
	}

	public void CleanNullFields()
	{
		this.fields.RemoveAll((global::FlowSimulationField i) => i == null);
	}

	public void AddSimulationField(global::FlowSimulationField field)
	{
		if (!this.fields.Contains(field))
		{
			this.fields.Add(field);
		}
	}

	public void ClearAllFields()
	{
		this.fields.Clear();
	}

	private void OnDrawGizmos()
	{
		global::UnityEngine.Gizmos.DrawWireCube(base.transform.position, new global::UnityEngine.Vector3(this.Dimensions.x, 0f, this.Dimensions.y));
	}

	public static global::Flowmap.SimulationPath SimulationPath;

	private static int _threadCount = 1;

	[global::UnityEngine.SerializeField]
	private global::System.Collections.Generic.List<global::FlowSimulationField> fields = new global::System.Collections.Generic.List<global::FlowSimulationField>();

	public bool gpuAcceleration;

	public bool autoAddChildFields = true;

	public int maxThreadCount = 1;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Vector2 dimensions = global::UnityEngine.Vector2.one;

	private global::UnityEngine.Vector3 cachedPosition;

	public int outputFileFormat;

	private global::FlowSimulator flowSimulator;

	private global::FlowHeightmap heightmap;
}
