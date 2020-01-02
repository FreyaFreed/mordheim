using System;
using UnityEngine;

public class ShaderSetter : global::UnityEngine.MonoBehaviour
{
	public void ApplyShaderParams()
	{
		global::UnityEngine.Renderer[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].transform.parent == null || componentsInChildren[i].transform.parent.gameObject.GetComponent<global::ItemController>() == null)
			{
				global::UnityEngine.Material[] materials = componentsInChildren[i].materials;
				for (int j = 0; j < materials.Length; j++)
				{
					this.ApplyShaderParams(materials[j]);
				}
			}
		}
	}

	private void ApplyShaderParams(global::UnityEngine.Material mat)
	{
		mat.SetColor("_Color", this.DiffColor);
		mat.SetColor("_SpecColor", this.SpecColor);
		mat.SetFloat("_SpecInt", this.SpecIntensity);
		mat.SetFloat("_Shininess", this.Sharpness);
		mat.SetFloat("_Fresnel", this.FresnelStrength);
		if (mat.HasProperty("_Illum"))
		{
			mat.SetColor("_GlowColor", this.GlowColor);
			mat.SetFloat("_GlowStrength", this.GlowStrength);
		}
	}

	public global::UnityEngine.Color DiffColor;

	public global::UnityEngine.Color SpecColor;

	public float SpecIntensity;

	public float Sharpness;

	public float FresnelStrength;

	public global::UnityEngine.Color GlowColor;

	public float GlowStrength;
}
