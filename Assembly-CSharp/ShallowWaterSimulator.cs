using System;
using System.IO;
using System.Linq;
using System.Threading;
using Flowmap;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Flowmaps/Simulators/Shallow Water")]
public class ShallowWaterSimulator : global::FlowSimulator
{
	public event global::Flowmap.VoidEvent OnRenderTextureReset;

	public event global::Flowmap.VoidEvent OnMaxStepsReached;

	private global::UnityEngine.Material SimulationMaterial
	{
		get
		{
			if (!this.simulationMaterial)
			{
				this.simulationMaterial = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/ShallowWaterFlowmapSimulator"));
				this.simulationMaterial.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			return this.simulationMaterial;
		}
	}

	public global::UnityEngine.Texture HeightFluidTexture
	{
		get
		{
			if (global::FlowmapGenerator.SimulationPath == global::Flowmap.SimulationPath.CPU)
			{
				return this.heightFluidCpu;
			}
			return this.heightFluidRT;
		}
	}

	public global::UnityEngine.Texture VelocityAccumulatedTexture
	{
		get
		{
			if (global::FlowmapGenerator.SimulationPath == global::Flowmap.SimulationPath.CPU && this.velocityAccumulatedCpu)
			{
				return this.velocityAccumulatedCpu;
			}
			if (this.velocityAccumulatedRT)
			{
				return this.velocityAccumulatedRT;
			}
			return null;
		}
	}

	public override void Init()
	{
		base.Init();
		this.Cleanup();
		global::Flowmap.SimulationPath simulationPath = global::FlowmapGenerator.SimulationPath;
		if (simulationPath != global::Flowmap.SimulationPath.GPU)
		{
			if (simulationPath == global::Flowmap.SimulationPath.CPU)
			{
				this.ResetCpuData();
				this.BakeFieldsCpu();
				for (int i = 0; i < this.resolutionX; i++)
				{
					for (int j = 0; j < this.resolutionY; j++)
					{
						this.simulationDataCpu[i][j].fluid = ((this.fluidDepth != global::Flowmap.FluidDepth.DeepWater) ? this.initialFluidAmount : ((1f - global::UnityEngine.Mathf.Ceil(this.simulationDataCpu[i][j].height)) * this.initialFluidAmount));
					}
				}
				this.initializedCpu = true;
			}
		}
		else
		{
			this.heightFluidRT = new global::UnityEngine.RenderTexture(this.resolutionX, this.resolutionY, 0, global::FlowmapGenerator.GetTwoChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.Linear);
			this.heightFluidRT.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.heightFluidRT.name = "HeightFluid";
			this.heightFluidRT.Create();
			this.fluidAddRT = new global::UnityEngine.RenderTexture(this.resolutionX, this.resolutionY, 0, global::FlowmapGenerator.GetSingleChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.Linear);
			this.fluidAddRT.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.fluidAddRT.name = "FluidAdd";
			this.fluidAddRT.Create();
			this.fluidRemoveRT = new global::UnityEngine.RenderTexture(this.resolutionX, this.resolutionY, 0, global::FlowmapGenerator.GetSingleChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.Linear);
			this.fluidRemoveRT.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.fluidRemoveRT.name = "FluidRemove";
			this.fluidRemoveRT.Create();
			this.fluidForceRT = new global::UnityEngine.RenderTexture(this.resolutionX, this.resolutionY, 0, global::FlowmapGenerator.GetFourChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.Linear);
			this.fluidForceRT.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.fluidForceRT.name = "FluidForce";
			this.fluidForceRT.Create();
			this.outflowRT = new global::UnityEngine.RenderTexture(this.resolutionX, this.resolutionY, 0, global::FlowmapGenerator.GetFourChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.Linear);
			this.outflowRT.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.outflowRT.name = "Outflow";
			this.outflowRT.Create();
			this.velocityRT = new global::UnityEngine.RenderTexture(this.resolutionX, this.resolutionY, 0, global::FlowmapGenerator.GetTwoChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.Linear);
			this.velocityRT.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.velocityRT.name = "Velocity";
			this.velocityRT.Create();
			this.velocityAccumulatedRT = new global::UnityEngine.RenderTexture(this.resolutionX, this.resolutionY, 0, global::FlowmapGenerator.GetFourChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.Linear);
			this.velocityAccumulatedRT.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.velocityAccumulatedRT.name = "VelocityAccumulated";
			this.velocityAccumulatedRT.Create();
			this.bufferRT1 = new global::UnityEngine.RenderTexture(this.resolutionX, this.resolutionY, 0, global::FlowmapGenerator.GetFourChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.Linear);
			this.bufferRT1.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.bufferRT1.name = "Buffer1";
			this.bufferRT1.Create();
			this.fieldRenderCamera = new global::UnityEngine.GameObject("Field Renderer", new global::System.Type[]
			{
				typeof(global::UnityEngine.Camera)
			}).GetComponent<global::UnityEngine.Camera>();
			this.fieldRenderCamera.gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.fieldRenderCamera.orthographic = true;
			this.fieldRenderCamera.orthographicSize = global::UnityEngine.Mathf.Max(base.Generator.Dimensions.x, base.Generator.Dimensions.y) * 0.5f;
			this.fieldRenderCamera.renderingPath = global::UnityEngine.RenderingPath.Forward;
			this.fieldRenderCamera.cullingMask = 1 << global::FlowmapGenerator.GpuRenderLayer.value;
			this.fieldRenderCamera.clearFlags = global::UnityEngine.CameraClearFlags.Color;
			this.fieldRenderCamera.backgroundColor = global::UnityEngine.Color.black;
			this.fieldRenderCamera.enabled = false;
			this.ResetGPUData();
			this.initializedGpu = true;
		}
		if (base.Generator.Heightmap is global::FlowRenderHeightmap)
		{
			(base.Generator.Heightmap as global::FlowRenderHeightmap).UpdateHeightmap();
		}
	}

	private void DestroyProperly(global::UnityEngine.Object obj)
	{
		if (global::UnityEngine.Application.isEditor || !global::UnityEngine.Application.isPlaying)
		{
			global::UnityEngine.Object.DestroyImmediate(obj);
		}
		else if (global::UnityEngine.Application.isPlaying && !global::UnityEngine.Application.isEditor)
		{
			global::UnityEngine.Object.Destroy(obj);
		}
	}

	private void Cleanup()
	{
		global::UnityEngine.RenderTexture.active = null;
		if (this.heightFluidRT)
		{
			this.DestroyProperly(this.heightFluidRT);
		}
		if (this.fluidAddRT)
		{
			this.DestroyProperly(this.fluidAddRT);
		}
		if (this.fluidRemoveRT)
		{
			this.DestroyProperly(this.fluidRemoveRT);
		}
		if (this.fluidForceRT)
		{
			this.DestroyProperly(this.fluidForceRT);
		}
		if (this.outflowRT)
		{
			this.DestroyProperly(this.outflowRT);
		}
		if (this.velocityRT)
		{
			this.DestroyProperly(this.velocityRT);
		}
		if (this.velocityAccumulatedRT)
		{
			this.DestroyProperly(this.velocityAccumulatedRT);
		}
		if (this.bufferRT1)
		{
			this.DestroyProperly(this.bufferRT1);
		}
		if (this.fieldRenderCamera)
		{
			this.DestroyProperly(this.fieldRenderCamera.gameObject);
		}
		if (this.simulationMaterial)
		{
			this.DestroyProperly(this.simulationMaterial);
		}
		this.initializedGpu = false;
		this.simulationDataCpu = null;
		if (this.heightFluidCpu)
		{
			this.DestroyProperly(this.heightFluidCpu);
		}
		if (this.velocityAccumulatedCpu)
		{
			this.DestroyProperly(this.heightFluidCpu);
		}
		this.initializedCpu = false;
	}

	private void OnDestroy()
	{
		this.Cleanup();
	}

	public override void Reset()
	{
		this.Init();
		base.Reset();
		this.AssignToMaterials();
	}

	private void ResetGPUData()
	{
		this.SimulationMaterial.SetColor("_Color", new global::UnityEngine.Color(0.5f, 0.5f, 0f, 1f));
		global::UnityEngine.Graphics.Blit(null, this.velocityRT, this.SimulationMaterial, 4);
		global::UnityEngine.Graphics.Blit(null, this.velocityAccumulatedRT, this.SimulationMaterial, 4);
		global::UnityEngine.Graphics.Blit(null, this.fluidForceRT, this.SimulationMaterial, 4);
		this.SimulationMaterial.SetColor("_Color", global::UnityEngine.Color.black);
		global::UnityEngine.Graphics.Blit(null, this.fluidAddRT, this.SimulationMaterial, 4);
		global::UnityEngine.Graphics.Blit(null, this.fluidRemoveRT, this.SimulationMaterial, 4);
		global::UnityEngine.Graphics.Blit(null, this.bufferRT1, this.SimulationMaterial, 4);
		this.SimulationMaterial.SetColor("_Color", new global::UnityEngine.Color(0f, 0f, 0f, 0f));
		global::UnityEngine.Graphics.Blit(null, this.outflowRT, this.SimulationMaterial, 4);
		if (base.Generator.Heightmap)
		{
			global::UnityEngine.Graphics.Blit(base.Generator.Heightmap.HeightmapTexture, this.heightFluidRT, this.SimulationMaterial, 6);
		}
		else
		{
			this.SimulationMaterial.SetColor("_Color", new global::UnityEngine.Color(0f, 0f, 0f, 0f));
			global::UnityEngine.Graphics.Blit(null, this.heightFluidRT, this.SimulationMaterial, 4);
		}
		if (this.initialFluidAmount > 0f)
		{
			this.SimulationMaterial.SetFloat("_DeepWater", (float)((this.fluidDepth != global::Flowmap.FluidDepth.DeepWater) ? 0 : 1));
			this.SimulationMaterial.SetFloat("_FluidAmount", this.initialFluidAmount);
			global::UnityEngine.Graphics.Blit(this.heightFluidRT, this.bufferRT1, this.SimulationMaterial, 14);
			global::UnityEngine.Graphics.Blit(this.bufferRT1, this.heightFluidRT);
		}
	}

	private void ResetCpuData()
	{
		if (this.simulationDataCpu == null)
		{
			this.simulationDataCpu = new global::Flowmap.SimulationData[this.resolutionX][];
		}
		for (int i = 0; i < this.resolutionX; i++)
		{
			if (this.simulationDataCpu[i] == null)
			{
				this.simulationDataCpu[i] = new global::Flowmap.SimulationData[this.resolutionY];
			}
			for (int j = 0; j < this.resolutionY; j++)
			{
				this.simulationDataCpu[i][j] = default(global::Flowmap.SimulationData);
				this.simulationDataCpu[i][j].velocity = new global::UnityEngine.Vector3(0.5f, 0.5f, 0f);
				this.simulationDataCpu[i][j].velocityAccumulated = new global::UnityEngine.Vector3(0.5f, 0.5f, 0f);
			}
		}
	}

	public override void StartSimulating()
	{
		base.StartSimulating();
		global::Flowmap.SimulationPath simulationPath = global::FlowmapGenerator.SimulationPath;
		if (simulationPath != global::Flowmap.SimulationPath.GPU)
		{
			if (simulationPath == global::Flowmap.SimulationPath.CPU)
			{
				if (this.simulationDataCpu == null)
				{
					this.initializedCpu = false;
				}
				if (!this.initializedCpu)
				{
					this.Init();
				}
			}
		}
		else if (!this.initializedGpu)
		{
			this.Init();
		}
	}

	public override void Tick()
	{
		base.Tick();
		if (!base.Simulating)
		{
			return;
		}
		global::Flowmap.SimulationPath simulationPath = global::FlowmapGenerator.SimulationPath;
		if (simulationPath != global::Flowmap.SimulationPath.GPU)
		{
			if (simulationPath == global::Flowmap.SimulationPath.CPU)
			{
				if (global::FlowmapGenerator.ThreadCount > 1)
				{
					int num = global::UnityEngine.Mathf.CeilToInt((float)this.resolutionX / (float)global::FlowmapGenerator.ThreadCount);
					global::System.Threading.ManualResetEvent[] array = new global::System.Threading.ManualResetEvent[global::FlowmapGenerator.ThreadCount];
					global::Flowmap.ArrayThreadedInfo[] array2 = new global::Flowmap.ArrayThreadedInfo[global::FlowmapGenerator.ThreadCount];
					for (int i = 0; i < global::FlowmapGenerator.ThreadCount; i++)
					{
						array[i] = new global::System.Threading.ManualResetEvent(false);
						array2[i] = new global::Flowmap.ArrayThreadedInfo(0, 0, null);
					}
					for (int j = 0; j < global::FlowmapGenerator.ThreadCount; j++)
					{
						array[j].Reset();
						array2[j].start = j * num;
						array2[j].length = ((j != global::FlowmapGenerator.ThreadCount - 1) ? num : (this.resolutionX - 1 - j * num));
						array2[j].resetEvent = array[j];
						global::System.Threading.ThreadPool.QueueUserWorkItem(new global::System.Threading.WaitCallback(this.AddRemoveFluidThreaded), array2[j]);
					}
					global::System.Threading.WaitHandle.WaitAll(array);
					for (int k = 0; k < global::FlowmapGenerator.ThreadCount; k++)
					{
						int start = k * num;
						int length = (k != global::FlowmapGenerator.ThreadCount - 1) ? num : (this.resolutionX - 1 - k * num);
						array[k] = new global::System.Threading.ManualResetEvent(false);
						global::System.Threading.ThreadPool.QueueUserWorkItem(new global::System.Threading.WaitCallback(this.OutflowThreaded), new global::Flowmap.ArrayThreadedInfo(start, length, array[k]));
					}
					global::System.Threading.WaitHandle.WaitAll(array);
					for (int l = 0; l < global::FlowmapGenerator.ThreadCount; l++)
					{
						int start2 = l * num;
						int length2 = (l != global::FlowmapGenerator.ThreadCount - 1) ? num : (this.resolutionX - 1 - l * num);
						array[l] = new global::System.Threading.ManualResetEvent(false);
						global::System.Threading.ThreadPool.QueueUserWorkItem(new global::System.Threading.WaitCallback(this.UpdateVelocityThreaded), new global::Flowmap.ArrayThreadedInfo(start2, length2, array[l]));
					}
					global::System.Threading.WaitHandle.WaitAll(array);
					if (this.simulateFoam)
					{
						for (int m = 0; m < global::FlowmapGenerator.ThreadCount; m++)
						{
							int start3 = m * num;
							int length3 = (m != global::FlowmapGenerator.ThreadCount - 1) ? num : (this.resolutionX - 1 - m * num);
							array[m] = new global::System.Threading.ManualResetEvent(false);
							global::System.Threading.ThreadPool.QueueUserWorkItem(new global::System.Threading.WaitCallback(this.FoamThreaded), new global::Flowmap.ArrayThreadedInfo(start3, length3, array[m]));
						}
						global::System.Threading.WaitHandle.WaitAll(array);
					}
					if (this.outputFilterStrength > 0f)
					{
						for (int n = 0; n < global::FlowmapGenerator.ThreadCount; n++)
						{
							int start4 = n * num;
							int length4 = (n != global::FlowmapGenerator.ThreadCount - 1) ? num : (this.resolutionX - 1 - n * num);
							array[n] = new global::System.Threading.ManualResetEvent(false);
							global::System.Threading.ThreadPool.QueueUserWorkItem(new global::System.Threading.WaitCallback(this.BlurVelocityAccumulatedHorizontalThreaded), new global::Flowmap.ArrayThreadedInfo(start4, length4, array[n]));
						}
						global::System.Threading.WaitHandle.WaitAll(array);
						for (int num2 = 0; num2 < global::FlowmapGenerator.ThreadCount; num2++)
						{
							int start5 = num2 * num;
							int length5 = (num2 != global::FlowmapGenerator.ThreadCount - 1) ? num : (this.resolutionX - 1 - num2 * num);
							array[num2] = new global::System.Threading.ManualResetEvent(false);
							global::System.Threading.ThreadPool.QueueUserWorkItem(new global::System.Threading.WaitCallback(this.BlurVelocityAccumulatedVerticalThreaded), new global::Flowmap.ArrayThreadedInfo(start5, length5, array[num2]));
						}
						global::System.Threading.WaitHandle.WaitAll(array);
					}
				}
				else
				{
					for (int num3 = 0; num3 < this.resolutionX; num3++)
					{
						for (int num4 = 0; num4 < this.resolutionY; num4++)
						{
							this.AddRemoveFluidCpu(num3, num4);
						}
					}
					for (int num5 = 0; num5 < this.resolutionX; num5++)
					{
						for (int num6 = 0; num6 < this.resolutionY; num6++)
						{
							this.OutflowCpu(num5, num6);
						}
					}
					for (int num7 = 0; num7 < this.resolutionX; num7++)
					{
						for (int num8 = 0; num8 < this.resolutionY; num8++)
						{
							this.UpdateVelocityCpu(num7, num8);
						}
					}
					if (this.simulateFoam)
					{
						for (int num9 = 0; num9 < this.resolutionX; num9++)
						{
							for (int num10 = 0; num10 < this.resolutionY; num10++)
							{
								this.FoamCpu(num9, num10);
							}
						}
					}
					if (this.outputFilterStrength > 0f)
					{
						for (int num11 = 0; num11 < this.resolutionX; num11++)
						{
							for (int num12 = 0; num12 < this.resolutionY; num12++)
							{
								this.BlurVelocityAccumulatedHorizontalCpu(num11, num12);
							}
						}
						for (int num13 = 0; num13 < this.resolutionX; num13++)
						{
							for (int num14 = 0; num14 < this.resolutionY; num14++)
							{
								this.BlurVelocityAccumulatedVerticalCpu(num13, num14);
							}
						}
					}
				}
				if (base.SimulationStepsCount % this.updateTextureDelayCPU == 0)
				{
					this.WriteCpuDataToTexture();
				}
			}
		}
		else
		{
			if (!this.heightFluidRT.IsCreated() || !this.outflowRT.IsCreated() || !this.velocityRT.IsCreated() || !this.velocityAccumulatedRT.IsCreated())
			{
				if (this.OnRenderTextureReset != null)
				{
					this.OnRenderTextureReset();
				}
				this.Init();
			}
			float num15 = base.Generator.transform.position.y;
			float num16 = base.Generator.transform.position.y;
			for (int num17 = 0; num17 < base.Generator.Fields.Length; num17++)
			{
				num15 = global::UnityEngine.Mathf.Max(num15, base.Generator.Fields[num17].transform.position.y);
				num16 = global::UnityEngine.Mathf.Min(num16, base.Generator.Fields[num17].transform.position.y);
			}
			this.fieldRenderCamera.transform.localPosition = base.Generator.transform.position;
			this.fieldRenderCamera.transform.position = new global::UnityEngine.Vector3(this.fieldRenderCamera.transform.position.x, num15 + 1f, this.fieldRenderCamera.transform.position.z);
			this.fieldRenderCamera.farClipPlane = num15 - num16 + 2f;
			this.fieldRenderCamera.transform.rotation = global::UnityEngine.Quaternion.LookRotation(global::UnityEngine.Vector3.down, global::UnityEngine.Vector3.forward);
			this.SimulationMaterial.SetVector("_Resolution", new global::UnityEngine.Vector4((float)this.resolutionX, (float)this.resolutionY, 0f, 0f));
			this.SimulationMaterial.SetFloat("_Timestep", this.timestep);
			this.SimulationMaterial.SetFloat("_Gravity", this.gravity);
			this.SimulationMaterial.SetFloat("_VelocityScale", this.velocityScale);
			foreach (global::FlowSimulationField flowSimulationField in base.Generator.Fields)
			{
				if (flowSimulationField.Pass == global::Flowmap.FieldPass.AddFluid)
				{
					flowSimulationField.TickStart();
				}
			}
			this.fieldRenderCamera.backgroundColor = global::UnityEngine.Color.black;
			this.fieldRenderCamera.targetTexture = this.fluidAddRT;
			this.fieldRenderCamera.RenderWithShader(global::UnityEngine.Shader.Find("Hidden/FlowmapOffscreenRender"), "Offscreen");
			foreach (global::FlowSimulationField flowSimulationField2 in base.Generator.Fields)
			{
				if (flowSimulationField2.Pass == global::Flowmap.FieldPass.AddFluid)
				{
					flowSimulationField2.TickEnd();
				}
			}
			foreach (global::FlowSimulationField flowSimulationField3 in base.Generator.Fields)
			{
				if (flowSimulationField3.Pass == global::Flowmap.FieldPass.RemoveFluid)
				{
					flowSimulationField3.TickStart();
				}
			}
			this.fieldRenderCamera.backgroundColor = global::UnityEngine.Color.black;
			this.fieldRenderCamera.targetTexture = this.fluidRemoveRT;
			this.fieldRenderCamera.RenderWithShader(global::UnityEngine.Shader.Find("Hidden/FlowmapOffscreenRender"), "Offscreen");
			foreach (global::FlowSimulationField flowSimulationField4 in base.Generator.Fields)
			{
				if (flowSimulationField4.Pass == global::Flowmap.FieldPass.RemoveFluid)
				{
					flowSimulationField4.TickEnd();
				}
			}
			foreach (global::FlowSimulationField flowSimulationField5 in base.Generator.Fields)
			{
				if (flowSimulationField5.Pass == global::Flowmap.FieldPass.Force)
				{
					flowSimulationField5.TickStart();
				}
			}
			this.fieldRenderCamera.backgroundColor = new global::UnityEngine.Color(global::UnityEngine.Mathf.LinearToGammaSpace(0.5f), global::UnityEngine.Mathf.LinearToGammaSpace(0.5f), 0f, 1f);
			this.fieldRenderCamera.targetTexture = this.fluidForceRT;
			this.fieldRenderCamera.RenderWithShader(global::UnityEngine.Shader.Find("Hidden/FlowmapOffscreenRender"), "Offscreen");
			foreach (global::FlowSimulationField flowSimulationField6 in base.Generator.Fields)
			{
				if (flowSimulationField6.Pass == global::Flowmap.FieldPass.Force)
				{
					flowSimulationField6.TickEnd();
				}
			}
			if (base.Generator.Heightmap && base.Generator.Heightmap is global::FlowRenderHeightmap && (base.Generator.Heightmap as global::FlowRenderHeightmap).dynamicUpdating)
			{
				(base.Generator.Heightmap as global::FlowRenderHeightmap).UpdateHeightmap();
			}
			if (base.Generator.Heightmap)
			{
				if (base.Generator.Heightmap is global::FlowTextureHeightmap && (base.Generator.Heightmap as global::FlowTextureHeightmap).isRaw)
				{
					this.SimulationMaterial.SetFloat("_IsFloatRGBA", 1f);
				}
				else
				{
					this.SimulationMaterial.SetFloat("_IsFloatRGBA", 0f);
				}
				this.SimulationMaterial.SetTexture("_NewHeightTex", base.Generator.Heightmap.HeightmapTexture);
				global::UnityEngine.Graphics.Blit(this.heightFluidRT, this.bufferRT1, this.SimulationMaterial, 9);
				global::UnityEngine.Graphics.Blit(this.bufferRT1, this.heightFluidRT);
			}
			foreach (global::FlowSimulationField flowSimulationField7 in base.Generator.Fields)
			{
				if (flowSimulationField7.Pass == global::Flowmap.FieldPass.Heightmap)
				{
					flowSimulationField7.TickStart();
				}
			}
			this.fieldRenderCamera.backgroundColor = global::UnityEngine.Color.black;
			global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(this.resolutionX, this.resolutionY, 0, global::FlowmapGenerator.GetSingleChannelRTFormat, global::UnityEngine.RenderTextureReadWrite.Linear);
			this.fieldRenderCamera.targetTexture = temporary;
			this.fieldRenderCamera.RenderWithShader(global::UnityEngine.Shader.Find("Hidden/FlowmapOffscreenRender"), "Offscreen");
			foreach (global::FlowSimulationField flowSimulationField8 in base.Generator.Fields)
			{
				if (flowSimulationField8.Pass == global::Flowmap.FieldPass.Heightmap)
				{
					flowSimulationField8.TickEnd();
				}
			}
			this.SimulationMaterial.SetTexture("_HeightmapFieldsTex", temporary);
			global::UnityEngine.Graphics.Blit(this.heightFluidRT, this.bufferRT1, this.SimulationMaterial, 11);
			global::UnityEngine.Graphics.Blit(this.bufferRT1, this.heightFluidRT);
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary);
			this.SimulationMaterial.SetTexture("_FluidAddTex", this.fluidAddRT);
			this.SimulationMaterial.SetTexture("_FluidRemoveTex", this.fluidRemoveRT);
			this.SimulationMaterial.SetFloat("_Evaporation", this.evaporationRate);
			this.SimulationMaterial.SetFloat("_FluidAddMultiplier", this.fluidAddMultiplier);
			this.SimulationMaterial.SetFloat("_FluidRemoveMultiplier", this.fluidRemoveMultiplier);
			global::UnityEngine.Graphics.Blit(this.heightFluidRT, this.bufferRT1, this.SimulationMaterial, 0);
			global::UnityEngine.Graphics.Blit(this.bufferRT1, this.heightFluidRT);
			this.SimulationMaterial.SetTexture("_FluidForceTex", this.fluidForceRT);
			this.SimulationMaterial.SetFloat("_FluidForceMultiplier", this.fluidForceMultiplier);
			this.SimulationMaterial.SetTexture("_OutflowTex", this.outflowRT);
			this.SimulationMaterial.SetTexture("_VelocityTex", this.velocityRT);
			this.SimulationMaterial.SetFloat("_BorderCollision", (float)((this.borderCollision != global::Flowmap.SimulationBorderCollision.Collide) ? 0 : 1));
			global::UnityEngine.Graphics.Blit(this.heightFluidRT, this.bufferRT1, this.SimulationMaterial, 1);
			global::UnityEngine.Graphics.Blit(this.bufferRT1, this.outflowRT);
			this.SimulationMaterial.SetTexture("_OutflowTex", this.outflowRT);
			global::UnityEngine.Graphics.Blit(this.heightFluidRT, this.bufferRT1, this.SimulationMaterial, 2);
			global::UnityEngine.Graphics.Blit(this.bufferRT1, this.heightFluidRT);
			this.SimulationMaterial.SetTexture("_OutflowTex", this.outflowRT);
			global::UnityEngine.Graphics.Blit(this.heightFluidRT, this.bufferRT1, this.SimulationMaterial, 3);
			global::UnityEngine.Graphics.Blit(this.bufferRT1, this.velocityRT);
			this.SimulationMaterial.SetFloat("_Delta", this.outputAccumulationRate);
			this.SimulationMaterial.SetTexture("_VelocityTex", this.velocityRT);
			this.SimulationMaterial.SetTexture("_VelocityAccumTex", this.velocityAccumulatedRT);
			global::UnityEngine.Graphics.Blit(this.heightFluidRT, this.bufferRT1, this.SimulationMaterial, 5);
			global::UnityEngine.Graphics.Blit(this.bufferRT1, this.velocityAccumulatedRT);
			if (this.simulateFoam)
			{
				this.SimulationMaterial.SetFloat("_Delta", this.outputAccumulationRate);
				this.SimulationMaterial.SetTexture("_FluidAddTex", this.fluidAddRT);
				this.SimulationMaterial.SetTexture("_VelocityAccumTex", this.velocityAccumulatedRT);
				this.SimulationMaterial.SetFloat("_FoamVelocityScale", this.foamVelocityScale);
				global::UnityEngine.Graphics.Blit(this.heightFluidRT, this.bufferRT1, this.SimulationMaterial, 10);
				global::UnityEngine.Graphics.Blit(this.bufferRT1, this.velocityAccumulatedRT);
			}
			if (this.writeFluidDepthInAlpha)
			{
				this.SimulationMaterial.SetTexture("_HeightFluidTex", this.heightFluidRT);
				global::UnityEngine.Graphics.Blit(this.velocityAccumulatedRT, this.bufferRT1, this.SimulationMaterial, 15);
				global::UnityEngine.Graphics.Blit(this.bufferRT1, this.velocityAccumulatedRT);
			}
			if (this.outputFilterStrength > 0f)
			{
				this.SimulationMaterial.SetFloat("_BlurSpread", (float)this.outputFilterSize);
				this.SimulationMaterial.SetFloat("_Strength", this.outputFilterStrength);
				global::UnityEngine.Graphics.Blit(this.velocityAccumulatedRT, this.bufferRT1, this.SimulationMaterial, 7);
				global::UnityEngine.Graphics.Blit(this.bufferRT1, this.velocityAccumulatedRT, this.SimulationMaterial, 8);
			}
		}
	}

