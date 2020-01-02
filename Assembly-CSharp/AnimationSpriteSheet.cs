using System;
using UnityEngine;

public class AnimationSpriteSheet : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.r = base.GetComponent<global::UnityEngine.Renderer>();
	}

	private void Update()
	{
		int num = global::UnityEngine.Mathf.FloorToInt(global::UnityEngine.Time.time * this.fps);
		num %= this.uvX * this.uvY;
		global::UnityEngine.Vector2 scale = new global::UnityEngine.Vector2(1f / (float)this.uvX, 1f / (float)this.uvY);
		int num2 = num % this.uvX;
		int num3 = num / this.uvX;
		global::UnityEngine.Vector2 offset = new global::UnityEngine.Vector2((float)num2 * scale.x, 1f - scale.y - (float)num3 * scale.y);
		this.r.material.SetTextureOffset("_MainTex", offset);
		this.r.material.SetTextureScale("_MainTex", scale);
	}

	public int uvX = 4;

	public int uvY = 2;

	public float fps = 24f;

	private global::UnityEngine.Renderer r;
}
