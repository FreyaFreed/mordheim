using System;
using System.Collections.Generic;
using UnityEngine;

public class TexturePacker : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
	}

	public void Merge()
	{
		global::UnityEngine.MeshRenderer component = base.gameObject.GetComponent<global::UnityEngine.MeshRenderer>();
		global::System.Collections.Generic.List<global::UnityEngine.Texture2D> list = new global::System.Collections.Generic.List<global::UnityEngine.Texture2D>();
		global::System.Collections.Generic.List<global::UnityEngine.Texture2D> list2 = new global::System.Collections.Generic.List<global::UnityEngine.Texture2D>();
		global::System.Collections.Generic.List<global::UnityEngine.Texture2D> list3 = new global::System.Collections.Generic.List<global::UnityEngine.Texture2D>();
		global::UnityEngine.Material[] materials = component.materials;
		foreach (global::UnityEngine.Material material in materials)
		{
			list.Add(material.mainTexture as global::UnityEngine.Texture2D);
			list2.Add(material.GetTexture("_BumpMap") as global::UnityEngine.Texture2D);
			list3.Add(material.GetTexture("_Spec_Gloss_Reflec_Masks") as global::UnityEngine.Texture2D);
		}
		global::UnityEngine.Texture2D texture2D = new global::UnityEngine.Texture2D(1, 1);
		global::UnityEngine.Texture2D texture2D2 = new global::UnityEngine.Texture2D(1, 1);
		global::UnityEngine.Texture2D texture2D3 = new global::UnityEngine.Texture2D(1, 1);
		global::UnityEngine.Rect[] array2 = texture2D.PackTextures(list.ToArray(), 0);
		texture2D2.PackTextures(list2.ToArray(), 0);
		texture2D3.PackTextures(list3.ToArray(), 0);
		global::UnityEngine.MeshFilter component2 = base.gameObject.GetComponent<global::UnityEngine.MeshFilter>();
		global::UnityEngine.Vector2[] uv = component2.mesh.uv;
		global::UnityEngine.Vector2[] array3 = new global::UnityEngine.Vector2[uv.Length];
		int num = 0;
		for (int j = 0; j < array3.Length; j++)
		{
			array3[j].x = global::UnityEngine.Mathf.Lerp(array2[num].xMin, array2[num].xMax, uv[j].x);
			array3[j].y = global::UnityEngine.Mathf.Lerp(array2[num].yMin, array2[num].yMax, uv[j].y);
		}
		global::UnityEngine.Material material2 = new global::UnityEngine.Material(global::UnityEngine.Shader.Find("HardSurface/Hardsurface Free/Opaque Specular"));
		material2.mainTexture = texture2D;
		material2.SetTexture("_BumpMap", texture2D2);
		material2.SetTexture("_Spec_Gloss_Reflec_Masks", texture2D3);
		component.materials = new global::UnityEngine.Material[]
		{
			material2
		};
	}

	private const string NORMAL_NAME = "_BumpMap";

	private const string SPEC_NAME = "_Spec_Gloss_Reflec_Masks";
}
