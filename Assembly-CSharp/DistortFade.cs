using System;
using System.Collections;
using UnityEngine;

public class DistortFade : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.elapsed = 0f;
		if (!base.GetComponent<global::UnityEngine.Renderer>().sharedMaterial.HasProperty("_BumpAmt"))
		{
			global::UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		base.GetComponent<global::UnityEngine.Renderer>().sharedMaterial.SetFloat("_BumpAmt", this.fadeFrom);
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
		this.fade = base.GetComponent<global::UnityEngine.Renderer>().sharedMaterial.GetFloat("_BumpAmt");
		float t = 0f;
		while (t < 1f)
		{
			t += global::UnityEngine.Time.deltaTime;
			float trans = t / this.fadeTime;
			this.fade = global::UnityEngine.Mathf.SmoothStep(this.fade, this.fadeTo, trans);
			base.GetComponent<global::UnityEngine.Renderer>().sharedMaterial.SetFloat("_BumpAmt", this.fade);
			yield return 0;
		}
		yield break;
	}

	public float timeToFade;

	public float fadeTime;

	public float fadeFrom;

	public float fadeTo;

	private float elapsed;

	private float fade;
}
