using System;
using Flowmap;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Flowmaps/Heightmap/Texture")]
public class FlowTextureHeightmap : global::FlowHeightmap
{
	public override global::UnityEngine.Texture HeightmapTexture
	{
		get
		{
			return this.heightmap;
		}
		set
		{
			this.heightmap = (value as global::UnityEngine.Texture2D);
		}
	}

	public override global::UnityEngine.Texture PreviewHeightmapTexture
	{
		get
		{
			if (this.isRaw)
			{
				if (this.rawPreview == null)
				{
					this.GenerateRawPreview();
				}
				return this.rawPreview;
			}
			return this.HeightmapTexture;
		}
	}

	public void GenerateRawPreview()
	{
		if (this.rawPreview)
		{
			global::UnityEngine.Object.DestroyImmediate(this.rawPreview);
		}
		if (this.heightmap)
		{
			this.rawPreview = global::Flowmap.TextureUtilities.GetRawPreviewTexture(this.heightmap);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.rawPreview)
		{
			global::UnityEngine.Object.DestroyImmediate(this.rawPreview);
		}
	}

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Texture2D heightmap;

	public bool isRaw;

	private global::UnityEngine.Texture2D rawPreview;
}
