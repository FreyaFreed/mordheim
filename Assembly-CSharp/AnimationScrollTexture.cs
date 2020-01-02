using System;
using UnityEngine;

public class AnimationScrollTexture : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.r = base.GetComponent<global::UnityEngine.Renderer>();
	}

	private void FixedUpdate()
	{
		float y = global::UnityEngine.Time.time * -this.Speed;
		this.r.material.mainTextureOffset = new global::UnityEngine.Vector2(0f, y);
	}

	public float Speed = 0.25f;

	private global::UnityEngine.Renderer r;
}
