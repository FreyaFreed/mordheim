using System;
using UnityEngine;

namespace mset
{
	public class SkyProbe
	{
		public SkyProbe()
		{
			global::mset.SkyProbe.buildRandomValueTable();
		}

		public static void buildRandomValueTable()
		{
			if (global::mset.SkyProbe.randomValues == null)
			{
				float num = (float)global::mset.SkyProbe.sampleCount;
				global::mset.SkyProbe.randomValues = new global::UnityEngine.Vector4[global::mset.SkyProbe.sampleCount];
				float[] array = new float[global::mset.SkyProbe.sampleCount];
				for (int i = 0; i < global::mset.SkyProbe.sampleCount; i++)
				{
					global::mset.SkyProbe.randomValues[i] = default(global::UnityEngine.Vector4);
					array[i] = (global::mset.SkyProbe.randomValues[i].x = (float)(i + 1) / num);
				}
				int num2 = global::mset.SkyProbe.sampleCount;
				for (int j = 0; j < global::mset.SkyProbe.sampleCount; j++)
				{
					int num3 = global::UnityEngine.Random.Range(0, num2 - 1);
					float num4 = array[num3];
					array[num3] = array[--num2];
					global::mset.SkyProbe.randomValues[j].y = num4;
					global::mset.SkyProbe.randomValues[j].z = global::UnityEngine.Mathf.Cos(6.28318548f * num4);
					global::mset.SkyProbe.randomValues[j].w = global::UnityEngine.Mathf.Sin(6.28318548f * num4);
				}
			}
		}

		public static void bindRandomValueTable(global::UnityEngine.Material mat, string paramName, int inputFaceSize)
		{
			for (int i = 0; i < global::mset.SkyProbe.sampleCount; i++)
			{
				mat.SetVector(paramName + i, global::mset.SkyProbe.randomValues[i]);
			}
			float num = (float)(inputFaceSize * inputFaceSize) / (float)global::mset.SkyProbe.sampleCount;
			num = 0.5f * global::UnityEngine.Mathf.Log(num, 2f) + 0.5f;
			mat.SetFloat("_ImportantLog", num);
		}

		public static void buildRandomValueCode()
		{
		}

		public void blur(global::UnityEngine.Cubemap targetCube, global::UnityEngine.Texture sourceCube, bool dstRGBM, bool srcRGBM, bool linear)
		{
			if (sourceCube == null || targetCube == null)
			{
				return;
			}
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("_temp_probe");
			gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			gameObject.SetActive(true);
			global::UnityEngine.Camera camera = gameObject.AddComponent<global::UnityEngine.Camera>();
			camera.renderingPath = this.renderPath;
			camera.useOcclusionCulling = false;
			global::UnityEngine.Material material = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/Marmoset/RGBM Cube"));
			global::UnityEngine.Matrix4x4 identity = global::UnityEngine.Matrix4x4.identity;
			int num = this.maxExponent;
			bool flag = this.generateMipChain;
			this.maxExponent = 8 * num;
			this.generateMipChain = false;
			this.convolve_internal(targetCube, sourceCube, dstRGBM, srcRGBM, linear, camera, material, identity);
			this.convolve_internal(targetCube, targetCube, dstRGBM, dstRGBM, linear, camera, material, identity);
			this.maxExponent = num;
			this.generateMipChain = flag;
			global::mset.SkyManager skyManager = global::mset.SkyManager.Get();
			if (skyManager)
			{
				skyManager.GlobalSky = skyManager.GlobalSky;
			}
			global::UnityEngine.Object.DestroyImmediate(material);
			global::UnityEngine.Object.DestroyImmediate(gameObject);
		}

		public void convolve(global::UnityEngine.Cubemap targetCube, global::UnityEngine.Texture sourceCube, bool dstRGBM, bool srcRGBM, bool linear)
		{
			if (targetCube == null)
			{
				return;
			}
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("_temp_probe");
			gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			gameObject.SetActive(true);
			global::UnityEngine.Camera camera = gameObject.AddComponent<global::UnityEngine.Camera>();
			camera.renderingPath = this.renderPath;
			camera.useOcclusionCulling = false;
			global::UnityEngine.Material material = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/Marmoset/RGBM Cube"));
			global::UnityEngine.Matrix4x4 identity = global::UnityEngine.Matrix4x4.identity;
			this.copy_internal(targetCube, sourceCube, dstRGBM, srcRGBM, linear, camera, material, identity);
			int num = this.maxExponent;
			this.maxExponent = 2 * num;
			this.convolve_internal(targetCube, sourceCube, dstRGBM, srcRGBM, linear, camera, material, identity);
			this.maxExponent = 8 * num;
			this.convolve_internal(targetCube, targetCube, dstRGBM, dstRGBM, linear, camera, material, identity);
			this.maxExponent = num;
			global::mset.SkyManager skyManager = global::mset.SkyManager.Get();
			if (skyManager)
			{
				skyManager.GlobalSky = skyManager.GlobalSky;
			}
			global::UnityEngine.Object.DestroyImmediate(material);
			global::UnityEngine.Object.DestroyImmediate(gameObject);
		}

