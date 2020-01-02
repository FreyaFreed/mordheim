using System;
using UnityEngine;

public class CheapUniStorm : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		if (this.heavyClouds != null)
		{
			this.heavyCloudsRenderer = this.heavyClouds.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.heavyCloudsLayer1 != null)
		{
			this.heavyCloudsLayer1Renderer = this.heavyCloudsLayer1.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.heavyCloudsLayerLight != null)
		{
			this.heavyCloudsLayerLightRenderer = this.heavyCloudsLayerLight.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.lightClouds1 != null)
		{
			this.lightClouds1Renderer = this.lightClouds1.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.lightClouds2 != null)
		{
			this.lightClouds2Renderer = this.lightClouds2.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.lightClouds3 != null)
		{
			this.lightClouds3Renderer = this.lightClouds3.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.lightClouds4 != null)
		{
			this.lightClouds4Renderer = this.lightClouds4.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.lightClouds5 != null)
		{
			this.lightClouds5Renderer = this.lightClouds5.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.highClouds1 != null)
		{
			this.highClouds1Renderer = this.highClouds1.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.highClouds2 != null)
		{
			this.highClouds2Renderer = this.highClouds2.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.mostlyCloudyClouds != null)
		{
			this.mostlyCloudyCloudsRenderer = this.mostlyCloudyClouds.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.starSphere != null)
		{
			this.starSphereRenderer = this.starSphere.GetComponent<global::UnityEngine.Renderer>();
		}
		if (this.rainDecal != null)
		{
			this.rainDecalRenderer = this.rainDecal.GetComponent<global::UnityEngine.Renderer>();
		}
	}

	private void Update()
	{
		float num = this.cloudSpeed * 0.001f;
		float num2 = this.heavyCloudSpeed * 0.001f;
		float num3 = 0.003f;
		float num4 = 0.004f;
		float num5 = global::UnityEngine.Time.time * num;
		float num6 = global::UnityEngine.Time.time * num2;
		float y = global::UnityEngine.Time.time * num3;
		float y2 = global::UnityEngine.Time.time * num;
		float num7 = global::UnityEngine.Time.time * num4;
		if (this.heavyClouds != null && this.heavyClouds.gameObject.activeSelf)
		{
			this.heavyCloudsRenderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(num6, num6, 0f, 0f));
		}
		if (this.heavyCloudsLayer1 != null && this.heavyCloudsLayer1.gameObject.activeSelf)
		{
			this.heavyCloudsLayer1Renderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(0f, num6, 0f, 0f));
		}
		if (this.heavyCloudsLayerLight != null && this.heavyCloudsLayerLight.gameObject.activeSelf)
		{
			this.heavyCloudsLayerLightRenderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(0f, num6, 0f, 0f));
		}
		if (this.lightClouds1 != null && this.lightClouds1.gameObject.activeSelf)
		{
			this.lightClouds1Renderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(0f, num5, 0f, 0f));
		}
		if (this.lightClouds2 != null && this.lightClouds2.gameObject.activeSelf)
		{
			this.lightClouds2Renderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(num5, num5, 0f, 0f));
		}
		if (this.lightClouds3 != null && this.lightClouds3.gameObject.activeSelf)
		{
			this.lightClouds3Renderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(0f, num5, 0f, 0f));
		}
		if (this.lightClouds4 != null && this.lightClouds4.gameObject.activeSelf)
		{
			this.lightClouds4Renderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(num5, num5, 0f, 0f));
		}
		if (this.lightClouds5 != null && this.lightClouds5.gameObject.activeSelf)
		{
			this.lightClouds5Renderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(0f, num5, 0f, 0f));
		}
		if (this.highClouds1 != null && this.highClouds1.gameObject.activeSelf)
		{
			this.highClouds1Renderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(0f, y2, 0f, 0f));
		}
		if (this.highClouds2 != null && this.highClouds2.gameObject.activeSelf)
		{
			this.highClouds2Renderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(0f, y2, 0f, 0f));
		}
		if (this.mostlyCloudyClouds != null && this.mostlyCloudyClouds.gameObject.activeSelf)
		{
			this.mostlyCloudyCloudsRenderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(0f, y, 0f, 0f));
		}
		if (this.starSphere != null && this.starSphere.gameObject.activeSelf)
		{
			this.starSphereRenderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(num7 / 2f, 0.02f, 0f, 0f));
			this.starSphereRenderer.sharedMaterial.SetColor("_TintColor", this.starBrightness);
		}
		if (this.rainDecal != null && this.rainDecal.gameObject.activeSelf)
		{
			this.rainDecalRenderer.sharedMaterial.SetVector("_Offset", new global::UnityEngine.Vector4(0f, num5 * 10f, 0f, 0f));
		}
	}

	public float cloudSpeed;

	public float heavyCloudSpeed;

	public global::UnityEngine.GameObject lightClouds1;

	public global::UnityEngine.GameObject lightClouds2;

	public global::UnityEngine.GameObject lightClouds3;

	public global::UnityEngine.GameObject lightClouds4;

	public global::UnityEngine.GameObject lightClouds5;

	public global::UnityEngine.GameObject highClouds1;

	public global::UnityEngine.GameObject highClouds2;

	public global::UnityEngine.GameObject mostlyCloudyClouds;

	public global::UnityEngine.GameObject heavyClouds;

	public global::UnityEngine.GameObject heavyCloudsLayer1;

	public global::UnityEngine.GameObject heavyCloudsLayerLight;

	public global::UnityEngine.GameObject starSphere;

	public global::UnityEngine.Color starBrightness;

	public global::UnityEngine.GameObject horizonObject;

	public global::UnityEngine.GameObject rainDecal;

	private global::UnityEngine.Renderer heavyCloudsRenderer;

	private global::UnityEngine.Renderer heavyCloudsLayer1Renderer;

	private global::UnityEngine.Renderer heavyCloudsLayerLightRenderer;

	private global::UnityEngine.Renderer lightClouds1Renderer;

	private global::UnityEngine.Renderer lightClouds2Renderer;

	private global::UnityEngine.Renderer lightClouds3Renderer;

	private global::UnityEngine.Renderer lightClouds4Renderer;

	private global::UnityEngine.Renderer lightClouds5Renderer;

	private global::UnityEngine.Renderer highClouds1Renderer;

	private global::UnityEngine.Renderer highClouds2Renderer;

	private global::UnityEngine.Renderer mostlyCloudyCloudsRenderer;

	private global::UnityEngine.Renderer starSphereRenderer;

	private global::UnityEngine.Renderer rainDecalRenderer;
}
