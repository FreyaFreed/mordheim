using System;
using System.Collections;
using UnityEngine;

public class AlphaFactorFade : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.elapsed = 0f;
		if (this.duplicateMat)
		{
			this.mat = base.GetComponent<global::UnityEngine.Renderer>().material;
		}
		else
		{
			this.mat = base.GetComponent<global::UnityEngine.Renderer>().sharedMaterial;
		}
		if (!this.mat.HasProperty("_AlphaFactor"))
		{
			global::UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		this.mat.SetFloat("_AlphaFactor", this.fadeFrom);
	}

	private void Update()
	{
		this.elapsed += global::UnityEngine.Time.deltaTime;
		if (this.elapsed >= this.timeToFade)
		{
			base.StartCoroutine(this.FadeAlphaFactor());
		}
	}

	private global::System.Collections.IEnumerator FadeAlphaFactor()
	{
		this.fade = this.mat.GetFloat("_AlphaFactor");
		float t = 0f;
		while (t < 1f)
		{
			t += global::UnityEngine.Time.deltaTime;
			float trans = t / this.fadeTime;
			this.fade = global::UnityEngine.Mathf.SmoothStep(this.fade, this.fadeTo, trans);
			this.mat.SetFloat("_AlphaFactor", this.fade);
			yield return 0;
		}
		yield break;
	}

	public float timeToFade;

	public float fadeTime;

	public float fadeFrom;

	public float fadeTo;

	public bool duplicateMat;

	private float elapsed;

	private float fade;

	private global::UnityEngine.Material mat;
}