	private void BakeFieldsCpu()
	{
		if (base.Generator.Heightmap)
		{
			global::UnityEngine.Texture2D texture2D = null;
			bool flag = false;
			bool flag2 = false;
			if (base.Generator.Heightmap is global::FlowTextureHeightmap && (base.Generator.Heightmap as global::FlowTextureHeightmap).HeightmapTexture as global::UnityEngine.Texture2D != null)
			{
				texture2D = ((base.Generator.Heightmap as global::FlowTextureHeightmap).HeightmapTexture as global::UnityEngine.Texture2D);
				flag = (base.Generator.Heightmap as global::FlowTextureHeightmap).isRaw;
			}
			else if (base.Generator.Heightmap is global::FlowRenderHeightmap && global::FlowRenderHeightmap.Supported)
			{
				(this.generator.Heightmap as global::FlowRenderHeightmap).UpdateHeightmap();
				global::UnityEngine.RenderTexture renderTexture = (this.generator.Heightmap as global::FlowRenderHeightmap).HeightmapTexture as global::UnityEngine.RenderTexture;
				global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(renderTexture.width, renderTexture.height, 0, global::UnityEngine.RenderTextureFormat.ARGB32, global::UnityEngine.RenderTextureReadWrite.Linear);
				global::UnityEngine.Graphics.Blit(renderTexture, temporary);
				texture2D = new global::UnityEngine.Texture2D(renderTexture.width, renderTexture.height);
				texture2D.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
				global::UnityEngine.RenderTexture.active = temporary;
				texture2D.ReadPixels(new global::UnityEngine.Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), 0, 0);
				texture2D.Apply();
				flag2 = true;
				global::UnityEngine.RenderTexture.ReleaseTemporary(temporary);
			}
			if (texture2D != null)
			{
				global::UnityEngine.Color[] pixels = texture2D.GetPixels();
				for (int i = 0; i < this.resolutionX; i++)
				{
					for (int j = 0; j < this.resolutionY; j++)
					{
						if (flag)
						{
							this.simulationDataCpu[i][j].height = global::Flowmap.TextureUtilities.DecodeFloatRGBA(global::Flowmap.TextureUtilities.SampleColorBilinear(pixels, texture2D.width, texture2D.height, (float)i / (float)this.resolutionX, (float)j / (float)this.resolutionY));
						}
						else
						{
							this.simulationDataCpu[i][j].height = global::Flowmap.TextureUtilities.SampleColorBilinear(pixels, texture2D.width, texture2D.height, (float)i / (float)this.resolutionX, (float)j / (float)this.resolutionY).r;
						}
					}
				}
				if (flag2)
				{
					if (global::UnityEngine.Application.isPlaying)
					{
						global::UnityEngine.Object.Destroy(texture2D);
					}
					else
					{
						global::UnityEngine.Object.DestroyImmediate(texture2D);
					}
				}
			}
		}
		if (global::FlowmapGenerator.ThreadCount > 1)
		{
			int num = global::UnityEngine.Mathf.CeilToInt((float)this.resolutionX / (float)global::FlowmapGenerator.ThreadCount);
			global::System.Threading.ManualResetEvent[] array = new global::System.Threading.ManualResetEvent[global::FlowmapGenerator.ThreadCount];
			global::FlowSimulationField[] array2 = (from f in base.Generator.Fields
			where f is global::FluidAddField && f.enabled
			select f).ToArray<global::FlowSimulationField>();
			foreach (global::FlowSimulationField flowSimulationField in array2)
			{
				flowSimulationField.TickStart();
			}
			for (int l = 0; l < global::FlowmapGenerator.ThreadCount; l++)
			{
				int start = l * num;
				int length = (l != global::FlowmapGenerator.ThreadCount - 1) ? num : (this.resolutionX - 1 - l * num);
				array[l] = new global::System.Threading.ManualResetEvent(false);
				global::System.Threading.ThreadPool.QueueUserWorkItem(new global::System.Threading.WaitCallback(this.BakeAddFluidThreaded), new global::Flowmap.ThreadedFieldBakeInfo(start, length, array[l], array2, base.Generator));
			}
			global::System.Threading.WaitHandle.WaitAll(array);
			foreach (global::FlowSimulationField flowSimulationField2 in array2)
			{
				flowSimulationField2.TickEnd();
			}
			global::FlowSimulationField[] array5 = (from f in base.Generator.Fields
			where f is global::FluidRemoveField && f.enabled
			select f).ToArray<global::FlowSimulationField>();
			foreach (global::FlowSimulationField flowSimulationField3 in array5)
			{
				flowSimulationField3.TickStart();
			}
			for (int num2 = 0; num2 < global::FlowmapGenerator.ThreadCount; num2++)
			{
				int start2 = num2 * num;
				int length2 = (num2 != global::FlowmapGenerator.ThreadCount - 1) ? num : (this.resolutionX - 1 - num2 * num);
				array[num2] = new global::System.Threading.ManualResetEvent(false);
				global::System.Threading.ThreadPool.QueueUserWorkItem(new global::System.Threading.WaitCallback(this.BakeRemoveFluidThreaded), new global::Flowmap.ThreadedFieldBakeInfo(start2, length2, array[num2], array5, base.Generator));
			}
			global::System.Threading.WaitHandle.WaitAll(array);
			foreach (global::FlowSimulationField flowSimulationField4 in array5)
			{
				flowSimulationField4.TickEnd();
			}
			global::FlowSimulationField[] array8 = (from f in base.Generator.Fields
			where f is global::FlowForceField && f.enabled
			select f).ToArray<global::FlowSimulationField>();
			foreach (global::FlowSimulationField flowSimulationField5 in array8)
			{
				flowSimulationField5.TickStart();
			}
			for (int num5 = 0; num5 < global::FlowmapGenerator.ThreadCount; num5++)
			{
				int start3 = num5 * num;
				int length3 = (num5 != global::FlowmapGenerator.ThreadCount - 1) ? num : (this.resolutionX - 1 - num5 * num);
				array[num5] = new global::System.Threading.ManualResetEvent(false);
				global::System.Threading.ThreadPool.QueueUserWorkItem(new global::System.Threading.WaitCallback(this.BakeForcesThreaded), new global::Flowmap.ThreadedFieldBakeInfo(start3, length3, array[num5], array8, base.Generator));
			}
			global::System.Threading.WaitHandle.WaitAll(array);
			foreach (global::FlowSimulationField flowSimulationField6 in array8)
			{
				flowSimulationField6.TickEnd();
			}
			global::FlowSimulationField[] array11 = (from f in base.Generator.Fields
			where f is global::HeightmapField && f.enabled
			select f).ToArray<global::FlowSimulationField>();
			foreach (global::FlowSimulationField flowSimulationField7 in array11)
			{
				flowSimulationField7.TickStart();
			}
			for (int num8 = 0; num8 < global::FlowmapGenerator.ThreadCount; num8++)
			{
				int start4 = num8 * num;
				int length4 = (num8 != global::FlowmapGenerator.ThreadCount - 1) ? num : (this.resolutionX - 1 - num8 * num);
				array[num8] = new global::System.Threading.ManualResetEvent(false);
				global::System.Threading.ThreadPool.QueueUserWorkItem(new global::System.Threading.WaitCallback(this.BakeHeightmapThreaded), new global::Flowmap.ThreadedFieldBakeInfo(start4, length4, array[num8], array11, base.Generator));
			}
			global::System.Threading.WaitHandle.WaitAll(array);
			foreach (global::FlowSimulationField flowSimulationField8 in array11)
			{
				flowSimulationField8.TickEnd();
			}
		}
		else
		{
			global::FlowSimulationField[] array14 = (from f in base.Generator.Fields
			where f is global::FluidAddField && f.enabled
			select f).ToArray<global::FlowSimulationField>();
			foreach (global::FlowSimulationField flowSimulationField9 in array14)
			{
				flowSimulationField9.TickStart();
			}
			for (int num11 = 0; num11 < this.resolutionX; num11++)
			{
				for (int num12 = 0; num12 < this.resolutionY; num12++)
				{
					foreach (global::FlowSimulationField flowSimulationField10 in array14)
					{
						global::Flowmap.SimulationData[] array17 = this.simulationDataCpu[num11];
						int num14 = num12;
						array17[num14].addFluid = array17[num14].addFluid + flowSimulationField10.GetStrengthCpu(base.Generator, new global::UnityEngine.Vector2((float)num11 / (float)this.resolutionX, (float)num12 / (float)this.resolutionY));
					}
				}
			}
			foreach (global::FlowSimulationField flowSimulationField11 in array14)
			{
				flowSimulationField11.TickEnd();
			}
			global::FlowSimulationField[] array19 = (from f in base.Generator.Fields
			where f is global::FluidRemoveField && f.enabled
			select f).ToArray<global::FlowSimulationField>();
			foreach (global::FlowSimulationField flowSimulationField12 in array19)
			{
				flowSimulationField12.TickStart();
			}
			for (int num17 = 0; num17 < this.resolutionX; num17++)
			{
				for (int num18 = 0; num18 < this.resolutionY; num18++)
				{
					foreach (global::FlowSimulationField flowSimulationField13 in array19)
					{
						global::Flowmap.SimulationData[] array22 = this.simulationDataCpu[num17];
						int num20 = num18;
						array22[num20].removeFluid = array22[num20].removeFluid + flowSimulationField13.GetStrengthCpu(base.Generator, new global::UnityEngine.Vector2((float)num17 / (float)this.resolutionX, (float)num18 / (float)this.resolutionY));
					}
				}
			}
			foreach (global::FlowSimulationField flowSimulationField14 in array19)
			{
				flowSimulationField14.TickEnd();
			}
			global::FlowSimulationField[] array24 = (from f in base.Generator.Fields
			where f is global::FlowForceField && f.enabled
			select f).ToArray<global::FlowSimulationField>();
			foreach (global::FlowSimulationField flowSimulationField15 in array24)
			{
				flowSimulationField15.TickStart();
			}
			for (int num23 = 0; num23 < this.resolutionX; num23++)
			{
				for (int num24 = 0; num24 < this.resolutionY; num24++)
				{
					foreach (global::FlowSimulationField flowSimulationField16 in array24)
					{
						global::Flowmap.SimulationData[] array27 = this.simulationDataCpu[num23];
						int num26 = num24;
						array27[num26].force = array27[num26].force + (flowSimulationField16 as global::FlowForceField).GetForceCpu(base.Generator, new global::UnityEngine.Vector2((float)num23 / (float)this.resolutionX, (float)num24 / (float)this.resolutionY));
						this.simulationDataCpu[num23][num24].force.z = global::UnityEngine.Mathf.Max(this.simulationDataCpu[num23][num24].force.z, 0f);
					}
				}
			}
			foreach (global::FlowSimulationField flowSimulationField17 in array24)
			{
				flowSimulationField17.TickEnd();
			}
			global::FlowSimulationField[] array29 = (from f in base.Generator.Fields
			where f is global::HeightmapField && f.enabled
			select f).ToArray<global::FlowSimulationField>();
			foreach (global::FlowSimulationField flowSimulationField18 in array29)
			{
				flowSimulationField18.TickStart();
			}
			for (int num29 = 0; num29 < this.resolutionX; num29++)
			{
				for (int num30 = 0; num30 < this.resolutionY; num30++)
				{
					foreach (global::FlowSimulationField flowSimulationField19 in array29)
					{
						float strengthCpu = flowSimulationField19.GetStrengthCpu(base.Generator, new global::UnityEngine.Vector2((float)num29 / (float)this.resolutionX, (float)num30 / (float)this.resolutionY));
						this.simulationDataCpu[num29][num30].height = global::UnityEngine.Mathf.Lerp(this.simulationDataCpu[num29][num30].height, strengthCpu, strengthCpu * (1f - this.simulationDataCpu[num29][num30].height));
					}
				}
			}
			foreach (global::FlowSimulationField flowSimulationField20 in array29)
			{
				flowSimulationField20.TickEnd();
			}
		}
		this.WriteCpuDataToTexture();
	}

	private void AddRemoveFluidCpu(int x, int y)
	{
		global::Flowmap.SimulationData[] array = this.simulationDataCpu[x];
		array[y].fluid = array[y].fluid + this.simulationDataCpu[x][y].addFluid * this.timestep * this.fluidAddMultiplier;
		this.simulationDataCpu[x][y].fluid = global::UnityEngine.Mathf.Max(0f, this.simulationDataCpu[x][y].fluid - this.simulationDataCpu[x][y].removeFluid * this.fluidRemoveMultiplier);
		this.simulationDataCpu[x][y].fluid = this.simulationDataCpu[x][y].fluid * (1f - this.evaporationRate * this.timestep);
	}

	private void OutflowCpu(int x, int y)
	{
		int num = global::UnityEngine.Mathf.Min(y + 1, this.resolutionY - 1);
		int num2 = global::UnityEngine.Mathf.Min(x + 1, this.resolutionX - 1);
		int num3 = global::UnityEngine.Mathf.Max(y - 1, 0);
		int num4 = global::UnityEngine.Mathf.Max(x - 1, 0);
		global::UnityEngine.Vector2 lhs = new global::UnityEngine.Vector2(this.simulationDataCpu[x][y].force.x, this.simulationDataCpu[x][y].force.y);
		float num5 = global::UnityEngine.Mathf.Max(0f, this.simulationDataCpu[x][y].outflow.x + this.timestep * this.gravity * (this.simulationDataCpu[x][y].height + this.simulationDataCpu[x][y].fluid - this.simulationDataCpu[x][num].height - this.simulationDataCpu[x][num].fluid) + global::UnityEngine.Mathf.Clamp01(global::UnityEngine.Vector2.Dot(lhs, new global::UnityEngine.Vector2(0f, 1f))) * this.timestep * this.fluidForceMultiplier);
		float num6 = global::UnityEngine.Mathf.Max(0f, this.simulationDataCpu[x][y].outflow.y + this.timestep * this.gravity * (this.simulationDataCpu[x][y].height + this.simulationDataCpu[x][y].fluid - this.simulationDataCpu[num2][y].height - this.simulationDataCpu[num2][y].fluid) + global::UnityEngine.Mathf.Clamp01(global::UnityEngine.Vector2.Dot(lhs, new global::UnityEngine.Vector2(1f, 0f))) * this.timestep * this.fluidForceMultiplier);
		float num7 = global::UnityEngine.Mathf.Max(0f, this.simulationDataCpu[x][y].outflow.z + this.timestep * this.gravity * (this.simulationDataCpu[x][y].height + this.simulationDataCpu[x][y].fluid - this.simulationDataCpu[x][num3].height - this.simulationDataCpu[x][num3].fluid) + global::UnityEngine.Mathf.Clamp01(global::UnityEngine.Vector2.Dot(lhs, new global::UnityEngine.Vector2(0f, -1f))) * this.timestep * this.fluidForceMultiplier);
		float num8 = global::UnityEngine.Mathf.Max(0f, this.simulationDataCpu[x][y].outflow.w + this.timestep * this.gravity * (this.simulationDataCpu[x][y].height + this.simulationDataCpu[x][y].fluid - this.simulationDataCpu[num4][y].height - this.simulationDataCpu[num4][y].fluid) + global::UnityEngine.Mathf.Clamp01(global::UnityEngine.Vector2.Dot(lhs, new global::UnityEngine.Vector2(-1f, 0f))) * this.timestep * this.fluidForceMultiplier);
		if (this.borderCollision == global::Flowmap.SimulationBorderCollision.PassThrough)
		{
			if (x == 0)
			{
				num6 = 0f;
			}
			if (x == this.resolutionX - 1)
			{
				num8 = 0f;
			}
			if (y == 0)
			{
				num5 = 0f;
			}
			if (y == this.resolutionY - 1)
			{
				num7 = 0f;
			}
		}
		float num9 = (num5 + num6 + num7 + num8 <= 0f) ? 0f : global::UnityEngine.Mathf.Min(1f, this.simulationDataCpu[x][y].fluid / (this.timestep * (num5 + num6 + num7 + num8)));
		num9 *= 1f - this.simulationDataCpu[x][y].force.z;
		this.simulationDataCpu[x][y].outflow = new global::UnityEngine.Vector4(num5 * num9, num6 * num9, num7 * num9, num8 * num9);
	}

	private void UpdateVelocityCpu(int x, int y)
	{
		int num = global::UnityEngine.Mathf.Min(y + 1, this.resolutionY - 1);
		int num2 = global::UnityEngine.Mathf.Min(x + 1, this.resolutionX - 1);
		int num3 = global::UnityEngine.Mathf.Max(y - 1, 0);
		int num4 = global::UnityEngine.Mathf.Max(x - 1, 0);
		float z = this.simulationDataCpu[x][num].outflow.z;
		float w = this.simulationDataCpu[num2][y].outflow.w;
		float x2 = this.simulationDataCpu[x][num3].outflow.x;
		float y2 = this.simulationDataCpu[num4][y].outflow.y;
		float num5 = this.timestep * (z + w + x2 + y2 - (this.simulationDataCpu[x][y].outflow.x + this.simulationDataCpu[x][y].outflow.y + this.simulationDataCpu[x][y].outflow.z + this.simulationDataCpu[x][y].outflow.w));
		this.simulationDataCpu[x][y].fluid = this.simulationDataCpu[x][y].fluid + num5;
		float num6 = 0.5f * (y2 - w + (this.simulationDataCpu[x][y].outflow.y - this.simulationDataCpu[x][y].outflow.w));
		float num7 = 0.5f * (x2 - z + (this.simulationDataCpu[x][y].outflow.x - this.simulationDataCpu[x][y].outflow.z));
		float num8 = 0.5f * (this.simulationDataCpu[x][y].fluid + (this.simulationDataCpu[x][y].fluid + num5));
		global::UnityEngine.Vector2 zero = global::UnityEngine.Vector2.zero;
		if (num8 != 0f)
		{
			zero.x = num6 / num8;
			zero.y = num7 / num8;
		}
		zero.x = global::UnityEngine.Mathf.Clamp(zero.x * this.velocityScale, -1f, 1f) * 0.5f + 0.5f;
		zero.y = global::UnityEngine.Mathf.Clamp(zero.y * this.velocityScale, -1f, 1f) * 0.5f + 0.5f;
		this.simulationDataCpu[x][y].velocity = zero;
		float z2 = this.simulationDataCpu[x][y].velocityAccumulated.z;
		this.simulationDataCpu[x][y].velocityAccumulated = global::UnityEngine.Vector3.Lerp(this.simulationDataCpu[x][y].velocityAccumulated, this.simulationDataCpu[x][y].velocity, this.outputAccumulationRate);
		this.simulationDataCpu[x][y].velocityAccumulated.z = z2;
	}

	private void BlurVelocityAccumulatedHorizontalCpu(int x, int y)
	{
		global::UnityEngine.Vector3 velocityAccumulated = this.simulationDataCpu[global::UnityEngine.Mathf.Max(0, x - 1)][y].velocityAccumulated;
		global::UnityEngine.Vector3 velocityAccumulated2 = this.simulationDataCpu[global::UnityEngine.Mathf.Min(this.resolutionX - 1, x + 1)][y].velocityAccumulated;
		this.simulationDataCpu[x][y].velocityAccumulated = global::UnityEngine.Vector3.Lerp(this.simulationDataCpu[x][y].velocityAccumulated, velocityAccumulated * 0.25f + this.simulationDataCpu[x][y].velocityAccumulated * 0.5f + velocityAccumulated2 * 0.25f, this.outputFilterStrength);
	}

	private void BlurVelocityAccumulatedVerticalCpu(int x, int y)
	{
		global::UnityEngine.Vector3 velocityAccumulated = this.simulationDataCpu[x][global::UnityEngine.Mathf.Max(0, y - 1)].velocityAccumulated;
		global::UnityEngine.Vector3 velocityAccumulated2 = this.simulationDataCpu[x][global::UnityEngine.Mathf.Min(this.resolutionY - 1, y + 1)].velocityAccumulated;
		this.simulationDataCpu[x][y].velocityAccumulated = global::UnityEngine.Vector3.Lerp(this.simulationDataCpu[x][y].velocityAccumulated, velocityAccumulated * 0.25f + this.simulationDataCpu[x][y].velocityAccumulated * 0.5f + velocityAccumulated2 * 0.25f, this.outputFilterStrength);
	}

	private void FoamCpu(int x, int y)
	{
		int num = global::UnityEngine.Mathf.Min(y + 1, this.resolutionY - 1);
		int num2 = global::UnityEngine.Mathf.Min(x + 1, this.resolutionX - 1);
		int num3 = global::UnityEngine.Mathf.Max(y - 1, 0);
		int num4 = global::UnityEngine.Mathf.Max(x - 1, 0);
		global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(this.simulationDataCpu[x][y].velocityAccumulated.x * 2f - 1f, this.simulationDataCpu[x][y].velocityAccumulated.y * 2f - 1f);
		float magnitude = vector.magnitude;
		global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(this.simulationDataCpu[x][num].velocityAccumulated.x * 2f - 1f, this.simulationDataCpu[x][num].velocityAccumulated.y * 2f - 1f);
		float magnitude2 = vector2.magnitude;
		global::UnityEngine.Vector2 vector3 = new global::UnityEngine.Vector2(this.simulationDataCpu[num2][y].velocityAccumulated.x * 2f - 1f, this.simulationDataCpu[num2][y].velocityAccumulated.y * 2f - 1f);
		float magnitude3 = vector3.magnitude;
		global::UnityEngine.Vector2 vector4 = new global::UnityEngine.Vector2(this.simulationDataCpu[x][num3].velocityAccumulated.x * 2f - 1f, this.simulationDataCpu[x][num3].velocityAccumulated.y * 2f - 1f);
		float magnitude4 = vector4.magnitude;
		global::UnityEngine.Vector2 vector5 = new global::UnityEngine.Vector2(this.simulationDataCpu[num4][y].velocityAccumulated.x * 2f - 1f, this.simulationDataCpu[num4][y].velocityAccumulated.y * 2f - 1f);
		float magnitude5 = vector5.magnitude;
		float value = 100f * (magnitude2 - magnitude + (magnitude3 - magnitude) + (magnitude4 - magnitude) + (magnitude5 - magnitude));
		float num5 = 1f;
		global::UnityEngine.Vector2 vector6 = new global::UnityEngine.Vector2(this.simulationDataCpu[x][y].velocity.x * 2f - 1f, this.simulationDataCpu[x][y].velocity.y * 2f - 1f);
		float num6 = global::UnityEngine.Mathf.Pow(num5 - global::UnityEngine.Mathf.Clamp01(vector6.magnitude * this.foamVelocityScale), 2f);
		num6 *= 1f - this.simulationDataCpu[x][y].addFluid;
		num6 = (global::UnityEngine.Mathf.Clamp01((num6 * 1.2f - 0.5f) * 4f) + 0.5f) * global::UnityEngine.Mathf.Clamp01(value);
		this.simulationDataCpu[x][y].velocityAccumulated.z = global::UnityEngine.Mathf.Lerp(this.simulationDataCpu[x][y].velocityAccumulated.z, num6, this.outputAccumulationRate);
	}

	private void BakeAddFluidThreaded(object threadContext)
	{
		global::Flowmap.ThreadedFieldBakeInfo threadedFieldBakeInfo = threadContext as global::Flowmap.ThreadedFieldBakeInfo;
		try
		{
			for (int i = threadedFieldBakeInfo.start; i < threadedFieldBakeInfo.start + threadedFieldBakeInfo.length; i++)
			{
				for (int j = 0; j < this.resolutionY; j++)
				{
					foreach (global::FlowSimulationField flowSimulationField in threadedFieldBakeInfo.fields)
					{
						global::Flowmap.SimulationData[] array = this.simulationDataCpu[i];
						int num = j;
						array[num].addFluid = array[num].addFluid + flowSimulationField.GetStrengthCpu(threadedFieldBakeInfo.generator, new global::UnityEngine.Vector2((float)i / (float)this.resolutionX, (float)j / (float)this.resolutionY));
					}
				}
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.Log(ex.ToString());
		}
		threadedFieldBakeInfo.resetEvent.Set();
	}

	private void BakeRemoveFluidThreaded(object threadContext)
	{
		global::Flowmap.ThreadedFieldBakeInfo threadedFieldBakeInfo = threadContext as global::Flowmap.ThreadedFieldBakeInfo;
		try
		{
			for (int i = threadedFieldBakeInfo.start; i < threadedFieldBakeInfo.start + threadedFieldBakeInfo.length; i++)
			{
				for (int j = 0; j < this.resolutionY; j++)
				{
					foreach (global::FlowSimulationField flowSimulationField in threadedFieldBakeInfo.fields)
					{
						global::Flowmap.SimulationData[] array = this.simulationDataCpu[i];
						int num = j;
						array[num].removeFluid = array[num].removeFluid + flowSimulationField.GetStrengthCpu(threadedFieldBakeInfo.generator, new global::UnityEngine.Vector2((float)i / (float)this.resolutionX, (float)j / (float)this.resolutionY));
					}
				}
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.Log(ex.ToString());
		}
		threadedFieldBakeInfo.resetEvent.Set();
	}

	private void BakeForcesThreaded(object threadContext)
	{
		global::Flowmap.ThreadedFieldBakeInfo threadedFieldBakeInfo = threadContext as global::Flowmap.ThreadedFieldBakeInfo;
		try
		{
			for (int i = threadedFieldBakeInfo.start; i < threadedFieldBakeInfo.start + threadedFieldBakeInfo.length; i++)
			{
				for (int j = 0; j < this.resolutionY; j++)
				{
					foreach (global::FlowSimulationField flowSimulationField in threadedFieldBakeInfo.fields)
					{
						global::Flowmap.SimulationData[] array = this.simulationDataCpu[i];
						int num = j;
						array[num].force = array[num].force + (flowSimulationField as global::FlowForceField).GetForceCpu(threadedFieldBakeInfo.generator, new global::UnityEngine.Vector2((float)i / (float)this.resolutionX, (float)j / (float)this.resolutionY));
						this.simulationDataCpu[i][j].force.z = global::UnityEngine.Mathf.Max(this.simulationDataCpu[i][j].force.z, 0f);
					}
				}
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.Log(ex.ToString());
		}
		threadedFieldBakeInfo.resetEvent.Set();
	}

	private void BakeHeightmapThreaded(object threadContext)
	{
		global::Flowmap.ThreadedFieldBakeInfo threadedFieldBakeInfo = threadContext as global::Flowmap.ThreadedFieldBakeInfo;
		try
		{
			for (int i = threadedFieldBakeInfo.start; i < threadedFieldBakeInfo.start + threadedFieldBakeInfo.length; i++)
			{
				for (int j = 0; j < this.resolutionY; j++)
				{
					foreach (global::FlowSimulationField flowSimulationField in threadedFieldBakeInfo.fields)
					{
						float strengthCpu = flowSimulationField.GetStrengthCpu(threadedFieldBakeInfo.generator, new global::UnityEngine.Vector2((float)i / (float)this.resolutionX, (float)j / (float)this.resolutionY));
						this.simulationDataCpu[i][j].height = global::UnityEngine.Mathf.Lerp(this.simulationDataCpu[i][j].height, strengthCpu, strengthCpu * (1f - this.simulationDataCpu[i][j].height));
					}
				}
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.Log(ex.ToString());
		}
		threadedFieldBakeInfo.resetEvent.Set();
	}

	private void AddRemoveFluidThreaded(object threadContext)
	{
		global::Flowmap.ArrayThreadedInfo arrayThreadedInfo = threadContext as global::Flowmap.ArrayThreadedInfo;
		try
		{
			for (int i = arrayThreadedInfo.start; i < arrayThreadedInfo.start + arrayThreadedInfo.length; i++)
			{
				for (int j = 0; j < this.resolutionY; j++)
				{
					this.AddRemoveFluidCpu(i, j);
				}
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.Log(ex.ToString());
		}
		arrayThreadedInfo.resetEvent.Set();
	}

	private void OutflowThreaded(object threadContext)
	{
		global::Flowmap.ArrayThreadedInfo arrayThreadedInfo = threadContext as global::Flowmap.ArrayThreadedInfo;
		try
		{
			for (int i = arrayThreadedInfo.start; i < arrayThreadedInfo.start + arrayThreadedInfo.length; i++)
			{
				for (int j = 0; j < this.resolutionY; j++)
				{
					this.OutflowCpu(i, j);
				}
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.Log(ex.ToString());
		}
		arrayThreadedInfo.resetEvent.Set();
	}

	private void UpdateVelocityThreaded(object threadContext)
	{
		global::Flowmap.ArrayThreadedInfo arrayThreadedInfo = threadContext as global::Flowmap.ArrayThreadedInfo;
		try
		{
			for (int i = arrayThreadedInfo.start; i < arrayThreadedInfo.start + arrayThreadedInfo.length; i++)
			{
				for (int j = 0; j < this.resolutionY; j++)
				{
					this.UpdateVelocityCpu(i, j);
				}
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.Log(ex.ToString());
		}
		arrayThreadedInfo.resetEvent.Set();
	}

	private void BlurVelocityAccumulatedHorizontalThreaded(object threadContext)
	{
		global::Flowmap.ArrayThreadedInfo arrayThreadedInfo = threadContext as global::Flowmap.ArrayThreadedInfo;
		try
		{
			for (int i = arrayThreadedInfo.start; i < arrayThreadedInfo.start + arrayThreadedInfo.length; i++)
			{
				for (int j = 0; j < this.resolutionY; j++)
				{
					this.BlurVelocityAccumulatedHorizontalCpu(i, j);
				}
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.Log(ex.ToString());
		}
		arrayThreadedInfo.resetEvent.Set();
	}

	private void BlurVelocityAccumulatedVerticalThreaded(object threadContext)
	{
		global::Flowmap.ArrayThreadedInfo arrayThreadedInfo = threadContext as global::Flowmap.ArrayThreadedInfo;
		try
		{
			for (int i = arrayThreadedInfo.start; i < arrayThreadedInfo.start + arrayThreadedInfo.length; i++)
			{
				for (int j = 0; j < this.resolutionY; j++)
				{
					this.BlurVelocityAccumulatedVerticalCpu(i, j);
				}
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.Log(ex.ToString());
		}
		arrayThreadedInfo.resetEvent.Set();
	}

	private void FoamThreaded(object threadContext)
	{
		global::Flowmap.ArrayThreadedInfo arrayThreadedInfo = threadContext as global::Flowmap.ArrayThreadedInfo;
		try
		{
			for (int i = arrayThreadedInfo.start; i < arrayThreadedInfo.start + arrayThreadedInfo.length; i++)
			{
				for (int j = 0; j < this.resolutionY; j++)
				{
					this.FoamCpu(i, j);
				}
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.Log(ex.ToString());
		}
		arrayThreadedInfo.resetEvent.Set();
	}

	protected override void Update()
	{
		base.Update();
		this.AssignToMaterials();
	}

	private void WriteCpuDataToTexture()
	{
		if (this.heightFluidCpu == null || this.heightFluidCpu.width != this.resolutionX || this.heightFluidCpu.height != this.resolutionY)
		{
			if (this.heightFluidCpu)
			{
				this.DestroyProperly(this.heightFluidCpu);
			}
			this.heightFluidCpu = new global::UnityEngine.Texture2D(this.resolutionX, this.resolutionY, global::UnityEngine.TextureFormat.ARGB32, true, true);
			this.heightFluidCpu.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.heightFluidCpu.name = "HeightFluidCpu";
		}
		global::UnityEngine.Color[] array = new global::UnityEngine.Color[this.resolutionX * this.resolutionY];
		for (int i = 0; i < this.resolutionY; i++)
		{
			for (int j = 0; j < this.resolutionX; j++)
			{
				array[j + i * this.resolutionX] = new global::UnityEngine.Color(this.simulationDataCpu[j][i].height, this.simulationDataCpu[j][i].fluid, 0f, 1f);
			}
		}
		this.heightFluidCpu.SetPixels(array);
		this.heightFluidCpu.Apply();
		if (this.velocityAccumulatedCpu == null || this.velocityAccumulatedCpu.width != this.resolutionX || this.velocityAccumulatedCpu.height != this.resolutionY)
		{
			if (this.velocityAccumulatedCpu)
			{
				this.DestroyProperly(this.velocityAccumulatedCpu);
			}
			this.velocityAccumulatedCpu = new global::UnityEngine.Texture2D(this.resolutionX, this.resolutionY, global::UnityEngine.TextureFormat.ARGB32, true, true);
			this.velocityAccumulatedCpu.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			this.velocityAccumulatedCpu.name = "VelocityAccumulatedCpu";
		}
		for (int k = 0; k < this.resolutionY; k++)
		{
			for (int l = 0; l < this.resolutionX; l++)
			{
				array[l + k * this.resolutionX] = new global::UnityEngine.Color(this.simulationDataCpu[l][k].velocityAccumulated.x, this.simulationDataCpu[l][k].velocityAccumulated.y, this.simulationDataCpu[l][k].velocityAccumulated.z, 1f);
			}
		}
		this.velocityAccumulatedCpu.SetPixels(array);
		this.velocityAccumulatedCpu.Apply();
	}

	private void AssignToMaterials()
	{
		if (this.assignFlowmapToMaterials != null)
		{
			foreach (global::UnityEngine.Material material in this.assignFlowmapToMaterials)
			{
				if (!(material == null))
				{
					if (this.assignFlowmap)
					{
						material.SetTexture(this.assignedFlowmapName, (global::FlowmapGenerator.SimulationPath != global::Flowmap.SimulationPath.GPU) ? this.velocityAccumulatedCpu : this.velocityAccumulatedRT);
					}
					if (this.assignHeightAndFluid)
					{
						material.SetTexture(this.assignedHeightAndFluidName, (global::FlowmapGenerator.SimulationPath != global::Flowmap.SimulationPath.GPU) ? this.heightFluidCpu : this.heightFluidRT);
					}
					if (this.assignUVScaleTransform)
					{
						if (base.Generator.Dimensions.x < base.Generator.Dimensions.y)
						{
							float num = base.Generator.Dimensions.y / base.Generator.Dimensions.x;
							material.SetVector(this.assignUVCoordsName, new global::UnityEngine.Vector4(base.Generator.Dimensions.x * num, base.Generator.Dimensions.y, base.Generator.Position.x, base.Generator.Position.z));
						}
						else
						{
							float num2 = base.Generator.Dimensions.x / base.Generator.Dimensions.y;
							material.SetVector(this.assignUVCoordsName, new global::UnityEngine.Vector4(base.Generator.Dimensions.x, base.Generator.Dimensions.y * num2, base.Generator.Position.x, base.Generator.Position.z));
						}
					}
				}
			}
		}
	}

	public void WriteTextureToDisk(global::ShallowWaterSimulator.OutputTexture textureToWrite, string path)
	{
		global::Flowmap.SimulationPath simulationPath = global::FlowmapGenerator.SimulationPath;
		if (simulationPath != global::Flowmap.SimulationPath.GPU)
		{
			if (simulationPath == global::Flowmap.SimulationPath.CPU)
			{
				switch (textureToWrite)
				{
				case global::ShallowWaterSimulator.OutputTexture.Flowmap:
					if (this.writeFoamSeparately)
					{
						global::UnityEngine.Texture2D flowmapWithoutFoamTextureCPU = this.GetFlowmapWithoutFoamTextureCPU();
						global::Flowmap.TextureUtilities.WriteTexture2DToFile(flowmapWithoutFoamTextureCPU, path, global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
						if (global::UnityEngine.Application.isPlaying)
						{
							global::UnityEngine.Object.Destroy(flowmapWithoutFoamTextureCPU);
						}
						else
						{
							global::UnityEngine.Object.DestroyImmediate(flowmapWithoutFoamTextureCPU);
						}
					}
					else
					{
						global::Flowmap.TextureUtilities.WriteTexture2DToFile(this.velocityAccumulatedCpu, path, global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
					}
					break;
				case global::ShallowWaterSimulator.OutputTexture.HeightAndFluid:
					global::Flowmap.TextureUtilities.WriteTexture2DToFile(this.heightFluidCpu, path, global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
					break;
				case global::ShallowWaterSimulator.OutputTexture.Foam:
				{
					global::UnityEngine.Texture2D foamTextureCPU = this.GetFoamTextureCPU();
					global::Flowmap.TextureUtilities.WriteTexture2DToFile(foamTextureCPU, path, global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
					if (global::UnityEngine.Application.isPlaying)
					{
						global::UnityEngine.Object.Destroy(foamTextureCPU);
					}
					else
					{
						global::UnityEngine.Object.DestroyImmediate(foamTextureCPU);
					}
					break;
				}
				}
			}
		}
		else
		{
			switch (textureToWrite)
			{
			case global::ShallowWaterSimulator.OutputTexture.Flowmap:
				if (this.writeFoamSeparately)
				{
					global::UnityEngine.RenderTexture flowmapWithoutFoamRT = this.GetFlowmapWithoutFoamRT();
					global::Flowmap.TextureUtilities.WriteRenderTextureToFile(flowmapWithoutFoamRT, path, true, global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
					if (global::UnityEngine.Application.isPlaying)
					{
						global::UnityEngine.Object.Destroy(flowmapWithoutFoamRT);
					}
				}
				else
				{
					global::Flowmap.TextureUtilities.WriteRenderTextureToFile(this.velocityAccumulatedRT, path, true, global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
				}
				break;
			case global::ShallowWaterSimulator.OutputTexture.HeightAndFluid:
				global::Flowmap.TextureUtilities.WriteRenderTextureToFile(this.heightFluidRT, path, true, global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat], "Hidden/WriteHeightFluid");
				break;
			case global::ShallowWaterSimulator.OutputTexture.Foam:
			{
				global::UnityEngine.RenderTexture foamRT = this.GetFoamRT();
				global::Flowmap.TextureUtilities.WriteRenderTextureToFile(foamRT, path, true, global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
				if (global::UnityEngine.Application.isPlaying)
				{
					global::UnityEngine.Object.Destroy(foamRT);
				}
				else
				{
					global::UnityEngine.Object.DestroyImmediate(foamRT);
				}
				break;
			}
			}
		}
	}

	private global::UnityEngine.Texture2D GetFoamTextureCPU()
	{
		global::UnityEngine.Texture2D texture2D = new global::UnityEngine.Texture2D(this.resolutionX, this.resolutionY, global::UnityEngine.TextureFormat.ARGB32, true);
		texture2D.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
		global::UnityEngine.Color[] array = new global::UnityEngine.Color[this.resolutionX * this.resolutionY];
		for (int i = 0; i < this.resolutionY; i++)
		{
			for (int j = 0; j < this.resolutionX; j++)
			{
				array[j + i * this.resolutionX] = new global::UnityEngine.Color(this.simulationDataCpu[j][i].velocityAccumulated.z, this.simulationDataCpu[j][i].velocityAccumulated.z, this.simulationDataCpu[j][i].velocityAccumulated.z, 1f);
			}
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	private global::UnityEngine.Texture2D GetFlowmapWithoutFoamTextureCPU()
	{
		global::UnityEngine.Texture2D texture2D = new global::UnityEngine.Texture2D(this.resolutionX, this.resolutionY, global::UnityEngine.TextureFormat.ARGB32, true);
		texture2D.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
		global::UnityEngine.Color[] array = new global::UnityEngine.Color[this.resolutionX * this.resolutionY];
		for (int i = 0; i < this.resolutionY; i++)
		{
			for (int j = 0; j < this.resolutionX; j++)
			{
				array[j + i * this.resolutionX] = new global::UnityEngine.Color(this.simulationDataCpu[j][i].velocityAccumulated.x, this.simulationDataCpu[j][i].velocityAccumulated.y, 0f, 1f);
			}
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	private global::UnityEngine.RenderTexture GetFoamRT()
	{
		global::UnityEngine.RenderTexture renderTexture = new global::UnityEngine.RenderTexture(this.resolutionX, this.resolutionY, 0, global::UnityEngine.RenderTextureFormat.ARGB32, global::UnityEngine.RenderTextureReadWrite.Linear);
		global::UnityEngine.Graphics.Blit(this.velocityAccumulatedRT, renderTexture, this.SimulationMaterial, 12);
		return renderTexture;
	}

	private global::UnityEngine.RenderTexture GetFlowmapWithoutFoamRT()
	{
		global::UnityEngine.RenderTexture renderTexture = new global::UnityEngine.RenderTexture(this.resolutionX, this.resolutionY, 0, global::UnityEngine.RenderTextureFormat.ARGB32, global::UnityEngine.RenderTextureReadWrite.Linear);
		global::UnityEngine.Graphics.Blit(this.velocityAccumulatedRT, renderTexture, this.SimulationMaterial, 13);
		return renderTexture;
	}

	protected override void MaxStepsReached()
	{
		base.MaxStepsReached();
		if (this.writeToFileOnMaxSimulationSteps && !string.IsNullOrEmpty(this.outputFolderPath) && global::System.IO.Directory.Exists(this.outputFolderPath))
		{
			this.WriteAllTextures();
		}
		if (this.OnMaxStepsReached != null)
		{
			this.OnMaxStepsReached();
		}
	}

	public void WriteAllTextures()
	{
		global::Flowmap.SimulationPath simulationPath = global::FlowmapGenerator.SimulationPath;
		if (simulationPath != global::Flowmap.SimulationPath.GPU)
		{
			if (simulationPath == global::Flowmap.SimulationPath.CPU)
			{
				if (this.simulationDataCpu == null)
				{
					this.Init();
				}
				this.WriteCpuDataToTexture();
				if (this.writeHeightAndFluid)
				{
					global::Flowmap.TextureUtilities.WriteTexture2DToFile(this.heightFluidCpu, this.outputFolderPath + "/" + this.outputPrefix + "HeightAndFluid", global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
				}
				if (this.writeFoamSeparately)
				{
					global::UnityEngine.Texture2D foamTextureCPU = this.GetFoamTextureCPU();
					global::Flowmap.TextureUtilities.WriteTexture2DToFile(foamTextureCPU, this.outputFolderPath + "/" + this.outputPrefix + "Foam", global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
					if (global::UnityEngine.Application.isPlaying)
					{
						global::UnityEngine.Object.Destroy(foamTextureCPU);
					}
					else
					{
						global::UnityEngine.Object.DestroyImmediate(foamTextureCPU);
					}
					global::UnityEngine.Texture2D flowmapWithoutFoamTextureCPU = this.GetFlowmapWithoutFoamTextureCPU();
					global::Flowmap.TextureUtilities.WriteTexture2DToFile(flowmapWithoutFoamTextureCPU, this.outputFolderPath + "/" + this.outputPrefix + "Flowmap", global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
					if (global::UnityEngine.Application.isPlaying)
					{
						global::UnityEngine.Object.Destroy(flowmapWithoutFoamTextureCPU);
					}
					else
					{
						global::UnityEngine.Object.DestroyImmediate(flowmapWithoutFoamTextureCPU);
					}
				}
				else
				{
					global::Flowmap.TextureUtilities.WriteTexture2DToFile(this.velocityAccumulatedCpu, this.outputFolderPath + "/" + this.outputPrefix + "Flowmap", global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
				}
			}
		}
		else
		{
			if (this.writeHeightAndFluid)
			{
				global::Flowmap.TextureUtilities.WriteRenderTextureToFile(this.heightFluidRT, this.outputFolderPath + "/" + this.outputPrefix + "HeightAndFluid", true, global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat], "Hidden/WriteHeightFluid");
			}
			if (this.writeFoamSeparately)
			{
				global::UnityEngine.RenderTexture foamRT = this.GetFoamRT();
				global::Flowmap.TextureUtilities.WriteRenderTextureToFile(foamRT, this.outputFolderPath + "/" + this.outputPrefix + "Foam", global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
				if (global::UnityEngine.Application.isPlaying)
				{
					global::UnityEngine.Object.Destroy(foamRT);
				}
				else
				{
					global::UnityEngine.Object.DestroyImmediate(foamRT);
				}
				global::UnityEngine.RenderTexture flowmapWithoutFoamRT = this.GetFlowmapWithoutFoamRT();
				global::Flowmap.TextureUtilities.WriteRenderTextureToFile(flowmapWithoutFoamRT, this.outputFolderPath + "/" + this.outputPrefix + "Flowmap", global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
				if (global::UnityEngine.Application.isPlaying)
				{
					global::UnityEngine.Object.Destroy(flowmapWithoutFoamRT);
				}
				else
				{
					global::UnityEngine.Object.DestroyImmediate(flowmapWithoutFoamRT);
				}
			}
			else
			{
				global::Flowmap.TextureUtilities.WriteRenderTextureToFile(this.velocityAccumulatedRT, this.outputFolderPath + "/" + this.outputPrefix + "Flowmap", global::Flowmap.TextureUtilities.SupportedFormats[this.generator.outputFileFormat]);
			}
		}
	}

	public int updateTextureDelayCPU = 10;

	public float timestep = 0.4f;

	public float evaporationRate = 0.001f;

	public float gravity = 1f;

	public float velocityScale = 1f;

	public float fluidAddMultiplier = 0.01f;

	public float fluidRemoveMultiplier = 0.01f;

	public float fluidForceMultiplier = 0.01f;

	public float initialFluidAmount;

	public global::Flowmap.FluidDepth fluidDepth;

	public float outputAccumulationRate = 0.02f;

	private int outputFilterSize = 1;

	public float outputFilterStrength = 1f;

	public bool simulateFoam;

	public float foamVelocityScale = 1f;

	public bool simulateFirstFluidHit;

	public float firstFluidHitTimeMax = 30f;

	public global::UnityEngine.Material[] assignFlowmapToMaterials;

	public bool assignFlowmap;

	public string assignedFlowmapName = "_FlowmapTex";

	public bool assignHeightAndFluid;

	public string assignedHeightAndFluidName = "_HeightFluidTex";

	public bool assignUVScaleTransform;

	public string assignUVCoordsName = "_FlowmapUV";

	public bool writeHeightAndFluid;

	public bool writeFoamSeparately;

	public bool writeFluidDepthInAlpha;

	private global::UnityEngine.RenderTexture heightFluidRT;

	private global::UnityEngine.RenderTexture heightPreviewRT;

	private global::UnityEngine.RenderTexture fluidPreviewRT;

	private global::UnityEngine.RenderTexture fluidAddRT;

	private global::UnityEngine.RenderTexture fluidRemoveRT;

	private global::UnityEngine.RenderTexture fluidForceRT;

	private global::UnityEngine.RenderTexture heightmapFieldsRT;

	private global::UnityEngine.RenderTexture outflowRT;

	private global::UnityEngine.RenderTexture bufferRT1;

	private global::UnityEngine.RenderTexture velocityRT;

	private global::UnityEngine.RenderTexture velocityAccumulatedRT;

	private global::UnityEngine.Material simulationMaterial;

	private global::UnityEngine.Camera fieldRenderCamera;

	private bool initializedGpu;

	[global::UnityEngine.SerializeField]
	[global::UnityEngine.HideInInspector]
	private global::Flowmap.SimulationData[][] simulationDataCpu;

	private global::UnityEngine.Texture2D heightFluidCpu;

	private global::UnityEngine.Texture2D velocityAccumulatedCpu;

	private bool initializedCpu;

	public enum OutputTexture
	{
		Flowmap,
		HeightAndFluid,
		Foam
	}
}
