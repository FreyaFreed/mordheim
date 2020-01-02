using System;
using Flowmap;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.RequireComponent(typeof(global::FlowmapGenerator))]
public class FlowHeightmap : global::UnityEngine.MonoBehaviour
{
	public virtual global::UnityEngine.Texture HeightmapTexture { get; set; }

	public virtual global::UnityEngine.Texture PreviewHeightmapTexture { get; set; }

	protected global::FlowmapGenerator Generator
	{
		get
		{
			if (!this.generator)
			{
				this.generator = base.GetComponent<global::FlowmapGenerator>();
			}
			return this.generator;
		}
	}

	protected virtual void OnDrawGizmosSelected()
	{
		this.DisplayPreviewHeightmap(true);
		this.UpdatePreviewHeightmap();
	}

	public void DisplayPreviewHeightmap(bool state)
	{
		this.wantsToDrawHeightmap = state;
		this.UpdatePreviewHeightmap();
	}

	public void UpdatePreviewHeightmap()
	{
		if (this.previewGameObject == null || this.previewMaterial == null)
		{
			this.Cleanup();
			this.previewGameObject = new global::UnityEngine.GameObject("Preview Heightmap");
			this.previewGameObject.hideFlags = (global::UnityEngine.HideFlags.HideInHierarchy | global::UnityEngine.HideFlags.HideInInspector | global::UnityEngine.HideFlags.NotEditable);
			global::UnityEngine.MeshFilter meshFilter = this.previewGameObject.AddComponent<global::UnityEngine.MeshFilter>();
			meshFilter.sharedMesh = global::Flowmap.Primitives.PlaneMesh;
			global::UnityEngine.MeshRenderer meshRenderer = this.previewGameObject.AddComponent<global::UnityEngine.MeshRenderer>();
			this.previewMaterial = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("Hidden/HeightmapFieldPreview"));
			this.previewMaterial.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			meshRenderer.sharedMaterial = this.previewMaterial;
		}
		if (this.previewHeightmap && this.wantsToDrawHeightmap)
		{
			this.previewMaterial.SetTexture("_MainTex", this.PreviewHeightmapTexture);
			this.previewMaterial.SetFloat("_Strength", 1f);
			this.previewGameObject.GetComponent<global::UnityEngine.Renderer>().enabled = true;
			this.previewGameObject.transform.position = base.transform.position;
			this.previewGameObject.transform.localScale = new global::UnityEngine.Vector3(this.Generator.Dimensions.x, 1f, this.Generator.Dimensions.y);
		}
		else
		{
			this.previewGameObject.GetComponent<global::UnityEngine.Renderer>().enabled = false;
		}
	}

	protected virtual void OnDrawGizmos()
	{
		this.DisplayPreviewHeightmap(false);
		this.UpdatePreviewHeightmap();
	}

	protected virtual void OnDestroy()
	{
		this.Cleanup();
	}

	private void Cleanup()
	{
		if (this.previewGameObject)
		{
			if (global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Object.Destroy(this.previewGameObject);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(this.previewGameObject);
			}
		}
		if (this.previewMaterial)
		{
			if (global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Object.Destroy(this.previewMaterial);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(this.previewMaterial);
			}
		}
	}

	public bool previewHeightmap;

	public bool drawPreviewPlane;

	private bool wantsToDrawHeightmap;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.GameObject previewGameObject;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Material previewMaterial;

	private global::FlowmapGenerator generator;
}
