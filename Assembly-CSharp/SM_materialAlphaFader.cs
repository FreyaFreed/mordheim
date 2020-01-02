using System;
using UnityEngine;

public class SM_materialAlphaFader : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.r = base.GetComponent<global::UnityEngine.Renderer>();
	}

	private void Update()
	{
		this.beginTintAlpha -= global::UnityEngine.Time.deltaTime * this.fadeSpeed;
		this.r.material.SetColor("_TintColor", new global::UnityEngine.Color(1f, 1f, 1f, this.beginTintAlpha));
	}

	public float fadeSpeed = 1f;

	public float beginTintAlpha = 0.5f;

	private global::UnityEngine.Renderer r;
}
