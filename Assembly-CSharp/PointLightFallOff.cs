using System;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class PointLightFallOff : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.GenerateTexture();
	}

	private void Update()
	{
		if (this.generate)
		{
			this.generate = false;
			this.GenerateTexture();
		}
	}

	private void GenerateTexture()
	{
		this.fallOffTexture = new global::UnityEngine.Texture2D(256, 256, global::UnityEngine.TextureFormat.ARGB32, false, true);
		this.fallOffTexture.filterMode = global::UnityEngine.FilterMode.Bilinear;
		this.fallOffTexture.wrapMode = global::UnityEngine.TextureWrapMode.Clamp;
		for (int i = 0; i < this.fallOffTexture.height; i++)
		{
			for (int j = 0; j < this.fallOffTexture.width; j++)
			{
				float time = ((float)j + 0.5f) / (float)this.fallOffTexture.width;
				float num = this.fallOffCurve.Evaluate(time);
				global::UnityEngine.Color color = new global::UnityEngine.Color(num, num, num, num);
				this.fallOffTexture.SetPixel(j, i, color);
			}
		}
		this.fallOffTexture.Apply();
		global::UnityEngine.Shader.SetGlobalTexture("_LookUpTexture", this.fallOffTexture);
	}

	public global::UnityEngine.AnimationCurve fallOffCurve;

	public global::UnityEngine.Texture2D fallOffTexture;

	public bool generate;
}
