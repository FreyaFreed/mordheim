using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshBatcher : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		if (this.batchOnAwake)
		{
			this.Batch();
		}
	}

	public void Batch()
	{
		this.TryBatch();
	}

	public void TryBatch()
	{
		this.meshGroups = new global::System.Collections.Generic.List<global::LODMeshGroup>();
		for (int i = 0; i < this.lodValues.Count; i++)
		{
			this.meshGroups.Add(new global::LODMeshGroup());
		}
		global::MeshBatcherBlocker[] componentsInChildren = base.GetComponentsInChildren<global::MeshBatcherBlocker>();
		global::System.Collections.Generic.List<global::UnityEngine.GameObject> list = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			list.Add(componentsInChildren[j].gameObject);
		}
		this.ParseTransform(list, base.transform);
		int count = this.lodValues.Count;
		global::UnityEngine.LOD[] array = new global::UnityEngine.LOD[count];
		for (int k = 0; k < this.meshGroups.Count; k++)
		{
			global::LODMeshGroup lodmeshGroup = this.meshGroups[k];
			global::System.Collections.Generic.List<global::UnityEngine.Renderer> list2 = new global::System.Collections.Generic.List<global::UnityEngine.Renderer>();
			foreach (global::UnityEngine.Material material in lodmeshGroup.materialInstances.Keys)
			{
				global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject();
				gameObject.name = material.name + "_LOD" + k;
				gameObject.transform.SetParent(base.transform);
				global::UnityEngine.Mesh mesh = new global::UnityEngine.Mesh();
				mesh.CombineMeshes(lodmeshGroup.materialInstances[material].ToArray());
				mesh.Optimize();
				global::UnityEngine.MeshFilter meshFilter = gameObject.AddComponent<global::UnityEngine.MeshFilter>();
				meshFilter.sharedMesh = mesh;
				global::UnityEngine.MeshRenderer meshRenderer = gameObject.AddComponent<global::UnityEngine.MeshRenderer>();
				meshRenderer.sharedMaterial = material;
				gameObject.SetLayerRecursively(base.gameObject.layer);
				list2.Add(gameObject.GetComponent<global::UnityEngine.Renderer>());
				mesh.UploadMeshData(true);
				gameObject.isStatic = true;
			}
			array[k] = new global::UnityEngine.LOD(this.lodValues[k], list2.ToArray());
		}
		global::UnityEngine.LODGroup lodgroup = base.gameObject.GetComponent<global::UnityEngine.LODGroup>();
		if (lodgroup == null)
		{
			lodgroup = base.gameObject.AddComponent<global::UnityEngine.LODGroup>();
		}
		lodgroup.SetLODs(array);
		lodgroup.RecalculateBounds();
		lodgroup.fadeMode = global::UnityEngine.LODFadeMode.CrossFade;
		lodgroup.animateCrossFading = true;
		global::UnityEngine.Object.Destroy(this);
	}

	private void ParseTransform(global::System.Collections.Generic.List<global::UnityEngine.GameObject> lockedObjects, global::UnityEngine.Transform trans)
	{
		global::System.Collections.Generic.List<global::UnityEngine.Transform> list = new global::System.Collections.Generic.List<global::UnityEngine.Transform>();
		for (int i = 0; i < trans.childCount; i++)
		{
			global::UnityEngine.Transform child = trans.GetChild(i);
			list.Add(child);
		}
		list.Sort(new global::TransformComparer());
		for (int j = 0; j < list.Count; j++)
		{
			global::UnityEngine.Transform transform = list[j];
			if (lockedObjects.IndexOf(transform.gameObject) == -1)
			{
				if (transform.name.Contains("LOD"))
				{
					int num = (int)char.GetNumericValue(transform.name[transform.name.Length - 1]);
					global::UnityEngine.MeshFilter component = transform.GetComponent<global::UnityEngine.MeshFilter>();
					global::UnityEngine.Material material = null;
					global::UnityEngine.Mesh mesh = null;
					if (num > 0 && (component == null || component.sharedMesh == null))
					{
						string value = transform.name.Substring(0, transform.name.LastIndexOf("_"));
						foreach (global::System.Collections.Generic.KeyValuePair<global::UnityEngine.Material, global::System.Collections.Generic.List<global::UnityEngine.CombineInstance>> keyValuePair in this.meshGroups[num - 1].materialInstances)
						{
							for (int k = 0; k < keyValuePair.Value.Count; k++)
							{
								if (keyValuePair.Value[k].mesh.name.Contains(value))
								{
									material = keyValuePair.Key;
									mesh = keyValuePair.Value[k].mesh;
								}
							}
						}
					}
					if (component != null && component.sharedMesh != null)
					{
						material = component.GetComponent<global::UnityEngine.Renderer>().sharedMaterial;
						mesh = component.sharedMesh;
					}
					if (mesh == null)
					{
						global::PandoraDebug.LogWarning("Transform : " + transform.name + " contains no mesh", "MESH_BATCHER", this);
					}
					else if (material == null)
					{
						global::PandoraDebug.LogWarning("Transform : " + transform.name + " contains no material", "MESH_BATCHER", this);
					}
					else
					{
						if (!this.meshGroups[num].materialInstances.ContainsKey(material))
						{
							this.meshGroups[num].materialInstances[material] = new global::System.Collections.Generic.List<global::UnityEngine.CombineInstance>();
						}
						global::UnityEngine.CombineInstance item = default(global::UnityEngine.CombineInstance);
						item.mesh = mesh;
						item.transform = transform.localToWorldMatrix;
						this.meshGroups[num].materialInstances[material].Add(item);
						if (transform.gameObject.GetComponent<global::UnityEngine.Collider>() && component != null)
						{
							global::UnityEngine.MeshRenderer component2 = component.gameObject.GetComponent<global::UnityEngine.MeshRenderer>();
							if (component2 != null)
							{
								global::UnityEngine.Object.DestroyImmediate(component2);
							}
							global::UnityEngine.Object.DestroyImmediate(component);
						}
						else
						{
							global::UnityEngine.Object.DestroyImmediate(transform.gameObject);
						}
					}
				}
				else if (transform.childCount > 0)
				{
					this.ParseTransform(lockedObjects, transform);
				}
			}
		}
	}

	public global::System.Collections.Generic.List<float> lodValues = new global::System.Collections.Generic.List<float>
	{
		0.4f,
		0.3f,
		0.02f
	};

	private global::System.Collections.Generic.List<global::LODMeshGroup> meshGroups;

	public bool batchOnAwake;
}
