using System;
using System.Collections.Generic;
using UnityEngine;

namespace mset
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Camera))]
	public class FreeProbe : global::UnityEngine.MonoBehaviour
	{
		private global::UnityEngine.Cubemap targetCube
		{
			get
			{
				return this._targetCube;
			}
			set
			{
				this._targetCube = value;
				this.UpdateFaceTexture();
			}
		}

		private void UpdateFaceTexture()
		{
			if (this._targetCube == null)
			{
				return;
			}
			if (this.faceTexture == null || this.faceTexture.width != this._targetCube.width)
			{
				if (this.faceTexture)
				{
					global::UnityEngine.Object.DestroyImmediate(this.faceTexture);
				}
				this.faceTexture = new global::UnityEngine.Texture2D(this._targetCube.width, this._targetCube.width, global::UnityEngine.TextureFormat.ARGB32, true, false);
				this.RT = global::UnityEngine.RenderTexture.GetTemporary(this._targetCube.width, this._targetCube.width, 24, global::UnityEngine.RenderTextureFormat.ARGBHalf, global::UnityEngine.RenderTextureReadWrite.Linear);
				this.RT.Release();
				this.RT.isCubemap = false;
				this.RT.useMipMap = false;
				this.RT.generateMips = false;
				this.RT.Create();
				if (!this.RT.IsCreated() && !this.RT.Create())
				{
					global::UnityEngine.Debug.LogWarning("Failed to create HDR RenderTexture, capturing in LDR mode.");
					global::UnityEngine.RenderTexture.ReleaseTemporary(this.RT);
					this.RT = null;
				}
			}
		}

		private void FreeFaceTexture()
		{
			if (this.faceTexture)
			{
				global::UnityEngine.Object.DestroyImmediate(this.faceTexture);
				this.faceTexture = null;
			}
			if (this.RT)
			{
				if (global::UnityEngine.RenderTexture.active == this.RT)
				{
					global::UnityEngine.RenderTexture.active = null;
				}
				global::UnityEngine.RenderTexture.ReleaseTemporary(this.RT);
				this.RT = null;
			}
			this.probeQueue = null;
		}

		private void Start()
		{
			this.UpdateFaceTexture();
			this.convolveSkybox = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/Marmoset/RGBM Convolve"));
			this.convolveSkybox.name = "Internal Convolution Skybox";
		}

		private void Awake()
		{
			this.sceneSkybox = global::UnityEngine.RenderSettings.skybox;
			global::mset.SkyManager skyManager = global::mset.SkyManager.Get();
			if (skyManager && skyManager.ProbeCamera)
			{
				base.GetComponent<global::UnityEngine.Camera>().CopyFrom(skyManager.ProbeCamera);
			}
			else if (global::UnityEngine.Camera.main)
			{
				base.GetComponent<global::UnityEngine.Camera>().CopyFrom(global::UnityEngine.Camera.main);
			}
		}

		public void QueueSkies(global::mset.Sky[] skiesToProbe)
		{
			if (this.probeQueue == null)
			{
				this.probeQueue = new global::System.Collections.Generic.Queue<global::mset.FreeProbe.ProbeTarget>();
			}
			else
			{
				this.probeQueue.Clear();
			}
			foreach (global::mset.Sky sky in skiesToProbe)
			{
				if (sky != null && sky.SpecularCube as global::UnityEngine.Cubemap != null)
				{
					this.QueueCubemap(sky.SpecularCube as global::UnityEngine.Cubemap, sky.HDRSpec, sky.transform.position, sky.transform.rotation);
				}
			}
		}

		public void QueueCubemap(global::UnityEngine.Cubemap cube, bool HDR, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot)
		{
			if (cube != null)
			{
				global::mset.FreeProbe.ProbeTarget probeTarget = new global::mset.FreeProbe.ProbeTarget();
				probeTarget.cube = cube;
				probeTarget.position = pos;
				probeTarget.rotation = rot;
				probeTarget.HDR = HDR;
				this.probeQueue.Enqueue(probeTarget);
				this.progressTotal++;
			}
		}

		private void ClearQueue()
		{
			this.probeQueue = null;
			this.progressTotal = 0;
			this.progress = 0;
		}

		public void RunQueue()
		{
			this.probeQueue.Enqueue(null);
			global::mset.SkyProbe.buildRandomValueTable();
			global::mset.SkyManager skyManager = global::mset.SkyManager.Get();
			if (skyManager.ProbeCamera)
			{
				base.GetComponent<global::UnityEngine.Camera>().CopyFrom(skyManager.ProbeCamera);
				this.defaultCullMask = skyManager.ProbeCamera.cullingMask;
			}
			else if (global::UnityEngine.Camera.main)
			{
				base.GetComponent<global::UnityEngine.Camera>().CopyFrom(global::UnityEngine.Camera.main);
				this.defaultCullMask = base.GetComponent<global::UnityEngine.Camera>().cullingMask;
			}
			this.disabledCameras.Clear();
			foreach (global::UnityEngine.Camera camera in global::UnityEngine.Camera.allCameras)
			{
				if (camera.enabled)
				{
					camera.enabled = false;
					this.disabledCameras.Add(camera);
				}
			}
			base.GetComponent<global::UnityEngine.Camera>().enabled = true;
			base.GetComponent<global::UnityEngine.Camera>().fieldOfView = 90f;
			base.GetComponent<global::UnityEngine.Camera>().clearFlags = global::UnityEngine.CameraClearFlags.Skybox;
			base.GetComponent<global::UnityEngine.Camera>().cullingMask = this.defaultCullMask;
			base.GetComponent<global::UnityEngine.Camera>().useOcclusionCulling = false;
			this.StartStage(global::mset.FreeProbe.Stage.NEXTSKY);
		}

		private void StartStage(global::mset.FreeProbe.Stage nextStage)
		{
			if (this.probeQueue == null)
			{
				nextStage = global::mset.FreeProbe.Stage.DONE;
			}
			if (nextStage == global::mset.FreeProbe.Stage.NEXTSKY)
			{
				global::UnityEngine.RenderSettings.skybox = this.sceneSkybox;
				global::mset.FreeProbe.ProbeTarget probeTarget = this.probeQueue.Dequeue();
				if (probeTarget != null)
				{
					this.progress++;
					if (this.ProgressCallback != null && this.progressTotal > 0)
					{
						this.ProgressCallback((float)this.progress / (float)this.progressTotal);
					}
					this.targetCube = probeTarget.cube;
					this.captureHDR = (probeTarget.HDR && this.RT != null);
					this.lookPos = probeTarget.position;
					this.lookRot = probeTarget.rotation;
				}
				else
				{
					nextStage = global::mset.FreeProbe.Stage.DONE;
				}
			}
			if (nextStage == global::mset.FreeProbe.Stage.CAPTURE)
			{
				this.drawShot = -1;
				global::UnityEngine.RenderSettings.skybox = this.sceneSkybox;
				this.targetMip = 0;
				this.captureSize = this.targetCube.width;
				this.mipCount = global::mset.QPow.Log2i(this.captureSize) - 1;
				base.GetComponent<global::UnityEngine.Camera>().cullingMask = this.defaultCullMask;
			}
			if (nextStage == global::mset.FreeProbe.Stage.CONVOLVE)
			{
				global::UnityEngine.Shader.SetGlobalVector("_UniformOcclusion", global::UnityEngine.Vector4.one);
				this.drawShot = 0;
				this.targetMip = 1;
				if (this.targetMip < this.mipCount)
				{
					base.GetComponent<global::UnityEngine.Camera>().cullingMask = 0;
					global::UnityEngine.RenderSettings.skybox = this.convolveSkybox;
					global::UnityEngine.Matrix4x4 identity = global::UnityEngine.Matrix4x4.identity;
					this.convolveSkybox.SetMatrix("_SkyMatrix", identity);
					this.convolveSkybox.SetTexture("_CubeHDR", this.targetCube);
					global::mset.FreeProbe.toggleKeywordPair("MARMO_RGBM_INPUT_ON", "MARMO_RGBM_INPUT_OFF", this.captureHDR && this.RT != null);
					global::mset.FreeProbe.toggleKeywordPair("MARMO_RGBM_OUTPUT_ON", "MARMO_RGBM_OUTPUT_OFF", this.captureHDR && this.RT != null);
					global::mset.SkyProbe.bindRandomValueTable(this.convolveSkybox, "_PhongRands", this.targetCube.width);
				}
			}
			if (nextStage == global::mset.FreeProbe.Stage.DONE)
			{
				global::UnityEngine.RenderSettings.skybox = this.sceneSkybox;
				this.ClearQueue();
				this.FreeFaceTexture();
				foreach (global::UnityEngine.Camera camera in this.disabledCameras)
				{
					camera.enabled = true;
				}
				this.disabledCameras.Clear();
				if (this.DoneCallback != null)
				{
					this.DoneCallback();
					this.DoneCallback = null;
				}
			}
			this.stage = nextStage;
		}

		private void OnPreCull()
		{
			if (this.stage == global::mset.FreeProbe.Stage.CAPTURE || this.stage == global::mset.FreeProbe.Stage.CONVOLVE || this.stage == global::mset.FreeProbe.Stage.PRECAPTURE)
			{
				if (this.stage == global::mset.FreeProbe.Stage.CONVOLVE)
				{
					this.captureSize = 1 << this.mipCount - this.targetMip;
					float value = (float)global::mset.QPow.clampedDownShift(this.maxExponent, this.targetMip - 1, 1);
					this.convolveSkybox.SetFloat("_SpecularExp", value);
					this.convolveSkybox.SetFloat("_SpecularScale", this.convolutionScale);
				}
				if (this.stage == global::mset.FreeProbe.Stage.CAPTURE || this.stage == global::mset.FreeProbe.Stage.PRECAPTURE)
				{
					global::UnityEngine.Shader.SetGlobalVector("_UniformOcclusion", this.exposures);
				}
				int num = this.captureSize;
				float width = (float)num / (float)global::UnityEngine.Screen.width;
				float height = (float)num / (float)global::UnityEngine.Screen.height;
				base.GetComponent<global::UnityEngine.Camera>().rect = new global::UnityEngine.Rect(0f, 0f, width, height);
				base.GetComponent<global::UnityEngine.Camera>().pixelRect = new global::UnityEngine.Rect(0f, 0f, (float)num, (float)num);
				base.transform.position = this.lookPos;
				base.transform.rotation = this.lookRot;
				if (this.stage == global::mset.FreeProbe.Stage.CAPTURE || this.stage == global::mset.FreeProbe.Stage.PRECAPTURE)
				{
					this.upLook = base.transform.up;
					this.forwardLook = base.transform.forward;
					this.rightLook = base.transform.right;
				}
				else
				{
					this.upLook = global::UnityEngine.Vector3.up;
					this.forwardLook = global::UnityEngine.Vector3.forward;
					this.rightLook = global::UnityEngine.Vector3.right;
				}
				if (this.drawShot == 0)
				{
					base.transform.LookAt(this.lookPos + this.forwardLook, this.upLook);
				}
				else if (this.drawShot == 1)
				{
					base.transform.LookAt(this.lookPos - this.forwardLook, this.upLook);
				}
				else if (this.drawShot == 2)
				{
					base.transform.LookAt(this.lookPos - this.rightLook, this.upLook);
				}
				else if (this.drawShot == 3)
				{
					base.transform.LookAt(this.lookPos + this.rightLook, this.upLook);
				}
				else if (this.drawShot == 4)
				{
					base.transform.LookAt(this.lookPos + this.upLook, this.forwardLook);
				}
				else if (this.drawShot == 5)
				{
					base.transform.LookAt(this.lookPos - this.upLook, -this.forwardLook);
				}
				base.GetComponent<global::UnityEngine.Camera>().ResetWorldToCameraMatrix();
			}
		}

		private void Update()
		{
			this.frameID++;
			if (this.RT && this.captureHDR && this.stage == global::mset.FreeProbe.Stage.CAPTURE)
			{
				this.stage = global::mset.FreeProbe.Stage.PRECAPTURE;
				bool hdr = base.GetComponent<global::UnityEngine.Camera>().hdr;
				base.GetComponent<global::UnityEngine.Camera>().hdr = true;
				global::UnityEngine.RenderTexture.active = global::UnityEngine.RenderTexture.active;
				global::UnityEngine.RenderTexture.active = this.RT;
				base.GetComponent<global::UnityEngine.Camera>().targetTexture = this.RT;
				base.GetComponent<global::UnityEngine.Camera>().Render();
				base.GetComponent<global::UnityEngine.Camera>().hdr = hdr;
				base.GetComponent<global::UnityEngine.Camera>().targetTexture = null;
				global::UnityEngine.RenderTexture.active = null;
				this.stage = global::mset.FreeProbe.Stage.CAPTURE;
			}
		}

		private void OnPostRender()
		{
			if (this.captureHDR && this.RT && this.stage == global::mset.FreeProbe.Stage.CAPTURE)
			{
				int width = this.RT.width;
				int num = 0;
				int num2 = 0;
				if (!this.blitMat)
				{
					this.blitMat = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/Marmoset/RGBM Blit"));
				}
				global::mset.FreeProbe.toggleKeywordPair("MARMO_RGBM_INPUT_ON", "MARMO_RGBM_INPUT_OFF", false);
				global::mset.FreeProbe.toggleKeywordPair("MARMO_RGBM_OUTPUT_ON", "MARMO_RGBM_OUTPUT_OFF", true);
				global::UnityEngine.GL.PushMatrix();
				global::UnityEngine.GL.LoadPixelMatrix(0f, (float)width, (float)width, 0f);
				global::UnityEngine.Graphics.DrawTexture(new global::UnityEngine.Rect((float)num, (float)num2, (float)width, (float)width), this.RT, this.blitMat);
				global::UnityEngine.GL.PopMatrix();
			}
			if (this.stage != global::mset.FreeProbe.Stage.NEXTSKY)
			{
				if (this.stage == global::mset.FreeProbe.Stage.CAPTURE || this.stage == global::mset.FreeProbe.Stage.CONVOLVE)
				{
					int num3 = this.captureSize;
					bool convertHDR = !this.captureHDR;
					if (num3 > global::UnityEngine.Screen.width || num3 > global::UnityEngine.Screen.height)
					{
						global::UnityEngine.Debug.LogWarning(string.Concat(new object[]
						{
							"<b>Skipping Cubemap</b> - The viewport is too small (",
							global::UnityEngine.Screen.width,
							"x",
							global::UnityEngine.Screen.height,
							") to probe the cubemap \"",
							this.targetCube.name,
							"\" (",
							num3,
							"x",
							num3,
							")"
						}));
						this.StartStage(global::mset.FreeProbe.Stage.NEXTSKY);
						return;
					}
					if (this.drawShot == 0)
					{
						this.faceTexture.ReadPixels(new global::UnityEngine.Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						global::mset.FreeProbe.SetFacePixels(this.targetCube, global::UnityEngine.CubemapFace.PositiveZ, this.faceTexture, this.targetMip, false, true, convertHDR);
					}
					else if (this.drawShot == 1)
					{
						this.faceTexture.ReadPixels(new global::UnityEngine.Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						global::mset.FreeProbe.SetFacePixels(this.targetCube, global::UnityEngine.CubemapFace.NegativeZ, this.faceTexture, this.targetMip, false, true, convertHDR);
					}
					else if (this.drawShot == 2)
					{
						this.faceTexture.ReadPixels(new global::UnityEngine.Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						global::mset.FreeProbe.SetFacePixels(this.targetCube, global::UnityEngine.CubemapFace.NegativeX, this.faceTexture, this.targetMip, false, true, convertHDR);
					}
					else if (this.drawShot == 3)
					{
						this.faceTexture.ReadPixels(new global::UnityEngine.Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						global::mset.FreeProbe.SetFacePixels(this.targetCube, global::UnityEngine.CubemapFace.PositiveX, this.faceTexture, this.targetMip, false, true, convertHDR);
					}
					else if (this.drawShot == 4)
					{
						this.faceTexture.ReadPixels(new global::UnityEngine.Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						global::mset.FreeProbe.SetFacePixels(this.targetCube, global::UnityEngine.CubemapFace.PositiveY, this.faceTexture, this.targetMip, true, false, convertHDR);
					}
					else if (this.drawShot == 5)
					{
						this.faceTexture.ReadPixels(new global::UnityEngine.Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						global::mset.FreeProbe.SetFacePixels(this.targetCube, global::UnityEngine.CubemapFace.NegativeY, this.faceTexture, this.targetMip, true, false, convertHDR);
						if (this.stage == global::mset.FreeProbe.Stage.CAPTURE)
						{
							this.targetCube.Apply(true, false);
							this.StartStage(global::mset.FreeProbe.Stage.CONVOLVE);
							return;
						}
						this.targetCube.Apply(false, false);
						this.targetMip++;
						if (this.targetMip < this.mipCount)
						{
							this.drawShot = 0;
							return;
						}
						this.StartStage(global::mset.FreeProbe.Stage.NEXTSKY);
						return;
					}
					this.drawShot++;
				}
				return;
			}
			if (this.targetCube != null)
			{
				this.StartStage(global::mset.FreeProbe.Stage.CAPTURE);
				return;
			}
			this.StartStage(global::mset.FreeProbe.Stage.DONE);
		}

		private static void SetFacePixels(global::UnityEngine.Cubemap cube, global::UnityEngine.CubemapFace face, global::UnityEngine.Texture2D tex, int mip, bool flipHorz, bool flipVert, bool convertHDR)
		{
			global::UnityEngine.Color[] pixels = tex.GetPixels();
			global::UnityEngine.Color color = global::UnityEngine.Color.black;
			int num = tex.width >> mip;
			int num2 = tex.height >> mip;
			global::UnityEngine.Color[] array = new global::UnityEngine.Color[num * num2];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					int num3 = i + j * tex.width;
					int num4 = i + j * num;
					array[num4] = pixels[num3];
					if (convertHDR)
					{
						array[num4].a = 0.166666672f;
					}
				}
			}
			if (flipHorz)
			{
				for (int k = 0; k < num / 2; k++)
				{
					for (int l = 0; l < num2; l++)
					{
						int num5 = num - k - 1;
						int num6 = k + l * num;
						int num7 = num5 + l * num;
						color = array[num7];
						array[num7] = array[num6];
						array[num6] = color;
					}
				}
			}
			if (flipVert)
			{
				for (int m = 0; m < num; m++)
				{
					for (int n = 0; n < num2 / 2; n++)
					{
						int num8 = num2 - n - 1;
						int num9 = m + n * num;
						int num10 = m + num8 * num;
						color = array[num10];
						array[num10] = array[num9];
						array[num9] = color;
					}
				}
			}
			cube.SetPixels(array, face, mip);
		}

		private static void toggleKeywordPair(string on, string off, bool yes)
		{
			if (yes)
			{
				global::UnityEngine.Shader.EnableKeyword(on);
				global::UnityEngine.Shader.DisableKeyword(off);
			}
			else
			{
				global::UnityEngine.Shader.EnableKeyword(off);
				global::UnityEngine.Shader.DisableKeyword(on);
			}
		}

		private static void toggleKeywordPair(global::UnityEngine.Material mat, string on, string off, bool yes)
		{
			if (yes)
			{
				mat.EnableKeyword(on);
				mat.DisableKeyword(off);
			}
			else
			{
				mat.EnableKeyword(off);
				mat.DisableKeyword(on);
			}
		}

		private global::UnityEngine.RenderTexture RT;

		public global::System.Action<float> ProgressCallback;

		public global::System.Action DoneCallback;

		public bool linear = true;

		public int maxExponent = 512;

		public global::UnityEngine.Vector4 exposures = global::UnityEngine.Vector4.one;

		public float convolutionScale = 1f;

		private global::System.Collections.Generic.List<global::UnityEngine.Camera> disabledCameras = new global::System.Collections.Generic.List<global::UnityEngine.Camera>();

		private global::UnityEngine.Cubemap _targetCube;

		private global::UnityEngine.Texture2D faceTexture;

		private global::mset.FreeProbe.Stage stage = global::mset.FreeProbe.Stage.DONE;

		private int drawShot;

		private int targetMip;

		private int mipCount;

		private int captureSize;

		private bool captureHDR = true;

		private int progress;

		private int progressTotal;

		private global::UnityEngine.Vector3 lookPos = global::UnityEngine.Vector3.zero;

		private global::UnityEngine.Quaternion lookRot = global::UnityEngine.Quaternion.identity;

		private global::UnityEngine.Vector3 forwardLook = global::UnityEngine.Vector3.forward;

		private global::UnityEngine.Vector3 rightLook = global::UnityEngine.Vector3.right;

		private global::UnityEngine.Vector3 upLook = global::UnityEngine.Vector3.up;

		private global::System.Collections.Generic.Queue<global::mset.FreeProbe.ProbeTarget> probeQueue;

		private int defaultCullMask = -1;

		private global::UnityEngine.Material sceneSkybox;

		private global::UnityEngine.Material convolveSkybox;

		private int frameID;

		private global::UnityEngine.Material blitMat;

		private enum Stage
		{
			NEXTSKY,
			PRECAPTURE,
			CAPTURE,
			CONVOLVE,
			DONE
		}

		private class ProbeTarget
		{
			public global::UnityEngine.Cubemap cube;

			public bool HDR;

			public global::UnityEngine.Vector3 position = global::UnityEngine.Vector3.zero;

			public global::UnityEngine.Quaternion rotation = global::UnityEngine.Quaternion.identity;
		}
	}
}