		public bool capture(global::UnityEngine.Texture targetCube, global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation, bool HDR, bool linear, bool convolve)
		{
			if (targetCube == null)
			{
				return false;
			}
			bool flag = false;
			if (this.cubeRT == null)
			{
				flag = true;
				this.cubeRT = global::UnityEngine.RenderTexture.GetTemporary(targetCube.width, targetCube.width, 24, global::UnityEngine.RenderTextureFormat.ARGBHalf, global::UnityEngine.RenderTextureReadWrite.Linear);
				this.cubeRT.Release();
				this.cubeRT.isCubemap = true;
				this.cubeRT.useMipMap = true;
				this.cubeRT.generateMips = true;
				this.cubeRT.Create();
				if (!this.cubeRT.IsCreated() && !this.cubeRT.Create())
				{
					this.cubeRT = global::UnityEngine.RenderTexture.GetTemporary(targetCube.width, targetCube.width, 24, global::UnityEngine.RenderTextureFormat.Default, global::UnityEngine.RenderTextureReadWrite.Linear);
					this.cubeRT.Release();
					this.cubeRT.isCubemap = true;
					this.cubeRT.useMipMap = true;
					this.cubeRT.generateMips = true;
					this.cubeRT.Create();
				}
			}
			if (!this.cubeRT.IsCreated() && !this.cubeRT.Create())
			{
				return false;
			}
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("_temp_probe");
			global::UnityEngine.Camera camera = gameObject.AddComponent<global::UnityEngine.Camera>();
			global::mset.SkyManager skyManager = global::mset.SkyManager.Get();
			if (skyManager && skyManager.ProbeCamera)
			{
				camera.CopyFrom(skyManager.ProbeCamera);
			}
			else if (global::UnityEngine.Camera.main)
			{
				camera.CopyFrom(global::UnityEngine.Camera.main);
			}
			camera.renderingPath = this.renderPath;
			camera.useOcclusionCulling = false;
			camera.hdr = true;
			gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			gameObject.SetActive(true);
			gameObject.transform.position = position;
			global::UnityEngine.Shader.SetGlobalVector("_UniformOcclusion", this.exposures);
			camera.RenderToCubemap(this.cubeRT);
			global::UnityEngine.Shader.SetGlobalVector("_UniformOcclusion", global::UnityEngine.Vector4.one);
			global::UnityEngine.Matrix4x4 identity = global::UnityEngine.Matrix4x4.identity;
			identity.SetTRS(position, rotation, global::UnityEngine.Vector3.one);
			global::UnityEngine.Material material = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/Marmoset/RGBM Cube"));
			bool srcRGBM = false;
			this.copy_internal(targetCube, this.cubeRT, HDR, srcRGBM, linear, camera, material, identity);
			if (convolve)
			{
				this.convolve_internal(targetCube, this.cubeRT, HDR, false, linear, camera, material, identity);
			}
			if (skyManager)
			{
				skyManager.GlobalSky = skyManager.GlobalSky;
			}
			global::UnityEngine.Object.DestroyImmediate(material);
			global::UnityEngine.Object.DestroyImmediate(gameObject);
			if (flag)
			{
				global::UnityEngine.Object.DestroyImmediate(this.cubeRT);
			}
			return true;
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

		private void copy_internal(global::UnityEngine.Texture dstCube, global::UnityEngine.Texture srcCube, bool dstRGBM, bool srcRGBM, bool linear, global::UnityEngine.Camera cam, global::UnityEngine.Material skyMat, global::UnityEngine.Matrix4x4 matrix)
		{
			bool hdr = cam.hdr;
			global::UnityEngine.CameraClearFlags clearFlags = cam.clearFlags;
			int cullingMask = cam.cullingMask;
			cam.clearFlags = global::UnityEngine.CameraClearFlags.Skybox;
			cam.cullingMask = 0;
			cam.hdr = !dstRGBM;
			skyMat.name = "Internal HDR to RGBM Skybox";
			skyMat.shader = global::UnityEngine.Shader.Find("Hidden/Marmoset/RGBM Cube");
			global::mset.SkyProbe.toggleKeywordPair("MARMO_RGBM_INPUT_ON", "MARMO_RGBM_INPUT_OFF", srcRGBM);
			global::mset.SkyProbe.toggleKeywordPair("MARMO_RGBM_OUTPUT_ON", "MARMO_RGBM_OUTPUT_OFF", dstRGBM);
			skyMat.SetMatrix("_SkyMatrix", matrix);
			skyMat.SetTexture("_CubeHDR", srcCube);
			global::UnityEngine.Material skybox = global::UnityEngine.RenderSettings.skybox;
			global::UnityEngine.RenderSettings.skybox = skyMat;
			global::UnityEngine.RenderTexture renderTexture = dstCube as global::UnityEngine.RenderTexture;
			global::UnityEngine.Cubemap cubemap = dstCube as global::UnityEngine.Cubemap;
			if (renderTexture)
			{
				cam.RenderToCubemap(renderTexture);
			}
			else if (cubemap)
			{
				cam.RenderToCubemap(cubemap);
			}
			cam.hdr = hdr;
			cam.clearFlags = clearFlags;
			cam.cullingMask = cullingMask;
			global::UnityEngine.RenderSettings.skybox = skybox;
		}

		private void convolve_internal(global::UnityEngine.Texture dstTex, global::UnityEngine.Texture srcCube, bool dstRGBM, bool srcRGBM, bool linear, global::UnityEngine.Camera cam, global::UnityEngine.Material skyMat, global::UnityEngine.Matrix4x4 matrix)
		{
			bool hdr = cam.hdr;
			global::UnityEngine.CameraClearFlags clearFlags = cam.clearFlags;
			int cullingMask = cam.cullingMask;
			cam.clearFlags = global::UnityEngine.CameraClearFlags.Skybox;
			cam.cullingMask = 0;
			cam.hdr = !dstRGBM;
			skyMat.name = "Internal Convolve Skybox";
			skyMat.shader = global::UnityEngine.Shader.Find("Hidden/Marmoset/RGBM Convolve");
			global::mset.SkyProbe.toggleKeywordPair("MARMO_RGBM_INPUT_ON", "MARMO_RGBM_INPUT_OFF", srcRGBM);
			global::mset.SkyProbe.toggleKeywordPair("MARMO_RGBM_OUTPUT_ON", "MARMO_RGBM_OUTPUT_OFF", dstRGBM);
			skyMat.SetMatrix("_SkyMatrix", matrix);
			skyMat.SetTexture("_CubeHDR", srcCube);
			global::mset.SkyProbe.bindRandomValueTable(skyMat, "_PhongRands", srcCube.width);
			global::UnityEngine.Material skybox = global::UnityEngine.RenderSettings.skybox;
			global::UnityEngine.RenderSettings.skybox = skyMat;
			global::UnityEngine.Cubemap cubemap = dstTex as global::UnityEngine.Cubemap;
			global::UnityEngine.RenderTexture renderTexture = dstTex as global::UnityEngine.RenderTexture;
			if (cubemap)
			{
				if (this.generateMipChain)
				{
					int num = global::mset.QPow.Log2i(cubemap.width) - 1;
					for (int i = (!this.highestMipIsMirror) ? 0 : 1; i < num; i++)
					{
						int size = 1 << num - i;
						float value = (float)global::mset.QPow.clampedDownShift(this.maxExponent, (!this.highestMipIsMirror) ? i : (i - 1), 1);
						skyMat.SetFloat("_SpecularExp", value);
						skyMat.SetFloat("_SpecularScale", this.convolutionScale);
						global::UnityEngine.Cubemap cubemap2 = new global::UnityEngine.Cubemap(size, cubemap.format, false);
						cam.RenderToCubemap(cubemap2);
						for (int j = 0; j < 6; j++)
						{
							global::UnityEngine.CubemapFace face = (global::UnityEngine.CubemapFace)j;
							cubemap.SetPixels(cubemap2.GetPixels(face), face, i);
						}
						global::UnityEngine.Object.DestroyImmediate(cubemap2);
					}
					cubemap.Apply(false);
				}
				else
				{
					skyMat.SetFloat("_SpecularExp", (float)this.maxExponent);
					skyMat.SetFloat("_SpecularScale", this.convolutionScale);
					cam.RenderToCubemap(cubemap);
				}
			}
			else if (renderTexture)
			{
				skyMat.SetFloat("_SpecularExp", (float)this.maxExponent);
				skyMat.SetFloat("_SpecularScale", this.convolutionScale);
				cam.RenderToCubemap(renderTexture);
			}
			cam.clearFlags = clearFlags;
			cam.cullingMask = cullingMask;
			cam.hdr = hdr;
			global::UnityEngine.RenderSettings.skybox = skybox;
		}

		public global::UnityEngine.RenderTexture cubeRT;

		public int maxExponent = 512;

		public global::UnityEngine.Vector4 exposures = global::UnityEngine.Vector4.one;

		public bool generateMipChain = true;

		public bool highestMipIsMirror = true;

		public float convolutionScale = 1f;

		public global::UnityEngine.RenderingPath renderPath = global::UnityEngine.RenderingPath.Forward;

		private static int sampleCount = 128;

		private static global::UnityEngine.Vector4[] randomValues;
	}
}
