using System;
using System.Collections.Generic;
using Prometheus;
using UnityEngine;

public class SkinnedMeshCombiner : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
	}

	public void MergeNoAtlas()
	{
		global::PandoraDebug.LogInfo("Merging " + base.name + " !", "GRAPHICS", this);
		this.PrePass();
		this.AttachAttachers();
		global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::UnityEngine.CombineInstance>> list = new global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::UnityEngine.CombineInstance>>();
		global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::UnityEngine.Material>> list2 = new global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::UnityEngine.Material>>();
		for (int i = 0; i < this.lodConfigs.Count; i++)
		{
			list.Add(new global::System.Collections.Generic.List<global::UnityEngine.CombineInstance>());
			list2.Add(new global::System.Collections.Generic.List<global::UnityEngine.Material>());
		}
		if (this.smRenderers.Count > 0)
		{
			for (int j = 0; j < this.smRenderers.Count; j++)
			{
				if (this.smRenderers[j].name.Contains("_LOD"))
				{
					global::UnityEngine.SkinnedMeshRenderer skinnedMeshRenderer = this.smRenderers[j];
					skinnedMeshRenderer.transform.rotation = global::UnityEngine.Quaternion.identity;
					skinnedMeshRenderer.transform.position = global::UnityEngine.Vector3.zero;
					int index = (int)char.GetNumericValue(skinnedMeshRenderer.name[skinnedMeshRenderer.name.Length - 1]);
					list2[index].Add(skinnedMeshRenderer.sharedMaterial);
					global::UnityEngine.CombineInstance item = default(global::UnityEngine.CombineInstance);
					item.mesh = skinnedMeshRenderer.sharedMesh;
					item.transform = skinnedMeshRenderer.transform.localToWorldMatrix;
					list[index].Add(item);
				}
				else
				{
					global::PandoraDebug.LogError(this.smRenderers[j].name + " is not conform", "MERGE", this);
				}
			}
			global::UnityEngine.LOD[] array = new global::UnityEngine.LOD[this.lodConfigs.Count];
			for (int k = 0; k < this.lodConfigs.Count; k++)
			{
				global::UnityEngine.Mesh mesh = new global::UnityEngine.Mesh();
				mesh.CombineMeshes(list[k].ToArray(), false);
				mesh.RecalculateBounds();
				global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject(base.gameObject.name + "_LOD" + k);
				gameObject.transform.SetParent(base.transform);
				gameObject.SetLayerRecursively(global::UnityEngine.LayerMask.NameToLayer("characters"));
				global::UnityEngine.SkinnedMeshRenderer skinnedMeshRenderer2 = gameObject.AddComponent<global::UnityEngine.SkinnedMeshRenderer>();
				skinnedMeshRenderer2.sharedMesh = mesh;
				skinnedMeshRenderer2.sharedMaterials = list2[k].ToArray();
				array[k] = new global::UnityEngine.LOD(this.lodConfigs[k].screenRelativeTransitionHeight, new global::UnityEngine.Renderer[]
				{
					skinnedMeshRenderer2
				});
			}
			this.InitLODGroup(array);
		}
		this.FinalPass();
	}

	private void InitLODGroup(global::UnityEngine.LOD[] lods)
	{
		global::UnityEngine.LODGroup lodgroup = base.GetComponent<global::UnityEngine.LODGroup>();
		if (lodgroup == null)
		{
			lodgroup = base.gameObject.AddComponent<global::UnityEngine.LODGroup>();
		}
		lodgroup.fadeMode = global::UnityEngine.LODFadeMode.CrossFade;
		lodgroup.animateCrossFading = true;
		lodgroup.SetLODs(lods);
		lodgroup.RecalculateBounds();
	}

	public void AttachAttachers()
	{
		global::BodyPartAttacher[] componentsInChildren = base.GetComponentsInChildren<global::BodyPartAttacher>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.AttachGameObject(componentsInChildren[i].gameObject, false);
		}
	}

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> AttachGameObject(global::UnityEngine.GameObject go, bool noLOD = false)
	{
		global::System.Collections.Generic.List<global::UnityEngine.GameObject> parts = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
		if (noLOD)
		{
			global::UnityEngine.LODGroup component = go.GetComponent<global::UnityEngine.LODGroup>();
			if (component != null)
			{
				global::UnityEngine.Object.Destroy(component);
			}
		}
		global::UnityEngine.SkinnedMeshRenderer[] componentsInChildren = go.GetComponentsInChildren<global::UnityEngine.SkinnedMeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			bool flag = componentsInChildren[i].GetComponent<global::UnityEngine.Cloth>() != null;
			if (!flag && noLOD && !componentsInChildren[i].name.Contains("LOD0"))
			{
				global::UnityEngine.Object.DestroyImmediate(componentsInChildren[i].gameObject);
			}
			else
			{
				this.AttachSkinMesh(componentsInChildren[i]);
				parts.Add(componentsInChildren[i].gameObject);
				if (!flag)
				{
					this.smRenderers.Add(componentsInChildren[i]);
					componentsInChildren[i].sharedMesh.RecalculateBounds();
				}
			}
		}
		global::Prometheus.OlympusFireStarter component2 = go.GetComponent<global::Prometheus.OlympusFireStarter>();
		if (component2 != null)
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(component2.fxName, base.GetComponent<global::UnitMenuController>(), null, delegate(global::UnityEngine.GameObject fx)
			{
				parts.Add(fx);
			}, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
		}
		global::UnityEngine.Object.Destroy(go);
		return parts;
	}

	private void AttachSkinMesh(global::UnityEngine.SkinnedMeshRenderer smr)
	{
		smr.transform.SetParent(base.transform);
		smr.bones = null;
		global::Arachne component = smr.gameObject.GetComponent<global::Arachne>();
		if (component != null)
		{
			component.Init();
		}
	}

	private global::UnityEngine.GameObject MergeLOD(int lodIdx)
	{
		global::System.Collections.Generic.List<global::UnityEngine.CombineInstance> list = new global::System.Collections.Generic.List<global::UnityEngine.CombineInstance>();
		global::System.Collections.Generic.List<global::UnityEngine.Texture2D> list2 = new global::System.Collections.Generic.List<global::UnityEngine.Texture2D>();
		global::System.Collections.Generic.List<global::UnityEngine.Texture2D> list3 = new global::System.Collections.Generic.List<global::UnityEngine.Texture2D>();
		global::System.Collections.Generic.List<global::UnityEngine.Texture2D> list4 = new global::System.Collections.Generic.List<global::UnityEngine.Texture2D>();
		global::System.Collections.Generic.List<global::UnityEngine.Texture2D> list5 = new global::System.Collections.Generic.List<global::UnityEngine.Texture2D>();
		global::System.Collections.Generic.List<global::UnityEngine.Vector2> list6 = new global::System.Collections.Generic.List<global::UnityEngine.Vector2>();
		global::System.Collections.Generic.List<int> list7 = new global::System.Collections.Generic.List<int>();
		foreach (global::UnityEngine.Renderer renderer in base.GetComponentsInChildren<global::UnityEngine.Renderer>())
		{
			if (renderer.material.HasProperty("_Color"))
			{
				renderer.material.color = renderer.sharedMaterial.color;
			}
		}
		bool flag = false;
		for (int j = 0; j < this.smRenderers.Count; j++)
		{
			if (this.smRenderers[j].material.HasProperty("_Illum"))
			{
				flag = true;
				break;
			}
		}
		for (int k = 0; k < this.smRenderers.Count; k++)
		{
			global::UnityEngine.SkinnedMeshRenderer skinnedMeshRenderer = this.smRenderers[k];
			if (skinnedMeshRenderer.name.Contains("_LOD" + lodIdx))
			{
				skinnedMeshRenderer.transform.rotation = global::UnityEngine.Quaternion.identity;
				skinnedMeshRenderer.transform.position = global::UnityEngine.Vector3.zero;
				list.Add(new global::UnityEngine.CombineInstance
				{
					mesh = skinnedMeshRenderer.sharedMesh,
					transform = skinnedMeshRenderer.transform.localToWorldMatrix
				});
				for (int l = 0; l < skinnedMeshRenderer.sharedMesh.uv.Length; l++)
				{
					list6.Add(skinnedMeshRenderer.sharedMesh.uv[l]);
				}
				list7.Add(skinnedMeshRenderer.sharedMesh.uv.Length);
				if (lodIdx == 0)
				{
					global::UnityEngine.Material material = skinnedMeshRenderer.GetComponent<global::UnityEngine.Renderer>().material;
					global::UnityEngine.Texture2D texture2D = material.mainTexture as global::UnityEngine.Texture2D;
					int width = texture2D.width;
					int height = texture2D.height;
					texture2D.name = string.Concat(new object[]
					{
						width,
						" ",
						height,
						" ",
						texture2D.name
					});
					int num = list2.IndexOf(texture2D);
					list2.Add(texture2D);
					list3.Add(this.GetTexture(material, "_BumpMap", width, height, true));
					list4.Add(this.GetTexture(material, "_SpecTex", width, height, true));
					if (flag)
					{
						if (material.HasProperty("_Illum") && material.GetTexture("_Illum") != null)
						{
							list5.Add(this.GetTexture(material, "_Illum", width, height, true));
						}
						else if (num != -1)
						{
							list5.Add(list5[num]);
						}
						else
						{
							global::UnityEngine.Texture2D texture2D2 = new global::UnityEngine.Texture2D(width, height, global::UnityEngine.TextureFormat.Alpha8, false);
							texture2D2.SetPixels32(new global::UnityEngine.Color32[width * height]);
							texture2D2.Apply();
							texture2D2.name = string.Concat(new object[]
							{
								texture2D.width,
								" ",
								texture2D.height,
								"fakeEmi "
							});
							list5.Add(texture2D2);
						}
					}
				}
			}
		}
		if (lodIdx == 0)
		{
			this.diffuseAtlas = new global::UnityEngine.Texture2D(1, 1, global::UnityEngine.TextureFormat.ARGB32, false);
			this.diffuseAtlas.filterMode = global::UnityEngine.FilterMode.Trilinear;
			this.diffuseAtlas.anisoLevel = 4;
			this.packingResult = this.diffuseAtlas.PackTextures(list2.ToArray(), 0, this.maxAtlasSize, false);
			this.diffuseAtlas.Compress(true);
			this.diffuseAtlas.Apply(true, true);
			this.normalAtlas = new global::UnityEngine.Texture2D(1, 1, global::UnityEngine.TextureFormat.RGB24, false, true);
			this.normalAtlas.filterMode = global::UnityEngine.FilterMode.Trilinear;
			this.normalAtlas.anisoLevel = 4;
			this.normalAtlas.PackTextures(list3.ToArray(), 0, this.maxAtlasSize, false);
			this.normalAtlas.Compress(true);
			this.normalAtlas.Apply(true, true);
			this.specAtlas = new global::UnityEngine.Texture2D(1, 1, global::UnityEngine.TextureFormat.ARGB32, false, true);
			this.specAtlas.filterMode = global::UnityEngine.FilterMode.Trilinear;
			this.specAtlas.anisoLevel = 4;
			this.specAtlas.PackTextures(list4.ToArray(), 0, this.maxAtlasSize, false);
			this.specAtlas.Compress(true);
			this.specAtlas.Apply(true, true);
			if (flag)
			{
				global::UnityEngine.Texture2D texture2D3 = new global::UnityEngine.Texture2D(1, 1, global::UnityEngine.TextureFormat.Alpha8, false, true);
				texture2D3.PackTextures(list5.ToArray(), 0, this.maxAtlasSize, false);
				this.emiAtlas = this.ScaleTexture(texture2D3, global::UnityEngine.TextureFormat.Alpha8, texture2D3.width / 4, texture2D3.height / 4);
			}
		}
		global::UnityEngine.Shader shader = (!flag) ? this.lodConfigs[lodIdx].shader : this.lodConfigs[lodIdx].shaderEmissive;
		if (this.oldShader == null || shader != this.oldShader)
		{
			this.combinedMat = new global::UnityEngine.Material(shader);
			this.combinedMat.mainTexture = this.diffuseAtlas;
			if (this.combinedMat.HasProperty("_BumpMap"))
			{
				this.combinedMat.SetTexture("_BumpMap", this.normalAtlas);
			}
			if (this.combinedMat.HasProperty("_SpecTex"))
			{
				this.combinedMat.SetTexture("_SpecTex", this.specAtlas);
			}
			if (this.combinedMat.HasProperty("_Illum"))
			{
				this.combinedMat.SetTexture("_Illum", this.emiAtlas);
			}
			this.oldShader = shader;
		}
		int num2 = 0;
		int num3 = 0;
		for (int m = 0; m < list6.Count; m++)
		{
			if (m == num3 + list7[num2])
			{
				num3 += list7[num2];
				num2++;
			}
			float x = global::UnityEngine.Mathf.Lerp(this.packingResult[num2].xMin, this.packingResult[num2].xMax, list6[m].x % 1f);
			float y = global::UnityEngine.Mathf.Lerp(this.packingResult[num2].yMin, this.packingResult[num2].yMax, list6[m].y % 1f);
			list6[m] = new global::UnityEngine.Vector2(x, y);
		}
		global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject(base.gameObject.name + "_LOD" + lodIdx);
		gameObject.transform.SetParent(base.transform);
		global::UnityEngine.SkinnedMeshRenderer skinnedMeshRenderer2 = gameObject.AddComponent<global::UnityEngine.SkinnedMeshRenderer>();
		global::UnityEngine.Mesh mesh = new global::UnityEngine.Mesh();
		mesh.CombineMeshes(list.ToArray());
		mesh.uv = list6.ToArray();
		skinnedMeshRenderer2.sharedMesh = mesh;
		skinnedMeshRenderer2.sharedMaterial = this.combinedMat;
		skinnedMeshRenderer2.updateWhenOffscreen = false;
		mesh.UploadMeshData(true);
		return gameObject;
	}

	private global::UnityEngine.Texture2D GetTexture(global::UnityEngine.Material mat, string name, int requiredWidth, int requiredHeight, bool overrideName = true)
	{
		global::UnityEngine.Texture2D texture2D = mat.GetTexture(name) as global::UnityEngine.Texture2D;
		if (overrideName)
		{
			texture2D.name = string.Concat(new object[]
			{
				texture2D.width,
				" ",
				texture2D.height,
				" ",
				texture2D.name
			});
		}
		return texture2D;
	}

	public void CheckTextures(global::UnityEngine.Material mat)
	{
		global::UnityEngine.Texture2D texture2D = mat.mainTexture as global::UnityEngine.Texture2D;
		int width = texture2D.width;
		int height = texture2D.height;
		this.GetTexture(mat, "_BumpMap", width, height, false);
		this.GetTexture(mat, "_SpecTex", width, height, false);
	}

	private void PrePass()
	{
		this.oldPos = base.transform.position;
		this.oldRot = base.transform.rotation;
		base.transform.position = global::UnityEngine.Vector3.zero;
		base.transform.rotation = global::UnityEngine.Quaternion.identity;
		this.mergeTime = global::UnityEngine.Time.realtimeSinceStartup;
	}

	private void FinalPass()
	{
		for (int i = 0; i < this.smRenderers.Count; i++)
		{
			global::UnityEngine.Object.Destroy(this.smRenderers[i].gameObject);
		}
		this.smRenderers.Clear();
		this.smRenderers = null;
		this.mergeTime = global::UnityEngine.Time.realtimeSinceStartup - this.mergeTime;
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"MergeTime ",
			base.name,
			" = ",
			this.mergeTime
		}), "GRAPHICS", this);
		base.transform.position = this.oldPos;
		base.transform.rotation = this.oldRot;
		global::UnityEngine.Animator component = base.GetComponent<global::UnityEngine.Animator>();
		if (component)
		{
			component.Rebind();
		}
		global::UnityEngine.Object.Destroy(this);
	}

	private void Weld(global::UnityEngine.Mesh mesh, float threshold, global::System.Collections.Generic.List<global::UnityEngine.BoneWeight> boneWeights = null)
	{
		global::UnityEngine.Vector3[] vertices = mesh.vertices;
		global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
		global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
		global::System.Collections.Generic.List<global::UnityEngine.Vector2> list3 = new global::System.Collections.Generic.List<global::UnityEngine.Vector2>();
		int num = 0;
		foreach (global::UnityEngine.Vector3 vector in vertices)
		{
			bool flag = false;
			foreach (global::UnityEngine.Vector3 a in list2)
			{
				if (global::UnityEngine.Vector3.Distance(a, vector) <= threshold)
				{
					flag = true;
					list.Add(num);
					break;
				}
			}
			if (!flag)
			{
				list2.Add(vector);
				list3.Add(mesh.uv[num]);
			}
			num++;
		}
		int[] triangles = mesh.triangles;
		for (int j = 0; j < triangles.Length; j++)
		{
			for (int k = 0; k < list2.Count; k++)
			{
				if (global::UnityEngine.Vector3.Distance(list2[k], vertices[triangles[j]]) <= threshold)
				{
					triangles[j] = k;
					break;
				}
			}
		}
		if (boneWeights != null)
		{
			for (int l = list.Count - 1; l >= 0; l--)
			{
				boneWeights.RemoveAt(list[l]);
			}
		}
		mesh.triangles = triangles;
		mesh.vertices = list2.ToArray();
		mesh.uv = list3.ToArray();
	}

	private global::UnityEngine.Texture2D ScaleTexture(global::UnityEngine.Texture2D source, global::UnityEngine.TextureFormat format, int targetWidth, int targetHeight)
	{
		global::UnityEngine.Texture2D texture2D = new global::UnityEngine.Texture2D(targetWidth, targetHeight, format, true);
		global::UnityEngine.Color[] pixels = texture2D.GetPixels(0);
		float num = 1f / (float)targetWidth;
		float num2 = 1f / (float)targetHeight;
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = source.GetPixelBilinear(num * ((float)i % (float)targetWidth), num2 * global::UnityEngine.Mathf.Floor((float)(i / targetWidth)));
		}
		texture2D.SetPixels(pixels, 0);
		texture2D.Apply(true, true);
		return texture2D;
	}

	public const string NORMAL_NAME = "_BumpMap";

	public const string SPEC_NAME = "_SpecTex";

	public const string OLD_SPEC_NAME = "_SpecMap";

	public const string EMI_NAME = "_Illum";

	private int maxAtlasSize = 2048;

	public global::System.Collections.Generic.List<global::LODConfig> lodConfigs;

	public global::System.Collections.Generic.List<global::UnityEngine.SkinnedMeshRenderer> smRenderers = new global::System.Collections.Generic.List<global::UnityEngine.SkinnedMeshRenderer>();

	private global::UnityEngine.Rect[] packingResult;

	private global::UnityEngine.Material combinedMat;

	private global::UnityEngine.Shader oldShader;

	private global::UnityEngine.Texture2D diffuseAtlas;

	private global::UnityEngine.Texture2D normalAtlas;

	private global::UnityEngine.Texture2D specAtlas;

	private global::UnityEngine.Texture2D emiAtlas;

	private global::UnityEngine.Vector3 oldPos;

	private global::UnityEngine.Quaternion oldRot;

	private float mergeTime;
}
